using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
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
            if (await this._registrationManager.RegisterUser(this.MailAdress, this.UserName, this.Password))
                App.MyNavigationService.GoBack();
        }
    }
}
