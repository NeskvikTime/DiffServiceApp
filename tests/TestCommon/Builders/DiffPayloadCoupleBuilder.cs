using DiffServiceApp.Domain.Aggregates;
using TestCommon.Common.Interfaces;

namespace TestCommon.Builders;
public class DiffPayloadCoupleBuilder : IBuilder<DiffPayloadCouple>
{
    private int _id;

    private byte[]? LeftPayloadValue;

    private byte[]? RightPayloadValue;

    public DiffPayloadCouple Build()
    {
        return new DiffPayloadCouple(_id, LeftPayloadValue, RightPayloadValue);
    }

    public DiffPayloadCoupleBuilder WithId(int id)
    {
        _id = id;
        return this;
    }

    public DiffPayloadCoupleBuilder WithLeftPayloadValue(byte[]? leftPayloadValue)
    {
        LeftPayloadValue = leftPayloadValue;
        return this;
    }

    public DiffPayloadCoupleBuilder WithRightPayloadValue(byte[]? rightPayloadValue)
    {
        RightPayloadValue = rightPayloadValue;
        return this;
    }
}
