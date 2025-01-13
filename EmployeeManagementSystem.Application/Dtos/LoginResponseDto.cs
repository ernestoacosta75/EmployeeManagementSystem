namespace EmployeeManagementSystem.Application.Dtos
{
    public record LoginResponseDto(
        bool Flag, 
        string Message = null!, 
        string Token = null!, 
        string RefreshToken = null!);
}
