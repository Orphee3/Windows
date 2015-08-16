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
            this.UserName = "MegaGomes";
            this.Password = "lepouletcestgenial";
            this._connectionManager = connectionmanager;
            this.LoginCommand = new DelegateCommand(LoginCommandExec);
            this.BackCommand = new DelegateCommand(App.MyNavigationService.GoBack);
        }

        private async void LoginCommandExec()
        {
            if (await this._connectionManager.ConnectUser(this.UserName, this.Password))
                App.MyNavigationService.GoBack();
        }
    }
}
