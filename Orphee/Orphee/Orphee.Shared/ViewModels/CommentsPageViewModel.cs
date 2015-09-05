using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    public class CommentsPageViewModel : ViewModel, ICommentsPageViewModel
    {
        public DelegateCommand BackCommand { get; private set; }

        public CommentsPageViewModel()
        {
            this.BackCommand = new DelegateCommand(() => App.MyNavigationService.GoBack());
        }
    }
}
