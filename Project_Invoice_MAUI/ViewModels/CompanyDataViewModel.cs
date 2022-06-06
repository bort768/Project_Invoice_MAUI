using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Newtonsoft.Json;
using Project_Invoice_MAUI.Models;
using Project_Invoice_MAUI.SavePathHelpers;
using Project_Invoice_MAUI.Services;
using Project_Invoice_MAUI.Singleton;
using Project_Invoice_MAUI.Views;


namespace Project_Invoice_MAUI.ViewModels
{

    public partial class CompanyDataViewModel : BaseViewModel
    {

        //public CommandBase SubmitCompanyDataCommand { get; set; }
        //public CommandBase Invoice_Format_By_Year_Command { get; set; }
        //public CommandBase Invoice_Format_By_Mounth_Command { get; set; }

        Firma firma = Firma.GetInstance();

        [ObservableProperty]
        private bool _broken_By_Mounth;
        partial void OnBroken_By_MounthChanged(bool value)
        {
            Invoice_Format_By_Mounth();
        }

        [ObservableProperty]
        private bool _broken_By_Year;
        partial void OnBroken_By_YearChanged(bool value)
        {
            Invoice_Format_By_Year();
        }


        [ObservableProperty]
        private string _invoice_format;

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

       

        


        //TODO: dodaj sprawdzanie nulla
        public bool ValidateNip(string nip)
        {
            nip = nip.Replace("-", string.Empty);

            if (nip.Length != 10 || nip.Any(chr => !char.IsDigit(chr))) return false;

            int[] weights = { 6, 5, 7, 2, 3, 4, 5, 6, 7, 0 };
            int sum = nip.Zip(weights, (digit, weight) => (digit - '0') * weight).Sum();

            return (sum % 11) == (nip[9] - '0');

        }

        public CompanyDataViewModel()
        {
            //_sevices = new CompanyDataService();

            //usuń pózniej
            if (firma.CompanyData == null)
            {
                var stream = FileSystem.Current.AppDataDirectory + JsonFilesPath.COMPANY_DATA;
                if (File.Exists(stream))
                {
                    firma.CompanyData = JsonConvert.DeserializeObject<CompanyData>(File.ReadAllText(stream));
                }
                //else
                //{
                //    firma.CompanyData = new CompanyData
                //    {
                //        Full_Name = "Hurtownia Artykułów Biurowych Bort sp. z o.o",
                //        NIP = "521-427-93-09",
                //        REGON = "012346678",
                //        Street = "Kwiatowa",
                //        House_Number = "6",/
                //        ZIP_Code = "36-200",
                //        Town = "Brzozów"
                //    };
                //    string output = JsonConvert.SerializeObject(firma.CompanyData, Formatting.Indented);
                //    File.WriteAllText(stream, output);

                //}

                //desrialize document numbering
                var streamDN = FileSystem.Current.AppDataDirectory + JsonFilesPath.DOCUMENT_NUMBERING;
                if (File.Exists(streamDN))
                {
                    firma.DocumentNumbering = JsonConvert.DeserializeObject<DocumentNumbering>(File.ReadAllText(streamDN));
                    Broken_By_Mounth = firma.DocumentNumbering.Broken_By_Mounth;
                    Broken_By_Year = firma.DocumentNumbering.Broken_By_Year;
                }
                

                //firma.CompanyData = JsonConvert.DeserializeObject<CompanyData>(output);
            
                
                


            }

            if (firma.CompanyData != null)
            {
                Full_Name = firma.CompanyData.Full_Name;
                NIP = firma.CompanyData.NIP;
                REGON = firma.CompanyData.REGON;
                Street = firma.CompanyData.Street;
                House_Number = firma.CompanyData.House_Number;
                ZIP_Code = firma.CompanyData.ZIP_Code;
                Town = firma.CompanyData.Town;
            }


            if (firma.DocumentNumbering != null)
            {
                Broken_By_Mounth = firma.DocumentNumbering.Broken_By_Mounth;
                Broken_By_Year = firma.DocumentNumbering.Broken_By_Year;
                Wypisanie_Invoice_Format();
            }
            else
            {
                Invoice_format = $"FV 1";
            }

        }

        #region Commands
        
        [ICommand]
        // TODO: dodaj sprawdzanie do nipu
        public async void SubmitCompanyData()
        {
            if (NIP is not null && ValidateNip(NIP))
            {
                firma.CompanyData = new CompanyData
                {
                    Full_Name = Full_Name,
                    NIP = NIP,
                    REGON = REGON,
                    Street = Street,
                    House_Number = House_Number,
                    ZIP_Code = ZIP_Code,
                    Town = Town
                };

                //JSON
                var stream = FileSystem.Current.AppDataDirectory + JsonFilesPath.COMPANY_DATA;
                string output = JsonConvert.SerializeObject(firma.CompanyData, Formatting.Indented);
                File.WriteAllText(stream, output);


                var streamDN = FileSystem.Current.AppDataDirectory + JsonFilesPath.DOCUMENT_NUMBERING;
                string outputDN = JsonConvert.SerializeObject(firma.CompanyData, Formatting.Indented);
                File.WriteAllText(streamDN, outputDN);

               

                await ToastSaveSucces();

            }
            else
            {
                await ToastSaveFail();
                //MessageBox.Show("NIP nie prawidłowy", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        [ICommand]
        public async void GoToBankAccountView()
        {
            await Shell.Current.GoToAsync(nameof(BankAccountView));
        }

        [ICommand]
        public async void GoToBossDataView()
        {
            await Shell.Current.GoToAsync(nameof(BossDataView));
        }


        #endregion

        #region Methods
        public void Invoice_Format_By_Year()
        {
            if (firma.DocumentNumbering == null)
            {
                firma.DocumentNumbering = new DocumentNumbering(Broken_By_Mounth, Broken_By_Year);
            }

            //Sprawdzanie jak numerować dokument
            if (Broken_By_Year == true)
            {
                firma.DocumentNumbering.Broken_By_Year = true;
            }
            else
            {
                firma.DocumentNumbering.Broken_By_Year = false;
            }

            Wypisanie_Invoice_Format();
        }

        public void Invoice_Format_By_Mounth()
        {
            if (firma.DocumentNumbering == null)
            {
                firma.DocumentNumbering = new DocumentNumbering(Broken_By_Mounth, Broken_By_Year);
            }

            //Sprawdzanie jak numerować dokument
            if (Broken_By_Mounth == true)
            {
                firma.DocumentNumbering.Broken_By_Mounth = true;
            }
            else
            {
                firma.DocumentNumbering.Broken_By_Mounth = false;
            }

            Wypisanie_Invoice_Format();
        }

        private void Wypisanie_Invoice_Format()
        {
            if (Broken_By_Year && Broken_By_Mounth)
            {
                Invoice_format = $"FV 1/{DateTime.Today.ToString("MM")}/{DateTime.Today.ToString("yyyy")}";
            }
            else if (Broken_By_Year)
            {
                Invoice_format = $"FV 1/{DateTime.Today.ToString("yyyy")}";
            }
            else if (Broken_By_Mounth)
            {
                Invoice_format = $"FV 1/{DateTime.Today.ToString("MM")}";
            }
            else
            {
                Invoice_format = $"FV 1";
            }

            

        }
        #endregion
    }
}
