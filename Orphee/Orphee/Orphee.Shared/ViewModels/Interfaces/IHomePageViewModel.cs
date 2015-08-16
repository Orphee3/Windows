using System.Collections.ObjectModel;

namespace Orphee.ViewModels.Interfaces
{
    public interface IHomePageViewModel
    {
        ObservableCollection<string> FlowList { get; set; } 
    }
}
