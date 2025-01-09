namespace EmployeeManagementSystem.Application.Dtos
{
    public record LoginResponseDto(bool flag, string Message = null!, string Token = null!, string RefreshToken = null!);
}
