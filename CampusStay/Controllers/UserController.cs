using CampusStay.DTO.Requests;
using CampusStay.Models.UserModels;
using CampusStay.Services.UserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using System;
using System.Threading.Tasks;

namespace CampusStay.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : RESTFulController
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [Authorize]

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
        {
            var userEmail = User.Identity.Name;

            if (request.NewPassword != request.ConfirmNewPassword)
                return BadRequest("New password and confirm password do not match");

            var user = await this.userService.GetUserByEmailAsync(userEmail);

            if (user == null)
                return BadRequest("User not found");

            if (user.Password == request.CurrentPassword)
            {
                user.Password = request.NewPassword;
                user.ConfirmPassword = request.ConfirmNewPassword;
                await this.userService.UpdateUserAsync(user);

                return Ok("Password changed successfully");
            }

            return BadRequest("Current Password is incorrect");
        }
    }
}
