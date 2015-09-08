using System;
using Windows.UI.Popups;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.RestApiManagement;
using Orphee.RestApiManagement.Interfaces;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    public class LoginPageViewModel : ViewModel, ILoginPageViewModel
    {
        public DelegateCommand LoginCommand { get; private set; }
        public DelegateCommand BackCommand { get; private set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string MailAdress { get; set; }
        private readonly IConnectionManager _connectionManager;

        public LoginPageViewModel(IConnectionManager connectionmanager)
        {
            this.UserName = "Jeanmich";
            this.Password = "Jeanmich";
            this._connectionManager = connectionmanager;
            this.LoginCommand = new DelegateCommand(LoginCommandExec);
            this.BackCommand = new DelegateCommand(App.MyNavigationService.GoBack);
        }

        private async void LoginCommandExec()
        {
            var isInternetConnected = RestApiManagerBase.Instance.NotificationRecieiver.IsInternet();
            if (await this._connectionManager.ConnectUser(this.UserName, this.Password))
                App.MyNavigationService.GoBack();
            else
                DisplayErrorMessage(isInternetConnected);
        }

        private async void DisplayErrorMessage(bool result)
        {
            var messageDialog = (result == false) ? new MessageDialog("Internet connexion unavailable") : new MessageDialog("Wrong password/user name"); 

            await messageDialog.ShowAsync();
        }
    }
}
