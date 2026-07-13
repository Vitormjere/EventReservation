using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventReservation.Domain.Entities {
    public class Reservation {
        public int Id { get; private set; }
        public int EventId { get; set; }
        public Event Event { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public ReservationStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }


    }
}
