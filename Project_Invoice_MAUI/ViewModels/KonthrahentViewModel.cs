using Project_Invoice_MAUI.Models;
using Project_Invoice_MAUI.Singleton;


namespace Project_Invoice_MAUI.ViewModels
{
    public partial class KonthrahentViewModel : BaseViewModel
    {

        CompanyData company;
        Kontrahent kontrahent;

        Firma firma = Firma.GetInstance();

        private string _lastVisetedKontrahent;
        public string LastVisetedKontrahent
        {
            get
            {
                return _lastVisetedKontrahent;
            }
            set
            {
                _lastVisetedKontrahent = value;
                int index = 0;
                foreach (var kontrahent in firma.kontrahents)
                {
                    if (kontrahent.Company.Full_Name == value)
                    {
                        SaveToLocal(index);
                    }
                    index++;
                }
                OnPropertyChanged();
            }
        }


        //private ObservableCollection<string> _listaDoCombobox;
        public ObservableCollection<string> listaDoCombobox { get; set; }

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
            if (firma.kontrahents != null)
            {
                int index = 0;
                foreach (var kontrahent in firma.kontrahents)
                {
                    if (kontrahent.Company.Full_Name == _lastVisetedKontrahent)
                    {
                        SaveToLocal(index);
                    }
                    index++;
                }


                listaDoCombobox = new();

                //średnia optymalizcja jeśli chodzi o wieksze tabele
                foreach (var item in firma.kontrahents)
                {
                    listaDoCombobox.Add(item.Company.Full_Name);
                }

            }
            else
            {
                listaDoCombobox = new();

                firma.kontrahents = new();
                firma.kontrahents.Add(new Kontrahent("BPK Oddział III w Brzozowie", "15 0011 0018 9123 3456 4567 8912",
                    new CompanyData("Dom Towarowy „KROS” SA", "117-00-88-765", "986674453", "Brzozowska", "56", "36-200", "Brzozów")));

                firma.kontrahents.Add(new Kontrahent("BT Oddział w Brzozowie", "19 0000 1245 6778 9189 1223 3456",
                    new CompanyData("Dom Handlowy „Mrówka” sp. z.o.o.", "823 12 20 711", "74185296", "Obwodowa", "2", "36-200", "Brzozów")));

            }



        }

        #region Commands
        [ICommand]
        public void SubmitKontrahent()
        {
            company = new(_full_Name, _nIP, _REGON, _street, _house_Number, _ZIP_Code, _town);
            kontrahent = new(BankAccount_Name, Account_Number, company);
            AddToKontrahents();
            LastVisetedKontrahent = _full_Name;
            listaDoCombobox.Add(_full_Name);
        }

        [ICommand]
        public void DeleteKontrahent()
        {
            RemoveFromKontrahents();
        }

        [ICommand]
        async Task GoToDetailsAsync()
        {
            await Shell.Current.GoToAsync($"{nameof(BankAccountViewModel)}", true);
        }



        #endregion


        private void SaveToLocal(int index)
        {
            BankAccount_Name = firma.kontrahents[index].BankAccount_Name;
            Account_Number = firma.kontrahents[index].Account_Number;
            Full_Name = firma.kontrahents[index].Company.Full_Name;
            NIP = firma.kontrahents[index].Company.NIP;
            REGON = firma.kontrahents[index].Company.REGON;
            Street = firma.kontrahents[index].Company.Street;
            House_Number = firma.kontrahents[index].Company.House_Number;
            Town = firma.kontrahents[index].Company.Town;
            ZIP_Code = firma.kontrahents[index].Company.ZIP_Code;

            //zrób kontrahenta do usuwania go
            company = new(_full_Name, _nIP, _REGON, _street, _house_Number, _ZIP_Code, _town);
            kontrahent = new(BankAccount_Name, Account_Number, company);
        }

        private void RemoveFromKontrahents()
        {
            if (firma.kontrahents == null)
            {
                //MessageBox.Show("Dane nie zostały usunięte", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            //To do naprawić to :) Kiedyś
            else if (firma.kontrahents.Contains(kontrahent))
            {
                firma.kontrahents.Remove(kontrahent);
            }
            else
            {
                //MessageBox.Show("Dane nie zostały usunięte", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void AddToKontrahents()
        {
            if (kontrahent == null)
            {
                //MessageBox.Show("Dane nie zostały zapisane", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (firma.kontrahents == null)
            {
                firma.kontrahents = new();
                firma.kontrahents.Add(kontrahent);
                //MessageBox.Show("Dane zapisane pomyślne", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (!firma.kontrahents.Contains(kontrahent))
            {
                firma.kontrahents.Add(kontrahent);
                //MessageBox.Show("Dane zapisane pomyślne", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                //MessageBox.Show("Dane już istnieją", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
}
