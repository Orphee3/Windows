using System.Collections.Generic;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    public class ChannelInfoPageViewModel : ViewModel, IChannelInfoPageViewModel
    {
        public List<string> CreationList { get; private set; }
        public DelegateCommand BackCommand { get; private set; }

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
    }
}
