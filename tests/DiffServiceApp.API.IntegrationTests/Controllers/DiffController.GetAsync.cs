using DiffServiceApp.API.SubcutaneousTests.Common;
using DiffServiceApp.Contracts.Common;
using DiffServiceApp.Contracts.Responses;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

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

        // Act
        var response = await _httpClient.GetAsync($"/v1/diff/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Get_DiffResult_WithEqualData_ReturnsEquals()
    {
        // Arrange
        var id = "2";
        var data = new { data = "AAAAAA==" };
        await _httpClient.PutAsJsonAsync($"/v1/diff/{id}/left", data);
        await _httpClient.PutAsJsonAsync($"/v1/diff/{id}/right", data);

        // Act
        var response = await _httpClient.GetAsync($"/v1/diff/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<GetResultResponse>();
        result.Should().NotBeNull();
        result!.Result.Should().Be("Equals");
    }

    [Fact]
    public async Task Get_DiffResult_WithDifferentSizes_ReturnsSizeDoNotMatch()
    {
        // Arrange
        var id = "3";
        var leftData = new { data = "AAAAAA==" };
        var rightData = new { data = "AAA=" }; // Different size
        await _httpClient.PutAsJsonAsync($"/v1/diff/{id}/left", leftData);
        await _httpClient.PutAsJsonAsync($"/v1/diff/{id}/right", rightData);

        // Act
        var response = await _httpClient.GetAsync($"/v1/diff/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<GetResultResponse>();
        result.Should().NotBeNull();
        result!.Result.Should().Be("SizeDoNotMatch");
    }

    [Fact]
    public async Task Get_DiffResult_WithDataMatchesButNotEqual_ReturnsContentDoNotMatch()
    {
        // Arrange
        var id = "4";
        var leftData = new { data = "AAAAAA==" };
        var rightData = new { data = "AQABAQ==" }; // Same size, different content
        await _httpClient.PutAsJsonAsync($"/v1/diff/{id}/left", leftData);
        await _httpClient.PutAsJsonAsync($"/v1/diff/{id}/right", rightData);

        List<DiffResponse>? diffResponses = [new DiffResponse(0, 1), new DiffResponse(2, 2)];

        // Act
        var response = await _httpClient.GetAsync($"/v1/diff/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<GetResultResponse>();
        result.Should().NotBeNull();
        result!.Result.Should().Be("ContentDoNotMatch");
        result.Diffs.Should().NotBeEmpty();
        result.Diffs.Should().HaveCount(2);
        result.Diffs.Should().BeEquivalentTo(diffResponses);
    }
}
