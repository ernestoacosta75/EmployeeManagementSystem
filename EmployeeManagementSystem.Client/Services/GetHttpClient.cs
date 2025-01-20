using Blazored.LocalStorage;
using EmployeeManagementSystem.Application.Dtos;
using EmployeeManagementSystem.Shared.Helpers;

namespace EmployeeManagementSystem.Client.Services
{
    public class GetHttpClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILocalStorageService _localStorageService;
        private const string HeaderKey = "Authorization";

        public GetHttpClient(IHttpClientFactory httpClientFactory, ILocalStorageService localStorageService)
        {
            _httpClientFactory = httpClientFactory;
            _localStorageService = localStorageService;
        }

        public async Task<HttpClient> GetPrivateHttpClient()
        {
            var client = _httpClientFactory.CreateClient("SystemApiClient");
            var stringToken = await _localStorageService.GetItemAsStringAsync(Constants.StorageKey);

            // If stringToken is null, it means that the use is not log in
            if (string.IsNullOrEmpty(stringToken)) return client;

            // If is present, it will be deserialized in UserSessionDto
            var deserializedToken = Serializations.DeserializeJsonString<UserSessionDto>(stringToken);

            if (deserializedToken == null) return client;

            // If is not null, add the token to the Authorization header
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", deserializedToken.Token);

            return client;
        }

        /**
         * This method returns an HTTP client for the public endpoint (those
         * that don't need an authorization header)
         */
        public HttpClient GetPublicHttpClient()
        {
            var client = _httpClientFactory.CreateClient("SystemApiClient");
            client.DefaultRequestHeaders.Remove(HeaderKey);

            return client;
        }
    }
}
