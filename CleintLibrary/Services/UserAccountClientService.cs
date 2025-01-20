using Blazored.LocalStorage;
using EmployeeManagementSystem.Application.Dtos;
using EmployeeManagementSystem.Application.Services;

namespace ClientLibrary.Services
{
    public class UserAccountClientService : IUserAccountService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorageService;

        public UserAccountClientService(HttpClient httpClient, ILocalStorageService localStorageService)
        {
            ArgumentNullException.ThrowIfNull(httpClient);
            ArgumentNullException.ThrowIfNull(localStorageService);
            
            _httpClient = httpClient;
            _localStorageService = localStorageService;
        }
        public async Task<GeneralResponseDto> CreateAsync(RegisterDto? user)
        {
            ArgumentNullException.ThrowIfNull(user);
            throw new NotImplementedException();
        }

        public async Task<LoginResponseDto> SignInAsync(LoginDto? user)
        {
            ArgumentNullException.ThrowIfNull(user);
            throw new NotImplementedException();
        }

        public async Task<LoginResponseDto> RefreshTokenAsync(RefreshTokenDto? token)
        {
            ArgumentNullException.ThrowIfNull(token);
            throw new NotImplementedException();
        }

        public async Task<WeatherForecast[]> GetWeatherForecast()
        {
            throw new NotImplementedException();
        }
    }
}
