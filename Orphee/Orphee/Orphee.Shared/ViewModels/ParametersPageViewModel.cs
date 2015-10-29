using Microsoft.Practices.Prism.Commands;
using Orphee.ViewModels.Interfaces;
using Orphee.UI;

namespace Orphee.ViewModels
{
    public class ParametersPageViewModel : IParametersPageViewModel
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

        private void LogoutCommandExec()
        {
            
        }

        private async void ResetPasswordCommandExec()
        {
            var resetPasswordMessageDialog = new MyResetPasswordMessageDialog();

            await resetPasswordMessageDialog.ShowAsync();
        }
    }
}
