using System.Collections.Generic;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    public class ChannelInfoPageViewModel : ViewModel, IChannelInfoPageViewModel
    {
        public List<string> CreationList { get; private set; }

        public ChannelInfoPageViewModel()
        {
            this.CreationList = new List<string>();
            InitCreationList();
        }

        private void InitCreationList()
        {
            for (var i = 0; i < 12; i++)
                this.CreationList.Add("Creation " + i);
        }
    }
}
