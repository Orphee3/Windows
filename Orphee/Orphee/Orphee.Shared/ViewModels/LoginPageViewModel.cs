using System.Threading.Tasks;
using Microsoft.Practices.Prism.Commands;
using Orphee.Models.Interfaces;
using Orphee.Models.OAuth2SDK;
using Orphee.Models.OAuth2SDK.Services.Interfaces;
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
        public DelegateCommand<string> SDKLoginCommand { get; private set; }
        /// <summary>User name of the user </summary>
        public string UserName { get; set; }
        /// <summary>User's password </summary>
        public string Password { get; set; }

        private readonly IConnectionManager _connectionManager;
        private readonly ISessionService _sessionService;
        private readonly IAuthLogin _authLogin;
        /// <summary>
        /// Constructor initializing connectionManager
        /// through dependency injection
        /// </summary>
        /// <param name="connectionmanager">Manages the connetion of the user so it gets logged in</param>
        public LoginPageViewModel(IConnectionManager connectionmanager, IOnUserLoginNewsGetter onUserLoginNewsGetter, ISessionService sessionService, IAuthLogin authLogin)
        {
            this._connectionManager = connectionmanager;
            this._onUserLoginNewsGetter = onUserLoginNewsGetter;
            this._sessionService = sessionService;
            this._authLogin = authLogin;
            this.ForgotPasswordCommand = new DelegateCommand(() => App.MyNavigationService.Navigate("ForgotPassword", null));
            this.CreateAccountCommand = new DelegateCommand(() => App.MyNavigationService.Navigate("Register", null));
            this.LoginCommand = new DelegateCommand(LoginCommandExec);
            this.SDKLoginCommand = new DelegateCommand<string>(SDKLoginCommandExec);
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

        private async void SDKLoginCommandExec(string provider)
        {
            var auth = await _sessionService.LoginAsync(provider);
            if (auth == null)
            {
                return;
            }
            if (!auth.Value)
                DisplayMessage("Authetification failed.");
            else
            {
                var session = await this._sessionService.GetSession();
                var result = await SendTokenToServer(provider, session.Code);
                if (result)
                    App.MyNavigationService.GoBack();
            }
        }

        private async Task<bool> SendTokenToServer(string provider, string code)
        {
            if (!App.InternetAvailabilityWatcher.IsInternetUp)
                return false;
            SetProgressRingVisibility(true);
            bool requestResult;
            try
            {
                requestResult = await this._authLogin.ConnectThroughSDK(provider, code, Constants.GoogleClientId, Constants.GoogleCallbackUrl);
            }
            catch
            {
                SetProgressRingVisibility(false);
                return false;
            }
            if (!requestResult)
                return false;
            else
            {
                var result = await this._onUserLoginNewsGetter.GetUserNewsInformation();
                while (!result)
                    result = await this._onUserLoginNewsGetter.GetUserNewsInformation();
                App.MyNavigationService.GoBack();
            }
            SetProgressRingVisibility(false);
            return true;
        }
    }
}
