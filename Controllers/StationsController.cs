using System.Security.Claims;
using FuelFinderApi.DTOs;
using FuelFinderApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FuelFinderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StationsController : ControllerBase
    {
        private readonly IStationService _stationService;

        public StationsController(IStationService stationService)
        {
            _stationService = stationService;
        }

        [HttpGet("nearby")]
        public async Task<ActionResult<IEnumerable<StationResponseDTO>>> GetNearbyStations(decimal latitude, decimal longitude, double radius)
        {
            var stations = await _stationService.GetNearbyStationsAsync(latitude, longitude, radius);
            return Ok(stations);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<StationResponseDTO>> AddStation(StationResponseDTO request)
        {
            // Extract userId from the authenticated user's claims
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized("User ID not found or invalid.");
            }

            try
            {
                var station = await _stationService.AddStationAsync(request, userId);
                return CreatedAtAction(nameof(GetNearbyStations), new { latitude = station.StationLatitude, longitude = station.StationLongitude }, station);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}
