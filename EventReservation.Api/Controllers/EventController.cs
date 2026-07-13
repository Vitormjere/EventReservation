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
    public class EventController : ControllerBase {
        private readonly ApplicationDbContext _context;

        public EventController(ApplicationDbContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() {
            var events = await _context.Events
                .Include(e => e.Organizer)
                .Select(e => new EventResponse {
                    Id = e.Id,
                    Title = e.Title,
                    Description = e.Description,
                    EventDate = e.EventDate,
                    Location = e.Location,
                    Capacity = e.Capacity,
                    OrganizerName = e.Organizer.Name
                })
                .ToListAsync();

            return Ok(events);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) {
            var eventItem = await _context.Events
                .Include(e => e.Organizer)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (eventItem is null) {
                return NotFound(new { message = "Evento não encontrado." });
            }

            var response = new EventResponse {
                Id = eventItem.Id,
                Title = eventItem.Title,
                Description = eventItem.Description,
                EventDate = eventItem.EventDate,
                Location = eventItem.Location,
                Capacity = eventItem.Capacity,
                OrganizerName = eventItem.Organizer.Name
            };

            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = "Organizer,Admin")]
        public async Task<IActionResult> Create(EventRequest request) {
            var organizerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var newEvent = new Event {
                Title = request.Title,
                Description = request.Description,
                EventDate = request.EventDate,
                Location = request.Location,
                Capacity = request.Capacity,
                OrganizerId = organizerId
            };

            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();

            return Created(string.Empty, new { message = "Evento criado com sucesso.", eventId = newEvent.Id });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Organizer,Admin")]
        public async Task<IActionResult> Update(int id, EventRequest request) {
            var eventItem = await _context.Events.FirstOrDefaultAsync(e => e.Id == id);

            if (eventItem is null) {
                return NotFound(new { message = "Evento não encontrado." });
            }

            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var isAdmin = User.IsInRole("Admin");

            if (eventItem.OrganizerId != currentUserId && !isAdmin) {
                return Forbid();
            }

            eventItem.Title = request.Title;
            eventItem.Description = request.Description;
            eventItem.EventDate = request.EventDate;
            eventItem.Location = request.Location;
            eventItem.Capacity = request.Capacity;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Evento atualizado com sucesso." });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Organizer,Admin")]
        public async Task<IActionResult> Delete(int id) {
            var eventItem = await _context.Events.FirstOrDefaultAsync(e => e.Id == id);

            if (eventItem is null) {
                return NotFound(new { message = "Evento não encontrado." });
            }

            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var isAdmin = User.IsInRole("Admin");

            if (eventItem.OrganizerId != currentUserId && !isAdmin) {
                return Forbid();
            }

            _context.Events.Remove(eventItem);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Evento excluído com sucesso." });
        }
    }
}