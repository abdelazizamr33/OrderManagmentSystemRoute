using AutoMapper;
using DAL.DTOs;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.MappingProfiles
{
    public class UserProfiles:Profile
    {
        public UserProfiles()
        {
            CreateMap<UserLoginDto,User>().ReverseMap();
            CreateMap<UserRegistrationDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.PasswordHash))
                .ReverseMap();
            CreateMap<User, UserResponseDto>();

        }
    }
}
