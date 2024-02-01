using DiffServiceApp.Application.DiffCouple.Update;
using DiffServiceApp.Application.SubcutaneousTests.Common;
using System.Text;

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
        var result = await _sender.Send(commadn);

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
        var result = await _sender.Send(commadn);

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

        var command = new UpdateDiffCommand(id, invalidData, diffDirection);

        // Act & Assert
        Func<Task> act = async () => await _sender.Send(command);
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

        var command = new UpdateDiffCommand(id, data, diffDirection);

        // Act & Assert
        Func<Task> act = async () => await _sender.Send(command);
        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task UpdateDiffCommand_ShouldThrowValidationException_WhenDiffDirectionIsInvalid()
    {
        // Arrange
        var id = _randomGenerator.Next(0, 1000);
        var data = "AAAAAA==";
        var invalidDiffDirection = "InvalidDirection"; // Not a valid direction

        var command = new UpdateDiffCommand(id, data, invalidDiffDirection);

        // Act & Assert
        Func<Task> act = async () => await _sender.Send(command);
        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task UpdateDiffCommand_ShouldHandleDuplicateSubmissionCorrectly()
    {
        // Arrange
        var id = _randomGenerator.Next(0, 1000);
        var data = "AAAAAA==";
        var diffDirection = DiffDirection.Left;

        var command = new UpdateDiffCommand(id, data, diffDirection);
        await _sender.Send(command); // First submission

        // Act
        var result = await _sender.Send(command);

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

        var command = new UpdateDiffCommand(id, base64LargeData, diffDirection);

        // Act
        var result = await _sender.Send(command);

        // Assert
        result.Id.Should().Be(id);
        result.LeftPayloadValue.Should().BeEquivalentTo(Convert.FromBase64String(base64LargeData));
    }
}
