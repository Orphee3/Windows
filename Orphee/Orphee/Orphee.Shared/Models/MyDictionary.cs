using System;
using Windows.UI.Xaml;
using Orphee.RestApiManagement;

namespace Orphee.Models
{
    public class MyDictionary
    {
        public Visibility MyMessageVisibility { get; private set; }
        public Visibility ItsMessageVisibility { get; private set; }
        public string Message { get; private set; }
        public string Hour { get; set; }
        public string UserPictureSource { get; private set; }

        public MyDictionary(Message message)
        {
            this.MyMessageVisibility = message.User.Id == RestApiManagerBase.Instance.UserData.User.Id ? Visibility.Visible : Visibility.Collapsed;
            this.ItsMessageVisibility = this.MyMessageVisibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
            this.Message = message.ReceivedMessage;
            this.Hour = message.Date.ToString("HH:mm");
            this.UserPictureSource = message.User.Picture ?? "/Assets/defaultUser.png";
        }
    }
}
