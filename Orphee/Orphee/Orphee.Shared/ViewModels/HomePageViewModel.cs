using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.RestApiManagement.Interfaces;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    public class HomePageViewModel : ViewModel, IHomePageViewModel
    {
        public ObservableCollection<string> FlowList { get; set; }
        private readonly INewsFlowGetter _newsFlowGetter;

        public HomePageViewModel(INewsFlowGetter newsFlowGetter)
        {
            this._newsFlowGetter = newsFlowGetter;
            this.FlowList = new ObservableCollection<string>();
            InitFlowList();
        }

        private void InitFlowList()
        {
            for (int i = 0; i < 250; i++)
            {
                this.FlowList.Add("Trou");
                this.FlowList.Add("Cul");
                this.FlowList.Add("Bite");
                this.FlowList.Add("Chatte");
            }
        }
    }
}
