using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    public class CreationInfoPageViewModel : ICreationInfoPageViewModel, INavigationAware
    {
        public DelegateCommand GoBackCommand { get; private set; }

        public CreationInfoPageViewModel()
        {
            this.GoBackCommand = new DelegateCommand(() => App.MyNavigationService.GoBack());
        }

        public void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            
        }

        public void OnNavigatedFrom(Dictionary<string, object> viewModelState, bool suspending)
        {
        }
    }
}
