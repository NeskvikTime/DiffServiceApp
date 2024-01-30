using DiffServiceApp.Domain.Enums;

namespace DiffServiceApp.Domain.Models;

public record DiffResult
{
    public ResultType DiffResultType { get; set; }

    public List<DiffPosition>? Diffs { get; set; }
}
