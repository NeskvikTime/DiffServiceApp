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

        int? currentOffset = null;
        for (int i = 0; i < left.Length; i++)
        {
            // Check if bytes different and we're not already tracking a difference
            if (left[i] != right[i] && !currentOffset.HasValue)
            {
                currentOffset = i; // Start tracking a new difference
                continue;
            }

            // If bytes are the same and we were tracking a difference, add the diff and end tracking
            if ((left[i] == right[i] || i == left.Length - 1) && currentOffset.HasValue)
            {
                // Calculate length differently if we're at the end of the arrays
                int length = left[i] == right[i] ? i - currentOffset.Value : (i - currentOffset.Value) + 1;
                diffs.Add(new DiffPosition(currentOffset.Value, length));
                currentOffset = null;
            }
        }

        // Handle case where the last bytes are different
        if (currentOffset.HasValue)
        {
            int length = left.Length - currentOffset.Value;
            diffs.Add(new DiffPosition(currentOffset.Value, length));
        }

        return diffs;
    }
}
