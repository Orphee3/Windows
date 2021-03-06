﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Orphee.Models.Interfaces;
using Orphee.RestApiManagement.Getters.Interfaces;
using Orphee.RestApiManagement.Models;
using Orphee.RestApiManagement.Models.Interfaces;

namespace Orphee.Models
{
    public class OnUserLoginNewsGetter : IOnUserLoginNewsGetter
    {
        private readonly IGetter _getter;
        private readonly IConversationParser _conversationParser;
        private readonly INewsParser _newsParser;
        public OnUserLoginNewsGetter(IGetter getter, IConversationParser conversationParser, INewsParser newsParser)
        {
            this._getter = getter;
            this._conversationParser = conversationParser;
            this._newsParser = newsParser;
        }
        public async Task<bool> GetUserNewsInformation()
        {
            var userFriends = (await this._getter.GetInfo<List<UserBase>>(RestApiManagerBase.Instance.RestApiPath["users"] + "/" + RestApiManagerBase.Instance.UserData.User.Id + "/friends")).OrderBy(f => f.Name).ToList();
            var userConversations = await this._getter.GetInfo<ObservableCollection<Conversation>>(RestApiManagerBase.Instance.RestApiPath["users"] + "/" + RestApiManagerBase.Instance.UserData.User.Id + "/rooms");
            var userNotifications = (await this._getter.GetInfo<List<News>>(RestApiManagerBase.Instance.RestApiPath["users"] + "/" + RestApiManagerBase.Instance.UserData.User.Id + "/news")).Where(t => t.Creator.Id != RestApiManagerBase.Instance.UserData.User.Id).ToList();
            if (userFriends == null || userConversations == null || userNotifications == null)
                return false;
            GetNewFriendsFromList(userFriends);
            GetNewConversationFromList(userConversations);
            GetNewNotificationsFromList(userNotifications);
            return true;
        }

        private void GetNewFriendsFromList(List<UserBase> userList)
        {
            if (RestApiManagerBase.Instance.UserData.User.FriendList == null)
                RestApiManagerBase.Instance.UserData.User.FriendList = new List<UserBase>();
            var userFriendList = RestApiManagerBase.Instance.UserData.User.FriendList;
            foreach (var user in userList.Where(user => userFriendList.All(u => u.Id != user.Id)))
                RestApiManagerBase.Instance.UserData.User.FriendList.Add(user);
        }

        private void GetNewConversationFromList(ObservableCollection<Conversation> conversationList)
        {
            if (RestApiManagerBase.Instance.UserData.User.ConversationList == null)
                RestApiManagerBase.Instance.UserData.User.ConversationList = new ObservableCollection<Conversation>();
            this._conversationParser.ParseConversationList(conversationList);
        }

        private void GetNewNotificationsFromList(List<News> newsList)
        {
            if (RestApiManagerBase.Instance.UserData.User.NotificationList == null)
                RestApiManagerBase.Instance.UserData.User.NotificationList = new List<News>();
            var userNotificationList = RestApiManagerBase.Instance.UserData.User.NotificationList;
            foreach (var news in newsList.Where(news => userNotificationList.All(n => n.Id != news.Id)))
                RestApiManagerBase.Instance.UserData.User.NotificationList.Add(news);
            this._newsParser.ParseNewsList();
        }
    }
}
