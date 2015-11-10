using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.Models.Interfaces;
using Orphee.RestApiManagement.Getters.Interfaces;
using Orphee.RestApiManagement.Models;
using Orphee.RestApiManagement.Models.Interfaces;
using Orphee.RestApiManagement.Posters.Interfaces;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    /// <summary>
    /// LoginPage view model
    /// </summary>
    public class LoginPageViewModel : ViewModelExtend, ILoginPageViewModel
    {
        /// <summary>Logs the user to the server </summary>
        public DelegateCommand LoginCommand { get; private set; }
        /// <summary>Redirects to the previous page </summary>
        public DelegateCommand BackCommand { get; private set; }
        public DelegateCommand ForgotPasswordCommand { get; private set; }
        public DelegateCommand CreateAccountCommand { get; private set; }
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
        public LoginPageViewModel(IConnectionManager connectionmanager, IOnUserLoginNewsGetter onUserLoginNewsGetter)
        {
            this._connectionManager = connectionmanager;
            this._onUserLoginNewsGetter = onUserLoginNewsGetter;
            this.ForgotPasswordCommand = new DelegateCommand(() => App.MyNavigationService.Navigate("ForgotPassword", null));
            this.CreateAccountCommand = new DelegateCommand(() => App.MyNavigationService.Navigate("Register", null));
            this.LoginCommand = new DelegateCommand(LoginCommandExec);
            this.BackCommand = new DelegateCommand(App.MyNavigationService.GoBack);
        }

        private async void LoginCommandExec()
        {
            if (!App.InternetAvailabilityWatcher.IsInternetUp)
                return;
            SetProgressRingVisibility(true);
            bool requestResult;
            try
            {
                requestResult = await this._connectionManager.ConnectUser(this.UserName, this.Password);
            }
            catch
            {
                SetProgressRingVisibility(false);
                return;
            }
            if (!requestResult)
                DisplayMessage("Wrong user name/password");
            else
            {
                var result = await this._onUserLoginNewsGetter.GetUserNewsInformation();
                while (!result)
                    result = await this._onUserLoginNewsGetter.GetUserNewsInformation();
                App.MyNavigationService.GoBack();
            }
            SetProgressRingVisibility(false);
        }
    }
}
