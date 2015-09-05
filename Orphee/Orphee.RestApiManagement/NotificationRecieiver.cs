using System.Collections.Generic;
using Windows.Networking.Connectivity;
using Newtonsoft.Json.Linq;
using Quobject.SocketIoClientDotNet.Client;

namespace Orphee.RestApiManagement
{
    public class NotificationRecieiver
    {
        private Socket _socket;
        public bool IsSocketConnected { get; private set; }

        public NotificationRecieiver()
        {
            NetworkInformation.NetworkStatusChanged += OnNetworkStatusChanged;
        }
        public void Run()
        {
            if (!IsInternet())
                return;
            this.IsSocketConnected = false;
            this._socket = IO.Socket("http://163.5.84.242:3000", new IO.Options()
            {
                Query = new Dictionary<string, string>() { {"token", RestApiManagerBase.Instance.UserData.Token} },
                ForceNew = true,
                Port = 3000,
                Hostname = "163.5.84.242",
            });
            this._socket.On(Socket.EVENT_CONNECT, () =>
            {
                if (!this.IsSocketConnected)
                {
                    RestApiManagerBase.Instance.UserData.User.HasReceivedFriendNotification = true;
                    this._socket.Emit("subscribe", JObject.FromObject(new {channel = RestApiManagerBase.Instance.UserData.User.Id}));
                    this.IsSocketConnected = true;
                }
            });
            this._socket.Connect();
            this._socket.On("friend", (data) =>
            {
                RestApiManagerBase.Instance.UserData.User.HasReceivedFriendNotification = true;
            });
            this._socket.On(Socket.EVENT_CONNECT_ERROR, (data) =>
            {
                if (IsInternet())
                    Run();
            });
            this._socket.On(Socket.EVENT_CONNECT_TIMEOUT, (data) =>
            {
                if (IsInternet())
                    Run();
            });

            this._socket.On("error", (data) =>
            {
                if (IsInternet())
                    Run();
            });
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
                CloseSocket();
            else
                Run();
        }
    }
}
