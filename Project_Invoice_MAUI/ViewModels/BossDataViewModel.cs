﻿using Project_Invoice_MAUI.Models;
using Project_Invoice_MAUI.Singleton;

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
            if (firma.BossData != null)
            {
                Name = firma.BossData.Name;
                Last_Name = firma.BossData.Last_Name;
                ID = firma.BossData.ID;
                Password = firma.BossData.Password;
            }
            else
            {
                firma.BossData = new BossData("Bort", "OwO", "Bo", "");
                Name = firma.BossData.Name;
                Last_Name = firma.BossData.Last_Name;
                ID = firma.BossData.ID;
                Password = firma.BossData.Password;
            }
        }


        [ICommand]
        public async void SubmitBossDataButton()
        {
            try
            {
                BossData bossData = new BossData(Name, Last_Name, ID, Password); // po co?
                firma.BossData = bossData;
                OK_Message_IS_Visble = await ChangeVisibleOK();
            }
            catch (Exception)
            {

                Error_Message_IS_Visble = await ChangeVisibleError();
            }
            
        }
    }
}
