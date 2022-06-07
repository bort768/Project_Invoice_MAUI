using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace Project_Invoice_MAUI.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(IsNotbusy))]
        bool isBusy;

        [ObservableProperty]
        string title;

        [ObservableProperty]
        private bool _oK_Message_IS_Visble;

        [ObservableProperty]
        private bool _error_Message_IS_Visble;

        public bool IsNotbusy => !IsBusy;

        public BaseViewModel()
        {

        }

        /// <summary>
        /// metoda do wyświetlania potwerdzenia
        /// </summary>
        /// <returns></returns>
        public static async Task ToastSaveSucces()
        {
            //toast
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            string text = "Dane Zapisane!";
            ToastDuration duration = ToastDuration.Short;
            double fontSize = 18;

            var toast = Toast.Make(text, duration, fontSize);

            await toast.Show(cancellationTokenSource.Token);
        }

        /// <summary>
        /// Przeciązona metoda do wyswetlania toastu z domyslnym tekstem
        /// </summary>
        /// <param name="text"> tekst jaki ma pokazać toast</param>
        /// <returns></returns>
        public static async Task ToastSaveSucces(string text)
        {
            //toast
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            //str text = "Dane Zapisane!";
            ToastDuration duration = ToastDuration.Short;
            double fontSize = 18;

            var toast = Toast.Make(text, duration, fontSize);

            await toast.Show(cancellationTokenSource.Token);
        }

        /// <summary>
        /// metoda do wyświetlania porazki
        /// </summary>
        /// <returns></returns>
        public static async Task ToastSaveFail()
        {
            //toast
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            string text = "Bład zapisu!";
            ToastDuration duration = ToastDuration.Long;
            double fontSize = 18;

            var toast = Toast.Make(text, duration, fontSize);
           

            await toast.Show(cancellationTokenSource.Token);
        }



        //public async Task<bool> ChangeVisibleOK()
        //{
        //    OK_Message_IS_Visble = true;
        //    await Task.Delay(1000);
        //    return false;
        //}

        //public async Task<bool> ChangeVisibleError()
        //{
        //    Error_Message_IS_Visble = true;
        //    await Task.Delay(1000);
        //    return false;
        //}
    }
}