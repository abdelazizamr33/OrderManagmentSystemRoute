using DAL.DTOs;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> LoginAsync(UserLoginDto loginDto);
        Task<UserResponseDto?> RegisterAsync(UserRegistrationDto registrationDto);
        string GenerateJwtToken(User user);
        Task<User?> GetUserFromTokenAsync(string token);
    }
}
