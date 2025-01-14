using EmployeeManagementSystem.Application.Dtos;

namespace EmployeeManagementSystem.Application.Services
{
    public interface IUserAccountService
    {
        Task<GeneralResponseDto> CreateAsync(RegisterDto? user);
        Task<LoginResponseDto> SignInAsync(LoginDto? user);
        Task<LoginResponseDto> RefreshTokenAsync(RefreshTokenDto token);
    }
}
