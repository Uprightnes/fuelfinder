namespace FuelFinderApi.Services.Interfaces
{
    public interface IAdminService
    {
        Task ApproveStationAsync(Guid stationId);
        Task ResolveFlagAsync(Guid stationFlagId);
    }
}
