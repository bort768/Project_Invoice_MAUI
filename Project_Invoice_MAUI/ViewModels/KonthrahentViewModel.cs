using Project_Invoice_MAUI.Models;
using Project_Invoice_MAUI.Services;
using Project_Invoice_MAUI.Singleton;
using Project_Invoice_MAUI.Views;

namespace Project_Invoice_MAUI.ViewModels
{
    public partial class KonthrahentViewModel : BaseViewModel
    {

        CompanyData company;
        Kontrahent kontrahent;

        Firma firma = Firma.GetInstance();

        Kontrahent kontrahent_Static = Firma.Static_Kontrahent;

        //private ObservableCollection<string> _listaDoCombobox;
        public ObservableCollection<string> listaDoCombobox { get; set; }

        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(Button_Is_Not_enable))]
        private bool _button_Is_Enabled;

        public bool Button_Is_Not_enable => !Button_Is_Enabled;

        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(Is_Not_Selected))]
        private bool _is_Selected;

        public bool Is_Not_Selected => !Is_Selected;



        [ObservableProperty]
        private string _bankAccount_Name;

        [ObservableProperty]
        private string _account_Number;

        [ObservableProperty]
        private string _full_Name;

        [ObservableProperty]
        private string _nIP;

        [ObservableProperty]
        private string _REGON;

        [ObservableProperty]
        private string _street;

        [ObservableProperty]
        private string _house_Number;

        [ObservableProperty]
        private string _apartment_Number;

        [ObservableProperty]
        private string _ZIP_Code;

        [ObservableProperty]
        private string _town;


        public KonthrahentViewModel()
        {
            if (kontrahent_Static is not null)
            {
                company = new CompanyData
                {
                    Full_Name     = kontrahent_Static.Company.Full_Name,  
                    NIP           = kontrahent_Static.Company.NIP,         
                    REGON         = kontrahent_Static.Company.REGON,       
                    Street        = kontrahent_Static.Company.Street,      
                    House_Number  = kontrahent_Static.Company.House_Number,
                    ZIP_Code      = kontrahent_Static.Company.ZIP_Code,    
                    Town          = kontrahent_Static.Company.Town
                };
                kontrahent = new(kontrahent_Static.BankAccount_Name, kontrahent_Static.Account_Number, company);

                Full_Name = kontrahent_Static.Company.Full_Name;
                NIP = kontrahent_Static.Company.NIP;
                REGON = kontrahent_Static.Company.REGON;
                Street = kontrahent_Static.Company.Street;
                House_Number = kontrahent_Static.Company.House_Number;
                ZIP_Code = kontrahent_Static.Company.ZIP_Code;
                Town = kontrahent_Static.Company.Town;

                BankAccount_Name = kontrahent_Static.BankAccount_Name;
                Account_Number = kontrahent_Static.Account_Number;
                kontrahent.ID = kontrahent_Static.ID;
                Is_Selected = kontrahent_Static.IsSelected;

            }
            else
            {
                //listaDoCombobox = new();

                firma.kontrahents = new();

                    
            }
        }

        #region Commands

        [ICommand]
        async Task GoBack()
        {
            await Shell.Current.GoToAsync($"{nameof(KontrahentCollectionView)}");
        }


        [ICommand]
        public async void DeleteKontrahent()
        {
            try
            {
                //var good = new Goods(_product_Name, _product_Code, _description, _price_Netto, _price_Brutto, _VAT, _Vat_Selected_Item);

                await KontrahentService.DeleteKontrahent(kontrahent);
                await Shell.Current.DisplayAlert("Towar usunięty", "Towar został usunięty", "ok");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Błąd", ex.Message, "ok");

            }

        }

        [ICommand]
        public async void UpdateKontrahent()
        {
            try
            {
                //var good = new Goods(_product_Name, _product_Code, _description, _price_Netto, _price_Brutto, _VAT, _Vat_Selected_Item);

                await KontrahentService.UpdateKontrahent(kontrahent);
                await Shell.Current.DisplayAlert("Towar zaktualizowany", "Towar zaktualizowany", "ok");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Błąd", ex.Message, "ok");
                
            }

        }

        [ICommand]
        public async void AddKontrahent()
        {
            //submit goods
            company = new CompanyData
            {
                Full_Name = _full_Name,
                NIP = _nIP,
                REGON = _REGON,
                Street = _street,
                House_Number = _house_Number,
                ZIP_Code = _ZIP_Code,
                Town = _town
            };
            kontrahent = new(BankAccount_Name, Account_Number, company);

            try
            {
                await KontrahentService.Addkontrahent(kontrahent);
                firma.kontrahents.Add(kontrahent);
                await ToastSaveSucces();
            }
            catch (Exception ex)
            {

                await Shell.Current.DisplayAlert("Błąd", ex.Message, "ok");
            }


        }

        #endregion


        
    }
}
