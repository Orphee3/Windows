﻿using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Microsoft.Practices.Prism.Commands;
using Orphee.RestApiManagement;

namespace Orphee.ViewModels.Interfaces
{
    public interface ISocialPageViewModel
    {
        ObservableCollection<User> UserList { get; set; }
        DelegateCommand LoginCommand { get; }
        DelegateCommand<User> AddFriendCommand { get; }
    }
}
