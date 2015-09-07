using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Microsoft.Practices.Prism.Commands;
using Orphee.Models;

namespace Orphee.ViewModels.Interfaces
{
    public interface IChatPageViewModel
    {
        DelegateCommand BackCommand { get; }
        ObservableCollection<MyDictionary> Conversation { get; }
    }
}
