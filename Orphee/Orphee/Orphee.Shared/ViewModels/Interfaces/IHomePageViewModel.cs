using System.Collections.ObjectModel;
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
    }
}
