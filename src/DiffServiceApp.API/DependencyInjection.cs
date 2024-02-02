using DiffServiceApp.API.Filters;
using Microsoft.OpenApi.Models;

namespace DiffServiceApp.API;
public static class DependencyInjection
{
    public static IServiceCollection RegisterApiServices(this IServiceCollection services)
    {
        services.AddControllers(cfg =>
        {
            cfg.Filters.Add(typeof(ExceptionFilter));
        });

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "DiffServiceApp.API", Version = "v1" });
        });
        services.AddHttpContextAccessor();

        return services;
    }
}