using Blazored.LocalStorage;
using EmployeeManagementSystem.Client.Services;
using EmployeeManagementSystem.Client.Services.Storage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace EmployeeManagementSystem.Client;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

        builder.Services.AddBlazoredLocalStorage();
        builder.Services.AddSingleton(typeof(LocalStorageService));
        builder.Services.AddSingleton(typeof(GetHttpClient));
        //builder.Services.AddScoped<IUserAccountService, UserAccountClientService>();

        await builder.Build().RunAsync();
    }
}
