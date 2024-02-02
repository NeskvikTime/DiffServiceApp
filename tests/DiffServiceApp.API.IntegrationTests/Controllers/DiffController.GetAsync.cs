using DiffServiceApp.Contracts.Common;
using DiffServiceApp.Contracts.Requests;
using DiffServiceApp.Contracts.Responses;
using DiffServiceApp.Domain.Enums;

namespace DiffServiceApp.API.IntegrationTests.Controllers;
public class GetAsyncTests : BaseIntegrationTest
{
    public GetAsyncTests(ApplicationApiFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Get_DiffResult_WithoutSettingData_ReturnsNotFound()
    {
        // Arrange
        int id = 1;
        string url = $"/v1/diff/{id}";

        // Act
        var response = await _httpClient.GetAsync(url);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Get_DiffResult_WithEqualData_ReturnsEquals()
    {
        // Arrange
        int id = 2;
        var data = new UpdateDiffValueRequest("AAAAAA==");
        string url = $"/v1/diff/{id}";
        await _httpClient.PutAsJsonAsync($"/v1/diff/{id}/left", data);
        await _httpClient.PutAsJsonAsync($"/v1/diff/{id}/right", data);

        // Act
        var response = await _httpClient.GetAsync(url);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<GetResultResponse>();
        result.Should().NotBeNull();
        result!.Result.Should().Be(ResultType.Equals.ToString());
    }

    [Fact]
    public async Task Get_DiffResult_WithDifferentSizes_ReturnsSizeDoNotMatch()
    {
        // Arrange
        int id = 3;
        var leftData = new UpdateDiffValueRequest("AAAAAA==");
        var rightData = new { data = "AAA=" }; // Different size
        string url = $"/v1/diff/{id}";
        await _httpClient.PutAsJsonAsync($"/v1/diff/{id}/left", leftData);
        await _httpClient.PutAsJsonAsync($"/v1/diff/{id}/right", rightData);

        // Act
        var response = await _httpClient.GetAsync(url);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<GetResultResponse>();
        result.Should().NotBeNull();
        result!.Result.Should().Be(ResultType.SizeDoNotMatch.ToString());
    }

    [Fact]
    public async Task Get_DiffResult_WithDataMatchesButNotEqual_ReturnsContentDoNotMatch()
    {
        // Arrange
        int id = 4;
        var leftData = new UpdateDiffValueRequest("AAAAAA==");
        var rightData = new UpdateDiffValueRequest("AQABAQ=="); // Same size, different content
        await _httpClient.PutAsJsonAsync($"/v1/diff/{id}/left", leftData);
        await _httpClient.PutAsJsonAsync($"/v1/diff/{id}/right", rightData);
        string url = $"/v1/diff/{id}";

        List<DiffResponse>? diffResponses = [new DiffResponse(0, 1), new DiffResponse(2, 2)];

        // Act
        var response = await _httpClient.GetAsync(url);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<GetResultResponse>();
        result.Should().NotBeNull();
        result!.Result.Should().Be(ResultType.ContentDoNotMatch.ToString());
        result.Diffs.Should().NotBeEmpty();
        result.Diffs.Should().HaveCount(2);
        result.Diffs.Should().BeEquivalentTo(diffResponses);
    }
}
