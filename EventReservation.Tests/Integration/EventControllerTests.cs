using System.Net;
using System.Net.Http.Json;
using EventReservation.Api.DTOs;
using Xunit;

namespace EventReservation.Tests.Integration {
    public class EventControllerTests : IClassFixture<CustomWebApplicationFactory> {
        private readonly HttpClient _client;

        public EventControllerTests(CustomWebApplicationFactory factory) {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CreateEvent_WithoutToken_ReturnsUnauthorized() {
            // Arrange
            var request = new EventRequest {
                Title = "Evento Teste",
                Description = "Descrição do evento teste",
                EventDate = DateTime.UtcNow.AddDays(10),
                Location = "São Paulo, SP",
                Capacity = 50
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/Events", request);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}