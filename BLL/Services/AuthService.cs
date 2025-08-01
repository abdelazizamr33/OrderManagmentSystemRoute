﻿using AutoMapper;
using BLL.Interfaces;
using BLL.Interfaces.Services;
using DAL.Data.Contexts;
using DAL.DTOs;
using DAL.Models;
using DAL.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        public AuthService(IUnitOfWork _unitOfWork,IMapper _mapper,IConfiguration _configuration)
        {
            unitOfWork = _unitOfWork;
            mapper = _mapper;
            configuration = _configuration;
        }


        public async Task<LoginResponseDto?> LoginAsync(UserLoginDto loginDto)
        {
            var user = await GetUserByUsernameAsync(loginDto.Username);
            if (user == null) return null;

            if (!VerifyPassword(loginDto.Password, user.PasswordHash))
                return null;

            var token = GenerateJwtToken(user);
            var expiresAt = DateTime.UtcNow.AddMinutes(60);

            return new LoginResponseDto
            {
                Token = token,
                User = mapper.Map<UserResponseDto>(user),
                ExpiresAt = expiresAt
            };
        }

        public async Task<UserResponseDto?> RegisterAsync(UserRegistrationDto registrationDto)
        {
            // Check if username already exists
            var existingUser = await GetUserByUsernameAsync(registrationDto.Username);
            if (existingUser != null)
                throw new InvalidOperationException("Username already exists");

            // Check if email already exists (check from Customer repository)
            var existingEmail = await unitOfWork.GetRepository<Customer>()
                .GetAllAsync(false);
            if (existingEmail.Any(c => c.Email == registrationDto.Email))
                throw new InvalidOperationException("Email already exists");

            // Create user
            var user = new User
            {
                Username = registrationDto.Username,
                PasswordHash = HashPassword(registrationDto.PasswordHash),
                Role = (registrationDto.Name != null && registrationDto.Email != null) ? UserRole.Customer : UserRole.Admin
            };

            await unitOfWork.GetRepository<User>().AddAsync(user);
            await unitOfWork.SaveChangesAsync();

            // Get the newly created user
            var createdUser = await GetUserByUsernameAsync(registrationDto.Username);

            // If user is a customer, create a Customer entity
            if (user.Role == UserRole.Customer)
            {
                var customer = new Customer
                {
                    Name = registrationDto.Name!,
                    Email = registrationDto.Email!,
                    User = createdUser! // set navigation property
                };

                await unitOfWork.GetRepository<Customer>().AddAsync(customer);
                await unitOfWork.SaveChangesAsync();
            }

            return mapper.Map<UserResponseDto>(user);
        }


        public string GenerateJwtToken(User user)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<User?> GetUserFromTokenAsync(string token)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"]!;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = key,
                    ClockSkew = TimeSpan.Zero
                }, out _);

                // Extract the user ID from the claim
                var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                    return null;

                var userId = int.Parse(userIdClaim.Value);

                // Fetch the user from database
                var user = await unitOfWork.GetRepository<User>().GetByIdAsync(userId);
                return user;
            }
            catch
            {
                return null;
            }
        }
        

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            var users = await unitOfWork.GetRepository<User>().GetAllAsync(false);
            var user = users.FirstOrDefault(u => u.Customer != null && u.Username == username);
            return user;
        }
        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private static bool VerifyPassword(string password, string hash)
        {
            return HashPassword(password) == hash;
        }

    }
}
