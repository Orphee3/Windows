using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Commands;
using Orphee.RestApiManagement.Models;

namespace Orphee.ViewModels.Interfaces
{
    /// <summary>
    /// MyCreationsPageViewModel interfacd
    /// </summary>
    public interface IMyCreationsPageViewModel
    {
        /// <summary>List of the user's creation </summary>
        ObservableCollection<Creation> CreationList { get; set; }
        /// <summary>Redirects to the previous page </summary>
        DelegateCommand BackCommand { get; }
    }
}
