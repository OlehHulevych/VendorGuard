using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VendorGuard.Application.Common.Interfaces;
using VendorGuard.Infrastructure.Persistence;
using VendorGuard.Infrastructure.Presistance;

namespace VendorGuard.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<SaveChangesInterceptor, AuditableEntitySaveChangesInterceptor>();
        services.AddScoped<IClocker, SystemClock>();

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.UseNpgsql(configuration.GetConnectionString("ConnectionString:DBString"));
            options.AddInterceptors(sp.GetService<ISaveChangesInterceptor>()!);
        });
        return services;
    }
}