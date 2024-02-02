using DiffServiceApp.Application.Common.Interfaces;

namespace DiffServiceApp.Infrastructure.Services;
public class SystemDateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
