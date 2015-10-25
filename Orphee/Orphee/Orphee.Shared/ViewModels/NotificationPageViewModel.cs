using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Popups;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.RestApiManagement.Getters.Interfaces;
using Orphee.RestApiManagement.Models;
using Orphee.RestApiManagement.Models.Interfaces;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    public class NotificationPageViewModel : ViewModel, INotificationPageViewModel
    {
        public DelegateCommand<INews> ChannelInfoCommand { get; private set; }
        public DelegateCommand<INews> CreationInfoCommand { get; private set; }
        public DelegateCommand BackButtonCommand { get; private set; }
        public ObservableCollection<News> NewsList { get; private set; }
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
                InitNewsList();
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
                return;
            }
            ParseData(newsTmpList);
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
