namespace Template.Application.Contract.Jobs.Eto;

public class BaseInfoChangeEto
{
    /// <summary>
    /// guid
    /// 全小写 32
    /// </summary>
    public string RequestId { get; set; } = default!;

    /// <summary>
    /// 数据表名  数据库表名，全大写
    /// </summary>
    public string TableName { get; set; } = default!;

    /// <summary>
    /// 操作类型
    /// </summary>
    public ActionType ActionType { get; set; }

    /// <summary>
    /// 业务类型
    /// </summary>
    public BusinessType BusinessType { get; set; }

    /// <summary>
    /// 业务id
    /// </summary>
    public string BusinessId { get; set; } = default!;

    /// <summary>
    /// 操作时间
    /// </summary>
    public DateTime? BusinessDate { get; set; }

    /// <summary>
    /// 操作人ID
    /// </summary>
    public string? BusinessUser { get; set; }

    /// <summary>
    /// 操作人Name
    /// </summary>
    public string? BusinessName { get; set; }
}

//数据表操作类型
public enum ActionType
{
    Add = 0,//新增
    Update = 1,//修改
    Delete = 2//删除
}

/// <summary>
/// 业务类型
/// </summary>
public enum BusinessType
{
    PatientInfo = 0,//患者基础信息
    UserInfo = 1,//系统用户信息
    SecOffice = 2,//科室信息
    ClinicRegister = 3,//门诊挂号
    EmergencyRegister = 4,//急诊挂号
    ClinicDiagnose = 5,//门诊诊断
    ClinicRecipe = 6,//门诊处方
    ClinicEmr = 7,//门诊病历
    PacsReport = 8,//检查报告
    LisReport = 9,//检验报告
    InfectiousDiseaseReporting = 10,//传染病上报卡
    InpRegister = 11,//患者入院
    InpOut = 12,//患者出院
    InpAdvice = 13,//医生医嘱
    InpDiagnose = 14,//住院诊断
    CaseRecord = 15,//病案首页
    InpEmrFirstRecord = 16,//住院首次病程
    InpEmrOtherRecord = 17,//住院日常病程
    InpEmrInRecord = 18,//入院记录
    InpEmrOutRecord = 19,//出院记录
    InpOutSecOffice = 20//患者出科==出院记录
}