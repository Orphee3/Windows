using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    public class FollowsPageViewModel : ViewModel, IFollowsPageViewModel
    { 
        public DelegateCommand BackCommand { get; private set; }

        public FollowsPageViewModel()
        {
            this.BackCommand = new DelegateCommand(() => App.MyNavigationService.GoBack());
        }
    }
}
