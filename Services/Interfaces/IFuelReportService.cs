using System.Drawing;
using System.Security.Claims;
using FuelFinderApi.DTOs;

namespace FuelFinderApi.Services.Interfaces
{
    public interface IFuelReportService
    {
        Task<FuelReportResponseDTO> SubmitReportAsync(FuelReportRequestDTO request, Guid userId);
        Task<IEnumerable<FuelReportResponseDTO>> GetReportsAsync(int page, int size, Guid? stationId);
        Task VoteOnReportAsync(Guid reportId, FuelReportVoteRequestDTO request, ClaimsPrincipal user);
    }
}
