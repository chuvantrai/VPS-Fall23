using Microsoft.Extensions.Options;
using VPS.MinIO.BusinessObjects.AppSetting;
using VPS.MinIO.Repository.MinIO;

namespace VPS.MinIO.Repository
{
    internal static class Extensions
    {
        internal static T GetMinIORepository<T>(AppSetting appSetting)
            where T : MinIOClient
        {
            T instance = (T)Activator.CreateInstance(typeof(T), new object[] { appSetting.MinIO.EndPoint, appSetting.MinIO.Port, appSetting.MinIO.UseSSL });
            return instance;
        }
        internal static IServiceCollection UseMinvoiceMinIORepository<IRepository, Repository>(this IServiceCollection services)
            where IRepository : class, IMinIOClient
            where Repository : MinIOClient, IRepository
        {
            return services
                .AddScoped<IRepository>(x =>
                {
                    BusinessObjects.AppSetting.AppSetting appSetting = x.GetRequiredService<IOptions<BusinessObjects.AppSetting.AppSetting>>().Value;
                    Repository instance = GetMinIORepository<Repository>(appSetting);
                    return instance;
                });
        }
    }
}
