using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.RestApiManagement;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    public class FriendPageViewModel : ViewModel, IFriendPageViewModel
    {
        public DelegateCommand GoBackCommand { get; private set; }
        public ObservableCollection<User> FriendList { get; private set; }

        public FriendPageViewModel()
        {
            this.GoBackCommand = new DelegateCommand(() => App.MyNavigationService.GoBack());
            this.FriendList = new ObservableCollection<User>();
            InitFriendList();
        }

        private void InitFriendList()
        {
            this.FriendList.Add(new User() { Name = "Jéromin" });
            this.FriendList.Add(new User() { Name = "Tristan" });
            this.FriendList.Add(new User() { Name = "Eric" });
            this.FriendList.Add(new User() { Name = "Massil" });
            this.FriendList.Add(new User() { Name = "Billy" });
            this.FriendList.Add(new User() { Name = "Charles-Henry" });
            this.FriendList.Add(new User() { Name = "Xavier-Ignace" });
        }
    }
}
