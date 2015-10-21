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
    /// LoginPage view model
    /// </summary>
    public class LoginPageViewModel : ViewModel, ILoginPageViewModel
    {
        /// <summary>Logs the user to the server </summary>
        public DelegateCommand LoginCommand { get; private set; }
        /// <summary>Redirects to the previous page </summary>
        public DelegateCommand BackCommand { get; private set; }
        /// <summary>User name of the user </summary>
        public string UserName { get; set; }
        /// <summary>User's password </summary>
        public string Password { get; set; }
        private readonly IConnectionManager _connectionManager;

        /// <summary>
        /// Constructor initializing connectionManager
        /// through dependency injection
        /// </summary>
        /// <param name="connectionmanager">Manages the connetion of the user so it gets logged in</param>
        public LoginPageViewModel(IConnectionManager connectionmanager)
        {
            this._connectionManager = connectionmanager;
            this.LoginCommand = new DelegateCommand(LoginCommandExec);
            this.BackCommand = new DelegateCommand(App.MyNavigationService.GoBack);
        }

        private async void LoginCommandExec()
        { 
            if (!RestApiManagerBase.Instance.NotificationRecieiver.IsInternet())
            {
                DisplayErrorMessage("Connexion unavailable");
                return;
            }
            bool requestResult;
            try
            {
                requestResult = await this._connectionManager.ConnectUser(this.UserName, this.Password);
            }
            catch
            {
                DisplayErrorMessage("Request failed");
                return;
            }
            if (!requestResult)
                DisplayErrorMessage("Wrong user name/password");
            else
                App.MyNavigationService.GoBack();
        }

        private async void DisplayErrorMessage(string message)
        {
            var messageDialog = new MessageDialog(message); 

            await messageDialog.ShowAsync();
        }
    }
}
