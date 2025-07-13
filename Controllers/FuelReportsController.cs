using System.Security.Claims;
using FuelFinderApi.DTOs;
using FuelFinderApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FuelFinderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FuelReportsController : ControllerBase
    {
        private readonly IFuelReportService _fuelReportService;

        public FuelReportsController(IFuelReportService fuelReportService)
        {
            _fuelReportService = fuelReportService;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<FuelReportResponseDTO>> SubmitReport(FuelReportRequestDTO request)
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
            var report = await _fuelReportService.SubmitReportAsync(request, userId);
            return CreatedAtAction(nameof(GetReports), new { stationId = report.StationId }, report);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FuelReportResponseDTO>>> GetReports(Guid? stationId)
        {
            var reports = await _fuelReportService.GetReportsAsync(stationId);
            return Ok(reports);
        }

        [Authorize]
        [HttpPost("{reportId}/vote")]
        public async Task<ActionResult> VoteOnReport(Guid reportId, FuelReportVoteRequestDTO request)
        {
            await _fuelReportService.VoteOnReportAsync(reportId, request, User);
            return NoContent();
        }
    }

}