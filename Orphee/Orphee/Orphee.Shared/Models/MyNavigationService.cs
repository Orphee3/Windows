using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Orphee.Models.Interfaces;
using Orphee.RestApiManagement.Models;
using Orphee.RestApiManagement.Models.Interfaces;

namespace Orphee.Models
{
    /// <summary>
    /// Manages the navigation between pages
    /// </summary>
    public class MyNavigationService : IMyNavigationService
    {
        private readonly INavigationService _navigationService;
        /// <summary>Name of the curent page </summary>
        public string CurrentPageName { get; set; }
        /// <summary>Foreground color of the BottomAppBar buttons </summary>
        public Dictionary<string, SolidColorBrush> ButtonForegroundColorList { get; private set; }

        public MyNavigationService(INavigationService navigationService)
        {
            this._navigationService = navigationService;
            this.ButtonForegroundColorList = new Dictionary<string, SolidColorBrush>()
            {
                { "Home", new SolidColorBrush(Color.FromArgb(100, 13, 71, 161))},
                { "Social", new SolidColorBrush(Colors.White)},
                { "Conversation", new SolidColorBrush(Colors.White)},
                { "Profile", new SolidColorBrush(Colors.White)},
            };
            RestApiManagerBase.Instance.RetreiveUser();
        }

        public void GoBack()
        {
            this._navigationService.GoBack();
        }

        public bool CanGoBack()
        {
            return this._navigationService.CanGoBack();
        }

        public void ClearHistory()
        {
        }

        public void RestoreSavedNavigation()
        {
        }

        public bool Navigate(string destination, object parameter)
        {
            this.CurrentPageName = destination;
            return this._navigationService.Navigate(destination, parameter);
        }

        public void Suspending()
        {
            if (App.InternetAvailabilityWatcher.SocketManager.IsSocketConnected)
                App.InternetAvailabilityWatcher.SocketManager.CloseSocket();
            if (RestApiManagerBase.Instance.IsConnected)
                RestApiManagerBase.Instance.SaveUser();
        }


        /// <summary>
        /// Sets the foreground color of the BottomAppBar buttons
        /// </summary>
        public void SetNewAppBarButtonColorValue()
        {
            foreach (var color in this.ButtonForegroundColorList)
                color.Value.Color = (this.CurrentPageName == color.Key) ? Color.FromArgb(100, 13, 71, 161) : Colors.White;
        }
    }
}
