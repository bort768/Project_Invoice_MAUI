using Project_Invoice_MAUI.Views;

namespace Project_Invoice_MAUI;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(AddGoodsView), typeof(AddGoodsView));
        Routing.RegisterRoute(nameof(BankAccountView), typeof(BankAccountView));
        Routing.RegisterRoute(nameof(BossDataView), typeof(BossDataView));
        Routing.RegisterRoute(nameof(InvoiceViewSell), typeof(InvoiceViewSell));
        Routing.RegisterRoute(nameof(CompanyDataView), typeof(CompanyDataView));
    }
}
