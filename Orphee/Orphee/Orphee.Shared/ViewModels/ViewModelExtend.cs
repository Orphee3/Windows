using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.Models.Interfaces;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    public class ViewModelExtend : ViewModel, ILoadingScreenComponents, IInternetConnexionAware
    {
        protected bool _isProgressRingActive;
        public bool IsProgressRingActive
        {
            get { return this._isProgressRingActive; }
            set
            {
                if (this._isProgressRingActive != value)
                    SetProperty(ref this._isProgressRingActive, value);
            }
        }
        protected Visibility _progressRingVisibility;
        public Visibility ProgressRingVisibility
        {
            get { return this._progressRingVisibility; }
            set
            {
                if (this._progressRingVisibility != value)
                    SetProperty(ref this._progressRingVisibility, value);
            }
        }

        protected Visibility _connexionUnavailableTextBlockVisibility;
        public Visibility ConnexionUnavailableTextBlockVisibility
        {
            get { return this._connexionUnavailableTextBlockVisibility; }
            set
            {
                if (this._connexionUnavailableTextBlockVisibility != value)
                    SetProperty(ref this._connexionUnavailableTextBlockVisibility, value);
            }
        }
        private readonly CoreDispatcher _dispatcher;
        protected IOnUserLoginNewsGetter _onUserLoginNewsGetter;

        public ViewModelExtend()
        {
            App.InternetAvailabilityWatcher.PropertyChanged += InternetAvailabilityWatcherOnPropertyChanged;
            if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                this._dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
            this.ConnexionUnavailableTextBlockVisibility = App.InternetAvailabilityWatcher.IsInternetUp ? Visibility.Collapsed : Visibility.Visible;
        }

        public override void OnNavigatedFrom(Dictionary<string, object> viewModelState, bool suspending)
        {
            App.InternetAvailabilityWatcher.PropertyChanged -= InternetAvailabilityWatcherOnPropertyChanged;
        }

        private async void InternetAvailabilityWatcherOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsInternetUp")
            {
                if (App.InternetAvailabilityWatcher.IsInternetUp)
                    await this._dispatcher.RunAsync(CoreDispatcherPriority.Normal, OnConnexionRetreived);
                else
                    await this._dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => this.ConnexionUnavailableTextBlockVisibility = Visibility.Visible);
            }
        }

        protected async void DisplayMessage(string message)
        {
            var messageDialog = new MessageDialog(message);

            await messageDialog.ShowAsync();
        }

        protected void SetProgressRingVisibility(bool isVisible)
        {
            this.ProgressRingVisibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
            this.IsProgressRingActive = isVisible;
        }

        public void OnConnexionRetreived()
        {
            this.ConnexionUnavailableTextBlockVisibility = Visibility.Collapsed;
        }

        protected bool VerifyReturnedValue<T>(T value, string message)
        {
            if (value != null)
                return true;
            if (message != "")
                DisplayMessage(message);
            if (this._isProgressRingActive)
                SetProgressRingVisibility(false);
            return false;
        }
    }
}
