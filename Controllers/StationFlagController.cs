using System.Security.Claims;
using FuelFinderApi.DTOs;
using FuelFinderApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FuelFinderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StationFlagController : ControllerBase
    {
        private readonly IStationFlagService _stationFlagService;
        public StationFlagController(IStationFlagService stationFlagService)
        {
            _stationFlagService = stationFlagService;
        }



        [Authorize]
        [HttpPost("{stationId}/flag")]
        public async Task<ActionResult> FlagStation(Guid stationId, StationFlagRequestDTO request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)
                ?? User.FindFirst("sub")
                ?? User.FindFirst("userId");

            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token");
            }

            if (!Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                return Unauthorized("Invalid user ID format");
            }

            await _stationFlagService.FlagStationAsync(stationId, request, userId);
            return NoContent();

        }

    }
}
