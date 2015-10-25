using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Microsoft.Practices.Prism.Commands;
using Orphee.RestApiManagement.Models;

namespace Orphee.ViewModels.Interfaces
{
    /// <summary>
    /// HomePageViewModel interface
    /// </summary>
    public interface IHomePageViewModel
    {
        /// <summary>List of popular creations</summary>
        ObservableCollection<Creation> PopularCreations { get; set; }
        DelegateCommand<Creation> CreationInfoCommand { get; }
        DelegateCommand<Creation> ChannelInfoCommand { get; }
    }
}
