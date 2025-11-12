using System.Reflection;
using System.Text;
using System.Xml.Linq;


namespace Hospital.Fw.OpenApiToTsClient;

/// <summary>
/// C# 类型转 TypeScript
/// </summary>
public static class CSharpToTypeScriptExtension
{
    /// <summary>
    /// 基本类型映射
    /// </summary>
    private static readonly Dictionary<Type, string> TypeMap = new()
    {
        { typeof(int), "number" },
        { typeof(long), "number" },
        { typeof(short), "number" },
        { typeof(byte), "number" },
        { typeof(sbyte), "number" },
        { typeof(uint), "number" },
        { typeof(ulong), "number" },
        { typeof(ushort), "number" },
        { typeof(float), "number" },
        { typeof(double), "number" },
        { typeof(decimal), "number" },
        { typeof(bool), "boolean" },
        { typeof(string), "string" },
        { typeof(char), "string" },
        { typeof(Guid), "string" },
        { typeof(object), "any" },
        { typeof(DateTime), "Date" },
        { typeof(DateTimeOffset), "Date" },
        { typeof(TimeSpan), "string" } // 或 number，按需调整
    };

    /// <summary>
    /// 缓存：程序集名称 → (成员ID → 格式化注释)
    /// </summary>
    private static readonly Dictionary<string, Dictionary<string, string>> _assemblyCommentsCache = new();

    /// <summary>
    /// 锁对象
    /// </summary>
    private static readonly object _lock = new();

    /// <summary>
    /// 获取指定类型的程序集对应的 XML 注释字典（线程安全、自动缓存）
    /// </summary>
    private static Dictionary<string, string> GetCommentsForAssembly(Type type)
    {
        Assembly assembly = type.Assembly;
        string? assemblyName = assembly.GetName().Name;
        if (assemblyName == null)
        {
            throw new InvalidOperationException(nameof(assemblyName));
        }

        lock (_lock)
        {
            if (_assemblyCommentsCache.TryGetValue(assemblyName, out Dictionary<string, string>? commentDict))
            {
                return commentDict;
            }

            commentDict = LoadXmlDocumentation(assembly);
            _assemblyCommentsCache[assemblyName] = commentDict;
            return commentDict;
        }
    }

    /// <summary>
    /// 从指定程序集加载 XML 文档注释
    /// </summary>
    private static Dictionary<string, string> LoadXmlDocumentation(Assembly assembly)
    {
        Dictionary<string, string> commentDict = new Dictionary<string, string>();
        string assemblyLocation = assembly.Location;

        if (string.IsNullOrEmpty(assemblyLocation))
        {
            Console.WriteLine($"⚠️ 无法确定程序集位置: {assembly.FullName}");
            return commentDict;
        }

        string? assemblyDir = Path.GetDirectoryName(assemblyLocation);
        if (assemblyDir == null)
        {
            Console.WriteLine($"⚠️ 获取程序集位置失败: {assembly.FullName}");
            return commentDict;
        }

        string xmlPath = Path.Combine(assemblyDir, $"{assembly.GetName().Name}.xml");

        if (!File.Exists(xmlPath))
        {
            Console.WriteLine($"📄 未找到 XML 文档文件: {xmlPath}");
            return commentDict;
        }

        try
        {
            XDocument xmlDoc = XDocument.Load(xmlPath);
            Console.WriteLine($"📄 已加载 XML 文档: {xmlPath}");

            XNamespace ns = xmlDoc.Root?.Name.Namespace ?? XNamespace.None;

            IEnumerable<XElement>? members = xmlDoc
                .Root?
                .Element(ns + "members")?
                .Elements(ns + "member");

            if (members == null) return commentDict;

            foreach (XElement member in members)
            {
                string? nameAttr = member.Attribute("name")?.Value;
                if (string.IsNullOrEmpty(nameAttr)) continue;

                string? summary = member.Element(ns + "summary")?.Value.Trim();
                if (string.IsNullOrEmpty(summary)) continue;

                string formatted = summary
                    .Replace("\r\n", "\n")
                    .Replace("\n", "\n * ")
                    .Trim();

                commentDict[nameAttr] = formatted;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"⚠️ 加载 XML 文档失败 ({xmlPath}): {ex.Message}");
        }

        return commentDict;
    }

    /// <summary>
    /// 获取成员的 summary 注释
    /// </summary>
    private static string GetSummaryComment(string memberId, Type type)
    {
        Dictionary<string, string> comments = GetCommentsForAssembly(type);
        return comments.TryGetValue(memberId, out string? comment) ? comment : null;
    }

    /// <summary>
    /// 添加 JSDoc 注释到 StringBuilder
    /// </summary>
    private static void AppendDocComment(StringBuilder sb, string comment, string indent = "")
    {
        if (string.IsNullOrWhiteSpace(comment)) return;

        sb.AppendLine($"{indent}/**");
        sb.AppendLine($"{indent} * {comment}");
        sb.AppendLine($"{indent} */");
    }

    /// <summary>
    /// 将指定 C# 类型转换为 TypeScript 接口并写入文件（如果文件已存在则覆盖）
    /// </summary>
    /// <param name="types">要转换的 C# 类型</param>
    /// <param name="outputDirectory">输出目录路径（例如 @"C:\MyTsFiles"）</param>
    /// <param name="includeNested">是否递归生成嵌套/依赖的复杂类型</param>
    public static void ConvertTypeToTypeScriptFileOverwrite(Type[] types, string outputDirectory, bool includeNested = false)
    {
        foreach (Type type in types)
        {
            if (string.IsNullOrWhiteSpace(outputDirectory))
            {
                throw new ArgumentException("输出目录不能为空。", nameof(outputDirectory));
            }

            Directory.CreateDirectory(outputDirectory); // 确保目录存在
            Dictionary<string, string> tsContent = ConvertTypeToTypeScript(type, includeNested);
            foreach ((string? key, string? value) in tsContent)
            {
                string filePath = Path.Combine(outputDirectory, key);
                File.WriteAllText(filePath, value, Encoding.UTF8);
                Console.WriteLine($"✅ 已生成（或覆盖）TypeScript 文件: {filePath}");
            }
        }
    }

    /// <summary>
    /// 返回 Dictionary(文件名, TypeScript 内容)
    /// </summary>
    /// <param name="rootType">类型</param>
    /// <param name="includeNested">包括嵌套</param>
    /// <returns>结果</returns>
    public static Dictionary<string, string> ConvertTypeToTypeScript(Type rootType, bool includeNested = false)
    {
        HashSet<Type> allTypes = new HashSet<Type>();
        CollectTypes(rootType, allTypes, includeNested);

        Dictionary<string, string> result = new Dictionary<string, string>();

        foreach (Type type in allTypes)
        {
            StringBuilder sb = new StringBuilder();
            HashSet<string> processed = new HashSet<string>();
            GenerateTypeScriptForType(type, sb, processed, includeNested, allTypes);
            string fileName = $"{GetTypeName(type)}.ts";
            result[fileName] = sb.ToString();
        }

        return result;
    }

    /// <summary>
    /// 递归收集所有需要生成的类型（包括接口和属性类型）
    /// </summary>
    /// <param name="type">类型</param>
    /// <param name="collected">collected</param>
    /// <param name="includeNested">includeNested</param>
    private static void CollectTypes(Type type, HashSet<Type> collected, bool includeNested)
    {
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            type = Nullable.GetUnderlyingType(type);
        }

        if (type == null)
        {
            return;
        }

        // ✅ 枚举也要收集（如果 includeNested 或本身就是根类型）
        if (type.IsEnum)
        {
            collected.Add(type);
            return;
        }


        if (!IsComplexType(type))
        {
            return;
        }

        if (!collected.Add(type))
        {
            return;
        }

        if (!includeNested)
        {
            return;
        }

        // 接口
        foreach (Type iface in GetDirectInterfaces(type))
        {
            if (IsComplexType(iface))
            {
                CollectTypes(iface, collected, true);
            }
        }

        // 属性类型
        foreach (PropertyInfo prop in GetDeclaredProperties(type))
        {
            Type propType = prop.PropertyType;
            if (IsComplexType(propType))
            {
                CollectTypes(propType, collected, true);
            }
        }
    }

    /// <summary>
    /// 获取类型所有公共属性（类：仅 DeclaredOnly；接口：全部）
    /// </summary>
    /// <param name="type">类型</param>
    /// <returns>PropertyInfo</returns>
    private static PropertyInfo[] GetDeclaredProperties(Type type)
    {
        if (type.IsInterface)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }

        const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;
        return type.GetProperties(flags);
    }

    /// <summary>
    /// 获取类型直接继承的接口
    /// </summary>
    /// <param name="type">类型</param>
    /// <returns>Type[]</returns>
    private static Type[] GetDirectInterfaces(Type type)
    {
        // 仅返回直接继承的接口（排除 System.*）
        return type.GetInterfaces()
            .Where(i => i.FullName != null && !i.FullName.StartsWith("System."))
            .ToArray();
    }

    /// <summary>
    /// 生成单个类型的 TypeScript 接口
    /// </summary>
    /// <param name="type">类型</param>
    /// <param name="sb">字符串</param>
    /// <param name="processedTypes">processedTypes</param>
    /// <param name="includeNested">includeNested</param>
    /// <param name="allTypes">allTypes</param>
    /// <summary>
    /// 生成单个类型的 TypeScript 接口或枚举
    /// </summary>
    /// <summary>
    /// 生成单个类型的 TypeScript 接口或枚举
    /// </summary>
    private static void GenerateTypeScriptForType(
        Type type,
        StringBuilder sb,
        HashSet<string> processedTypes,
        bool includeNested,
        HashSet<Type> allTypes = null)
    {
        // 处理 Nullable<T>
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            type = Nullable.GetUnderlyingType(type);
        }

        if (type == null)
            return;

        if (!IsComplexType(type))
            return;

        string typeName = GetTypeName(type);
        if (!processedTypes.Add(typeName))
            return;

        // ✅ 枚举：直接生成
        if (type.IsEnum)
        {
            string enumComment = GetSummaryComment($"T:{type.FullName}", type);
            AppendDocComment(sb, enumComment);

            sb.AppendLine($"export enum {type.Name} {{");

            string[] names = Enum.GetNames(type);
            Type underlyingType = Enum.GetUnderlyingType(type);

            for (int i = 0; i < names.Length; i++)
            {
                string name = names[i];
                object value = Enum.Parse(type, name);
                object rawValue = Convert.ChangeType(value, underlyingType);

                string fieldComment = GetSummaryComment($"F:{type.FullName}.{name}", type);
                AppendDocComment(sb, fieldComment, "  ");

                string suffix = i == names.Length - 1 ? "" : ",";
                sb.AppendLine($"  {ToCamelCase(name)} = {rawValue}{suffix}");

                if (i < names.Length - 1)
                {
                    sb.AppendLine();
                }
            }

            sb.AppendLine("}");
            sb.AppendLine();
            return;
        }

        // 递归处理嵌套类型（仅当未提供 allTypes 时）
        if (includeNested && allTypes == null)
        {
            foreach (Type iface in GetDirectInterfaces(type))
            {
                if (IsComplexType(iface) && !processedTypes.Contains(GetTypeName(iface)))
                {
                    GenerateTypeScriptForType(iface, sb, processedTypes, true);
                }
            }

            foreach (PropertyInfo prop in GetDeclaredProperties(type))
            {
                Type propType = prop.PropertyType;
                if (IsComplexType(propType) && !processedTypes.Contains(GetTypeName(propType)))
                {
                    GenerateTypeScriptForType(propType, sb, processedTypes, true);
                }
            }
        }

        // ====== 收集所有需要导入的依赖类型（接口 + 枚举） ======
        HashSet<string> importsToGenerate = new HashSet<string>(); // 用 HashSet 防重复

        // 1. 直接实现的接口
        Type[] directInterfaces = GetDirectInterfaces(type);
        foreach (Type iface in directInterfaces)
        {
            if (allTypes?.Contains(iface) == true)
            {
                string name = GetTypeName(iface);
                importsToGenerate.Add($"import {{ {name} }} from './{name}.ts';");
            }
        }

        // 2. 属性中用到的复杂类型（包括枚举！）
        PropertyInfo[] declaredProps = GetDeclaredProperties(type);
        foreach (PropertyInfo prop in declaredProps)
        {
            Type? propType = prop.PropertyType;

            // 解包 Nullable<T>
            if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                propType = Nullable.GetUnderlyingType(propType);
            }

            // 如果是复杂类型（含枚举）且在 allTypes 中
            if (propType != null && IsComplexType(propType) && allTypes?.Contains(propType) == true)
            {
                string name = GetTypeName(propType);
                importsToGenerate.Add($"import {{ {name} }} from './{name}.ts';");
            }
        }

        // 输出 import 语句
        if (importsToGenerate.Count > 0)
        {
            foreach (string? importLine in importsToGenerate.OrderBy(x => x)) // 可选：排序保持一致
            {
                sb.AppendLine(importLine);
            }
            sb.AppendLine(); // import 后空一行
        }

        // 构建 extends 子句
        string extendsClause = "";
        if (directInterfaces.Length > 0)
        {
            string ifaceNames = string.Join(", ", directInterfaces.Select(GetTypeName));
            extendsClause = $" extends {ifaceNames}";
        }

        // 生成 interface 注释和声明
        string classComment = GetSummaryComment($"T:{type.FullName}", type);
        AppendDocComment(sb, classComment);
        sb.AppendLine($"export interface {typeName}{extendsClause} {{");

        // 过滤继承的属性（避免重复）
        HashSet<string> inheritedPropertyNames = new HashSet<string>();
        if (type.IsClass)
        {
            foreach (Type iface in type.GetInterfaces())
            {
                foreach (PropertyInfo prop in iface.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                    inheritedPropertyNames.Add(prop.Name);
            }
        }
        else if (type.IsInterface)
        {
            foreach (Type iface in type.GetInterfaces())
            {
                foreach (PropertyInfo prop in iface.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                    inheritedPropertyNames.Add(prop.Name);
            }
        }

        PropertyInfo[] propsToGenerate = declaredProps.Where(p => !inheritedPropertyNames.Contains(p.Name)).ToArray();

        for (int i = 0; i < propsToGenerate.Length; i++)
        {
            if (i > 0) sb.AppendLine();

            PropertyInfo prop = propsToGenerate[i];
            if (prop.DeclaringType != null)
            {
                string propComment = GetSummaryComment($"P:{prop.DeclaringType.FullName}.{prop.Name}", type);
                AppendDocComment(sb, propComment, "  ");
            }

            string camelName = ToCamelCase(prop.Name);
            bool isNullable = IsPropertyNullable(prop);
            string tsType = GetScriptTypeCore(prop.PropertyType);
            string optionalMark = isNullable ? "?" : "";

            sb.AppendLine($"  {camelName}{optionalMark}: {tsType}");
        }

        sb.AppendLine("}");
        sb.AppendLine();
    }

    /// <summary>
    /// 判断属性是否可空
    /// </summary>
    /// <param name="prop">PropertyInfo</param>
    /// <returns>结果</returns>
    private static bool IsPropertyNullable(PropertyInfo prop)
    {
        Type type = prop.PropertyType;

        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            return true;

        if (type.IsValueType)
            return false;

        return true;
    }

    /// <summary>
    /// 获取脚本类型
    /// </summary>
    /// <param name="type">类型</param>
    /// <returns>结果</returns>
    private static string GetScriptTypeCore(Type type)
    {
        if (type.IsArray)
        {
            string elemType = GetScriptTypeCore(type.GetElementType());
            return $"Array<{elemType}>";
        }

        if (type.IsGenericType)
        {
            Type genericTypeDef = type.GetGenericTypeDefinition();
            Type[] args = type.GetGenericArguments();

            if (genericTypeDef == typeof(List<>) ||
                typeof(IEnumerable<>).IsAssignableFrom(genericTypeDef) ||
                genericTypeDef == typeof(IList<>) ||
                genericTypeDef == typeof(ICollection<>) ||
                genericTypeDef == typeof(HashSet<>) ||
                genericTypeDef == typeof(IReadOnlyCollection<>) ||
                genericTypeDef == typeof(IReadOnlyList<>))
            {
                string elemType = GetScriptTypeCore(args[0]);
                return $"Array<{elemType}>";
            }

            if (genericTypeDef == typeof(Nullable<>))
            {
                return GetScriptTypeCore(args[0]);
            }
        }

        // ✅ 枚举：直接返回类型名（如 FeeStatus）
        if (type.IsEnum)
        {
            return type.Name;
        }

        if (TypeMap.TryGetValue(type, out string? tsType))
        {
            return tsType;
        }

        return IsComplexType(type) ? GetTypeName(type) : "any";
    }

    /// <summary>
    /// 获取类型名称
    /// </summary>
    /// <param name="type">类型</param>
    /// <returns>结果</returns>
    private static string GetTypeName(Type type)
    {
        if (type.IsGenericType)
        {
            string name = type.Name.Split('`')[0];
            string args = string.Join(", ", type.GetGenericArguments().Select(GetTypeName));
            return $"{name}<{args}>";
        }
        return type.Name;
    }

    /// <summary>
    /// 转换为驼峰命名
    /// </summary>
    /// <param name="name">名称</param>
    /// <returns>结果</returns>
    private static string ToCamelCase(string name)
    {
        if (string.IsNullOrEmpty(name) || char.IsLower(name[0]))
            return name;

        if (name.Length == 1)
            return name.ToLowerInvariant();

        return char.ToLowerInvariant(name[0]) + name[1..];
    }

    /// <summary>
    /// 判断是否为复杂类型
    /// </summary>
    /// <param name="type">类型</param>
    /// <returns>结果</returns>
    private static bool IsComplexType(Type type)
    {
        if (type == null)
        {
            return false;
        }

        // ✅ 枚举不是复杂类型
        if (type.IsEnum)
        {
            return true;
        }

        if (type.IsPrimitive)
        {
            return false;
        }

        if (type == typeof(string) ||
            type == typeof(decimal) ||
            type == typeof(DateTime) ||
            type == typeof(DateTimeOffset) ||
            type == typeof(Guid) ||
            type == typeof(TimeSpan) ||
            type == typeof(byte[]))
        {
            return false;
        }

        if (type.IsValueType && !type.IsGenericType)
        {
            return false;
        }

        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            return IsComplexType(Nullable.GetUnderlyingType(type)!);
        }

        if (type.IsArray)
        {
            return false;
        }

        if (type.IsGenericType)
        {
            Type genericDef = type.GetGenericTypeDefinition();
            Type[] collectionTypes = new[]
            {
                typeof(IEnumerable<>),
                typeof(ICollection<>),
                typeof(IList<>),
                typeof(List<>),
                typeof(HashSet<>),
                typeof(IReadOnlyCollection<>),
                typeof(IReadOnlyList<>)
            };

            if (collectionTypes.Any(c => c.IsAssignableFrom(genericDef)))
            {
                return false;
            }
        }

        if (type.IsInterface)
        {
            return true;
        }

        if (!type.IsValueType && type != typeof(object))
        {
            return true;
        }

        return false;
    }
    
    /// <summary>
    /// (测试)
    /// </summary>
    public static void ConvertTypeToTypeScriptTest()
    {
        // 获取所在类的程序集
        var assembly = typeof(CSharpToTypeScriptExtension).Assembly;
        var targetTypes = assembly
            .GetTypes()
            .Where(t =>
                t.IsClass &&
                !t.IsAbstract &&
                (t.Name.EndsWith("Dto", StringComparison.Ordinal) ||
                 t.Name.EndsWith("Input", StringComparison.Ordinal))
            )
            .ToArray();
        var path = Path.Combine(AppContext.BaseDirectory, "ts");
        CSharpToTypeScriptExtension.ConvertTypeToTypeScriptFileOverwrite(
            targetTypes, path, true);
    }
}