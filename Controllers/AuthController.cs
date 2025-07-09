using FuelFinderApi.DTOs;
using FuelFinderApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FuelFinderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDTO>> Login(LoginRequestDTO request)
        {
            var tokens = await _authService.LoginAsync(request);
            return Ok(tokens);
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<TokenResponseDTO>> Refresh(RefreshTokenRequestDTO request)
        {
            var tokens = await _authService.RefreshTokenAsync(request);
            return Ok(tokens);
        }
    }
}