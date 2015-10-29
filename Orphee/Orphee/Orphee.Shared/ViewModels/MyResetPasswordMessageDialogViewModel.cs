using System.Threading.Tasks;
using Windows.UI.Xaml;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.RestApiManagement.Posters.Interfaces;

namespace Orphee.ViewModels
{
    public class MyResetPasswordMessageDialogViewModel : ViewModel
    {
        private TaskCompletionSource<bool> _taskCompletionSource;
        public string ActualPassword { get; set; }
        public string NewPassword { get; set; }
        public string NewPasswordConfirmation { get; set; }
        public DelegateCommand ValidateCommand { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }
        public Visibility ErrorBoxVisibility { get; private set; }
        public string ErrorBoxText { get; private set; }
        private bool _isOpen;
        public bool IsOpen
        {
            get { return this._isOpen; }
            set
            {
                if (this._isOpen != value)
                    SetProperty(ref this._isOpen, value);
            }
        }

        private IPasswordReseter _passwordReseter;

        public MyResetPasswordMessageDialogViewModel(IPasswordReseter passwordReseter)
        {
            this._passwordReseter = passwordReseter;
            this.ValidateCommand = new DelegateCommand(ValidationCommandExec);
            this.CancelCommand = new DelegateCommand(CancelCommandExec);
        }

        public Task<bool> ShowAsync()
        {
            this._taskCompletionSource = new TaskCompletionSource<bool>();

            this.IsOpen = true;
            return this._taskCompletionSource.Task;
        }

        private void ValidationCommandExec()
        {
            if (this.NewPassword != this.NewPasswordConfirmation)
            {
                this.ErrorBoxText = "New password and confirmation are different";
                this.ErrorBoxVisibility = Visibility.Visible;
            }
            else if (this.NewPassword.Length < 6 || this.NewPassword.Length > 20)
            {
                this.ErrorBoxText = "Password must be composed of 6 to 20 characters";
                this.ErrorBoxVisibility = Visibility.Visible;
            }
            //else
            //    this._passwordReseter.ResetPassword(this.ActualPassword, this.NewPassword);
        }

        private void CancelCommandExec()
        {
            Close();
        }

        private void Close()
        {
            this.IsOpen = false;
            this._taskCompletionSource.SetResult(true);
        }
    }
}
