using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.Storage;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.FileManagement.Interfaces;
using Orphee.RestApiManagement;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    public class ProfilePageViewModel : ViewModel, IProfilePageViewModel
    {
        public ObservableCollection<IStorageFile> CreationList { get; set; } 
        public DelegateCommand LoginButton { get; }
        public DelegateCommand RegisterButton { get; }
        public string DisconnectedMessage { get; }
        public Visibility ButtonsVisibility { get; private set; }
        public Visibility ListViewVisibility { get; private set; }
        private readonly IOrpheeFilesGetter _orpheeFilesGetter;
        private MediaElement _mediaElement;

        public ProfilePageViewModel(IOrpheeFilesGetter orpheeFilesGetter)
        {
            this._mediaElement = new MediaElement();
            this._orpheeFilesGetter = orpheeFilesGetter;
            this.CreationList = new ObservableCollection<IStorageFile>();
            this.DisconnectedMessage = "To access your profile info you have \nto login or to create an account";
            this.ButtonsVisibility = Visibility.Visible;
            this.ListViewVisibility = Visibility.Collapsed;
            if (RestApiManagerBase.Instance.IsConnected)
            {
                this.ButtonsVisibility = Visibility.Collapsed;
                this.ListViewVisibility = Visibility.Visible;
                InitCreationList();
            }
            this.LoginButton = new DelegateCommand(() => App.MyNavigationService.Navigate("Login", null));
            this.RegisterButton = new DelegateCommand(() => App.MyNavigationService.Navigate("Register", null));
        }

        public override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            if (RestApiManagerBase.Instance.IsConnected && this.ButtonsVisibility == Visibility.Visible)
            {
                this.ButtonsVisibility = Visibility.Collapsed;
                this.ListViewVisibility = Visibility.Visible;
                InitCreationList();
            }
        }

        private async void InitCreationList()
        {
            var result = await this._orpheeFilesGetter.RetrieveOrpheeFiles();
            foreach (var creation in result)
                this.CreationList.Add(creation);
        }

        public async void PlaySelectedFile(string fileName)
        {
            var file = await KnownFolders.MusicLibrary.GetFileAsync(fileName);
            var stream = await file.OpenAsync(FileAccessMode.Read);
            this._mediaElement.SetSource(stream, file.ContentType);
            this._mediaElement.Play();
        }
    }
}
