using Microsoft.Extensions.Logging;
using QuizzingApp341.Models;
using System.Windows;

namespace QuizzingApp341;
public static class MauiProgram 
{
    public static IBusinessLogic BusinessLogic = new BusinessLogic();

    public static MauiApp CreateMauiApp() {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts => {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
            builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
