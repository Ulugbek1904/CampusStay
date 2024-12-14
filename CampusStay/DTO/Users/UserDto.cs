using CampusStay.Models.UserModels;
using System;

namespace CampusStay.DTO.Users
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public UserRole Role { get; set; }
    }
}
