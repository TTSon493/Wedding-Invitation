using Wedding.Utility.Constants;
using StackExchange.Redis;

namespace Wedding.API.Extentions;

public static class RedisServiceExtensions
{
    public static WebApplicationBuilder AddRedisCache(this WebApplicationBuilder builder)
    {
        string connectionString =
            builder.Configuration.GetSection("Redis")[StaticConnectionString.REDIS_ConnectionString];
        builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(connectionString));
        return builder;
    }
}
