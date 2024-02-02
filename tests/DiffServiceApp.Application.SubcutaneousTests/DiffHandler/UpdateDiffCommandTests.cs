using DiffServiceApp.Application.DiffCouple.Update;
using System.Text;
using TestCommon.Common;

namespace DiffServiceApp.Application.SubcutaneousTests.DiffHandler;
public class UpdateDiffCommandTests : BaseIntegrationTest
{
    private readonly Random _randomGenerator;

    public UpdateDiffCommandTests(ApplicationApiFactory factory) : base(factory)
    {
        _randomGenerator = new Random();
    }

    [Fact]
    public async Task UpdateDiffCommand_ReturnNewLeftDiffPayload_WhenPayloadNotCreated()
    {
        // Arrange
        var id = _randomGenerator.Next(0, 1000); ;
        var data = "AAAAAA==";
        var diffDirection = DiffDirection.Left;

        var expectedLefPayloadValue = Convert.FromBase64String(data);

        var cancellationToken = CancellationToken.None;

        UpdateDiffCommand commadn = new UpdateDiffCommand(id, data, diffDirection);

        // Act
        var result = await _sender.Send(commadn, cancellationToken);

        // Assert
        result.Id.Should().Be(id);
        result.Should().NotBeNull();
        result.LeftPayloadValue.Should().NotBeNullOrEmpty();
        result.LeftPayloadValue.Should().BeEquivalentTo(expectedLefPayloadValue);
        result.RightPayloadValue.Should().BeNull();
    }

    [Fact]
    public async Task UpdateDiffCommand_ReturnNewRightDiffPayload_WhenPayloadNotCreated()
    {
        // Arrange
        var id = _randomGenerator.Next(0, 1000); ;
        var data = "AAAAAA==";
        var diffDirection = DiffDirection.Right;

        var expectedLefPayloadValue = Convert.FromBase64String(data);

        var cancellationToken = CancellationToken.None;

        UpdateDiffCommand commadn = new UpdateDiffCommand(id, data, diffDirection);

        // Act
        var result = await _sender.Send(commadn, cancellationToken);

        // Assert
        result.Id.Should().Be(id);
        result.Should().NotBeNull();
        result.RightPayloadValue.Should().NotBeNullOrEmpty();
        result.RightPayloadValue.Should().BeEquivalentTo(expectedLefPayloadValue);
        result.LeftPayloadValue.Should().BeNull();
    }

    [Fact]
    public async Task UpdateDiffCommand_ShouldThrowValidationException_WhenDataIsInvalid()
    {
        // Arrange
        var id = _randomGenerator.Next(0, 1000);
        var invalidData = "NotBase64String";
        var diffDirection = nameof(DiffDirection.Left);

        var cancellationToken = CancellationToken.None;

        var command = new UpdateDiffCommand(id, invalidData, diffDirection);

        // Act
        Func<Task> act = async () => await _sender.Send(command, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task UpdateDiffCommand_ShouldThrowValidationException_WhenDataIsEmptyOrNull(string data)
    {
        // Arrange
        var id = _randomGenerator.Next(0, 1000);
        var diffDirection = nameof(DiffDirection.Left);

        var cancellationToken = CancellationToken.None;

        var command = new UpdateDiffCommand(id, data, diffDirection);

        // Act
        Func<Task> act = async () => await _sender.Send(command, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task UpdateDiffCommand_ShouldThrowValidationException_WhenDiffDirectionIsInvalid()
    {
        // Arrange
        var id = _randomGenerator.Next(0, 1000);
        var data = "AAAAAA==";
        var invalidDiffDirection = "InvalidDirection"; // Not a valid direction

        var cancellationToken = CancellationToken.None;

        var command = new UpdateDiffCommand(id, data, invalidDiffDirection);

        // Act
        Func<Task> act = async () => await _sender.Send(command, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task UpdateDiffCommand_ShouldHandleDuplicateSubmissionCorrectly()
    {
        // Arrange
        var id = _randomGenerator.Next(0, 1000);
        var data = "AAAAAA==";
        var diffDirection = DiffDirection.Left;

        var cancellationToken = CancellationToken.None;

        var command = new UpdateDiffCommand(id, data, diffDirection);
        await _sender.Send(command, cancellationToken); // First submission

        // Act
        var result = await _sender.Send(command, cancellationToken);

        // Assert
        result.Id.Should().Be(id);
        result.LeftPayloadValue.Should().BeEquivalentTo(Convert.FromBase64String(data));
    }

    [Fact]
    public async Task UpdateDiffCommand_ShouldHandleLargeData()
    {
        // Arrange
        var id = _randomGenerator.Next(0, 1000);
        var largeData = new string('A', 10000); // Example of large data
        var base64LargeData = Convert.ToBase64String(Encoding.UTF8.GetBytes(largeData));
        var diffDirection = DiffDirection.Left;

        var cancellationToken = CancellationToken.None;

        var command = new UpdateDiffCommand(id, base64LargeData, diffDirection);

        // Act
        var result = await _sender.Send(command, cancellationToken);

        // Assert
        result.Id.Should().Be(id);
        result.LeftPayloadValue.Should().BeEquivalentTo(Convert.FromBase64String(base64LargeData));
    }
}
