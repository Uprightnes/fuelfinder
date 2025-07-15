using FuelFinderApi.DTOs;
using FuelFinderApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace FuelFinderApi.Controllers
{

    [Microsoft.AspNetCore.Mvc.Route("api/admin")]
    [ApiController]
    [Authorize(Policy = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost("stations/approve")]
        public async Task<IActionResult> ApproveStaion(ApproveStationDTO approveStation)
        {
            await _adminService.ApproveStationAsync(approveStation.StationId);
            return Ok();
        }

        [HttpPost("flags/resolve")]
        public async Task<IActionResult> ResolveFlag(Guid stationFlagId)
        {
            await _adminService.ResolveFlagAsync(stationFlagId);
            return Ok();

        }
    }

}
