using System;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Orphee.Models.Interfaces;
using Orphee.RestApiManagement;
using Orphee.UI;

namespace Orphee.Models
{
    public class MyNavigationService : IMyNavigationService
    {
        private readonly INavigationService _navigationService;
        public string CurrentPageName { get; set; }
        public Dictionary<string, SolidColorBrush> ButtonForegroundColorList { get; }
        public MyBottomAppBar MyBottomAppBar { get; private set; }
        public MyNavigationService(INavigationService navigationService)
        {
            this._navigationService = navigationService;
            this.ButtonForegroundColorList = new Dictionary<string, SolidColorBrush>()
            {
                { "Home", new SolidColorBrush(Color.FromArgb(100, 13, 71, 161))},
                { "Friends", new SolidColorBrush(Colors.White)},
                { "Messages", new SolidColorBrush(Colors.White)},
                { "Profile", new SolidColorBrush(Colors.White)},
            };
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
            throw new NotImplementedException();
        }

        public bool Navigate(string destination, object parameter)
        {
            this.CurrentPageName = destination;
            return this._navigationService.Navigate(destination, parameter);
        }

        public void RestoreSavedNavigation()
        {
            throw new NotImplementedException();
        }

        public void Suspending()
        {
           if (RestApiManagerBase.Instance.NotificationRecieiver.IsSocketConnected)
                RestApiManagerBase.Instance.NotificationRecieiver.CloseSocket();
        }

        public void NotifyUser()
        {
            //this.RootFrame
        }

        public void SetNewAppBarButtonColorValue()
        {
            foreach (var color in this.ButtonForegroundColorList)
                color.Value.Color = (this.CurrentPageName == color.Key) ? Color.FromArgb(100, 13, 71, 161) : Colors.White;
        }
    }
}
