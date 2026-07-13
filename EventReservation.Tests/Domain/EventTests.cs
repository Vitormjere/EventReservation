using EventReservation.Domain.Entities;
using Xunit;

namespace EventReservation.Tests.Domain {
    public class EventTests {
        [Fact]
        public void CanReserve_WhenEventIsFull_ReturnsFalse() {
            // Arrange
            var eventItem = new Event {
                Capacity = 2,
                EventDate = DateTime.UtcNow.AddDays(5),
                Reservations = new List<Reservation>
                {
                    new Reservation { UserId = 1, Status = ReservationStatus.Confirmed },
                    new Reservation { UserId = 2, Status = ReservationStatus.Confirmed }
                }
            };

            // Act
            var (canReserve, errorMessage) = eventItem.CanReserve(userId: 3, currentDate: DateTime.UtcNow);

            // Assert
            Assert.False(canReserve);
            Assert.Equal("Este evento atingiu a capacidade máxima de participantes.", errorMessage);
        }

        [Fact]
        public void CanReserve_WhenEventHasAlreadyOccurred_ReturnsFalse() {
            // Arrange
            var eventItem = new Event {
                Capacity = 10,
                EventDate = DateTime.UtcNow.AddDays(-5), // evento 5 dias no passado
                Reservations = new List<Reservation>()
            };

            // Act
            var (canReserve, errorMessage) = eventItem.CanReserve(userId: 1, currentDate: DateTime.UtcNow);

            // Assert
            Assert.False(canReserve);
            Assert.Equal("Não é possível reservar um evento que já ocorreu.", errorMessage);
        }

        [Fact]
        public void CanReserve_WhenUserAlreadyReserved_ReturnsFalse() {
            // Arrange
            var eventItem = new Event {
                Capacity = 10,
                EventDate = DateTime.UtcNow.AddDays(5),
                Reservations = new List<Reservation>
                {
            new Reservation { UserId = 1, Status = ReservationStatus.Confirmed }
        }
            };

            // Act
            var (canReserve, errorMessage) = eventItem.CanReserve(userId: 1, currentDate: DateTime.UtcNow);

            // Assert
            Assert.False(canReserve);
            Assert.Equal("Você já possui uma reserva confirmada para este evento.", errorMessage);
        }

        [Fact]
        public void CanReserve_WhenEventHasAvailableSpotsAndUserHasNotReserved_ReturnsTrue() {
            // Arrange
            var eventItem = new Event {
                Capacity = 10,
                EventDate = DateTime.UtcNow.AddDays(5),
                Reservations = new List<Reservation>
                {
            new Reservation { UserId = 1, Status = ReservationStatus.Confirmed }
        }
            };

            // Act
            var (canReserve, errorMessage) = eventItem.CanReserve(userId: 2, currentDate: DateTime.UtcNow);

            // Assert
            Assert.True(canReserve);
            Assert.Null(errorMessage);
        }
    }
}