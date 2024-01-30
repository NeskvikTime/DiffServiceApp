using DiffServiceApp.Application.Common.Interfaces;
using DiffServiceApp.Domain.Aggregates;
using DiffServiceApp.Domain.Enums;
using DiffServiceApp.Domain.Models;

namespace DiffServiceApp.Application.Services;

public class DiffCoupleProcessor : IDiffCoupleProcessor
{
    public DiffResult GetDiffResult(DiffPayloadCouple dataPayload)
    {
        var result = new DiffResult();

        byte[] left = dataPayload.LeftPayloadValue ?? Array.Empty<byte>();
        byte[] right = dataPayload.RightPayloadValue ?? Array.Empty<byte>();

        if ((left.Length == 0 || right.Length == 0) || (left.Length != right.Length))
        {
            result.DiffResultType = ResultType.SizeDoNotMatch;
            return result;
        }

        if (left.SequenceEqual(right))
        {
            result.DiffResultType = ResultType.Equals;
            return result;
        }

        List<DiffPosition> diffs = new List<DiffPosition>();

        for (int i = 0; i < left.Length; i++)
        {
            if (left[i] != right[i])
            {
                int length = 1;
                while (i + length < left.Length && left[i + length] != right[i + length])
                {
                    length++;
                }

                diffs.Add(new DiffPosition(i, length));
                i += length - 1;
            }
        }

        result.DiffResultType = ResultType.ContentDoNotMatch;
        result.Diffs = diffs;

        return result;
    }
}
