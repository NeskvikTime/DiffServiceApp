using DiffServiceApp.Application.DiffCouple.Queries.GetResult;
using DiffServiceApp.Contracts.Common;
using DiffServiceApp.Contracts.Exceptions;
using TestCommon.Builders;
using TestCommon.Common;

namespace DiffServiceApp.Application.SubcutaneousTests.DiffHandler;
public class GetDiffResultQueryTests : BaseIntegrationTest
{
    private readonly Random _randomGenerator;

    public GetDiffResultQueryTests(ApplicationApiFactory factory) : base(factory)
    {
        _randomGenerator = new Random();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task GetDiffResultQuery_ShouldThrowNotFoundException_WhenIdIsInvalid(int invalidId)
    {
        // Arrange
        var query = new GetDiffResultQuery(invalidId);

        var cancellationToken = CancellationToken.None;

        // Act
        Func<Task> act = async () => await _sender.Send(query, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task GetDiffResultQuery_ShouldThrowNotFoundException_WhenIdDoesNotExist()
    {
        // Arrange
        var nonExistentId = _randomGenerator.Next(1000, 2000);
        var query = new GetDiffResultQuery(nonExistentId);

        var cancellationToken = CancellationToken.None;

        // Act
        Func<Task> act = async () => await _sender.Send(query, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task GetDiffResultQuery_ShouldReturnResult_WhenQueryIsValid()
    {
        // Arrange
        var validId = _randomGenerator.Next(0, 1000);
        var leftPayload = Convert.FromBase64String("AAAAAA==");
        var rightPayload = Convert.FromBase64String("BBBBBB==");

        var newDiffPayloadCouple = new DiffPayloadCoupleBuilder()
            .WithId(validId)
            .WithLeftPayloadValue(leftPayload)
            .WithRightPayloadValue(rightPayload)
            .Build();

        var cancellationToken = CancellationToken.None;

        await _diffCouplesRepository.CreateDiffCoupleAsync(newDiffPayloadCouple, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var query = new GetDiffResultQuery(validId);

        List<DiffResponse>? diffs = [new DiffResponse(0, 4)];
        string expectedResult = ResultType.ContentDoNotMatch.ToString();

        // Act
        var result = await _sender.Send(query, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Result.Should().Be(expectedResult);
        result.Diffs.Should().BeEquivalentTo(diffs);
    }

    [Fact]
    public async Task GetDiffResultQuery_ShouldThrowNotFoundException_WhenOnlyLeftSideIsAssigned()
    {
        // Arrange
        var idWithOnlyLeft = _randomGenerator.Next(0, 1000);
        var leftPayload = Convert.FromBase64String("AAAAAA==");

        var cancellationToken = CancellationToken.None;

        var diffPayloadCouple = new DiffPayloadCouple(idWithOnlyLeft, leftPayload, null);
        await _diffCouplesRepository.CreateDiffCoupleAsync(diffPayloadCouple, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var query = new GetDiffResultQuery(idWithOnlyLeft);

        // Ac
        Func<Task> act = async () => await _sender.Send(query, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task GetDiffResultQuery_ShouldThrowNotFoundException_WhenOnlyRightSideIsAssigned()
    {
        // Arrange
        var idWithOnlyRight = _randomGenerator.Next(0, 1000);
        var rightPayload = Convert.FromBase64String("BBBBBB==");

        var cancellationToken = CancellationToken.None;

        var diffPayloadCouple = new DiffPayloadCouple(idWithOnlyRight, null, rightPayload);
        await _diffCouplesRepository.CreateDiffCoupleAsync(diffPayloadCouple, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var query = new GetDiffResultQuery(idWithOnlyRight);

        // Act
        Func<Task> act = async () => await _sender.Send(query, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task GetDiffResultQuery_ShouldReturnEqualResult_WhenBothSidesAreEqual()
    {
        // Arrange
        var idWithEqualSides = _randomGenerator.Next(0, 1000);
        var payload = Convert.FromBase64String("AAAAAA==");

        var cancellationToken = CancellationToken.None;

        var diffPayloadCouple = new DiffPayloadCouple(idWithEqualSides, payload, payload);
        await _diffCouplesRepository.CreateDiffCoupleAsync(diffPayloadCouple, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var query = new GetDiffResultQuery(idWithEqualSides);

        // Act
        var result = await _sender.Send(query, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Result.Should().Be(ResultType.Equals.ToString());
        result.Diffs.Should().BeNull();
    }
}
