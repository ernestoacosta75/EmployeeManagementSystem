using EmployeeManagementSystem.Application.Dtos;

namespace EmployeeManagementSystem.Application.Services.UserAccount
{
    public class UserAccount : IUserAccount
    {
        public async Task<GeneralResponseDto> CreateAsync(RegisterDto user)
        {
            throw new NotImplementedException();
        }

        public async Task<LoginResponseDto> SignInAsync(LoginDto user)
        {
            throw new NotImplementedException();
        }
    }
}
