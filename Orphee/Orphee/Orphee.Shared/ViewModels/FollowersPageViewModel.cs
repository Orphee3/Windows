using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    public class FollowersPageViewModel : ViewModel, IFollowersPageViewModel
    {
        public DelegateCommand BackCommand { get; private set; }

        public FollowersPageViewModel()
        {
            this.BackCommand = new DelegateCommand(() => App.MyNavigationService.GoBack());
        }
    }
}
