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
            Task LoadList = GetGoodsAsync();
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
        async Task GoToAddGoods()
        {
            Firma.Static_Goods = null;
            
            await Shell.Current.GoToAsync($"{nameof(AddGoodsView)}", true);
        }

        [ICommand]
        async Task GoToDetailsEdit(Goods goods)
        {
            if (goods == null)
                return;

            //clear firma goods
            Firma.Static_Goods = null;
            goods.IsSelected = true;
            //przypisanie wartosci
            Firma.Static_Goods = goods;

            await Shell.Current.GoToAsync($"{nameof(AddGoodsView)}", true);
            //await Shell.Current.GoToAsync()
        }


    }
}
