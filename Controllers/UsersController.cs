using FuelFinderApi.DTOs;
using FuelFinderApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FuelFinderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<ActionResult<UserResponseDTO>> Register(UserRegisterRequestDTO request)
        {
            var user = await _userService.RegisterAsync(request);
            return CreatedAtAction(nameof(Register), 
                new { id = user.FuelFinderUserId }, 
                user);
        }

    }
}
