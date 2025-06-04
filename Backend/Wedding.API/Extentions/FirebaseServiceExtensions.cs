using Wedding.Service.IService;
using Wedding.Service.Service;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;

namespace Wedding.API.Extentions;

public static class FirebaseServiceExtensions
{
    public static IServiceCollection AddFirebaseServices(this IServiceCollection services)
    {
        var credentialPath = Path.Combine(Directory.GetCurrentDirectory(),
            "wedding-firebase-storage-d46bbc474f44.json");
        FirebaseApp.Create(new AppOptions()
        {
            Credential = GoogleCredential.FromFile(credentialPath)
        });
        services.AddSingleton(StorageClient.Create(GoogleCredential.FromFile(credentialPath)));
        services.AddScoped<IFirebaseService, FirebaseService>();
        return services;
    }
}