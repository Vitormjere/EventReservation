using System.ComponentModel.DataAnnotations;

namespace EventReservation.Api.DTOs {
    public class ReservationRequest {
        [Required(ErrorMessage = "O Id do evento é obrigatório.")]
        public int EventId { get; set; }
    }
}