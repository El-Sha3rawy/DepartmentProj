using Auth.Proto;
using FluentValidation;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using Shared.Proto;
using System.Globalization;
using TestDept;
using TestDept.UI_Validation;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddLocalization(options => options.ResourcesPath = "SharedResources");

builder.Services.AddScoped<IValidator<DeptModel>, CreateDeptValidator>();
builder.Services.AddScoped<IValidator<DeptModel>, UpdateDeptValidator>();


builder.Services.AddSingleton<DepartmentService.DepartmentServiceClient>(services =>
{
    var grpcWebHandler = new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler());
    var httpClient = new HttpClient(grpcWebHandler)
    {
        BaseAddress = new Uri("https://localhost:7137") 
    };
    var channel = GrpcChannel.ForAddress("https://localhost:7137", new GrpcChannelOptions
    {
        HttpClient = httpClient
    });
    return new DepartmentService.DepartmentServiceClient(channel);
});

builder.Services.AddSingleton<AuthService.AuthServiceClient>(services =>
{
    var grpcWebHandler = new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler());
    var httpClient = new HttpClient(grpcWebHandler)
    {
        BaseAddress = new Uri("https://localhost:7137") 
    };
    var channel = GrpcChannel.ForAddress("https://localhost:7137", new GrpcChannelOptions
    {
        HttpClient = httpClient
    });
    return new AuthService.AuthServiceClient(channel);
});

var Host = builder.Build();

var js = Host.Services.GetRequiredService<IJSRuntime>();
var savedCulture = await js.InvokeAsync<string>("blazorCulture.get") ?? "en-US";

var culture = new CultureInfo(savedCulture);
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

await Host.RunAsync();
