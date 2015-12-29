using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Orphee.RestApiManagement.Models;
using Orphee.RestApiManagement.Models.Interfaces;
using Quobject.SocketIoClientDotNet.Client;

namespace Orphee.Models
{
    public class SocketListener
    {
        private readonly Socket _socket;
        private readonly IConversationParser _conversationParser;

        public SocketListener(Socket socket, IConversationParser conversationParser)
        {
            this._socket = socket;
            this._conversationParser = conversationParser;
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
            this._socket.On("join game room", JoinGameRoomNotificationReceiver);
            this._socket.On("create game room", CreateGameRoomNotificationReceiver);
            this._socket.On("get game rooms", GetGameRoomsNotificationReceiver);
            this._socket.On("data game", DataGameNotificationReceiver);
            this._socket.On("get all data game", GetAllDataGameNotificationReceiver);
            this._socket.On("host send data", HostSendDataNotificationReceiver);
            this._socket.On("big bang", BigBangNotificationReceiver);
        }

        #region Listeners

        private void JoinGameRoomNotificationReceiver(object data)
        {
            var dataToString = data.ToString();
        }

        private void GetGameRoomsNotificationReceiver(object data)
        {
            var dataToString = data.ToString();
        }
        private void CreateGameRoomNotificationReceiver(object data)
        {
            var dataToString = data.ToString();
        }
        private void DataGameNotificationReceiver(object data)
        {
            var dataToString = data.ToString();
        }
        private void GetAllDataGameNotificationReceiver(object data)
        {
            var dataToString = data.ToString();
        }
        private void HostSendDataNotificationReceiver(object data)
        {
            var dataToString = data.ToString();
        }
        private void BigBangNotificationReceiver(object data)
        {
            var dataToString = data.ToString();
        }


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

        private  void ErrorNotificationReceiver(object data)
        {

        }

        private async void FriendNotificationReceiver(object data)
        {
            var userJson = JObject.FromObject(data);
            var userAskingForFriendShip = JsonConvert.DeserializeObject<UserBase>(userJson["userSource"].ToString());

            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                RestApiManagerBase.Instance.UserData.User.HasReceivedFriendNotification = true;
                RestApiManagerBase.Instance.UserData.User.PendingFriendList.Add(userAskingForFriendShip);
            });
        }

        private async void NewFriendNotificationReceiver(object data)
        {
            var userJson = JObject.FromObject(data);
            var userThatConfirmedFriendship = JsonConvert.DeserializeObject<UserBase>(userJson["userSource"].ToString());

            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                RestApiManagerBase.Instance.UserData.User.HasReceivedFriendConfirmationNotification = true;
                RestApiManagerBase.Instance.UserData.User.FriendList.Add(userThatConfirmedFriendship);
            });
        }

        private void CommentNotificationReceiver(object data)
        {
            var userJson = JObject.FromObject(data);
            var media = JsonConvert.DeserializeObject<Creation>(userJson["media"].ToString());
            var comment = new Comment {CreationId = media.Id,};
            RestApiManagerBase.Instance.UserData.User.PendingCommentList.Add(comment);
            RestApiManagerBase.Instance.UserData.User.HasReceivedCommentNotification = true;
        }

        private void LikeNotificationReceiver(object data)
        {

        }

        private void CreationNotificationReceiver(object data)
        {

        }

        private async void MessageNotificationReceiver(object data)
        {
            var dataString = JObject.FromObject(data);
            var message = JsonConvert.DeserializeObject<Message>(dataString["message"].ToString());
            message.Type = dataString["type"].ToString();
            message.TargetRoom = message.Type != "group message" ? dataString["target"]["_id"].ToString() : dataString["target"].ToString();
            message.User = JsonConvert.DeserializeObject<UserBase>(dataString["source"].ToString());
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                message.SetProperties();
                var belongingConversation = RestApiManagerBase.Instance.UserData.User.ConversationList.FirstOrDefault(conv => conv.Id == message.TargetRoom);
                if (belongingConversation == null)
                    CreateNewConversation(message);
                else
                    UpdateBelongingConversation(RestApiManagerBase.Instance.UserData.User.ConversationList.IndexOf(belongingConversation), message);
                RestApiManagerBase.Instance.UserData.User.HasReceivedMessageNotification = true;
            });
        }

        private void CreateNewConversation(Message message)
        {
            var newConversation = new Conversation() { ConversationPictureSource = message.UserPictureSource, Id = message.TargetRoom, IsPrivate = message.Type == "private message", UserList = new List<UserBase> { message.User } };
            newConversation.Messages.Add(message);
            newConversation.HasReceivedNewMessage = true;
            this._conversationParser.ParseConversationList(new ObservableCollection<Conversation> { newConversation });
        }

        private void UpdateBelongingConversation(int conversationIndex, Message message)
        {
            RestApiManagerBase.Instance.UserData.User.ConversationList[conversationIndex].LastMessageDateString = message.Hour;
            RestApiManagerBase.Instance.UserData.User.ConversationList[conversationIndex].LastMessagePreview = message;
            RestApiManagerBase.Instance.UserData.User.ConversationList[conversationIndex].LastMessagePreview.ReceivedMessage = this._conversationParser.GetSubstringIfTooLong(message.ReceivedMessage);
            RestApiManagerBase.Instance.UserData.User.ConversationList[conversationIndex].HasReceivedNewMessage = true;
        }

        private void ChatGroupCreatedNotificationReceiver(object data)
        {
            var convertedData = JObject.FromObject(data);
            RestApiManagerBase.Instance.UserData.User.ConversationList.Last().Id = convertedData["room"]["_id"].ToString();
        }

        private void PieceInfoNotificationReceiver(object data)
        {

        }

        private void TempoNotificationReceiver(object data)
        {

        }

        private void NoteNotificationReceiver(object data)
        {

        }

        private void AddColumnsNotificationReceiver(object data)
        {

        }

        private void PieceQuitNotificationReceiver(object data)
        {

        }

        #endregion
    }
}
