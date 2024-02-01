using DiffServiceApp.Application.Common.Interfaces;
using DiffServiceApp.Domain.Enums;
using DiffServiceApp.Domain.Models;

namespace DiffServiceApp.Application.Services;
public class DiffProcessor : IDiffProcessor
{
    public DiffResult Process(byte[] left, byte[] right)
    {
        // Initial validation and quick checks
        if (left.SequenceEqual(right))
        {
            return new DiffResult { DiffResultType = ResultType.Equals };
        }

        if (left.Length != right.Length)
        {
            return new DiffResult { DiffResultType = ResultType.SizeDoNotMatch };
        }

        // Detailed diff calculation
        var diffs = CalculateDiffs(left, right);

        return new DiffResult
        {
            DiffResultType = ResultType.ContentDoNotMatch,
            Diffs = diffs
        };
    }

    private List<DiffPosition> CalculateDiffs(byte[] left, byte[] right)
    {
        var diffs = new List<DiffPosition>();

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
                i += (length - 1);
            }
        }

        return diffs;
    }
}
