using DiffServiceApp.Application.Common.Behaviours;
using DiffServiceApp.Application.Common.Interfaces;
using DiffServiceApp.Application.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DiffServiceApp.Application;
public static class DependencyInjection
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssemblyContaining(typeof(DependencyInjection));

            options.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddScoped<IDiffCoupleProcessor, DiffCoupleProcessor>();

        services.AddValidatorsFromAssemblyContaining(typeof(DependencyInjection));

        return services;
    }
}
