using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Newtonsoft.Json.Linq;
using Orphee.RestApiManagement.Models;
using Quobject.SocketIoClientDotNet.Client;

namespace Orphee.Models
{
    public class SocketManager
    {
        public bool IsSocketConnected { get; set; }
        public SocketEmitter SocketEmitter { get; private set; }
        private SocketListener _socketListener;
        private Socket _socket;

        public SocketManager()
        {
            RestApiManagerBase.Instance.PropertyChanged += InstanceOnPropertyChanged;
        }

        public void InitSocket()
        {
            if (!this.IsSocketConnected && RestApiManagerBase.Instance.UserData != null)
            {
                this.IsSocketConnected = false;
                this._socket = IO.Socket("http://163.5.84.242:3000", new IO.Options()
                {
                    Query = new Dictionary<string, string>() { { "token", RestApiManagerBase.Instance.UserData.Token } },
                    ForceNew = true,
                    Port = 3000,
                    Hostname = "163.5.84.242",
                });
                this.SocketEmitter = new SocketEmitter(this._socket);
                this._socketListener = new SocketListener(this._socket, new ConversationParser());
                this._socketListener.InitSocketListeners();
                this._socket.On(Socket.EVENT_CONNECT, () =>
                {
                    this._socket.Emit("subscribe", JObject.FromObject(new { channel = RestApiManagerBase.Instance.UserData.User.Id }));
                    this.IsSocketConnected = true;
                });
            }
        }

        private async void InstanceOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "IsConnected")
                return;
            if (RestApiManagerBase.Instance.IsConnected && App.InternetAvailabilityWatcher.IsInternetUp)
                await App.Current.Resources.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {

                if (this._socket == null)
                    InitSocket();
                else
                    this._socket.Connect();
            });
            else
                await App.Current.Resources.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, CloseSocket);
        }

        public void CloseSocket()
        {
            if (this._socket == null)
                return;
            this._socket.Close();
            this.IsSocketConnected = false;
        }

        public void ConnectSocket()
        {
            this._socket.Connect();
        }
    }
}
