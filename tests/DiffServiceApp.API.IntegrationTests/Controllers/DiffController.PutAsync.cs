using DiffServiceApp.Domain.Aggregates;
using System.Net;
using System.Net.Http.Json;
using TestCommon.DiffServiceApp;

namespace DiffServiceApp.API.IntegrationTests.Controllers;
public class PutAsyncTests : BaseIntegrationTest
{


    public PutAsyncTests(ApplicationApiFactory factory) : base(factory)
    {

    }

    [Fact]
    public async Task Put_LeftSide_WithValidData_ReturnsCreated()
    {
        // Arrange
        int id = 1;
        var base64Data = "AAAAAA==";
        var data = new { data = base64Data };
        var url = $"/v1/diff/{id}/left";
        var expectedPayload = Convert.FromBase64String(base64Data);

        // Act
        var response = await _httpClient.PutAsJsonAsync(url, data);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var diffPayloadCouple = await response.Content.ReadFromJsonAsync<DiffPayloadCouple>();

        diffPayloadCouple.Should().NotBeNull();
        diffPayloadCouple!.LeftPayloadValue.Should().BeEquivalentTo(expectedPayload);
        diffPayloadCouple.RightPayloadValue.Should().BeNull();
    }

    [Fact]
    public async Task Put_RightSide_WithValidData_ReturnsCreated_And_VerifiesDiffPayloadCouple()
    {
        // Arrange
        int id = 1;
        var base64Data = "AAAAAA==";
        var data = new { data = base64Data };
        var url = $"/v1/diff/{id}/right";
        var expectedPayload = Convert.FromBase64String(base64Data);

        // Act
        var response = await _httpClient.PutAsJsonAsync(url, data);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var diffPayloadCouple = await response.Content.ReadFromJsonAsync<DiffPayloadCouple>();

        diffPayloadCouple.Should().NotBeNull();
        diffPayloadCouple!.RightPayloadValue.Should().BeEquivalentTo(expectedPayload);
        diffPayloadCouple.LeftPayloadValue.Should().BeNull();
    }

    [Fact]
    public async Task Put_LeftSide_WithInvalidBase64Data_ReturnsBadRequest()
    {
        // Arrange
        var id = "4";
        var invalidData = new { data = "InvalidBase64==" };
        var url = $"/v1/diff/{id}/left";

        // Act
        var response = await _httpClient.PutAsJsonAsync(url, invalidData);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Put_RightSide_WithEmptyData_ReturnsBadRequest()
    {
        // Arrange
        var id = "5";
        var emptyData = new { data = "" };
        var url = $"/v1/diff/{id}/right";

        // Act
        var response = await _httpClient.PutAsJsonAsync(url, emptyData);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
