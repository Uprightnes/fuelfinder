using FuelFinderApi.DTOs;

namespace FuelFinderApi.Services.Interfaces
{
    public interface IAuthService
    {
        Task<TokenResponseDTO> LoginAsync(LoginRequestDTO request);
        Task<TokenResponseDTO> RefreshTokenAsync(RefreshTokenRequestDTO request);
    }
}
