using Newtonsoft.Json;
using Project_Invoice_MAUI.Models;
using Project_Invoice_MAUI.SavePathHelpers;
using Project_Invoice_MAUI.Services;
using Project_Invoice_MAUI.Singleton;
using Project_Invoice_MAUI.Views;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Font = Microsoft.Maui.Font;

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
        

        public BankAccountViewModel()
        {
            CurrencyComboBox = new List<string>();
            CurrencyComboBox.Add("PLN");
            CurrencyComboBox.Add("UwU");
            CurrencyComboBox.Add("In progress");

            //_sevices = new CompanyDataService();
            var streamBA = FileSystem.Current.AppDataDirectory + JsonFilesPath.BANK_ACCOUNT;
            if (File.Exists(streamBA))
            {
                firma.BankAccount = JsonConvert.DeserializeObject<BankAccount>(File.ReadAllText(streamBA));
                BankAccount_Name = firma.BankAccount.BankAccount_Name;
                Account_Number = firma.BankAccount.Account_Number;
                Currency = firma.BankAccount.Currency;
                Value = firma.BankAccount.Value;
                //Value = firma.BankAccount.Value.ToString();
            }
            //else
            //{
            //    firma.BankAccount = new("BANK POLSKIEJ SPÓŁDZIELCZOŚCI SA F. w Porębie", "87 1930 1334 1353 2760 2968 7558", "PLN", 740);
            //    BankAccount_Name = firma.BankAccount.BankAccount_Name;
            //    Account_Number = firma.BankAccount.Account_Number;
            //    Currency = firma.BankAccount.Currency;
            //    Value = firma.BankAccount.Value;
            //}


        }

        [ICommand]
        public async void SubmitBankAccount()
        {

            if (Account_Number == null || BankAccount_Name == null)
            {
                await ToastSaveFail();
                return;
            }

            try
            {
                firma.BankAccount = new BankAccount(_bankAccount_Name, _account_Number, _currency, _value);

                var streamBA = FileSystem.Current.AppDataDirectory + JsonFilesPath.BANK_ACCOUNT;
                string output = JsonConvert.SerializeObject(firma.BankAccount, Formatting.Indented);
                File.WriteAllText(streamBA, output);
                await SnackbarBA();
                //MessageBox.Show("Dazne zapisane", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);

                await ToastSaveSucces();
            }
            catch (Exception)
            {
                await ToastSaveFail();
            }
            

        }

        private static async Task SnackbarBA()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            var snackbarOptions = new SnackbarOptions
            {
                BackgroundColor = Colors.DarkGray,
                TextColor = Colors.White,
                ActionButtonTextColor = Colors.BlueViolet,
                CornerRadius = new CornerRadius(10),
                Font = Font.SystemFontOfSize(14),
                ActionButtonFont = Font.SystemFontOfSize(14),
                CharacterSpacing = 0.5
                
            };

            string text = "";
            string actionButtonText = "Nastepna strona";


            TimeSpan duration = TimeSpan.FromSeconds(3);

            Action action = () => snackbarOptions.ActionButtonTextColor = Colors.Green;

            var snackbar = Snackbar.Make(text, action, actionButtonText, duration, snackbarOptions);

            await snackbar.Show(cancellationTokenSource.Token);
        }



        [ICommand]
        public async void GoBack()
        {
            await Shell.Current.GoToAsync("..");
        }

        [ICommand]
        public async void GoToBossDataView()
        {
            await Shell.Current.GoToAsync($"../{nameof(BossDataView)}");
        }
    }
}
