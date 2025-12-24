using Hospital.Fw.Domain.Shared.Core.Exception;
using SqlSugar;

namespace Hospital.Fw.Domain;

/// <summary>
/// SqlSugar扩展
/// </summary>
public static class SqlSugarExtension
{
    /// <param name="deleteObj">删除对象</param>
    /// <typeparam name="T">删除对象类型</typeparam>
    extension<T>(IDeleteable<T> deleteObj) where T : class, new()
    {
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="errorMessage">错误信息</param>
        /// <returns>影响行数</returns>
        public async Task ExecuteCommandAsync(string errorMessage)
        {
            var actualCount = await deleteObj.ExecuteCommandAsync();
            EnsureExpectedRowCount(actualCount, errorMessage);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="expectedCount">期望行数</param>
        /// <param name="errorMessage">错误信息</param>
        /// <returns>影响行数</returns>
        public async Task ExecuteCommandAsync(int expectedCount = 1,
            string errorMessage = "实际操作影响行数与期望影响行数不一致")
        {
            var actualCount = await deleteObj.ExecuteCommandAsync();
            EnsureExpectedRowCount(actualCount, expectedCount, errorMessage);
        }
    }

    /// <param name="updateable">更新对象</param>
    /// <typeparam name="T">更新对象类型</typeparam>
    extension<T>(IUpdateable<T> updateable) where T : class, new()
    {
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="errorMessage">错误信息</param>
        /// <returns>影响行数</returns>
        public async Task ExecuteCommandAsync(string errorMessage)
        {
            var actualCount = await updateable.ExecuteCommandAsync();
            EnsureExpectedRowCount(actualCount, errorMessage);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="expectedCount">期望行数</param>
        /// <param name="errorMessage">错误信息</param>
        /// <returns>影响行数</returns>
        public async Task ExecuteCommandAsync(int expectedCount = 1,
            string errorMessage = "实际操作影响行数与期望影响行数不一致")
        {
            var actualCount = await updateable.ExecuteCommandAsync();
            EnsureExpectedRowCount(actualCount, expectedCount, errorMessage);
        }
    }

    /// <param name="insertable">插入对象</param>
    /// <typeparam name="T">插入对象类型</typeparam>
    extension<T>(IInsertable<T> insertable) where T : class, new()
    {
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="errorMessage">错误信息</param>
        /// <returns>影响行数</returns>
        public async Task ExecuteCommandAsync(string errorMessage)
        {
            var actualCount = await insertable.ExecuteCommandAsync();
            EnsureExpectedRowCount(actualCount, errorMessage);
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="expectedCount">期望行数</param>
        /// <param name="errorMessage">错误信息</param>
        /// <returns>影响行数</returns>
        public async Task ExecuteCommandAsync(int expectedCount = 1,
            string errorMessage = "实际操作影响行数与期望影响行数不一致")
        {
            var actualCount = await insertable.ExecuteCommandAsync();
            EnsureExpectedRowCount(actualCount, expectedCount, errorMessage);
        }
    }

    /// <summary>
    /// 确保期望的行数
    /// </summary>
    /// <param name="actualCount">实际行数</param>
    /// <param name="expectedCount">期望行数</param>
    /// <param name="errorMessage">错误信息</param>
    private static void EnsureExpectedRowCount(int actualCount, int expectedCount = 1,
        string errorMessage = "实际操作影响行数与期望影响行数不一致")
    {
        if (actualCount != expectedCount)
        {
            throw new CustomException(errorMessage);
        }
    }

    /// <summary>
    /// 确保期望的行数
    /// </summary>
    /// <param name="actualCount">实际行数</param>
    /// <param name="errorMessage">错误信息</param>
    private static void EnsureExpectedRowCount(int actualCount, string errorMessage)
    {
        EnsureExpectedRowCount(actualCount, 1, errorMessage);
    }
}