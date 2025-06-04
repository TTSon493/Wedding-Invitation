using Hangfire;
using Wedding.Utility.Constants;

namespace Wedding.API.Extentions;

public static class HangfireServiceExtensions
{
    public static WebApplicationBuilder AddHangfireServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(
                builder.Configuration.GetConnectionString(StaticConnectionString.SQLDB_AzureConnection))
        );

        builder.Services.AddHangfireServer();
        return builder;
    }
}