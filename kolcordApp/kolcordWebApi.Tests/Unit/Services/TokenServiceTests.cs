// Unit/Services/TokenServiceTests.cs
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using kolcordWebApi.Models;
using kolcordWebApi.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace kolcordWebApi.Tests.Unit.Services
{
    public class TokenServiceTests
    {
        private readonly TokenService _tokenService;
        private readonly Mock<IConfiguration> _mockConfig;

        public TokenServiceTests()
        {
            _mockConfig = new Mock<IConfiguration>();

            var validKey = "this-is-a-very-long-secret-key-for-testing-purposes-that-is-over-64-characters-long-to-meet-512-bit-requirement";
            _mockConfig.Setup(x => x["JWT:SigningKey"]).Returns(validKey);
            _mockConfig.Setup(x => x["JWT:Issuer"]).Returns("test-issuer");
            _mockConfig.Setup(x => x["JWT:Audience"]).Returns("test-audience");

            _tokenService = new TokenService(_mockConfig.Object);
        }

        [Fact]
        public void CreateToken_ValidUser_ReturnsToken()
        {
            var user = new ApplicationUser
            {
                Id = "user123",
                UserName = "testuser",
                Email = "test@example.com"
            };

            var token = _tokenService.CreateToken(user);

            Assert.NotNull(token);
            Assert.NotEmpty(token);

            var handler = new JwtSecurityTokenHandler();
            Assert.True(handler.CanReadToken(token));
        }

        [Fact]
        public void CreateToken_ContainsCorrectClaims()
        {
            var user = new ApplicationUser
            {
                Id = "user123",
                UserName = "testuser",
                Email = "test@example.com"
            };

            var token = _tokenService.CreateToken(user);
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            Assert.Contains(jwtToken.Claims, c => c.Type == "nameid" && c.Value == "user123");
            Assert.Contains(jwtToken.Claims, c => c.Type == "email" && c.Value == "test@example.com");
            Assert.Contains(jwtToken.Claims, c => c.Type == "given_name" && c.Value == "testuser");
        }

        [Fact]
        public void CreateToken_HasCorrectIssuerAndAudience()
        {
            var user = new ApplicationUser
            {
                Id = "user123",
                UserName = "testuser",
                Email = "test@example.com"
            };

            var token = _tokenService.CreateToken(user);
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            Assert.Equal("test-issuer", jwtToken.Issuer);
            Assert.Contains("test-audience", jwtToken.Audiences);
        }

        [Fact]
        public void CreateToken_TokenExpiresInFuture()
        {
            var user = new ApplicationUser
            {
                Id = "user123",
                UserName = "testuser",
                Email = "test@example.com"
            };

            var token = _tokenService.CreateToken(user);
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            Assert.True(jwtToken.ValidTo > DateTime.UtcNow);
        }

        [Fact]
        public void CreateToken_DifferentUsers_GenerateDifferentTokens()
        {
            var user1 = new ApplicationUser { Id = "user1", UserName = "user1", Email = "user1@test.com" };
            var user2 = new ApplicationUser { Id = "user2", UserName = "user2", Email = "user2@test.com" };

            var token1 = _tokenService.CreateToken(user1);
            var token2 = _tokenService.CreateToken(user2);

            Assert.NotEqual(token1, token2);

            var handler = new JwtSecurityTokenHandler();
            var claims1 = handler.ReadJwtToken(token1).Claims;
            var claims2 = handler.ReadJwtToken(token2).Claims;

            Assert.Contains(claims1, c => c.Value == "user1");
            Assert.Contains(claims2, c => c.Value == "user2");
        }

        [Fact]
        public void CreateToken_NullUser_ThrowsException()
        {
            ApplicationUser user = null;

            Assert.Throws<NullReferenceException>(() => _tokenService.CreateToken(user));
        }

        [Fact]
        public void Constructor_MissingSigningKey_ThrowsException()
        {
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(x => x["JWT:SigningKey"]).Returns((string)null);

            Assert.Throws<ArgumentNullException>(() => new TokenService(mockConfig.Object));
        }
    }
}