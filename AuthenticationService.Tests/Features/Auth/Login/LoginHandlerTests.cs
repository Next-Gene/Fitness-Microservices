using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AuthenticationService.Contarcts;
using AuthenticationService.Features.Auth.Login;
using AuthenticationService.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace AuthenticationService.Tests.Features.Auth.Login
{
    public class LoginHandlerTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly LoginHandler _handler;

        public LoginHandlerTests()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            _tokenServiceMock = new Mock<ITokenService>();
            _handler = new LoginHandler(_userManagerMock.Object, _tokenServiceMock.Object);
        }

        [Fact]
        public async Task Handle_UserNotFound_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((ApplicationUser)null);

            var command = new LoginCommand("nonexistent@example.com", "Password123!", false);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<UnauthorizedAccessException>().WithMessage("Invalid email or password.");
        }

        [Fact]
        public async Task Handle_InvalidPassword_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            var user = new ApplicationUser { Email = "test@example.com" };
            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            _userManagerMock.Setup(x => x.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            var command = new LoginCommand("test@example.com", "WrongPassword", false);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<UnauthorizedAccessException>().WithMessage("Invalid email or password.");
        }

        [Fact]
        public async Task Handle_ValidCredentials_ReturnsLoginResponse()
        {
            // Arrange
            var user = new ApplicationUser 
            { 
                Id = Guid.NewGuid(), 
                Email = "test@example.com", 
                UserName = "testuser",
                FirstName = "Test",
                LastName = "User"
            };
            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            _userManagerMock.Setup(x => x.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(true);
            _tokenServiceMock.Setup(x => x.GenerateTokensAsync(It.IsAny<ApplicationUser>(), It.IsAny<bool>()))
                .ReturnsAsync(("access_token", "refresh_token"));
            _userManagerMock.Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(new List<string> { "User" });

            var command = new LoginCommand("test@example.com", "CorrectPassword123!", true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.UserId.Should().Be(user.Id);
            result.Token.Should().Be("access_token");
            result.RefreshToken.Should().Be("refresh_token");
            result.Roles.Should().Contain("User");
        }
    }
}
