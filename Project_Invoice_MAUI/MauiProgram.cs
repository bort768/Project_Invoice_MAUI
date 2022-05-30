using Project_Invoice_MAUI.ViewModels;

namespace Project_Invoice_MAUI;

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

        builder.Services.AddSingleton<CompanyDataViewModel>();
        builder.Services.AddSingleton<BankAccountViewModel>();
        builder.Services.AddSingleton<BossDataViewModel>();
        builder.Services.AddSingleton<InvoiceViewModel>();
        builder.Services.AddSingleton<KonthrahentViewModel>();


        return builder.Build();


    }
}
