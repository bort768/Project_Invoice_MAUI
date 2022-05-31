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

        public BaseViewModel()
        {

        }

        public bool IsNotbusy => !IsBusy;


        public async Task<bool> ChangeVisibleOK()
        {
            OK_Message_IS_Visble = true;
            await Task.Delay(1000);
            return false;
        }

        public async Task<bool> ChangeVisibleError()
        {
            Error_Message_IS_Visble = true;
            await Task.Delay(1000);
            return false;
        }
    }
}