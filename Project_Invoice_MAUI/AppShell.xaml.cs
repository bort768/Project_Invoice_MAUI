using Project_Invoice_MAUI.Views;

namespace Project_Invoice_MAUI;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute($"{nameof(AddGoodsCollectionView)}/{nameof(AddGoodsView)}", typeof(AddGoodsView));
        Routing.RegisterRoute(nameof(AddGoodsCollectionView), typeof(AddGoodsCollectionView));

        Routing.RegisterRoute(nameof(InvoiceViewSell), typeof(InvoiceViewSell));

        //Company data
        Routing.RegisterRoute(nameof(CompanyDataView), typeof(CompanyDataView));
        Routing.RegisterRoute($"{nameof(CompanyDataView)}/{nameof(BankAccountView)}", typeof(BankAccountView));
        Routing.RegisterRoute($"{nameof(CompanyDataView)}/{nameof(BossDataView)}", typeof(BossDataView));

        Routing.RegisterRoute(nameof(HomeView), typeof(HomeView));

        Routing.RegisterRoute(nameof(KontrahenciView), typeof(KontrahenciView));
        //Routing.RegisterRoute(nameof(AddGoodsView), typeof(AddGoodsView));
    }
}
