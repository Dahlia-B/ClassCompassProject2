using Microsoft.Extensions.Logging;
using ClassCompassApp.Services;

namespace ClassCompassApp;

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
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // Register HTTP client
        builder.Services.AddHttpClient();
        
        // Register all services
        builder.Services.AddSingleton<ApiService>();
        builder.Services.AddSingleton<IGradesHttpService>();
        builder.Services.AddSingleton<HomeworkApi>();
        builder.Services.AddSingleton<AttendanceApi>();

        return builder.Build();
    }
}
