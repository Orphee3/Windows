using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.RestApiManagement.Posters.Interfaces;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    public class ForgotPasswordPageViewModel : ViewModelExtend, IForgotPasswordPageViewModel
    {
        public DelegateCommand SendCommand { get; private set; }
        public DelegateCommand GoBackCommend { get; private set; }
        private string _userMailAdress;

        public string UserMailAdress
        {
            get { return this._userMailAdress; }
            set
            {
                if (this._userMailAdress != value)
                    SetProperty(ref this._userMailAdress, value);
            }
        }
        private readonly IForgotPasswordReseter _forgotPasswordReseter;

        public ForgotPasswordPageViewModel(IForgotPasswordReseter forgotPasswordReseter)
        {
            SetProgressRingVisibility(true);
            this._forgotPasswordReseter = forgotPasswordReseter;
            this.SendCommand = new DelegateCommand(SendCommandExec);
            this.GoBackCommend = new DelegateCommand(() => App.MyNavigationService.GoBack());
        }

        private async void SendCommandExec()
        {
            if (string.IsNullOrEmpty(this.UserMailAdress) || !this.UserMailAdress.Contains("@") || !this.UserMailAdress.Contains("."))
            {
                DisplayMessage("Invalid mail adress");
                return;
            }
            var result = await this._forgotPasswordReseter.ResetForgotPassword(this.UserMailAdress);
            VerifyReturnedValue(result, "");
            DisplayMessage(!result ? "Invalid mail adress" : "The password reset link has been sent to your mail box !");
            SetProgressRingVisibility(false);
        }
    }
}
