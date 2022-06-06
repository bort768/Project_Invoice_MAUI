using Newtonsoft.Json;
using Project_Invoice_MAUI.Models;
using Project_Invoice_MAUI.SavePathHelpers;
using Project_Invoice_MAUI.Singleton;
using Project_Invoice_MAUI.Views;

namespace Project_Invoice_MAUI.ViewModels
{
    public partial class BossDataViewModel : BaseViewModel
    {
        Firma firma = Firma.GetInstance();

        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private string _last_Name;

        [ObservableProperty]
        private string _ID;

        [ObservableProperty]
        private string _password;


        public BossDataViewModel()
        {
            var streamBD = FileSystem.Current.AppDataDirectory + JsonFilesPath.BOSS_DATA;
            if (File.Exists(streamBD))
            {
                firma.BossData = JsonConvert.DeserializeObject<BossData>(File.ReadAllText(streamBD));
                Name = firma.BossData.Name;
                Last_Name = firma.BossData.Last_Name;
                ID = firma.BossData.ID;
                Password = firma.BossData.Password;
            }
            //else
            //{
            //    firma.BossData = new BossData("Bort", "OwO", "Bo", "");
            //    Name = firma.BossData.Name;
            //    Last_Name = firma.BossData.Last_Name;
            //    ID = firma.BossData.ID;
            //    Password = firma.BossData.Password;
            //}
        }


        [ICommand]
        public async Task SubmitBossDataButtonAsync()
        {
            try
            {  
                firma.BossData = new BossData(Name, Last_Name, ID, Password);
                var streamBD = FileSystem.Current.AppDataDirectory + JsonFilesPath.BOSS_DATA;
                string output = JsonConvert.SerializeObject(firma.BossData, Formatting.Indented);
                File.WriteAllText(streamBD, output);
                //OK_Message_IS_Visble = await ChangeVisibleOK();
                await ToastSaveSucces();
            }
            catch (Exception)
            {
                await ToastSaveFail();
                //Error_Message_IS_Visble = await ChangeVisibleError();
            }
            
        }

        [ICommand]
        public async void GoBack()
        {
            await Shell.Current.GoToAsync("..");
        }

        [ICommand]
        public async void GoToBankAccount()
        {
            await Shell.Current.GoToAsync($"../{nameof(BankAccountView)}");
        }
    }
}
