namespace Orphee.ViewModels.UI
{
    public class MyBottomAppBarViewModel : IMyBottomAppBarViewModel
    {
        public DelegateCommand<Button> ButtonCommand { get; private set; }
        private INavigationService _navigationService;

        public MyBottomAppBarViewModel(INavigationService navigationService)
        {
            this._navigationService = navigationService;
            this.ButtonCommand = new DelegateCommand<Button>(ButtonCommandExec);
        }

        private void ButtonCommandExec(Button button)
        {
            this._navigationService.Navigate(Type.GetType("Orphee.Views." + button.Name));
        }
    }
}
