namespace EventReservation.Api.DTOs {
    public class ReservationResponse {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string EventTitle { get; set; }
        public DateTime EventDate { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}