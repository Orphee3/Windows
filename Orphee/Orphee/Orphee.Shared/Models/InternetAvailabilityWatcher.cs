using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.Networking.Connectivity;
using Orphee.RestApiManagement.Annotations;
using Orphee.RestApiManagement.Models;

namespace Orphee.Models
{
    public class InternetAvailabilityWatcher : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public SocketManager SocketManager { get; private set; }
        private bool _isInternetUp;
        public bool IsInternetUp
        {
            get { return this._isInternetUp; }
            set
            {
                if (this._isInternetUp == value)
                    return;
                this._isInternetUp = value;
                OnPropertyChanged(nameof(this.IsInternetUp));
            }
        }

        public InternetAvailabilityWatcher()
        {
            NetworkInformation.NetworkStatusChanged += OnNetworkStatusChanged;
            this.SocketManager = new SocketManager();
            this.IsInternetUp = IsInternet();
        }

        private void OnNetworkStatusChanged(object sender)
        {
            if (!IsInternet())
            {
                this.SocketManager.CloseSocket();
                this.IsInternetUp = false;
            }
            else
            {
                this.IsInternetUp = true;
                if (!this.SocketManager.IsSocketConnected)
                    this.SocketManager.InitSocket();
                else
                    this.SocketManager.ConnectSocket();
            }
        }

        private bool IsInternet()
        {
            var connections = NetworkInformation.GetInternetConnectionProfile();
            return connections != null && connections.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
