using Project_Invoice_MAUI.Models;
using Project_Invoice_MAUI.SaveFileHelper;
using Project_Invoice_MAUI.Services;
using Project_Invoice_MAUI.Singleton;
using Project_Invoice_MAUI.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Invoice_MAUI.ViewModels
{
    public partial class AddGoodsCollectionViewModel : BaseViewModel
    {

        /// <summary>
        /// Lista do colletionview
        /// </summary>
        
        public ObservableCollection<Goods> List_of_goods { get; set; } = new();

        Firma firma = Firma.GetInstance(); 

        public AddGoodsCollectionViewModel()
        {
            Title = "Lista produktów";


        }

        [ICommand]
        async Task GetGoodsAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                var Goods = await GoodsService.GetGoods();

                if (List_of_goods.Count != 0)
                    List_of_goods.Clear();


                //no nie jest to optymalne
                firma.goods = Goods;

                // wywala bład// juz nie
                Goods.ForEach(Goods => List_of_goods.Add(Goods));


                List_of_goods.Add(new Goods("Produkt", "23", "Tak", 23, 28.29, Vat_Helper.VAT_23, Vat_Helper.VAT_23_String));
                List_of_goods.Add(new Goods("Taśma", "78", "Tak", 40, 49.2, Vat_Helper.VAT_7, Vat_Helper.VAT_7_String));
                List_of_goods.Add(new Goods("UwU Shrek UwU", "69", "Tak", 100, 123, Vat_Helper.VAT_3, Vat_Helper.VAT_3_String));
            }
            catch (Exception ex)
            {              
                await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }

        }

        [ICommand]
        async Task GoToDetails(Goods goods)
        {
            if (goods == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(AddGoodsView)}", true, new Dictionary<string, object>
        {
            {"Goods", goods }
        });
        }

        [ICommand]
        async Task GoToDetailsEdit(Goods goods)
        {
            if (goods == null)
                return;

            //clear firma goods
            Firma.Static_Goods = null;

            //przypisanie wartosci
            Firma.Static_Goods = goods;
            await Shell.Current.GoToAsync($"{nameof(AddGoodsView)}", true);
        }


    }
}
