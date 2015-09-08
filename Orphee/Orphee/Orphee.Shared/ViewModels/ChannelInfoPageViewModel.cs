using System.Collections.Generic;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.RestApiManagement;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    public class ChannelInfoPageViewModel : ViewModel, IChannelInfoPageViewModel
    {
        public List<string> CreationList { get; private set; }
        public DelegateCommand BackCommand { get; private set; }
        private string _userName;

        public string UserName
        {
            get { return this._userName; }
            set
            {
                if (this._userName != value)
                    SetProperty(ref this._userName, value);
            }
        }

        public ChannelInfoPageViewModel()
        {
            this.CreationList = new List<string>();
            this.BackCommand = new DelegateCommand(() => App.MyNavigationService.GoBack());
            InitCreationList();
        }

        private void InitCreationList()
        {
            for (var i = 0; i < 12; i++)
                this.CreationList.Add("Creation " + i);
        }

        public override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            var user = navigationParameter as User;
            if (user != null)
                this.UserName = user.Name;
            else
                this.UserName = "User";
        }
    }
}
