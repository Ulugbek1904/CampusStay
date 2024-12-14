using CampusStay.DTO.Users;

namespace CampusStay.DTO.Requests
{
    public class LoginResponseDto
    {
        public string Token { get; set; }
        public UserDto User { get; set; }
    }
}
