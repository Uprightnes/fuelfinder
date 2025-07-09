using System.Security.Cryptography;
using System.Text;
using FuelFinderApi.Data;
using FuelFinderApi.DTOs;
using FuelFinderApi.Mappers;
using FuelFinderApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FuelFinderApi.Services
{
    public class UserService: IUserService
    {
        private readonly ApplicationDBContext _context;

        public UserService(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<UserResponseDTO> RegisterAsync(UserRegisterRequestDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                throw new ArgumentException("Username, email, and password are required.");
            }

            if (await _context.FuelFinderUsers.AnyAsync(u => u.Email == request.Email && !u.IsSoftDeleted))
            {
                throw new InvalidOperationException("User already exists.");
            }

            string passwordHash;
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(request.Password);
                var hash = sha256.ComputeHash(bytes);
                passwordHash = Convert.ToBase64String(hash);
            }

            var user = request.ToModel(passwordHash); 
            _context.FuelFinderUsers.Add(user);
            await _context.SaveChangesAsync();

            return user.ToDto(); 
        }
    }
}

