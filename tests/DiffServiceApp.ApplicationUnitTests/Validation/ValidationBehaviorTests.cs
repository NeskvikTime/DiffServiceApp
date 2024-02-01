using DiffServiceApp.Application.Common.Behaviours;
using DiffServiceApp.Application.DiffCouple.Update;
using DiffServiceApp.Domain.Aggregates;
using DiffServiceApp.Domain.Models;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace DiffServiceApp.ApplicationUnitTests.Validation;
public class ValidationBehaviorTests
{
    private readonly IValidator<UpdateDiffCommand> _mockValidator;
    private readonly RequestHandlerDelegate<DiffPayloadCouple> _mockNext;

    public ValidationBehaviorTests()
    {
        _mockValidator = Substitute.For<IValidator<UpdateDiffCommand>>();
        _mockNext = Substitute.For<RequestHandlerDelegate<DiffPayloadCouple>>();
    }

    [Fact]
    public async Task Handle_ValidationSucceeds_InvokesNext()
    {
        // Arrange
        var command = new UpdateDiffCommand(1, "AAAA==", DiffDirection.Left);
        var diffPayloadCouple = new DiffPayloadCouple(1);
        _mockValidator.ValidateAsync(Arg.Any<ValidationContext<UpdateDiffCommand>>(), Arg.Any<CancellationToken>())
                      .Returns(new ValidationResult());
        _mockNext.Invoke().Returns(Task.FromResult(diffPayloadCouple));

        var validationBehavior = new ValidationBehavior<UpdateDiffCommand, DiffPayloadCouple>(_mockValidator);

        // Act
        var result = await validationBehavior.Handle(command, _mockNext, CancellationToken.None);

        // Assert
        await _mockNext.Received(1).Invoke();
        result.Should().BeEquivalentTo(diffPayloadCouple);
    }

    [Fact]
    public async Task Handle_ValidationFails_ThrowsValidationException()
    {
        // Arrange
        var command = new UpdateDiffCommand(1, "invalid data", DiffDirection.Left);
        var validationFailures = new List<ValidationFailure> { new ValidationFailure("data", "Invalid data format") };

        _mockValidator.ValidateAsync(Arg.Any<ValidationContext<UpdateDiffCommand>>(), Arg.Any<CancellationToken>())
                      .ThrowsAsync(new ValidationException(validationFailures));

        var validationBehavior = new ValidationBehavior<UpdateDiffCommand, DiffPayloadCouple>(_mockValidator);

        // Act
        Func<Task> act = async () => await validationBehavior.Handle(command, _mockNext, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }
}
