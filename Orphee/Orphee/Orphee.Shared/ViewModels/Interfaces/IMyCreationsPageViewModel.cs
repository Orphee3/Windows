using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Data;
using Microsoft.Practices.Prism.Commands;
using Orphee.RestApiManagement.Models;

namespace Orphee.ViewModels.Interfaces
{
    public interface IMyCreationsPageViewModel
    {
        ObservableCollection<Creation> CreationList { get; set; }
        DelegateCommand BackCommand { get; }
    }
}
