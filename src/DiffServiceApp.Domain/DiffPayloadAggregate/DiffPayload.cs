using DiffServiceApp.Domain.Common;

namespace DiffServiceApp.Domain.DiffPayloadAggregate;
public class DiffPayload : AggregateRoot
{
    public byte[]? LeftValue { get; set; } = null;

    public byte[]? RightValue { get; set; } = null;
}