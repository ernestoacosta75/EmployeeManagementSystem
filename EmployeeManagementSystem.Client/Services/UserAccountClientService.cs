using System.Net.Http.Json;
using System.Text.Json;
using Blazored.LocalStorage;
using EmployeeManagementSystem.Application.Dtos;
using EmployeeManagementSystem.Application.Services;
using EmployeeManagementSystem.Client.Services;
using EmployeeManagementSystem.Shared.Helpers;

namespace ClientLibrary.Services
{
    public class UserAccountClientService : IUserAccountService
    {
        public const string AuthUrl = "api/authentication";
        private readonly GetHttpClient _getHttpClient;
        private readonly ILocalStorageService _localStorageService;

        public UserAccountClientService(GetHttpClient getHttpClient, ILocalStorageService localStorageService)
        {
            ArgumentNullException.ThrowIfNull(getHttpClient);
            ArgumentNullException.ThrowIfNull(localStorageService);

            _getHttpClient = getHttpClient;
            _localStorageService = localStorageService;
        }
        public async Task<GeneralResponseDto> CreateAsync(RegisterDto? user)
        {
            ArgumentNullException.ThrowIfNull(user);

            var httpClient = _getHttpClient.GetPublicHttpClient();
            var result = await httpClient.PostAsJsonAsync($"{AuthUrl}/register", user);

            if (!result.IsSuccessStatusCode) return new GeneralResponseDto(false, "Error occurred");

            return (await result.Content.ReadFromJsonAsync<GeneralResponseDto>())!;
        }

        public async Task<LoginResponseDto> SignInAsync(LoginDto? user)
        {
            ArgumentNullException.ThrowIfNull(user);

            var httpClient = _getHttpClient.GetPublicHttpClient();
            var result = await httpClient.PostAsJsonAsync($"{AuthUrl}/login", user);

            if (!result.IsSuccessStatusCode) return new LoginResponseDto(false, "Error occurred");

            return (await result.Content.ReadFromJsonAsync<LoginResponseDto>())!;
        }

        public async Task<LoginResponseDto> RefreshTokenAsync(RefreshTokenDto? token)
        {
            ArgumentNullException.ThrowIfNull(token);
            throw new NotImplementedException();
        }

        public async Task<WeatherForecast[]> GetWeatherForecast()
        {
            var httpClient = await _getHttpClient.GetPrivateHttpClient();
            var result = await httpClient.GetFromJsonAsync<WeatherForecast[]>("api/weatherforecast");

            return result;
        }
    }
}
