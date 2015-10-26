using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Core;
using Orphee.RestApiManagement.Models.Interfaces;

namespace Orphee.RestApiManagement.Models
{
    public class NewsParser : INewsParser
    {
        private Dictionary<string, string> _messageDictionary;

        public NewsParser()
        {
            this._messageDictionary = new Dictionary<string, string>();
            InitMessageDictionary();
        }

        private void InitMessageDictionary()
        {
            this._messageDictionary.Add("like", " liked");
            this._messageDictionary.Add("comments", " commented");
            this._messageDictionary.Add("friend", " wants to be your friend");
            this._messageDictionary.Add("newFriend", " is now your friend");
            this._messageDictionary.Add("creations", " has posted a new creation");
        }

        public void ParseNewsList()
        {
            foreach (var news in RestApiManagerBase.Instance.UserData.User.NotificationList)
                news.Message = GenerateMessage(news);
        }

        private string GenerateMessage(News news)
        {
            if (news.Type == "like" || news.Type == "comments")
                return (news.Creator.Name + this._messageDictionary[news.Type] + " " + news.Creation.Name + " !");
            if (news.Type == "friend" || news.Type == "newFriend")
            {
                RestApiManagerBase.Instance.UserData.User.HasReceivedFriendConfirmationNotification = true;
                if (news.Type == "friend")
                    RestApiManagerBase.Instance.UserData.User.PendingFriendList.Add(news.Creator);
            }
             return news.Creator.Name + this._messageDictionary[news.Type] + " !";
        }
    }
}
