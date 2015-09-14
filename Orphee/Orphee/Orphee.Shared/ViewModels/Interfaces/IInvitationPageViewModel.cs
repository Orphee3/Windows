using System;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Commands;
using Orphee.RestApiManagement;
using Orphee.RestApiManagement.Models;

namespace Orphee.ViewModels.Interfaces
{
    public interface IInvitationPageViewModel
    {
        DelegateCommand GoBackCommand { get; }
        DelegateCommand<User> AcceptCommand { get; }
        DelegateCommand<User> CancelCommand { get; }
        ObservableCollection<User> InvitationList { get; } 
    }
}
