using CommunityToolkit.Maui;
using Project_Invoice_MAUI.ViewModels;
using Project_Invoice_MAUI.Views;

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

        builder.UseMauiApp<App>().UseMauiCommunityToolkit();

        builder.Services.AddSingleton<CompanyDataViewModel>();
        builder.Services.AddSingleton<BankAccountViewModel>();
        builder.Services.AddSingleton<BossDataViewModel>();
        builder.Services.AddSingleton<InvoiceViewModel>();
        builder.Services.AddSingleton<KonthrahentViewModel>();
        builder.Services.AddSingleton<AddGoodsCollectionViewModel>();
        builder.Services.AddSingleton<KontrahentCollectionViewModel>();

        builder.Services.AddTransient<KonthrahentViewModel>();
        builder.Services.AddTransient<KontrahenciView>();

        builder.Services.AddTransient<AddGoodsViewModel>();
        builder.Services.AddTransient<AddGoodsView>();


        return builder.Build();


    }
}
