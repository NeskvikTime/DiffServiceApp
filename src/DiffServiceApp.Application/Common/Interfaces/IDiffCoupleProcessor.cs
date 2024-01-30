using DiffServiceApp.Domain.Aggregates;
using DiffServiceApp.Domain.Models;

namespace DiffServiceApp.Application.Common.Interfaces;

public interface IDiffCoupleProcessor
{
    DiffResult GetDiffResult(DiffPayloadCouple dataPayload);
}
