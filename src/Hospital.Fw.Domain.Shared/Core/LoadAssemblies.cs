using Hospital.Fw.Domain.Shared.Core.Options;
using Microsoft.Extensions.DependencyModel;
using System.Reflection;
using System.Runtime.Loader;

namespace Hospital.Fw.Domain.Shared.Core;

public static class LoadAssemblies
{
    /// <summary>
    /// 配置需要优先加载的程序集列表
    /// </summary>
    public static List<Assembly> AssembliesStartingWith { get; private set; }

    /// <summary>
    /// 静态构造函数，初始化程序集列表
    /// </summary>
    static LoadAssemblies()
    {
        (IEnumerable<Assembly>? assemblies, IEnumerable<Assembly>? externalAssemblies, IEnumerable<string>? pathOfExternalAssemblies) = GetAssemblies();
        AssembliesStartingWith = assemblies.ToList();
    }

    /// <summary>
    /// 私有设置，避免重复解析
    /// </summary>
    internal static AppSettingsOptions Settings = new AppSettingsOptions
    {
        SupportPackageNamePrefixs = new[] { "Hospital" },
        ExternalAssemblies = new[] { string.Empty },
        ExcludeAssemblies = new[] { string.Empty },
        EnabledReferenceAssemblyScan = false
    };


    /// <summary>
    /// 配置应用程序选项
    /// </summary>
    /// <param name="options">配置</param>
    public static void Configure(Action<AppSettingsOptions> options)
    {
        options.Invoke(Settings);
    }

    /// <summary>
    /// 获取应用有效程序集
    /// </summary>
    /// <returns>IEnumerable</returns>
    private static (IEnumerable<Assembly> Assemblies, IEnumerable<Assembly> ExternalAssemblies, IEnumerable<string> PathOfExternalAssemblies) GetAssemblies()
    {
        // 需排除的程序集后缀
        string[] excludeAssemblyNames = new string[] {
                "Database.Migrations"
            };

        // 读取应用配置
        string[] supportPackageNamePrefixs = Settings.SupportPackageNamePrefixs ?? Array.Empty<string>();

        IEnumerable<Assembly> scanAssemblies;

        // 获取入口程序集
        Assembly? entryAssembly = Assembly.GetEntryAssembly();

        // 非独立发布/非单文件发布
        if (!string.IsNullOrWhiteSpace(entryAssembly.Location))
        {
            DependencyContext? dependencyContext = DependencyContext.Default;

            // 读取项目程序集或 Furion 官方发布的包，或手动添加引用的dll，或配置特定的包前缀
            scanAssemblies = dependencyContext.RuntimeLibraries
               .Where(u =>
                      (u.Type == "project" && !excludeAssemblyNames.Any(j => u.Name.EndsWith(j))) ||
                      (u.Type == "package" && supportPackageNamePrefixs.Any(p => u.Name.StartsWith(p) && u.RuntimeAssemblyGroups.Count > 0)) ||
                      (Settings.EnabledReferenceAssemblyScan == true && u.Type == "reference"))    // 判断是否启用引用程序集扫描
               .Select(u => Reflect.GetAssembly(u.Name));
        }
        // 独立发布/单文件发布
        else
        {
            IEnumerable<Assembly> fixedSingleFileAssemblies = new[] { entryAssembly };

            // 扫描实现 ISingleFilePublish 接口的类型
            Type? singleFilePublishType = entryAssembly.GetTypes()
                                                .FirstOrDefault(u => u.IsClass && !u.IsInterface && !u.IsAbstract && typeof(ISingleFilePublish).IsAssignableFrom(u));
            if (singleFilePublishType != null)
            {
                ISingleFilePublish? singleFilePublish = Activator.CreateInstance(singleFilePublishType) as ISingleFilePublish;

                // 加载用户自定义配置单文件所需程序集
                Assembly[] nativeAssemblies = singleFilePublish.IncludeAssemblies();
                IEnumerable<Assembly> loadAssemblies = singleFilePublish.IncludeAssemblyNames()
                                                .Select(u => Reflect.GetAssembly(u));

                fixedSingleFileAssemblies = fixedSingleFileAssemblies.Concat(nativeAssemblies)
                                                            .Concat(loadAssemblies);
            }
            else
            {
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Red;
                // 提示没有正确配置单文件配置
                Console.WriteLine("Deploy Console"
                    , "Single file deploy error."
                    , "##Exception## Single file deployment configuration error.");
                Console.ResetColor();
            }

            // 通过 AppDomain.CurrentDomain 扫描，默认为延迟加载，正常只能扫描到 Furion 和 入口程序集（启动层）
            scanAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                                    .Where(ass =>
                                            // 排除 System，Microsoft，netstandard 开头的程序集
                                            !ass.FullName.StartsWith(nameof(System))
                                            && !ass.FullName.StartsWith(nameof(Microsoft))
                                            && !ass.FullName.StartsWith("netstandard"))
                                    .Concat(fixedSingleFileAssemblies)
                                    .Distinct();
        }

        IEnumerable<Assembly> externalAssemblies = Array.Empty<Assembly>();
        IEnumerable<string> pathOfExternalAssemblies = Array.Empty<string>();

        // 加载 appsettings.json 配置的外部程序集
        if (Settings.ExternalAssemblies != null && Settings.ExternalAssemblies.Any())
        {
            List<string> externalDlls = new List<string>();
            foreach (string item in Settings.ExternalAssemblies)
            {
                if (string.IsNullOrWhiteSpace(item)) continue;

                string path = Path.Combine(AppContext.BaseDirectory, item);

                // 若以 .dll 结尾则认为是一个文件
                if (item.EndsWith(".dll"))
                {
                    if (File.Exists(path)) externalDlls.Add(path);
                }
                // 否则作为目录查找或拼接 .dll 后缀作为文件名查找
                else
                {
                    // 作为目录查找所有 .dll 文件
                    if (Directory.Exists(path))
                    {
                        externalDlls.AddRange(Directory.EnumerateFiles(path, "*.dll", SearchOption.AllDirectories));
                    }
                    // 拼接 .dll 后缀查找
                    else
                    {
                        string pathDll = path + ".dll";
                        if (File.Exists(pathDll)) externalDlls.Add(pathDll);
                    }
                }
            }

            // 加载外部程序集
            foreach (string assemblyFileFullPath in externalDlls)
            {
                // 根据路径加载程序集
                Assembly loadedAssembly = Reflect.LoadAssembly(assemblyFileFullPath);
                if (loadedAssembly == default) continue;
                Assembly[] assembly = new[] { loadedAssembly };

                if (scanAssemblies.Any(u => u == loadedAssembly)) continue;

                // 合并程序集
                scanAssemblies = scanAssemblies.Concat(assembly);
                externalAssemblies = externalAssemblies.Concat(assembly);
                pathOfExternalAssemblies = pathOfExternalAssemblies.Concat(new[] { assemblyFileFullPath });
            }
        }

        // 处理排除的程序集
        if (Settings.ExcludeAssemblies != null && Settings.ExcludeAssemblies.Any())
        {
            scanAssemblies = scanAssemblies.Where(ass => !Settings.ExcludeAssemblies.Contains(ass.GetName().Name, StringComparer.OrdinalIgnoreCase));
        }

        return (scanAssemblies, externalAssemblies, pathOfExternalAssemblies);
    }
}

/// <summary>
/// 内部反射静态类
/// </summary>
internal static class Reflect
{
    /// <summary>
    /// 获取入口程序集
    /// </summary>
    /// <returns></returns>
    internal static Assembly GetEntryAssembly()
    {
        return Assembly.GetEntryAssembly();
    }

    /// <summary>
    /// 根据程序集名称获取运行时程序集
    /// </summary>
    /// <param name="assemblyName"></param>
    /// <returns></returns>
    internal static Assembly GetAssembly(string assemblyName)
    {
        try
        {
            // 加载程序集
            return AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(assemblyName));
        }
        catch
        {
            // 记录异常
            Console.WriteLine($"Failed to load assembly '{assemblyName}'");
            return null;
        }
    }

    /// <summary>
    /// 根据路径加载程序集
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    internal static Assembly LoadAssembly(string path)
    {
        if (!File.Exists(path)) return default;
        return Assembly.LoadFrom(path);
    }

    /// <summary>
    /// 通过流加载程序集
    /// </summary>
    /// <param name="assembly"></param>
    /// <returns></returns>
    internal static Assembly LoadAssembly(MemoryStream assembly)
    {
        return Assembly.Load(assembly.ToArray());
    }

    /// <summary>
    /// 根据程序集名称、类型完整限定名获取运行时类型
    /// </summary>
    /// <param name="assemblyName"></param>
    /// <param name="typeFullName"></param>
    /// <returns></returns>
    internal static Type GetType(string assemblyName, string typeFullName)
    {
        return GetAssembly(assemblyName).GetType(typeFullName);
    }

    /// <summary>
    /// 根据程序集和类型完全限定名获取运行时类型
    /// </summary>
    /// <param name="assembly"></param>
    /// <param name="typeFullName"></param>
    /// <returns></returns>
    internal static Type GetType(Assembly assembly, string typeFullName)
    {
        return assembly.GetType(typeFullName);
    }

    /// <summary>
    /// 根据程序集和类型完全限定名获取运行时类型
    /// </summary>
    /// <param name="assembly"></param>
    /// <param name="typeFullName"></param>
    /// <returns></returns>
    internal static Type GetType(MemoryStream assembly, string typeFullName)
    {
        return LoadAssembly(assembly).GetType(typeFullName);
    }

    /// <summary>
    /// 获取程序集名称
    /// </summary>
    /// <param name="assembly"></param>
    /// <returns></returns>
    internal static string GetAssemblyName(Assembly assembly)
    {
        return assembly.GetName().Name;
    }

    /// <summary>
    /// 获取程序集名称
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    internal static string GetAssemblyName(Type type)
    {
        return GetAssemblyName(type.GetTypeInfo());
    }

    /// <summary>
    /// 获取程序集名称
    /// </summary>
    /// <param name="typeInfo"></param>
    /// <returns></returns>
    internal static string GetAssemblyName(TypeInfo typeInfo)
    {
        return GetAssemblyName(typeInfo.Assembly);
    }

    /// <summary>
    /// 加载程序集类型，支持格式：程序集;完全限定的类型名称
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    internal static Type GetStringType(string str)
    {
        string[] typeDefinitions = str.Split(';');

        // 类型格式必须以分号作为分隔程序集名称和完全限定的类型名称
        if (typeDefinitions.Length != 2)
        {
            throw new InvalidOperationException("The type format must use a semicolon as the separator between the assembly name and the fully qualified type name.");
        }

        return GetType(typeDefinitions[0], typeDefinitions[1]);
    }
}

/// <summary>
/// 解决单文件发布程序集扫描问题
/// </summary>
public interface ISingleFilePublish
{
    /// <summary>
    /// 包含程序集数组
    /// </summary>
    /// <remarks>配置单文件发布扫描程序集</remarks>
    /// <returns></returns>
    Assembly[] IncludeAssemblies();

    /// <summary>
    /// 包含程序集名称数组
    /// </summary>
    /// <remarks>配置单文件发布扫描程序集名称</remarks>
    /// <returns></returns>
    string[] IncludeAssemblyNames();
}