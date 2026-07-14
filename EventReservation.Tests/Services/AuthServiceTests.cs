using EventReservation.Api.Services;
using EventReservation.Domain.Entities;
using Xunit;

namespace EventReservation.Tests.Services {
    public class AuthServiceTests {
        [Fact]
        public void HashPassword_ReturnsHashDifferentFromOriginalPassword() {
            // Arrange
            var authService = new AuthService();
            var user = new User { Email = "test@test.com" };
            var plainPassword = "MinhaSenh@123";

            // Act
            var hashedPassword = authService.HashPassword(user, plainPassword);

            // Assert
            Assert.NotEqual(plainPassword, hashedPassword);
        }

        [Fact]
        public void VerifyPassword_WithCorrectPassword_ReturnsTrue() {
            // Arrange
            var authService = new AuthService();
            var user = new User { Email = "test@test.com" };
            var plainPassword = "MinhaSenh@123";
            var hashedPassword = authService.HashPassword(user, plainPassword);

            // Act
            var isValid = authService.VerifyPassword(user, hashedPassword, plainPassword);

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void VerifyPassword_WithIncorrectPassword_ReturnsFalse() {
            // Arrange
            var authService = new AuthService();
            var user = new User { Email = "test@test.com" };
            var correctPassword = "MinhaSenh@123";
            var wrongPassword = "SenhaErrada456";
            var hashedPassword = authService.HashPassword(user, correctPassword);

            // Act
            var isValid = authService.VerifyPassword(user, hashedPassword, wrongPassword);

            // Assert
            Assert.False(isValid);
        }
    }
}