using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Orphee.RestApiManagement.Models;
using Quobject.SocketIoClientDotNet.Client;

namespace Orphee.Models
{
    public class SocketListener
    {
        private readonly Socket _socket;

        public SocketListener(Socket socket)
        {
            this._socket = socket;
        }

        public void InitSocketListeners()
        {
            this._socket.On(Socket.EVENT_CONNECT_ERROR, ConnectErrorNotificationReceiver);
            this._socket.On(Socket.EVENT_CONNECT_TIMEOUT, ConnectTimeoutNotificationReceiver);
            this._socket.On(Socket.EVENT_DISCONNECT, DisconnectNotificationReceiver);
            this._socket.On(Socket.EVENT_RECONNECT, ReconnectNotificationReceiver);
            this._socket.On(Socket.EVENT_RECONNECTING, ReconnectingNotificationReceiver);
            this._socket.On(Socket.EVENT_RECONNECT_ATTEMPT, ReconnectAttemptNotificationReceiver);
            this._socket.On(Socket.EVENT_RECONNECT_FAILED, ReconnectFailedNotificationReceiver);
            this._socket.On(Socket.EVENT_RECONNECT_ERROR, ReconnectErrorNotificationReceiver);
            this._socket.On(Socket.EVENT_ERROR, ErrorNotificationReceiver);
            this._socket.On("friend", FriendNotificationReceiver);
            this._socket.On("newFriend", NewFriendNotificationReceiver);
            this._socket.On("comments", CommentNotificationReceiver);
            this._socket.On("likes", LikeNotificationReceiver);
            this._socket.On("creations", CreationNotificationReceiver);
            this._socket.On("private message", MessageNotificationReceiver);
            this._socket.On("group message", MessageNotificationReceiver);
            this._socket.On("create chat group", ChatGroupCreatedNotificationReceiver);
        }

        #region Listeners

        private void ConnectErrorNotificationReceiver(object data)
        {

        }

        private void ConnectTimeoutNotificationReceiver(object data)
        {

        }

        private void DisconnectNotificationReceiver(object data)
        {

        }

        private void ReconnectNotificationReceiver(object data)
        {

        }

        private void ReconnectingNotificationReceiver(object data)
        {

        }

        private void ReconnectAttemptNotificationReceiver(object data)
        {

        }

        private void ReconnectFailedNotificationReceiver(object data)
        {

        }

        private void ReconnectErrorNotificationReceiver(object data)
        {

        }

        private void ErrorNotificationReceiver(object data)
        {

        }

        private void FriendNotificationReceiver(object data)
        {
            var userJson = JObject.FromObject(data);
            var userAskingForFriendShip = JsonConvert.DeserializeObject<UserBase>(userJson["userSource"].ToString());
            RestApiManagerBase.Instance.UserData.User.HasReceivedFriendNotification = true;
            RestApiManagerBase.Instance.UserData.User.PendingFriendList.Add(userAskingForFriendShip);
        }

        private void NewFriendNotificationReceiver(object data)
        {
            var userJson = JObject.FromObject(data);
            var userThatConfirmedFriendship = JsonConvert.DeserializeObject<UserBase>(userJson["userSource"].ToString());
            RestApiManagerBase.Instance.UserData.User.HasReceivedFriendConfirmationNotification = true;
            RestApiManagerBase.Instance.UserData.User.FriendList.Add(userThatConfirmedFriendship);
        }

        private void CommentNotificationReceiver(object data)
        {
            var userJson = JObject.FromObject(data);
            var media = JsonConvert.DeserializeObject<Creation>(userJson["media"].ToString());
            var comment = new Comment { CreationId = media.Id, };
            RestApiManagerBase.Instance.UserData.User.PendingCommentList.Add(comment);
            RestApiManagerBase.Instance.UserData.User.HasReceivedCommentNotification = true;
        }

        private void LikeNotificationReceiver(object data)
        {

        }

        private void CreationNotificationReceiver(object data)
        {

        }

        private void MessageNotificationReceiver(object data)
        {
            var dataString = JObject.FromObject(data);
            var message = JsonConvert.DeserializeObject<Message>(dataString["message"].ToString());
            message.TargetRoom = dataString["target"].ToString();
            message.Type = dataString["type"].ToString();
            message.User = JsonConvert.DeserializeObject<UserBase>(dataString["source"].ToString());
            RestApiManagerBase.Instance.UserData.User.PendingMessageList.Add(message);
            RestApiManagerBase.Instance.UserData.User.HasReceivedMessageNotification = true;
        }

        private void ChatGroupCreatedNotificationReceiver(object data)
        {
            var convertedData = JObject.FromObject(data);
            RestApiManagerBase.Instance.UserData.User.ConversationList.Last().Id = convertedData["room"]["_id"].ToString();
        }
        #endregion
    }
}
