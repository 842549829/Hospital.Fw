using Hospital.Fw.BackgroundJobs.Abstractions;

namespace Hospital.Fw.Test.Jobs.Eto;

/// <summary>
/// 任务实体
/// </summary>
public class JobTaskEto<T>
{
    /// <summary>
    /// 下次开始执行时间
    /// </summary>
    public DateTime? NextTryTime { get; set; }

    /// <summary>
    /// 优先级(值越大的优先级越高 默认15)
    /// </summary>
    public BackgroundJobPriority? Priority { get; set; }

    /// <summary>
    ///业务参数(必传以任务的方式传递)
    /// </summary>
    public required T JobArgs { get; set; }
}

/// <summary>
/// 起初参数
/// </summary>
public class OriginallyEto
{
    /// <summary>
    /// 起初开始时间
    /// </summary>
    public DateTime? StartTime { get; set; }
}

/// <summary>
/// 用户起初
/// </summary>
public class UserInitEto : OriginallyEto;

/// <summary>
/// 用户创建
/// </summary>
public class UserCreateEto : BaseInfoChangeEto;

/// <summary>
/// 用户修改
/// </summary>
public class UserUpdateEto : BaseInfoChangeEto;

/// <summary>
/// 用户删除
/// </summary>
public class UserDeleteEto : BaseInfoChangeEto;

/// <summary>
/// 部门起初
/// </summary>
public class DeptInitEto : OriginallyEto;

/// <summary>
/// 部门创建
/// </summary>
public class DeptCreateEto : BaseInfoChangeEto;

/// <summary>
/// 部门修改
/// </summary>
public class DeptUpdateEto : BaseInfoChangeEto;

/// <summary>
/// 部门删除
/// </summary>
public class DeptDeleteEto : BaseInfoChangeEto;

/// <summary>
/// 患者起初 
/// </summary>
public class PatientInitEto : OriginallyEto;

/// <summary>
/// 患者创建
/// </summary>
public class PatientCreateEto : BaseInfoChangeEto;

/// <summary>
/// 患者修改 
/// </summary>
public class PatientUpdateEto : BaseInfoChangeEto;

/// <summary>
/// 患者删除
/// </summary>
public class PatientDeleteEto : BaseInfoChangeEto;