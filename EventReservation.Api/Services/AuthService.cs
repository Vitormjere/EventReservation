using EventReservation.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace EventReservation.Api.Services {
    public class AuthService : IAuthService {
        private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

        public string HashPassword(User user, string password) {
            return _passwordHasher.HashPassword(user, password);
        }

        public bool VerifyPassword(User user, string hashedPassword, string providedPassword) {
            var result = _passwordHasher.VerifyHashedPassword(user, hashedPassword, providedPassword);
            return result == PasswordVerificationResult.Success;
        }
    }
}