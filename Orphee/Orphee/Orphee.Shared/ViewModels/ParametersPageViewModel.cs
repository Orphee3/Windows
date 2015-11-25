using System.Collections.Generic;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Commands;
using Orphee.RestApiManagement.Models;
using Orphee.UI;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    public class ParametersPageViewModel : ViewModelExtend, IParametersPageViewModel
    {
        public DelegateCommand DeleteAccoutCommand { get; private set; }
        public DelegateCommand LogoutCommand { get; private set; }
        public DelegateCommand ResetPasswordCommand { get; private set; }
        public DelegateCommand GoBackCommand { get; private set; }

        public ParametersPageViewModel()
        {
            this.DeleteAccoutCommand = new DelegateCommand(DeleteAccountCommandExec);
            this.LogoutCommand = new DelegateCommand(LogoutCommandExec);
            this.ResetPasswordCommand = new DelegateCommand(ResetPasswordCommandExec);
            this.GoBackCommand = new DelegateCommand(() => App.MyNavigationService.GoBack());
        }

        private async void DeleteAccountCommandExec()
        {
            var deleteAccountMessageDialog = new MyDeleteAccountMessageDialog();

            await deleteAccountMessageDialog.ShowAsync();
        }

        public override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            App.InternetAvailabilityWatcher.PropertyChanged += InternetAvailabilityWatcherOnPropertyChanged;
        }

        private void LogoutCommandExec()
        {
            RestApiManagerBase.Instance.Logout();
            App.MyNavigationService.GoBack();
        }

        private async void ResetPasswordCommandExec()
        {
            var resetPasswordMessageDialog = new MyResetPasswordMessageDialog();

            await resetPasswordMessageDialog.ShowAsync();
        }
    }
}
