using Hospital.Fw.Domain.Shared.Core.Autofac;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Fw.HttpApi.Controllers;

/// <summary>
/// 基础控制器
/// </summary>
public abstract class BaseController : ControllerBase
{
    /// <summary>
    /// 服务提供者
    /// </summary>
    [Autowired]
    public required IServiceProvider ServiceProvider { get; set; }
}