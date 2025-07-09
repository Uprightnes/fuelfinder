using FuelFinderApi.DTOs;

namespace FuelFinderApi.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserResponseDTO> RegisterAsync(UserRegisterRequestDTO request);
    }
}
