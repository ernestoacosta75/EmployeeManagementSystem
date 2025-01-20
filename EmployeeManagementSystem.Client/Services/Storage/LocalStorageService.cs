using Blazored.LocalStorage;
using EmployeeManagementSystem.Shared.Helpers;

namespace EmployeeManagementSystem.Client.Services.Storage
{
    public class LocalStorageService
    {
        private readonly ILocalStorageService _localStorageService;

        public LocalStorageService(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        public async Task<string> GetToken(string storageKey) => await _localStorageService.GetItemAsStringAsync(storageKey);
        public async Task SetToken(string item) =>
            await _localStorageService.SetItemAsStringAsync(Constants.StorageKey, item);

        public async Task RemoveToken(string storageKey) => await _localStorageService.RemoveItemAsync(storageKey);

        public async Task<bool> ContainsKey(string storageKey) => await _localStorageService.ContainKeyAsync(storageKey);
    }
}
