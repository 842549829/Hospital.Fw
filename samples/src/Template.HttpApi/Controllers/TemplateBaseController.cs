using Hospital.Fw.Domain.Shared.Core.Autofac;
using Microsoft.AspNetCore.Mvc;

namespace Template.HttpApi.Controllers;

public abstract class TemplateBaseController : ControllerBase
{
    [Autowired]
    public required IServiceProvider ServiceProvider { get; set; }
}