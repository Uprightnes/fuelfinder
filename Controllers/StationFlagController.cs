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
            await _stationFlagService.FlagStationAsync(stationId, request, Guid.NewGuid()); // Replace with JWT user ID
            return NoContent();
        }

    }
}
