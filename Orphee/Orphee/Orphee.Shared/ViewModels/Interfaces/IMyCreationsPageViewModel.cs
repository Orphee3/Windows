using System.Collections.Generic;
using Windows.UI.Xaml.Data;
using Microsoft.Practices.Prism.Commands;
using Orphee.RestApiManagement.Models;

namespace Orphee.ViewModels.Interfaces
{
    public interface IMyCreationsPageViewModel
    {
        DelegateCommand BackCommand { get; }
    }
}
