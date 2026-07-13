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

    }
}
