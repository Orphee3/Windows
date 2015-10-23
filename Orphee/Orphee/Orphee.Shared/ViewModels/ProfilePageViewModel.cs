using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.RestApiManagement.Models;
using Orphee.RestApiManagement.Senders;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    /// <summary>
    /// ProfilePage view model
    /// </summary>
    public class ProfilePageViewModel : ViewModel, IProfilePageViewModel
    {
        /// <summary>Logged user name</summary>
        public string UserName { get; private set; }
        /// <summary>Number of creation of the logged user </summary>
        public int NumberOfCreations { get; private set; }
        /// <summary>Number of comments of the logged user </summary>
        public int NumberOfComments { get; private set; }
        /// <summary>Number of follows of the logged user </summary>
        public int NumberOfFollows { get; private set; }
        /// <summary>Number of followers of the logged user</summary>
        public int NumberOfFollowers { get; private set; }
        private string _userProfilePicture;
        /// <summary>User picture source</summary>
        public string UserPictureSource
        {
            get { return this._userProfilePicture; }
            set
            {
                if (this._userProfilePicture != value)
                    SetProperty(ref this._userProfilePicture, value);
            }
        }
        private SolidColorBrush _backgroundColorBrush;
        /// <summary>Background color of the profile page </summary>
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
        /// <summary>Visible if the user is disconnected. Hidden otherwise </summary>
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
        /// <summary>Visible if the user is connected. Hidden otherwise </summary>
        public Visibility ConnectedStackPanelVisibility
        {
            get { return this._connectedStackPanelVisibility; }
            set
            {
                if (this._connectedStackPanelVisibility != value)
                    SetProperty(ref this._connectedStackPanelVisibility, value);
            }
        }
        /// <summary>Redirects to MyCreationPage</summary>
        public DelegateCommand MyCreationsCommand { get; private set; }
        /// <summary>Logs the user out</summary>
        public DelegateCommand LogoutCommand { get; private set; }
        /// <summary>Redirects to LoginPage</summary>
        public DelegateCommand FriendPageCommand { get; private set; }
        /// <summary>Redirects to FriendPage</summary>
        public DelegateCommand LoginCommand { get; private set; }
        /// <summary>Redirects to EditProfilePage</summary>
        public DelegateCommand EditProfileCommand { get; private set; }
        /// <summary>
        /// Constructor
        /// </summary>
        public ProfilePageViewModel()
        {
            this.LoginCommand = new DelegateCommand(() => App.MyNavigationService.Navigate("Login", null));
            this.MyCreationsCommand = new DelegateCommand(() => App.MyNavigationService.Navigate("MyCreations", null));
            this.LogoutCommand = new DelegateCommand(LogoutCommandExec);
            this.FriendPageCommand = new DelegateCommand(() => App.MyNavigationService.Navigate("Friend", null));
            this.EditProfileCommand = new DelegateCommand(() => App.MyNavigationService.Navigate("ProfileEdition", null));
        }

        /// <summary>
        /// Called when navigated to this page
        /// </summary>
        /// <param name="navigationParameter"></param>
        /// <param name="navigationMode"></param>
        /// <param name="viewModelState"></param>
        public override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            App.MyNavigationService.CurrentPageName = "Profile";
            if (!RestApiManagerBase.Instance.NotificationRecieiver.IsInternet())
                DisplayErrorMessage("Connexion unavailable");
            SetPropertiesDependingOnConnectionState();
        }

        private void SetPropertiesDependingOnConnectionState()
        {
            if (RestApiManagerBase.Instance.IsConnected)
            { 
                this.UserPictureSource = RestApiManagerBase.Instance.UserData.User.Picture ?? "/Assets/defaultUser.png";
                if (RestApiManagerBase.Instance.NotificationRecieiver.IsInternet())
                    InitBackgroundPictureColor();
                this.DisconnectedStackPanelVisibility = Visibility.Collapsed;
                this.ConnectedStackPanelVisibility = Visibility.Visible;
                this.UserName = RestApiManagerBase.Instance.UserData.User.UserName;
               // this.NumberOfCreations = RestApiManagerBase.Instance.UserData.User.Creations.Count;
               // this.NumberOfComments = RestApiManagerBase.Instance.UserData.User.Comments.Count;
                this.NumberOfFollows = 0;
                this.NumberOfFollowers = 0;
            }
            else
            {
                this.DisconnectedStackPanelVisibility = Visibility.Visible;
                this.ConnectedStackPanelVisibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Updates the picture source
        /// </summary>
        public void UpdatePictureSource()
        {
            this.UserPictureSource = RestApiManagerBase.Instance.UserData.User.Picture;
            InitBackgroundPictureColor();
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
            IRandomAccessStream stream;
            try
            {
                if (RestApiManagerBase.Instance.UserData.User.Picture != null)
                {
                    var streamReference = RandomAccessStreamReference.CreateFromUri(new Uri(this.UserPictureSource)).OpenReadAsync();
                    stream = await streamReference;
                }
                else
                {
                    var storageFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx://" + this.UserPictureSource));
                    stream = await storageFile.OpenAsync(FileAccessMode.Read);
                }
            }
            catch
            {
                return (Color.FromArgb(0xFF, 0x78, 0xC7, 0xF9));
            }
            //Create a decoder for the image
            var decoder = await BitmapDecoder.CreateAsync(stream);
            //Create a transform to get a 1x1 image
            var myTransform = new BitmapTransform {ScaledHeight = 10, ScaledWidth = 10};
            //Get the pixel provider
            var pixels = await decoder.GetPixelDataAsync(BitmapPixelFormat.Rgba8, BitmapAlphaMode.Straight, myTransform, ExifOrientationMode.RespectExifOrientation, ColorManagementMode.ColorManageToSRgb);
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
            var alphaByte = (byte) (alpha/(bytes.Length/4));
            var bytered = (byte) (red/(bytes.Length/4));
            var bytegreen = (byte) (green/(bytes.Length/4));
            var byteblue = (byte) (blue/(bytes.Length/4));
            return (Color.FromArgb(alphaByte, bytered, bytegreen, byteblue));
        }

        private async void DisplayErrorMessage(string message)
        {
            var messageDialog = new MessageDialog(message);

            await messageDialog.ShowAsync();
        }
    }
}
