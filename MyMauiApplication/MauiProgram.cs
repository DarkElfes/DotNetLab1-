using Microsoft.Extensions.Logging;
using MyMauiApplication.Services;
using Library.IServices;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Http;
using MyMauiApplication.Handlers;
using Microsoft.AspNetCore.Components.Authorization;

namespace MyMauiApplication
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("MyMauiApplication.appsettings.json");

            var config = new ConfigurationBuilder()
                .AddJsonStream(stream)
                .Build();
            builder.Configuration.AddConfiguration(config);


            builder.Services.AddMauiBlazorWebView();


            builder.Services.AddAuthorizationCore();
            builder.Services.AddCascadingAuthenticationState();

            builder.Services.AddScoped<IVeilNavigationManager, VeilNavigationManager>();
            builder.Services.AddScoped<ICardServiceClient, CardServiceClient>();
            builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();
            builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();
            builder.Services.AddTransient<AuthenticationHandler>();

            builder.Services.AddHttpClient("ServerApi")
                   .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.Configuration["ServerUrl"] ?? ""))
                   .AddHttpMessageHandler<AuthenticationHandler>();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
