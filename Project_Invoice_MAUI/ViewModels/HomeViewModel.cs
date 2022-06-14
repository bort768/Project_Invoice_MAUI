using Project_Invoice_MAUI.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Invoice_MAUI.ViewModels
{
    public partial class HomeViewModel : BaseViewModel
    {
        [ICommand]
        public async void GoToCompanyDataView()
        {
            await Shell.Current.GoToAsync(nameof(CompanyDataView));
        }

        [ICommand]
        public async void GoToGoods()
        {
            await Shell.Current.GoToAsync(nameof(AddGoodsCollectionView));
        }

        [ICommand]
        public async void GoToKontrahents()
        {
            await Shell.Current.GoToAsync(nameof(KontrahentCollectionView));
        }

        [ICommand]
        public async void GoToInvoiceSell()
        {
            await Shell.Current.GoToAsync(nameof(InvoiceViewSell));
        }

    }
}
