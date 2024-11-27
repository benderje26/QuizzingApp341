using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using QuizzingApp341.Models;

namespace QuizzingApp341;
public static class MauiProgram {
    public static IBusinessLogic BusinessLogic = new BusinessLogic(new SupabaseDatabase());

    public static MauiApp CreateMauiApp() {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
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
