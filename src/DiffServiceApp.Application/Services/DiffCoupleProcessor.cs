using DiffServiceApp.Application.Common.Interfaces;
using DiffServiceApp.Domain.Aggregates;
using DiffServiceApp.Domain.Models;

namespace DiffServiceApp.Application.Services;

public sealed class DiffCoupleProcessor : IDiffCoupleProcessor
{
    private readonly IDiffProcessor _diffProcessor;

    public DiffCoupleProcessor(IDiffProcessor diffProcessor)
    {
        _diffProcessor = diffProcessor;
    }

    public DiffResult GetDiffResult(DiffPayloadCouple dataPayload)
    {
        byte[] left = dataPayload.LeftPayloadValue ?? Array.Empty<byte>();
        byte[] right = dataPayload.RightPayloadValue ?? Array.Empty<byte>();

        return _diffProcessor.Process(left, right);
    }
}
