using DAL.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs
{
    public class UserRegistrationDto
    {
        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string PasswordHash { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Name { get; set; } = string.Empty;

        [EmailAddress]
        public string? Email { get; set; } = string.Empty;
    }

    public class UserLoginDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }

    public class UserResponseDto
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
    }

    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public UserResponseDto User { get; set; } = new();
        public DateTime ExpiresAt { get; set; }
    }
}
