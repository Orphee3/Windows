using System;
using Windows.UI.Popups;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.RestApiManagement;
using Orphee.RestApiManagement.Interfaces;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    public class RegisterPageViewModel : ViewModel, IRegisterPageViewModel
    {
        public DelegateCommand RegisterCommand { get; private set; }
        public DelegateCommand BackCommand { get; private set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string MailAdress { get; set; }
        private readonly IRegistrationManager _registrationManager;

        public RegisterPageViewModel(IRegistrationManager registrationManager)
        {
            this._registrationManager = registrationManager;
            this._registrationManager = registrationManager;
            this.RegisterCommand = new DelegateCommand(RegisterCommandExec);
            this.BackCommand = new DelegateCommand(App.MyNavigationService.GoBack);
        }

        private async void RegisterCommandExec()
        {
            bool isInternetConnected;
            if ((isInternetConnected = RestApiManagerBase.Instance.NotificationRecieiver.IsInternet()) && await this._registrationManager.RegisterUser(this.UserName, this.MailAdress, this.Password))
                App.MyNavigationService.GoBack();
            else
                DisplayErrorMessage(isInternetConnected);
        }

        private async void DisplayErrorMessage(bool result)
        {
            var messageDialog = (result == false) ? new MessageDialog("Internet connexion unavailable") : new MessageDialog("User name/mail adress already used");

            await messageDialog.ShowAsync();
        } 
    }
}
