using FuelFinderApi.DTOs;

namespace FuelFinderApi.Services.Interfaces
{
    public interface IFuelReportService
    {
        Task<FuelReportResponseDTO> SubmitReportAsync(FuelReportRequestDTO request, Guid userId);
        Task<IEnumerable<FuelReportResponseDTO>> GetReportsAsync(Guid? stationId);
        Task VoteOnReportAsync(Guid reportId, FuelReportVoteRequestDTO request);
    }
}
