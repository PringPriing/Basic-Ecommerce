using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Ecommerce.Server.Data.Models;
using Ecommerce.Server.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Ecommerce.Server.Tests.Services;

public class TokenServiceTests
{
    private readonly IConfiguration _config;
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    private readonly TokenService _sut;

    public TokenServiceTests()
    {
        _config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Key"] = "TestSecretKeyThatIsAtLeast32CharsLong!",
                ["Jwt:Issuer"] = "TestIssuer",
                ["Jwt:Audience"] = "TestAudience",
                ["Jwt:ExpiresHours"] = "24"
            })
            .Build();

        var store = new Mock<IUserStore<ApplicationUser>>();
        _userManagerMock = new Mock<UserManager<ApplicationUser>>(
            store.Object, null!, null!, null!, null!, null!, null!, null!, null!);

        _sut = new TokenService(_config, _userManagerMock.Object);
    }

    [Fact]
    public async Task GenerateTokenAsync_ReturnsValidJwt()
    {
        var user = new ApplicationUser
        {
            Id = "user-1",
            Email = "test@example.com",
            UserName = "test@example.com",
            FirstName = "John",
            LastName = "Doe"
        };
        _userManagerMock.Setup(m => m.GetRolesAsync(user))
            .ReturnsAsync(new List<string> { "Customer" });

        var token = await _sut.GenerateTokenAsync(user);

        token.Should().NotBeNullOrEmpty();
        var handler = new JwtSecurityTokenHandler();
        handler.CanReadToken(token).Should().BeTrue();
    }

    [Fact]
    public async Task GenerateTokenAsync_ContainsExpectedClaims()
    {
        var user = new ApplicationUser
        {
            Id = "user-42",
            Email = "jane@example.com",
            UserName = "jane@example.com",
            FirstName = "Jane",
            LastName = "Smith"
        };
        _userManagerMock.Setup(m => m.GetRolesAsync(user))
            .ReturnsAsync(new List<string> { "Admin" });

        var token = await _sut.GenerateTokenAsync(user);

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        jwt.Claims.Should().Contain(c => c.Type == ClaimTypes.NameIdentifier && c.Value == "user-42");
        jwt.Claims.Should().Contain(c => c.Type == ClaimTypes.Email && c.Value == "jane@example.com");
        jwt.Claims.Should().Contain(c => c.Type == ClaimTypes.GivenName && c.Value == "Jane");
        jwt.Claims.Should().Contain(c => c.Type == ClaimTypes.Role && c.Value == "Admin");
    }

    [Fact]
    public async Task GenerateTokenAsync_TokenHas24HourExpiry()
    {
        var user = new ApplicationUser
        {
            Id = "user-1",
            Email = "x@x.com",
            UserName = "x@x.com",
            FirstName = "X",
            LastName = "Y"
        };
        _userManagerMock.Setup(m => m.GetRolesAsync(user))
            .ReturnsAsync(new List<string>());

        var before = DateTime.UtcNow.AddHours(23.9);
        var token = await _sut.GenerateTokenAsync(user);
        var after = DateTime.UtcNow.AddHours(24.1);

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        jwt.ValidTo.Should().BeAfter(before);
        jwt.ValidTo.Should().BeBefore(after);
    }
}
