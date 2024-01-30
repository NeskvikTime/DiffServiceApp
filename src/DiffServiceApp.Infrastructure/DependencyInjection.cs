using DiffServiceApp.Application.Common.Interfaces;
using DiffServiceApp.Domain.Common.Interfaces;
using DiffServiceApp.Infrastructure.Persistance;
using DiffServiceApp.Infrastructure.Persistance.Common;
using DiffServiceApp.Infrastructure.Persistance.Repository;
using DiffServiceApp.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DiffServiceApp.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IDateTimeProvider, SystemDateTimeProvider>();

        services.AddScoped<SaveChangesInterceptor>();

        services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
        {
            options.UseSqlServer(configuration.GetConnectionString("MyDbContext"))
                .AddInterceptors(serviceProvider.GetRequiredService<SaveChangesInterceptor>());
        });

        services.AddScoped<IDbInitializer, ApplicationDbContext>();
        services.AddScoped<IDiffCouplesRepository, DiffCouplesRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
