using EventReservation.Domain.Entities;

namespace EventReservation.Api.Services {
    public interface IAuthService {
        string HashPassword(User user, string password);
        bool VerifyPassword(User user, string hashedPassword, string providedPassword);
    }
}