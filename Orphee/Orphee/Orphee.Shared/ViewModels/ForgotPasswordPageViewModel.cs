using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.RestApiManagement.Posters.Interfaces;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    public class ForgotPasswordPageViewModel : ViewModel, IForgotPasswordPageViewModel, ILoadingScreenComponents
    {
        private bool _isProgressiveRingActive;

        public bool IsProgressRingActive
        {
            get { return this._isProgressiveRingActive; }
            set
            {
                if (this._isProgressiveRingActive != value)
                    SetProperty(ref this._isProgressiveRingActive, value);
            }
        }

        private Visibility _progressRingVisibility;

        public Visibility ProgressRingVisibility
        {
            get { return this._progressRingVisibility; }
            set
            {
                if (this._progressRingVisibility != value)
                    SetProperty(ref this._progressRingVisibility, value);
            }
        }
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
            SetProgressRingProperties(true);
            this._forgotPasswordReseter = forgotPasswordReseter;
            this.SendCommand = new DelegateCommand(SendCommandExec);
            this.GoBackCommend = new DelegateCommand(() => App.MyNavigationService.GoBack());
        }

        private async void SendCommandExec()
        {
            bool result = false;
            if (string.IsNullOrEmpty(this.UserMailAdress) || !this.UserMailAdress.Contains("@") || !this.UserMailAdress.Contains("."))
            {
                DisplayMessageError("Invalid mail adress");
                return;
            }
            try
            {
                result = await this._forgotPasswordReseter.ResetForgotPassword(this.UserMailAdress);
            }
            catch (Exception)
            {
                DisplayMessageError("Request has failed");
            }
            DisplayMessageError(!result ? "Invalid mail adress" : "The password reset link has been sent to your mail box !");
            SetProgressRingProperties(false);
        }

        private async void DisplayMessageError(string message)
        {
            var messageDialog = new MessageDialog(message);

            await messageDialog.ShowAsync();
        }

        private void SetProgressRingProperties(bool isActive)
        {
            this.IsProgressRingActive = isActive;
            this.ProgressRingVisibility = isActive ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
