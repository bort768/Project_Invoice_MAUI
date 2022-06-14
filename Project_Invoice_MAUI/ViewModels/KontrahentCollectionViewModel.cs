using Project_Invoice_MAUI.Models;
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
    public partial class KontrahentCollectionViewModel : BaseViewModel 
    {
        public ObservableCollection<Kontrahent> List_of_Kontrahents { get; set; } = new();

        Firma firma = Firma.GetInstance();

        public KontrahentCollectionViewModel()
        {
            Title = "Lista Kontrahentów";
            Task loadKontrahent = GetKontrahents();
        }

        [ICommand]
        async Task GetKontrahents()
        {
            
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                var kontrahents = await KontrahentService.GetKontrahents();

                if (List_of_Kontrahents.Count != 0)
                    List_of_Kontrahents.Clear();

                //AddRadomData();
                //no nie jest to optymalne
                firma.kontrahents = kontrahents;

                // wywala bład// juz nie
                kontrahents.ForEach(kontrahent => List_of_Kontrahents.Add(kontrahent));



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

        private void AddRadomData()
        {
            List_of_Kontrahents.Add(new Kontrahent("BPK Oddział III w Brzozowie", "15 0011 0018 9123 3456 4567 8912",
                                    new CompanyData
                                    {
                                        Full_Name = "Dom Towarowy „KROS” SA",
                                        NIP = "117-00-88-765",
                                        REGON = "986674453",
                                        Street = "Brzozowska",
                                        House_Number = "56",
                                        ZIP_Code = "36-200",
                                        Town = "Brzozów"
                                    }));

            List_of_Kontrahents.Add(new Kontrahent("BT Oddział w Brzozowie", "19 0000 1245 6778 9189 1223 3456",
                new CompanyData
                {
                    Full_Name = "Dom Handlowy „Mrówka” sp. z.o.o.",
                    NIP = "823 12 20 711",
                    REGON = "74185296",
                    Street = "Obwodowa",
                    House_Number = "2",
                    ZIP_Code = "36-200",
                    Town = "Brzozów"
                }));
        }



        [ICommand]
        async Task GoToAddKontrahents()
        {
            await Shell.Current.GoToAsync(nameof(KontrahenciView), true);
        }

        [ICommand]
        async Task GoToAdd()
        {
            await Shell.Current.GoToAsync(nameof(KontrahenciView), true);
        }



        [ICommand]
        async Task GoToDetailsEdit(Kontrahent kontrahent)
        {
            if (kontrahent == null)
                return;

            //clear firma goods
            Firma.Static_Kontrahent = null;
            kontrahent.IsSelected = true;
            //przypisanie wartosci
            Firma.Static_Kontrahent = kontrahent;

            await Shell.Current.GoToAsync($"{nameof(KontrahentCollectionView)}/{nameof(KontrahenciView)}", true);
            //await Shell.Current.GoToAsync()
        }
    }
}
