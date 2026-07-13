namespace EventReservation.Api.DTOs {
    public class EventResponse {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime EventDate { get; set; }
        public string Location { get; set; }
        public int Capacity { get; set; }
        public string OrganizerName { get; set; }
    }
}