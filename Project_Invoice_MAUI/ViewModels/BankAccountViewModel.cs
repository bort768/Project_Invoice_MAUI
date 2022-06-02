using Project_Invoice_MAUI.Models;
using Project_Invoice_MAUI.Singleton;
using Project_Invoice_MAUI.Views;

namespace Project_Invoice_MAUI.ViewModels
{
    public partial class BankAccountViewModel : BaseViewModel
    {
        //public CommandBase SubmitCommandButton { get; set; }

        Firma firma = Firma.GetInstance();


        [ObservableProperty]
        private List<string> _currencyComboBox;

        [ObservableProperty]
        private string _bankAccount_Name;

        [ObservableProperty]
        private string _account_Number;

        [ObservableProperty]


        private string _currency;

        [ObservableProperty]
        private int _value;

        //public string Value
        //{
        //    get
        //    {
        //        return _value.ToString();
        //    }
        //    set
        //    {
        //        try
        //        {
        //            _value = Convert.ToInt32(value);
        //            OnPropertyChanged();
        //        }
        //        catch (Exception)
        //        {
        //            //MessageBox.Show("UwU watrość nie jest liczbą", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
        //        }

        //    }
        //}

        public BankAccountViewModel()
        {
            CurrencyComboBox = new List<string>();
            CurrencyComboBox.Add("PLN");
            CurrencyComboBox.Add("UwU");
            CurrencyComboBox.Add("In progress");



            if (firma.BankAccount != null)
            {
                BankAccount_Name = firma.BankAccount.BankAccount_Name;
                Account_Number = firma.BankAccount.Account_Number;
                Currency = firma.BankAccount.Currency;
                Value = firma.BankAccount.Value;
                //Value = firma.BankAccount.Value.ToString();
            }
            else
            {
                firma.BankAccount = new("BANK POLSKIEJ SPÓŁDZIELCZOŚCI SA F. w Porębie", "87 1930 1334 1353 2760 2968 7558", "PLN", 740);
                BankAccount_Name = firma.BankAccount.BankAccount_Name;
                Account_Number = firma.BankAccount.Account_Number;
                Currency = firma.BankAccount.Currency;
                Value = firma.BankAccount.Value;
            }


        }

        [ICommand]
        public void SubmitCommandButton()
        {
            if (BankAccount_Name == null)
            {
                //MessageBox.Show("Błąd zapisu", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (Account_Number == null)
            {
                //MessageBox.Show("Błąd zapisu", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            firma.BankAccount = new BankAccount(_bankAccount_Name, _account_Number, _currency, _value);
            //MessageBox.Show("Dazne zapisane", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        [ICommand]
        public async void GoToNextPage()
        {
            await Shell.Current.GoToAsync($"..");
        }
    }
}
