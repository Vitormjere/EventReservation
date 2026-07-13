using System.ComponentModel.DataAnnotations;

namespace EventReservation.Api.DTOs {
    public class EventRequest {
        [Required(ErrorMessage = "O título é obrigatório.")]
        [MaxLength(200, ErrorMessage = "O título deve ter no máximo 200 caracteres.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "A descrição é obrigatória.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "A data do evento é obrigatória.")]
        public DateTime EventDate { get; set; }

        [Required(ErrorMessage = "O local é obrigatório.")]
        [MaxLength(200, ErrorMessage = "O local deve ter no máximo 200 caracteres.")]
        public string Location { get; set; }

        [Required(ErrorMessage = "A capacidade é obrigatória.")]
        [Range(1, int.MaxValue, ErrorMessage = "A capacidade deve ser maior que zero.")]
        public int Capacity { get; set; }
    }
}