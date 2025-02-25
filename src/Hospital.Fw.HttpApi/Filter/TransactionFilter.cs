using Microsoft.AspNetCore.Mvc.Filters;
using SqlSugar;

namespace Hospital.Fw.HttpApi.Filter;

/// <summary>
/// 事务过滤器
/// </summary>
public class TransactionFilter : IActionFilter
{
    /// <summary>
    /// 在执行操作方法之前执行
    /// </summary>
    /// <param name="context">context</param>
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var db = context.HttpContext.RequestServices.GetRequiredService<ISqlSugarClient>();

        db.AsTenant().BeginTran();
    }

    /// <summary>
    /// 在执行操作方法之后执行
    /// </summary>
    /// <param name="context">context</param>
    public void OnActionExecuted(ActionExecutedContext context)
    {
        var db = context.HttpContext.RequestServices.GetRequiredService<ISqlSugarClient>();
        if (context.Exception == null)
        {
            db.AsTenant().CommitTran();
        }
        else
        {
            db.AsTenant().RollbackTran();
        }
    }
}