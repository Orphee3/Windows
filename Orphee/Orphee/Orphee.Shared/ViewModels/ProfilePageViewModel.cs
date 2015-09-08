using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Orphee.RestApiManagement;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    public class ProfilePageViewModel : ViewModel, IProfilePageViewModel, INavigationAware
    {
        public string UserName { get; private set; }
        public int NumberOfCreations { get; private set; }
        public int NumberOfComments { get; private set; }
        public int NumberOfFollows { get; private set; }
        public int NumberOfFollowers { get; private set; }
        public string UserPictureSource { get; private set; }
        private SolidColorBrush _backgroundColorBrush;
        public SolidColorBrush BackgroundPictureColor
        {
            get { return this._backgroundColorBrush; }
            set
            {
                if (this._backgroundColorBrush != value)
                    SetProperty(ref this._backgroundColorBrush, value);
            }
        }
        private Visibility _disconnectedStackPanelVisibility;
        public Visibility DisconnectedStackPanelVisibility
        {
            get { return this._disconnectedStackPanelVisibility; }
            set
            {
                if (this._disconnectedStackPanelVisibility != value)
                    SetProperty(ref this._disconnectedStackPanelVisibility, value);
            }
        }
        private Visibility _connectedStackPanelVisibility;
        public Visibility ConnectedStackPanelVisibility
        {
            get { return this._connectedStackPanelVisibility; }
            set
            {
                if (this._connectedStackPanelVisibility != value)
                    SetProperty(ref this._connectedStackPanelVisibility, value);
            }
        }
        public DelegateCommand LoginCommand { get; private set; }
        public DelegateCommand LogoutCommand { get; private set; }
        public DelegateCommand FriendPageCommand { get; private set; }

        public ProfilePageViewModel()
        {
            SetPropertiesDependingOnConnectionState();
            this.UserPictureSource = "/Assets/flower3.jpg";
            InitBackgroundPictureColor();
            this.LoginCommand = new DelegateCommand(() => App.MyNavigationService.Navigate("Login", null));
            this.LogoutCommand = new DelegateCommand(LogoutCommandExec);
            this.FriendPageCommand = new DelegateCommand(() => App.MyNavigationService.Navigate("Friend", null));
        }

        public override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            App.MyNavigationService.CurrentPageName = "Profile";
            SetPropertiesDependingOnConnectionState();
        }

        private void SetPropertiesDependingOnConnectionState()
        {
            if (RestApiManagerBase.Instance.IsConnected && RestApiManagerBase.Instance.NotificationRecieiver.IsInternet())
            {
                //this.UserPhoto = RestApiManagerBase.Instance.UserPhoto;
                this.DisconnectedStackPanelVisibility = Visibility.Collapsed;
                this.ConnectedStackPanelVisibility = Visibility.Visible;
                this.UserName = RestApiManagerBase.Instance.UserData.User.UserName;
                this.NumberOfCreations = RestApiManagerBase.Instance.UserData.User.Creations.Count;
                this.NumberOfComments = RestApiManagerBase.Instance.UserData.User.Comments.Count;
                this.NumberOfFollows = 0;
                this.NumberOfFollowers = 0;
            }
            else
            {
                this.DisconnectedStackPanelVisibility = Visibility.Visible;
                this.ConnectedStackPanelVisibility = Visibility.Collapsed;
            }
        }

        private void LogoutCommandExec()
        {
            RestApiManagerBase.Instance.Logout();
            SetPropertiesDependingOnConnectionState();
        }

        private async void InitBackgroundPictureColor()
        {
            this.BackgroundPictureColor = new SolidColorBrush { Color = await SearchDominantPictureColor() };
        }

        private async Task<Color> SearchDominantPictureColor()
        {
            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx://" + this.UserPictureSource));

            using (var stream = await file.OpenAsync(FileAccessMode.Read))
            {
                //Create a decoder for the image
                var decoder = await BitmapDecoder.CreateAsync(stream);

                //Create a transform to get a 1x1 image
                var myTransform = new BitmapTransform { ScaledHeight = decoder.PixelHeight, ScaledWidth = decoder.PixelWidth };

                //Get the pixel provider
                var pixels = await decoder.GetPixelDataAsync(
                    BitmapPixelFormat.Rgba8,
                    BitmapAlphaMode.Straight,
                    myTransform,
                    ExifOrientationMode.RespectExifOrientation,
                    ColorManagementMode.ColorManageToSRgb);

                //Get the bytes of the 1x1 scaled image
                var bytes = pixels.DetachPixelData();

                int alpha = 0;
                int red = 0;
                int blue = 0;
                int green = 0;
                for (var i = 0; i < bytes.Length; i += 4)
                {
                    red += bytes[i];
                    green += bytes[i + 1];
                    blue += bytes[i + 2];
                    alpha += bytes[i + 3];
                }
                var alphaByte = (byte)(alpha / (bytes.Length / 4));
                var bytered = (byte)(red / (bytes.Length / 4));
                var bytegreen = (byte)(green / (bytes.Length / 4));
                var byteblue = (byte)(blue / (bytes.Length / 4));
                return (Color.FromArgb(alphaByte, bytered, bytegreen, byteblue));
            }
        }
    }
}
