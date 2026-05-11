using System.Net;
using System.Net.Http.Json;
using Ecommerce.Shared.DTOs.Auth;
using FluentAssertions;

namespace Ecommerce.Server.Tests.Integration;

public class AuthControllerTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AuthControllerTests(TestWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Register_ReturnsOkWithToken_WhenValidRequest()
    {
        var request = new RegisterRequest
        {
            FirstName = "Test",
            LastName = "User",
            Email = $"test_{Guid.NewGuid()}@example.com",
            Password = "Test@123!"
        };

        var response = await _client.PostAsJsonAsync("api/auth/register", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<AuthResponse>();
        body!.Token.Should().NotBeNullOrEmpty();
        body.Email.Should().Be(request.Email);
        body.Roles.Should().Contain("Customer");
    }

    [Fact]
    public async Task Register_ReturnsBadRequest_WhenEmailAlreadyTaken()
    {
        var email = $"dup_{Guid.NewGuid()}@example.com";
        var request = new RegisterRequest
        {
            FirstName = "A", LastName = "B",
            Email = email, Password = "Test@123!"
        };

        await _client.PostAsJsonAsync("api/auth/register", request);
        var secondResponse = await _client.PostAsJsonAsync("api/auth/register", request);

        secondResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Login_ReturnsOkWithToken_WhenValidCredentials()
    {
        var email = $"login_{Guid.NewGuid()}@example.com";
        await _client.PostAsJsonAsync("api/auth/register", new RegisterRequest
        {
            FirstName = "A", LastName = "B",
            Email = email, Password = "Test@123!"
        });

        var response = await _client.PostAsJsonAsync("api/auth/login", new LoginRequest
        {
            Email = email,
            Password = "Test@123!"
        });

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<AuthResponse>();
        body!.Token.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Login_ReturnsUnauthorized_WhenInvalidCredentials()
    {
        var response = await _client.PostAsJsonAsync("api/auth/login", new LoginRequest
        {
            Email = "nobody@example.com",
            Password = "WrongPassword!"
        });

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
