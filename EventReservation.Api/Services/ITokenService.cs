using EventReservation.Domain.Entities;

namespace EventReservation.Api.Services {
    public interface ITokenService {
        string GenerateToken(User user);
    }
}