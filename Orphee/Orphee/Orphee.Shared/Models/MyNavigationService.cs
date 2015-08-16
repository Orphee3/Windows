using System;
using Microsoft.Practices.Prism.Mvvm.Interfaces;

namespace Orphee.Models
{
    public class MyNavigationService : INavigationService
    {
        private readonly INavigationService _navigationService;

        public MyNavigationService(INavigationService navigationService)
        {
            this._navigationService = navigationService;
        }
        public bool CanGoBack()
        {
            return this._navigationService.CanGoBack();
        }

        public void ClearHistory()
        {
            throw new NotImplementedException();
        }

        public void GoBack()
        {
            this._navigationService.GoBack();
        }

        public bool Navigate(string pageToken, object parameter)
        {
            return this._navigationService.Navigate(pageToken, parameter);
        }

        public void RestoreSavedNavigation()
        {
            throw new NotImplementedException();
        }

        public void Suspending()
        {
            throw new NotImplementedException();
        }
    }
}
