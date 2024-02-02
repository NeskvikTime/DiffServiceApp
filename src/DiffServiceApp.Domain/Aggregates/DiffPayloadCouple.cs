using DiffServiceApp.Domain.Common;

namespace DiffServiceApp.Domain.Aggregates;
public class DiffPayloadCouple : AggregateRoot
{
    public byte[]? LeftPayloadValue { get; set; }

    public byte[]? RightPayloadValue { get; set; }

    public DiffPayloadCouple(int id, byte[]? leftPayloadValue = null, byte[]? rightPayloadValue = null) : base(id)
    {
        LeftPayloadValue = leftPayloadValue;
        RightPayloadValue = rightPayloadValue;
    }
}