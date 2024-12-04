//using Hospital.Fw.BackgroundJobs.Abstractions;

//namespace Hospital.Fw.BackgroundJobs.Implementations;

//public class BackgroundJobStore(ISqlSugarClient dbContext, IMapper mapper) : IBackgroundJobStore
//{
//    /// <summary>
//    /// Gets a BackgroundJobInfo based on the given jobId.
//    /// </summary>
//    /// <param name="jobId">The Job Unique Identifier.</param>
//    /// <returns>The BackgroundJobInfo object.</returns>
//    public async Task<BackgroundJobInfo> FindAsync(string jobId)
//    {
//        var db = GetSqlSugarScopeProvider();
//        var docTask = await db.Queryable<JobTask>().FirstAsync(a => a.Id == jobId);
//        return ToBackgroundJobInfo(docTask);
//    }

//    /// <summary>
//    /// Inserts a background job.
//    /// </summary>
//    /// <param name="jobInfo">Job information.</param>
//    public async Task InsertAsync(BackgroundJobInfo jobInfo)
//    {
//        var db = GetSqlSugarScopeProvider();
//        var docTask = ToDocTask(jobInfo);
//        var result = await db.Insertable(docTask).ExecuteCommandAsync();
//        if (result != 1)
//        {
//            throw new Exception("Insert failed.");
//        }
//    }

//    /// <summary>
//    /// Gets waiting jobs. It should get jobs based on these:
//    /// Conditions: !IsAbandoned And NextTryTime &lt;= Clock.Now.
//    /// Order by: Priority DESC, TryCount ASC, NextTryTime ASC.
//    /// Maximum result: <paramref name="maxResultCount"/>.
//    /// </summary>
//    /// <param name="maxResultCount">Maximum result count.</param>
//    public async Task<List<BackgroundJobInfo>> GetWaitingJobsAsync(int maxResultCount)
//    {
//        var db = GetSqlSugarScopeProvider();
//        var now = DateTime.Now;
//        var docTask = await db.Queryable<JobTask>()
//            .Where(t => !t.IsAbandoned && t.NextTryTime <= now)
//            .OrderByDescending(t => t.Priority)
//            .OrderBy(t => t.TryCount)
//            .OrderBy(t => t.NextTryTime)
//            .Take(maxResultCount)
//            .ToListAsync();
//        var jobInfos = docTask.Select(ToBackgroundJobInfo);
//        return jobInfos.ToList();
//    }

//    /// <summary>
//    /// Deletes a job.
//    /// </summary>
//    /// <param name="jobId">The Job Unique Identifier.</param>
//    public async Task DeleteAsync(string jobId)
//    {
//        var db = GetSqlSugarScopeProvider();
//        var docTask = await db.Queryable<JobTask>().FirstAsync(a => a.Id == jobId);
//        if (docTask != null)
//        {
//            await db.Deleteable(docTask).ExecuteCommandAsync();
//        }
//    }

//    /// <summary>
//    /// Updates a job.
//    /// </summary>
//    /// <param name="jobInfo">Job information.</param>
//    public async Task UpdateAsync(BackgroundJobInfo jobInfo)
//    {
//        var db = GetSqlSugarScopeProvider();
//        var docTask = await db.Queryable<JobTask>().FirstAsync(a => a.Id == jobInfo.Id);
//        if (docTask != null)
//        {
//            UpdateDocTask(docTask, jobInfo);
//            await db.Updateable(docTask).ExecuteCommandAsync();
//        }
//    }

//    public BackgroundJobInfo ToBackgroundJobInfo(JobTask docTask)
//    {
//        return mapper.Map<BackgroundJobInfo>(docTask);
//    }

//    public JobTask ToDocTask(BackgroundJobInfo backgroundJobInfo)
//    {
//        return mapper.Map<JobTask>(backgroundJobInfo);
//    }

//    public static void UpdateDocTask(JobTask docTask, BackgroundJobInfo backgroundJobInfo)
//    {
//        docTask.CreationTime = backgroundJobInfo.CreationTime;
//        docTask.IsAbandoned = backgroundJobInfo.IsAbandoned;
//        docTask.JobArgs = backgroundJobInfo.JobArgs;
//        docTask.JobName = backgroundJobInfo.JobName;
//        docTask.LastTryTime = backgroundJobInfo.LastTryTime;
//        docTask.NextTryTime = backgroundJobInfo.NextTryTime;
//        docTask.Priority = backgroundJobInfo.Priority;
//        docTask.TryCount = backgroundJobInfo.TryCount;
//    }

//    private SqlSugarScopeProvider GetSqlSugarScopeProvider()
//    {
//        return dbContext.AsTenant().GetConnectionScope(SqlSugarCoreDbConst.Job);
//    }
//}