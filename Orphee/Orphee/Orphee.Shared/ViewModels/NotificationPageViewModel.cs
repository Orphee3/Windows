using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.RestApiManagement.Models;
using Orphee.RestApiManagement.Models.Interfaces;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    public class NotificationPageViewModel : ViewModel, INotificationPageViewModel, ILoadingScreenComponents
    {
        public DelegateCommand<INews> ChannelInfoCommand { get; private set; }
        public DelegateCommand<INews> CreationInfoCommand { get; private set; }
        public DelegateCommand BackButtonCommand { get; private set; }
        public ObservableCollection<News> NewsList { get; private set; }
        private bool _isProgressRingActive;

        public bool IsProgressRingActive
        {
            get { return this._isProgressRingActive; }
            set
            {
                if (this._isProgressRingActive != value)
                    SetProperty(ref this._isProgressRingActive, value);
            }
        }
        private Visibility _progressRingVisibility;

        public Visibility ProgressRingVisibility
        {
            get { return this._progressRingVisibility; }
            set
            {
                if (this._progressRingVisibility != value)
                    SetProperty(ref this._progressRingVisibility, value);
            }
        }

        public NotificationPageViewModel()
        {
            this.BackButtonCommand = new DelegateCommand(() => App.MyNavigationService.GoBack());
            this.ChannelInfoCommand = new DelegateCommand<INews>(ChannelInfoCommandExec);
            this.CreationInfoCommand = new DelegateCommand<INews>(CreationInfoCommandExec);
            this.NewsList = new ObservableCollection<News>();
            this.ProgressRingVisibility = Visibility.Visible;
            this.IsProgressRingActive = true;
            InitNewsList();

        }

        private void InitNewsList()
        {
            foreach (var news in RestApiManagerBase.Instance.UserData.User.NotificationList)
                this.NewsList.Add(news);
            this.IsProgressRingActive = false;
            this.ProgressRingVisibility = Visibility.Collapsed;
        }

        private void CreationInfoCommandExec(INews news)
        {

        }

        private void ChannelInfoCommandExec(INews news)
        {
            
        }
    }
}
