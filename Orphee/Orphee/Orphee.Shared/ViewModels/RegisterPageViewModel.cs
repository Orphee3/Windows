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
            if (!App.InternetAvailabilityWatcher.IsInternetUp)
            {
                DisplayErrorMessage("Connexion unavailable");
                return;
            }
            bool requestResult;
            try
            {
                requestResult = await this._registrationManager.RegisterUser(this.UserName, this.MailAdress, this.Password);
            }
            catch (Exception)
            {
                DisplayErrorMessage("Request failed");
                return;
            }
            if (!requestResult)
                DisplayErrorMessage("Mail address/user name already used");
            else
            {
                App.MyNavigationService.GoBack();
                App.MyNavigationService.GoBack();
            }
        }

        private async void DisplayErrorMessage(string message)
        {
            var messageDialog = new MessageDialog(message);

            await messageDialog.ShowAsync();
        } 
    }
}
