using System;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.RestApiManagement;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    public class InvitationPageViewModel : ViewModel, IInvitationPageViewModel
    {
        public DelegateCommand GoBackCommand { get; private set; }
        public DelegateCommand<User> AcceptCommand { get; private set; }
        public DelegateCommand<User> CancelCommand { get; private set; }
        public ObservableCollection<User> InvitationList { get; private set; }

        public InvitationPageViewModel()
        {
            this.AcceptCommand = new DelegateCommand<User>((user) => this.InvitationList.Remove(user));
            this.CancelCommand = new DelegateCommand<User>((user) => this.InvitationList.Remove(user));
            this.GoBackCommand = new DelegateCommand(() => App.MyNavigationService.GoBack());
            this.InvitationList = new ObservableCollection<User>();
            InitInvitationList();
        }

        private void InitInvitationList()
        {
            this.InvitationList.Add(new User() { Name = "Paul" });
            this.InvitationList.Add(new User() { Name = "Martin" });
            this.InvitationList.Add(new User() { Name = "Bob Lennon" });
            this.InvitationList.Add(new User() { Name = "Mr Poulpe" });
        }
    }
}
