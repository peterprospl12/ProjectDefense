using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectDefense.ConsoleApp;
using ProjectDefense.ConsoleApp.Clients;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddHttpClient<ApiClient>(client =>
            {
                var baseUrl = context.Configuration["ApiBaseUrl"];
                if (string.IsNullOrEmpty(baseUrl))
                {
                    throw new InvalidOperationException("API Base URL is not configured in appsettings.json.");
                }
                client.BaseAddress = new Uri(baseUrl);
            })
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            });

        services.AddTransient<ConsoleApp>();
    })
    .Build();

var app = host.Services.GetRequiredService<ConsoleApp>();
await app.RunAsync();