namespace Project_Invoice_MAUI.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(IsNotbusy))]
        bool isBusy;

        [ObservableProperty]
        string title;

        public BaseViewModel()
        {

        }

        public bool IsNotbusy => !IsBusy;
    }
}