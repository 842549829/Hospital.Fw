using Hospital.Fw.BackgroundJobs.Abstractions;
using Hospital.Fw.Test.Jobs.Eto;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Fw.Test.Controllers;

/// <summary>
/// 用户任务
/// </summary>
/// <param name="backgroundJobManager">backgroundJobManager</param>
[ApiController]
[Route("/api/task/user")]
public class TaskUserController(IBackgroundJobManager backgroundJobManager) : ControllerBase
{
    [HttpPost("init")]
    public async Task<bool> InitAsync([FromBody] JobTaskEto<UserInitEto> input)
    {
        var priority = input.Priority ?? BackgroundJobPriority.Normal;
        await backgroundJobManager.EnqueueAsync(input.JobArgs, priority, input.NextTryTime);
        return true;
    }

    [HttpPost("create")]
    public async Task<bool> CreateAsync([FromBody] JobTaskEto<UserCreateEto> input)
    {
        var priority = input.Priority ?? BackgroundJobPriority.Normal;
        await backgroundJobManager.EnqueueAsync(input.JobArgs, priority, input.NextTryTime);
        return true;
    }

    [HttpPost("update")]
    public async Task<bool> UpdateAsync([FromBody] JobTaskEto<UserUpdateEto> input)
    {
        var priority = input.Priority ?? BackgroundJobPriority.Normal;
        await backgroundJobManager.EnqueueAsync(input.JobArgs, priority, input.NextTryTime);
        return true;
    }

    [HttpPost("delete")]
    public async Task<bool> DeleteAsync([FromBody] JobTaskEto<UserDeleteEto> input)
    {
        var priority = input.Priority ?? BackgroundJobPriority.Normal;
        await backgroundJobManager.EnqueueAsync(input.JobArgs, priority, input.NextTryTime);
        return true;
    }
}