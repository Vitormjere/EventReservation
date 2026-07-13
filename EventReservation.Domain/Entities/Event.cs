using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventReservation.Domain.Entities {
    public class Event {
        public int Id { get; private set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime EventDate { get; set; }
        public string Location { get; set; }
        public int Capacity { get; set; }
        public int OrganizerId { get; set; }
        public User Organizer { get; set; }
        public List<Reservation> Reservations { get; set; } = new List<Reservation>();

        public (bool CanReserve, string? ErrorMessage) CanReserve(int userId, DateTime currentDate) {
            if (EventDate < currentDate) {
                return (false, "Não é possível reservar um evento que já ocorreu.");
            }

            var confirmedReservations = Reservations.Count(r => r.Status == ReservationStatus.Confirmed);

            if (confirmedReservations >= Capacity) {
                return (false, "Este evento atingiu a capacidade máxima de participantes.");
            }

            var alreadyReserved = Reservations.Any(r => r.UserId == userId && r.Status == ReservationStatus.Confirmed);

            if (alreadyReserved) {
                return (false, "Você já possui uma reserva confirmada para este evento.");
            }

            return (true, null);
        }
    }
}
