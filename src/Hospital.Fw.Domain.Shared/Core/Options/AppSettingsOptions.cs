namespace Hospital.Fw.Domain.Shared.Core.Options;

/// <summary>
/// 配置信息
/// </summary>
public class AppSettingsOptions
{
    /// <summary>
    /// 配置支持的包前缀名
    /// </summary>
    public List<string> SupportPackageNamePrefixs { get; set; } = [];

    /// <summary>
    /// 是否启用引用程序集扫描
    /// </summary>
    public bool? EnabledReferenceAssemblyScan { get; set; }

    /// <summary>
    /// 外部程序集
    /// </summary>
    /// <remarks>扫描 dll 文件，如果是单文件发布，需拷贝放在根目录下</remarks>
    public List<string> ExternalAssemblies { get; set; } = [];

    /// <summary>
    /// 排除扫描的程序集
    /// </summary>
    public List<string> ExcludeAssemblies { get; set; } = [];
}