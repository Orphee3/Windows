using System.Collections.Generic;
using Windows.Networking.Connectivity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Orphee.RestApiManagement.Models;
using Quobject.SocketIoClientDotNet.Client;

namespace Orphee.RestApiManagement.Socket_Management
{
    public class NotificationRecieiver
    {
        private Socket _socket;
        public bool IsSocketConnected { get; private set; }

        public NotificationRecieiver()
        {
            NetworkInformation.NetworkStatusChanged += OnNetworkStatusChanged;
        }

        public void InitSocket()
        {
            if (!this.IsSocketConnected && RestApiManagerBase.Instance.UserData != null)
            {
                this.IsSocketConnected = false;
                this._socket = IO.Socket("http://163.5.84.242:3000", new IO.Options()
                {
                    Query = new Dictionary<string, string>() {{"token", RestApiManagerBase.Instance.UserData.Token}},
                    ForceNew = true,
                    Port = 3000,
                    Hostname = "163.5.84.242",
                });
                Run();
            }
        }
        public void Run()
        {
            this._socket.On("connect", () =>
            {
                if (!this.IsSocketConnected && RestApiManagerBase.Instance.UserData != null)
                {
                    this._socket.Emit("subscribe",
                        JObject.FromObject(new {channel = RestApiManagerBase.Instance.UserData.User.Id}));
                    this.IsSocketConnected = true;
                }
            });
            this._socket.On("friend", (data) =>
            {
                var userJson = JObject.FromObject(data);
                var userAskingForFriendShip = JsonConvert.DeserializeObject<User>(userJson["userSource"].ToString());
                RestApiManagerBase.Instance.UserData.User.HasReceivedFriendNotification = true;
                RestApiManagerBase.Instance.UserData.User.PendingFriendList.Add(userAskingForFriendShip);
            });
            this._socket.On("newFriend", (data) =>
            {
                RestApiManagerBase.Instance.UserData.User.HasReceivedFriendConfirmationNotification = true;
            });
            this._socket.On("comments", (data) =>
            {
                var userJson = JObject.FromObject(data);
                var media = JsonConvert.DeserializeObject<Creation>(userJson["media"].ToString());
                var comment = new Comment
                {
                    CreationId = media.Id,
                };
                RestApiManagerBase.Instance.UserData.User.PendingCommentList.Add(comment);
                RestApiManagerBase.Instance.UserData.User.HasReceivedCommentNotification = true;
            });
            this._socket.On("likes", (data) =>
            {

            });
            this._socket.On("creations", (data) =>
            {

            });
            this._socket.On(Socket.EVENT_CONNECT_ERROR, (data) =>
            {
                CloseSocket();
                if (IsInternet())
                    this._socket.Connect();
            });
            this._socket.On(Socket.EVENT_CONNECT_TIMEOUT, (data) =>
            {
                CloseSocket();
                if (IsInternet())
                    this._socket.Connect();
            });

            this._socket.On("error", (data) =>
            {
                CloseSocket();
                if (IsInternet())
                    this._socket.Connect();
            });
            this._socket.On("private message", (data) =>
            {
                var userJson = JObject.FromObject(data);
                var creator = JsonConvert.DeserializeObject<User>(userJson["source"].ToString());
                if (RestApiManagerBase.Instance.UserData.User.Id != creator.Id)
                {
                    var message = new Message
                    {
                        User = creator,
                        ReceivedMessage = userJson["message"]["message"].ToString()
                    };
                    RestApiManagerBase.Instance.UserData.User.PendingMessageList.Add(message);
                    RestApiManagerBase.Instance.UserData.User.HasReceivedMessageNotification = true;
                }
            });
            this._socket.On(Socket.EVENT_DISCONNECT, () =>
            {

            });
        }

        public void SendMessage(string messageToSend, List<User> userList)
        {
            foreach (var user in userList)
                this._socket.Emit("private message", JObject.FromObject(new { to = user.Id, message = messageToSend }));
        }

        public void CloseSocket()
        {
            if (this._socket != null)
            {
                this._socket.Disconnect();
                this._socket.Close();
                this.IsSocketConnected = false;
            }
        }

        public bool IsInternet()
        {
            var connections = NetworkInformation.GetInternetConnectionProfile();
            var internet = connections != null && connections.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
            return internet;
        }

        private void OnNetworkStatusChanged(object sender)
        {
            var internetConnectionProfile = NetworkInformation.GetInternetConnectionProfile();

            if (internetConnectionProfile == null)
            {
                RestApiManagerBase.Instance.IsConnected = false;
                CloseSocket();
            }
            else
            {
                if (RestApiManagerBase.Instance.UserData != null)
                {
                    RestApiManagerBase.Instance.IsConnected = true;
                    this._socket.Connect();
                }
            }
        }
    }
}
