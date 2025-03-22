using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace SnapshotHash
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSnapshotHash(this IServiceCollection services)
        {
            services.TryAddSingleton<IPayloadHasher, PayloadHasher>();

            return services;
        }
    }
}
