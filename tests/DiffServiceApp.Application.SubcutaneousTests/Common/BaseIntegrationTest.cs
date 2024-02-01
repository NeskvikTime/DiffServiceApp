using DiffServiceApp.Application.Common.Interfaces;
using DiffServiceApp.Domain.Common.Interfaces;
using DiffServiceApp.Infrastructure.Persistance;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace DiffServiceApp.Application.SubcutaneousTests.Common;
public abstract class BaseIntegrationTest
    : IClassFixture<ApplicationApiFactory>,
      IDisposable
{
    private readonly IServiceScope _scope;
    protected readonly ISender _sender;
    protected readonly IDiffCouplesRepository _diffCouplesRepository;
    protected readonly IUnitOfWork _unitOfWork;

    internal readonly ApplicationDbContext DbContext;

    internal BaseIntegrationTest(ApplicationApiFactory factory)
    {
        _scope = factory.Services.CreateScope();

        _sender = _scope.ServiceProvider.GetRequiredService<ISender>();
        _diffCouplesRepository = _scope.ServiceProvider.GetRequiredService<IDiffCouplesRepository>();
        _unitOfWork = _scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        DbContext = _scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();
    }

    public void Dispose()
    {
        _scope?.Dispose();
        DbContext?.Dispose();
    }
}
