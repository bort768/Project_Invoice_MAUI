using Project_Invoice_MAUI.Models;
using Project_Invoice_MAUI.Singleton;

namespace Project_Invoice_MAUI.ViewModels
{
    [INotifyPropertyChanged]
    public partial class BankAccountViewModel
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

        /* Niescalona zmiana z projektu „Project_Invoice_MAUI (net6.0-windows10.0.19041.0)”
        Przed:
                private string _currency;


                private int _value;
        Po:
                private string _currency;


                private int _value;
        */

        /* Niescalona zmiana z projektu „Project_Invoice_MAUI (net6.0-maccatalyst)”
        Przed:
                private string _currency;


                private int _value;
        Po:
                private string _currency;


                private int _value;
        */

        /* Niescalona zmiana z projektu „Project_Invoice_MAUI (net6.0-ios)”
        Przed:
                private string _currency;


                private int _value;
        Po:
                private string _currency;


                private int _value;
        */
        private string _currency;


        private int _value;
        public string Value
        {
            get
            {
                return _value.ToString();
            }
            set
            {
                try
                {
                    _value = Convert.ToInt32(value);
                    OnPropertyChanged();
                }
                catch (Exception)
                {
                    //MessageBox.Show("UwU watrość nie jest liczbą", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
        }

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
                Value = firma.BankAccount.Value.ToString();
            }
            else
            {
                firma.BankAccount = new("BANK POLSKIEJ SPÓŁDZIELCZOŚCI SA F. w Porębie", "87 1930 1334 1353 2760 2968 7558", "PLN", 740);
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
    }
}
