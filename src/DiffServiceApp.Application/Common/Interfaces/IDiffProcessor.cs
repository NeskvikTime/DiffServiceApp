using DiffServiceApp.Domain.Models;

namespace DiffServiceApp.Application.Common.Interfaces;
public interface IDiffProcessor
{
    DiffResult Process(byte[] left, byte[] right);
}
