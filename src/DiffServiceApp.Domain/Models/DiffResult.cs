using DiffServiceApp.Domain.Enums;

namespace DiffServiceApp.Domain.Models;
public class DiffResult
{
    public ResultType DiffResultType { get; set; } = ResultType.NotFound;

    public List<DiffPosition> Diffs { get; set; } = new List<DiffPosition>();
}
