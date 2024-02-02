using DiffServiceApp.Contracts.Common;

namespace DiffServiceApp.Contracts.Responses;
public class GetResultResponse
{
    public string Result { get; set; } = default!;

    public List<DiffResponse>? Diffs { get; set; } = null;
}
