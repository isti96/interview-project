using interview_project.Database.Entities;
using Microsoft.AspNetCore.Identity;

namespace interview_project.Services
{
    public interface IPasswordService
    {
        string HashPassword(AppUser user, string password);

        bool VerifyPassword(AppUser user, string hashedPassword, string inputPassword);
    }

    public class PasswordService : IPasswordService
    {
        private readonly IPasswordHasher<AppUser> _passwordHasher;

        public PasswordService(IPasswordHasher<AppUser> passwordHasher)
        {
            _passwordHasher = passwordHasher;
        }

        public string HashPassword(AppUser user, string password)
        {
            return _passwordHasher.HashPassword(user, password);
        }

        public bool VerifyPassword(AppUser user, string hashedPassword, string inputPassword)
        {
            var result = _passwordHasher.VerifyHashedPassword(user, hashedPassword, inputPassword);
            return result == PasswordVerificationResult.Success;
        }
    }
}