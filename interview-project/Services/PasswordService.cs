using interview_project.Database.Entities;
using Microsoft.AspNetCore.Identity;

namespace interview_project.Services
{
    public class PasswordService
    {
        private readonly PasswordHasher<AppUser> _hasher = new();

        public string HashPassword(string password, AppUser user)
        {
            return _hasher.HashPassword(user, password);
        }

        public bool VerifyPassword(string hashedPassword, string inputPassword, AppUser user)
        {
            var result = _hasher.VerifyHashedPassword(user, hashedPassword, inputPassword);
            return result == PasswordVerificationResult.Success;
        }
    }
}