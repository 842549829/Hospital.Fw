namespace Hospital.Fw.HttpApi.Filter;

/// <summary>
/// 注册
/// </summary>
public static class TransactionFilterExtensions
{
    /// <summary>
    /// 注册
    /// </summary>
    /// <param name="services">services</param>
    /// <returns>IServiceCollection</returns>

    public static IServiceCollection AddTransactionFilter(this IServiceCollection services)
    {
        services.AddScoped<TransactionFilter>();
        return services;
    }
}