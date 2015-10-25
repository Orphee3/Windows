using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.RestApiManagement.Getters.Interfaces;
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
        private readonly IGetter _getter;

        public NotificationPageViewModel(IGetter getter)
        {
            this._getter = getter;
            this.BackButtonCommand = new DelegateCommand(() => App.MyNavigationService.GoBack());
            this.ChannelInfoCommand = new DelegateCommand<INews>(ChannelInfoCommandExec);
            this.CreationInfoCommand = new DelegateCommand<INews>(CreationInfoCommandExec);
            this.NewsList = new ObservableCollection<News>();
            if (!RestApiManagerBase.Instance.NotificationRecieiver.IsInternet())
                DisplayMessage("Connexion unavailable");
            else
            {
                this.ProgressRingVisibility = Visibility.Visible;
                this.IsProgressRingActive = true;
                InitNewsList();
            }
        }

        private async void InitNewsList()
        {
            List<News> newsTmpList;
            try
            {
                newsTmpList = await this._getter.GetInfo<List<News>>(RestApiManagerBase.Instance.RestApiPath["users"] + "/" + RestApiManagerBase.Instance.UserData.User.Id + "/news");
            }
            catch (Exception)
            {
                DisplayMessage("Request failed");
                this.IsProgressRingActive = false;
                this.ProgressRingVisibility = Visibility.Collapsed;
                return;
            }
            ParseData(newsTmpList);
            this.IsProgressRingActive = false;
            this.ProgressRingVisibility = Visibility.Collapsed;
        }

        private void ParseData(List<News> newsTmpList)
        {
            foreach (var news in newsTmpList)
            {
                if (news.Creator.Id != RestApiManagerBase.Instance.UserData.User.Id)
                {
                    if (news.Type == "like")
                        news.Message = news.Creator.Name + " liked" + news.Creation.Name + " !";
                    else if (news.Type == "comments")
                        news.Message = news.Creator.Name + " commented" + news.Creation.Name + " !";
                    else if (news.Type == "friend")
                        news.Message = news.Creator.Name + " wants to be your friend !";
                    else if (news.Type == "newFriend")
                        news.Message = news.Creator.Name + " is now your friend !";
                    else if (news.Type == "creations")
                        news.Message = news.Creator.Name + " has posted a new creation !";
                    this.NewsList.Add(news);
                }
            }
        }

        private void CreationInfoCommandExec(INews news)
        {

        }

        private void ChannelInfoCommandExec(INews news)
        {
            
        }

        private async void DisplayMessage(string message)
        {
            var messageDialog = new MessageDialog(message);

            await messageDialog.ShowAsync();
        }
    }
}
