using System.Security.Claims;
using EventReservation.Api.DTOs;
using EventReservation.Domain.Entities;
using EventReservation.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventReservation.Api.Controllers {
    [ApiController]
    [Route("api/[controller]s")]
    [Authorize]
    public class ReservationController : ControllerBase {
        private readonly ApplicationDbContext _context;

        public ReservationController(ApplicationDbContext context) {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReservationRequest request) {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var eventItem = await _context.Events
                .Include(e => e.Reservations)
                .FirstOrDefaultAsync(e => e.Id == request.EventId);

            if (eventItem is null) {
                return NotFound(new { message = "Evento não encontrado." });
            }

            var (canReserve, errorMessage) = eventItem.CanReserve(userId, DateTime.UtcNow);

            if (!canReserve) {
                return BadRequest(new { message = errorMessage });
            }

            var reservation = new Reservation {
                EventId = eventItem.Id,
                UserId = userId,
                Status = ReservationStatus.Confirmed,
                CreatedAt = DateTime.UtcNow
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            return Created(string.Empty, new { message = "Reserva realizada com sucesso.", reservationId = reservation.Id });
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetMyReservations() {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var reservations = await _context.Reservations
                .Include(r => r.Event)
                .Where(r => r.UserId == userId)
                .Select(r => new ReservationResponse {
                    Id = r.Id,
                    EventId = r.EventId,
                    EventTitle = r.Event.Title,
                    EventDate = r.Event.EventDate,
                    Status = r.Status.ToString(),
                    CreatedAt = r.CreatedAt
                })
                .ToListAsync();

            return Ok(reservations);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Cancel(int id) {
            var reservation = await _context.Reservations.FirstOrDefaultAsync(r => r.Id == id);

            if (reservation is null) {
                return NotFound(new { message = "Reserva não encontrada." });
            }

            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var isAdmin = User.IsInRole("Admin");

            if (reservation.UserId != currentUserId && !isAdmin) {
                return Forbid();
            }

            reservation.Status = ReservationStatus.Cancelled;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Reserva cancelada com sucesso." });
        }
    }
}