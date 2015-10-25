using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Commands;
using Orphee.RestApiManagement.Models;
using Orphee.RestApiManagement.Models.Interfaces;

namespace Orphee.ViewModels.Interfaces
{
    public interface INotificationPageViewModel
    {
        DelegateCommand<INews> ChannelInfoCommand { get; }
        DelegateCommand<INews> CreationInfoCommand { get; }
        DelegateCommand BackButtonCommand { get; }
        ObservableCollection<News> NewsList { get; } 
    }
}
