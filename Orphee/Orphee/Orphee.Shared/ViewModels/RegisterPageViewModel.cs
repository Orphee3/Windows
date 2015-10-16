using System;
using Windows.UI.Popups;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.RestApiManagement.Models;
using Orphee.RestApiManagement.Posters.Interfaces;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    /// <summary>
    /// RegisterPage view model
    /// </summary>
    public class RegisterPageViewModel : ViewModel, IRegisterPageViewModel
    {
        /// <summary>Register command </summary>
        public DelegateCommand RegisterCommand { get; private set; }
        /// <summary>Back command </summary>
        public DelegateCommand BackCommand { get; private set; }
        /// <summary>New account userName</summary>
        public string UserName { get; set; }
        /// <summary>New account password </summary>
        public string Password { get; set; }
        /// <summary>New account mail adress </summary>
        public string MailAdress { get; set; }
        private readonly IRegistrationManager _registrationManager;

        /// <summary>
        /// Constructor initialiing registrationManager
        /// through dependency injection
        /// </summary>
        /// <param name="registrationManager"></param>
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
            {
                App.MyNavigationService.GoBack();
                App.MyNavigationService.GoBack();
            }
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
