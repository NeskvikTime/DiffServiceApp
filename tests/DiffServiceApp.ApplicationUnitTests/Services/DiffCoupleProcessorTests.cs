using DiffServiceApp.Application.Common.Interfaces;
using DiffServiceApp.Application.Services;
using DiffServiceApp.Domain.Aggregates;
using DiffServiceApp.Domain.Enums;
using DiffServiceApp.Domain.Models;
using TestCommon.Builders;

namespace DiffServiceApp.Application.UnitTests.Services;
public class DiffCoupleProcessorTests
{
    private readonly IDiffProcessor _diffProcessor = Substitute.For<IDiffProcessor>();
    private readonly DiffCoupleProcessor _diffCoupleProcessor;

    public DiffCoupleProcessorTests()
    {
        _diffCoupleProcessor = new DiffCoupleProcessor(_diffProcessor);
    }

    [Fact]
    public void GetDiffResult_WithBothPayloadsNull_ReturnsEquals()
    {
        // Arrange
        var dataPayload = new DiffPayloadCoupleBuilder()
            .WithId(1)
            .Build();

        _diffProcessor.Process(Arg.Any<byte[]>(),
            Arg.Any<byte[]>())
            .Returns(new DiffResult { DiffResultType = ResultType.Equals });

        // Act
        var result = _diffCoupleProcessor.GetDiffResult(dataPayload);

        // Assert
        result.DiffResultType.Should().Be(ResultType.Equals);
        _diffProcessor.Received(1).Process(Arg.Is<byte[]>(x => x.Length == 0), Arg.Is<byte[]>(x => x.Length == 0));
    }

    [Fact]
    public void GetDiffResult_WithLeftPayloadOnly_ReturnsContentDoNotMatch()
    {
        // Arrange
        var leftPayload = new byte[] { 1, 2, 3 };
        var dataPayload = new DiffPayloadCouple(1, leftPayload, null);

        _diffProcessor.Process(Arg.Any<byte[]>(), Arg.Any<byte[]>())
            .Returns(new DiffResult { DiffResultType = ResultType.ContentDoNotMatch });

        // Act
        var result = _diffCoupleProcessor.GetDiffResult(dataPayload);

        // Assert
        result.DiffResultType.Should().Be(ResultType.ContentDoNotMatch);
        _diffProcessor.Received(1)
            .Process(Arg.Is<byte[]>(x => x.SequenceEqual(leftPayload)), Arg.Is<byte[]>(x => x.Length == 0));
    }

    [Fact]
    public void GetDiffResult_WithBothPayloadsProvidedAndDiffer_ReturnsContentDoNotMatch()
    {
        // Arrange
        byte[] leftPayload = [1, 2, 3];
        byte[] rightPayload = [4, 5, 6];
        var dataPayload = new DiffPayloadCouple(1, leftPayload, rightPayload);

        _diffProcessor.Process(leftPayload, rightPayload).Returns(new DiffResult
        {
            DiffResultType = ResultType.ContentDoNotMatch,
            Diffs = new List<DiffPosition> { new DiffPosition(0, 3) }
        });

        // Act
        var result = _diffCoupleProcessor.GetDiffResult(dataPayload);

        // Assert
        result.DiffResultType.Should().Be(ResultType.ContentDoNotMatch);
        result.Diffs.Should().HaveCount(1);
        _diffProcessor.Received(1).Process(leftPayload, rightPayload);
    }
}