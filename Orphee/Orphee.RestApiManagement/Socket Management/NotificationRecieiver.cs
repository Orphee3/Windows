using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Networking.Connectivity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Orphee.RestApiManagement.Models;
using Quobject.SocketIoClientDotNet.Client;

namespace Orphee.RestApiManagement.Socket_Management
{
    /// <summary>
    /// Class managing the IOSocket
    /// </summary>
    public class NotificationRecieiver
    {
        private Socket _socket;
        /// <summary>True if the socket is connected and false if it's not</summary>
        public bool IsSocketConnected { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public NotificationRecieiver()
        {
            NetworkInformation.NetworkStatusChanged += OnNetworkStatusChanged;
        }

        /// <summary>
        /// Initialize the socket
        /// </summary>
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

        /// <summary>
        /// Creates the listeners
        /// </summary>
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
            this._socket.On("friend", data =>
            {
                var userJson = JObject.FromObject(data);
                var userAskingForFriendShip = JsonConvert.DeserializeObject<User>(userJson["userSource"].ToString());
                RestApiManagerBase.Instance.UserData.User.HasReceivedFriendNotification = true;
                RestApiManagerBase.Instance.UserData.User.PendingFriendList.Add(userAskingForFriendShip);
            });
            this._socket.On("newFriend", data =>
            {
                RestApiManagerBase.Instance.UserData.User.HasReceivedFriendConfirmationNotification = true;
            });
            this._socket.On("comments", data =>
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
            this._socket.On("likes", data =>
            {

            });
            this._socket.On("creations", data =>
            {

            });
            this._socket.On(Socket.EVENT_CONNECT_ERROR, data =>
            {
                CloseSocket();
                if (IsInternet())
                    this._socket.Connect();
            });
            this._socket.On(Socket.EVENT_CONNECT_TIMEOUT, data =>
            {
                CloseSocket();
                if (IsInternet())
                    this._socket.Connect();
            });

            this._socket.On("error", data =>
            {
                CloseSocket();
                if (IsInternet())
                    this._socket.Connect();
            });
            this._socket.On("private message", data =>
            {
                var dataString = JObject.FromObject(data);
                var message = JsonConvert.DeserializeObject<Message>(dataString["message"].ToString());
                message.Type = dataString["type"].ToString();
                message.User = JsonConvert.DeserializeObject<User>(dataString["source"].ToString());
                RestApiManagerBase.Instance.UserData.User.PendingMessageList.Add(message);
                RestApiManagerBase.Instance.UserData.User.HasReceivedMessageNotification = true;
            });
            this._socket.On("group message", data =>
            {
                var dataString = JObject.FromObject(data);
                var test = data.ToString();
                var message = JsonConvert.DeserializeObject<Message>(dataString["message"].ToString());
                message.TargetRoom = dataString["target"].ToString();
                message.Type = dataString["type"].ToString();
                message.User = JsonConvert.DeserializeObject<User>(dataString["source"].ToString());
                RestApiManagerBase.Instance.UserData.User.PendingMessageList.Add(message);
                RestApiManagerBase.Instance.UserData.User.HasReceivedMessageNotification = true;
            });
            this._socket.On("create chat group", data =>
            {
                var convertedData = JObject.FromObject(data);
                var test = data.ToString();
                RestApiManagerBase.Instance.UserData.User.ConversationList.Last().Id = convertedData["room"]["_id"].ToString();
            });
            this._socket.On(Socket.EVENT_DISCONNECT, () =>
            {

            });
        }

        /// <summary>
        /// Sends a message through IOSocket
        /// </summary>
        /// <param name="messageToSend">Message to send</param>
        /// <param name="userList">Target users of the message</param>
        public async Task<bool> SendPrivateMessage(string messageToSend, User user)
        {
            if (!IsInternet())
                return false;
            try
            {
                var result = await Task.FromResult(this._socket.Emit("private message", JObject.FromObject(new { to = user.Id, message = messageToSend })));
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> SendGroupMessage(string messageToSend, string roomId)
        {
            if (!IsInternet())
                return false;
            try
            {
                var result = await Task.FromResult(this._socket.Emit("group message", JObject.FromObject(new {to = roomId, message = messageToSend})));
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> CreateGroupChat(List<string> userList)
        {
            if (!IsInternet())
                return false;
            try
            {
                var result = await Task.FromResult(this._socket.Emit("create chat group", JObject.FromObject(new {people = userList})));
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Close the socket
        /// </summary>
        public void CloseSocket()
        {
            if (this._socket != null)
            {
                this._socket.Disconnect();
                this._socket.Close();
                this.IsSocketConnected = false;
            }
        }

        /// <summary>
        /// Checks if the internet connexion is available
        /// </summary>
        /// <returns></returns>
        public bool IsInternet()
        {
            ConnectionProfile connections;
            try
            {
                connections = NetworkInformation.GetInternetConnectionProfile();
            }
            catch (Exception)
            {
                return IsInternet();
            }
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
