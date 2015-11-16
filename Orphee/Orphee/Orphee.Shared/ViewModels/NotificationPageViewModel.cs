using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Commands;
using Orphee.RestApiManagement.Models;
using Orphee.RestApiManagement.Models.Interfaces;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    public class NotificationPageViewModel : ViewModelExtend, INotificationPageViewModel
    {
        public DelegateCommand<INews> ChannelInfoCommand { get; private set; }
        public DelegateCommand<INews> CreationInfoCommand { get; private set; }
        public DelegateCommand BackButtonCommand { get; private set; }
        public ObservableCollection<News> NewsList { get; private set; }

        public NotificationPageViewModel()
        {
            this.BackButtonCommand = new DelegateCommand(() => App.MyNavigationService.GoBack());
            this.ChannelInfoCommand = new DelegateCommand<INews>(ChannelInfoCommandExec);
            this.CreationInfoCommand = new DelegateCommand<INews>(CreationInfoCommandExec);
            this.NewsList = new ObservableCollection<News>();
            SetProgressRingVisibility(true);
            InitNewsList();
        }

        private void InitNewsList()
        {
            if (!VerifyReturnedValue(RestApiManagerBase.Instance.UserData.User.NotificationList, ""))
                return;
            foreach (var news in RestApiManagerBase.Instance.UserData.User.NotificationList)
                this.NewsList.Add(news);
        }

        private void CreationInfoCommandExec(INews news)
        {

        }

        private void ChannelInfoCommandExec(INews news)
        {
            
        }
    }
}
