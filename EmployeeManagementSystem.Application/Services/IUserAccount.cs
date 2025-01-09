using EmployeeManagementSystem.Application.Dtos;

namespace EmployeeManagementSystem.Application.Services
{
    public interface IUserAccount
    {
        Task<GeneralResponseDto> CreateAsync(RegisterDto user);
        Task<LoginResponseDto> SignInAsync(LoginDto user);
    }
}
