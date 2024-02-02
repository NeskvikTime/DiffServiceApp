using DiffServiceApp.Application.Services;
using DiffServiceApp.Domain.Enums;
using DiffServiceApp.Domain.Models;
using FluentAssertions;

namespace DiffServiceApp.Application.UnitTests.Services;

public class DiffProcessorTests
{
    [Fact]
    public void Process_WhenArraysAreEqual_ReturnsEqualsResult()
    {
        // Arrange
        var processor = new DiffProcessor();
        byte[] left = [1, 2, 3];
        byte[] right = [1, 2, 3];

        // Act
        var result = processor.Process(left, right);

        // Assert
        result.DiffResultType.Should().Be(ResultType.Equals);
        result.Diffs.Should().BeNull();
    }

    [Fact]
    public void Process_WhenArraysAreOfDifferentSizes_ReturnsSizeDoNotMatchResult()
    {
        // Arrange
        var processor = new DiffProcessor();
        byte[] left = [1, 2, 3];
        byte[] right = [1, 2];

        // Act
        var result = processor.Process(left, right);

        // Assert
        result.DiffResultType.Should().Be(ResultType.SizeDoNotMatch);
        result.Diffs.Should().BeNull();
    }

    [Fact]
    public void Process_WhenArraysAreOfSameSizeButDiffer_ReturnsContentDoNotMatchResultWithDiffs()
    {
        // Arrange
        var processor = new DiffProcessor();
        byte[] left = [1, 2, 3, 4];
        byte[] right = [1, 1, 3, 1];

        // Expected differences: [1, 1] and [3, 1]
        var expectedDiffs = new List<DiffPosition>
        {
        new DiffPosition(1, 1),
        new DiffPosition(3, 1)
        };

        // Act
        var result = processor.Process(left, right);

        // Assert
        result.DiffResultType.Should().Be(ResultType.ContentDoNotMatch);
        result.Diffs.Should().BeEquivalentTo(expectedDiffs, options => options.WithStrictOrdering());
    }

    [Fact]
    public void Process_WhenDifferencesStartAtBeginning_CorrectlyIdentifiesDiffs()
    {
        // Arrange
        var processor = new DiffProcessor();
        byte[] left = [0, 2, 3, 4];
        byte[] right = [1, 2, 3, 4];

        // Act
        var result = processor.Process(left, right);

        // Assert
        result.DiffResultType.Should().Be(ResultType.ContentDoNotMatch);
        result.Diffs.Should().HaveCount(1);
        result.Diffs!.First().Offset.Should().Be(0);
        result.Diffs!.First().Length.Should().Be(1);
    }

    [Fact]
    public void Process_WhenDifferencesAtEnd_CorrectlyIdentifiesDiffs()
    {
        // Arrange
        var processor = new DiffProcessor();
        byte[] left = [1, 2, 3, 0];
        byte[] right = [1, 2, 3, 4];

        // Act
        var result = processor.Process(left, right);

        // Assert
        result.DiffResultType.Should().Be(ResultType.ContentDoNotMatch);
        result.Diffs.Should().ContainSingle();
        result.Diffs!.First().Offset.Should().Be(3);
        result.Diffs!.First().Length.Should().Be(1);
    }

    [Fact]
    public void Process_WhenConsecutiveDifferences_CorrectlyGroupsDiffs()
    {
        // Arrange
        var processor = new DiffProcessor();
        byte[] left = [1, 0, 0, 4];
        byte[] right = [1, 2, 3, 4];

        // Act
        var result = processor.Process(left, right);

        // Assert
        result.DiffResultType.Should().Be(ResultType.ContentDoNotMatch);
        result.Diffs.Should().HaveCount(1);
        result.Diffs!.First().Offset.Should().Be(1);
        result.Diffs!.First().Length.Should().Be(2);
    }

    [Fact]
    public void Process_WhenNonConsecutiveDifferences_CorrectlyIdentifiesMultipleDiffs()
    {
        // Arrange
        var processor = new DiffProcessor();
        byte[] left = [1, 0, 3, 0];
        byte[] right = [1, 2, 3, 4];

        // Act
        var result = processor.Process(left, right);

        // Assert
        result.DiffResultType.Should().Be(ResultType.ContentDoNotMatch);
        result.Diffs.Should().HaveCount(2);
        result.Diffs[0].Offset.Should().Be(1);
        result.Diffs[0].Length.Should().Be(1);
        result.Diffs[1].Offset.Should().Be(3);
        result.Diffs[1].Length.Should().Be(1);
    }
}
