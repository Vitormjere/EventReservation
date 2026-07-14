using System.Net;
using System.Net.Http.Json;
using EventReservation.Api.DTOs;
using Xunit;

namespace EventReservation.Tests.Integration {
    public class AuthControllerTests : IClassFixture<CustomWebApplicationFactory> {
        private readonly HttpClient _client;

        public AuthControllerTests(CustomWebApplicationFactory factory) {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Register_WithValidData_ReturnsCreated() {
            // Arrange
            var request = new RegisterRequest {
                Name = "Maria Teste",
                Email = "maria@teste.com",
                Password = "senha123"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/Auth/register", request);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task Login_WithValidCredentials_ReturnsOkWithToken() {
            // Arrange
            var registerRequest = new RegisterRequest {
                Name = "Carlos Teste",
                Email = "carlos@teste.com",
                Password = "senha123"
            };

            await _client.PostAsJsonAsync("/api/Auth/register", registerRequest);

            var loginRequest = new LoginRequest {
                Email = "carlos@teste.com",
                Password = "senha123"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/Auth/login", loginRequest);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}