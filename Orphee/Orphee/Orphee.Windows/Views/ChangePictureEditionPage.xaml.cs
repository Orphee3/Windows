using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Pickers.Provider;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using CSWindowsStoreAppCropBitmap;
using Orphee.CreationShared;
using Orphee.RestApiManagement.Senders;

namespace Orphee.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    ///  SelectedRegion selectedRegion;

    // The current image file to be cropped.
    public sealed partial class ChangePictureEditionPage : Page
    {
        SelectedRegion selectedRegion;

        // The current image file to be cropped. 
        StorageFile sourceImageFile = null;

        uint sourceImagePixelWidth;
        uint sourceImagePixelHeight;

        /// <summary> 
        /// The size of the corners. 
        /// </summary> 
        double cornerSize;

        double CornerSize
        {
            get
            {
                if (cornerSize <= 0)
                {
                    cornerSize = (double)Application.Current.Resources["Size"];
                }

                return cornerSize;
            }
        }

        /// <summary> 
        /// The previous points of all the pointers. 
        /// </summary> 
        Dictionary<uint, Point?> pointerPositionHistory = new Dictionary<uint, Point?>();

        private FileUploader _fileUploader;
        public ChangePictureEditionPage()
        {
            this.InitializeComponent();
            selectRegion.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;

            selectedRegion = new SelectedRegion { MinSelectRegionSize = 300 };
            this.DataContext = selectedRegion;
        }

        /// <summary> 
        /// Invoked when this page is about to be displayed in a Frame. 
        /// </summary> 
        /// <param name="e">Event data that describes how this page was reached.  The Parameter 
        /// property is typically used to configure the page.</param> 
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            selectedRegion.PropertyChanged += selectedRegion_PropertyChanged;

            this.sourceImage.ImageFailed += this.sourceImage_ImageFailed;


            // Handle the manipulation events of the selectRegion 
            selectRegion.ManipulationDelta += selectRegion_ManipulationDelta;
            selectRegion.ManipulationCompleted += selectRegion_ManipulationCompleted;

            this.sourceImage.SizeChanged += sourceImage_SizeChanged;

            ManagePickedFile(e.Parameter as StorageFile);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            selectedRegion.PropertyChanged -= selectedRegion_PropertyChanged;

            this.sourceImage.ImageFailed -= this.sourceImage_ImageFailed;


            // Handle the manipulation events of the selectRegion 
            selectRegion.ManipulationDelta -= selectRegion_ManipulationDelta;
            selectRegion.ManipulationCompleted -= selectRegion_ManipulationCompleted;

            this.sourceImage.SizeChanged -= sourceImage_SizeChanged;

        }


        #region Open an image, handle the select region changed event and save the image. 

        /// <summary> 
        /// Let user choose an image and load it. 
        /// </summary> 
        async private void openImageButton_Click(object sender, RoutedEventArgs e)
        {
            bool unsnapped = ((ApplicationView.Value != ApplicationViewState.Snapped) || ApplicationView.TryUnsnap());
            if (!unsnapped)
            {
                NotifyUser("Cannot unsnap the sample.");
                return;
            }


            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".bmp");
            openPicker.FileTypeFilter.Add(".png");
            StorageFile imgFile = await openPicker.PickSingleFileAsync();
            ManagePickedFile(imgFile);
        }

        private async void ManagePickedFile(StorageFile imgFile)
        {
            if (imgFile != null)
            {

                this.previewImage.Source = null;
                this.sourceImage.Source = null;
                this.imageCanvas.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                this.originalImageInfoText.Text = string.Empty;
                this.selectInfoInBitmapText.Text = string.Empty;
                this.saveImageButton.IsEnabled = false;

                // Ensure the stream is disposed once the image is loaded 
                using (IRandomAccessStream fileStream = await imgFile.OpenAsync(Windows.Storage.FileAccessMode.Read))
                {
                    this.sourceImageFile = imgFile;
                    BitmapDecoder decoder = await BitmapDecoder.CreateAsync(fileStream);

                    this.sourceImagePixelHeight = decoder.PixelHeight;
                    this.sourceImagePixelWidth = decoder.PixelWidth;
                }

                if (this.sourceImagePixelHeight < 300 || this.sourceImagePixelWidth < 300)
                {
                    this.NotifyUser(string.Format("Please select an image which is larger than {0}*{0}", 300));
                    return;
                }
                else
                {
                    double sourceImageScale = 1;

                    if (this.sourceImagePixelHeight < this.sourceImageGrid.ActualHeight && this.sourceImagePixelWidth < this.sourceImageGrid.ActualWidth)
                    {
                        this.sourceImage.Stretch = Windows.UI.Xaml.Media.Stretch.None;
                    }
                    else
                    {
                        sourceImageScale = Math.Min(this.sourceImageGrid.ActualWidth / this.sourceImagePixelWidth, this.sourceImageGrid.ActualHeight / this.sourceImagePixelHeight);
                        this.sourceImage.Stretch = Windows.UI.Xaml.Media.Stretch.Uniform;
                    }

                    this.sourceImage.Source = await CropBitmap.GetCroppedBitmapAsync(this.sourceImageFile, new Point(0, 0), new Size(this.sourceImagePixelWidth, this.sourceImagePixelHeight), sourceImageScale);
                    this.originalImageInfoText.Text = string.Format("Original Image Size: {0}*{1} ", this.sourceImagePixelWidth, this.sourceImagePixelHeight);
                }
            }
        }

        /// <summary> 
        /// Show the error message if the image is failed. 
        /// </summary> 
        private void sourceImage_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            this.NotifyUser(string.Format("Failed to open the image file: {0}", e.ErrorMessage));
        }

        /// <summary> 
        /// This event will be fired when  
        /// 1. An new image is opened. 
        /// 2. The source of the sourceImage is set to null. 
        /// 3. The view state of this application is changed. 
        /// </summary> 
        /// <param name="sender"></param> 
        /// <param name="e"></param> 
        void sourceImage_SizeChanged(object sender, SizeChangedEventArgs e)
        {

            if (e.NewSize.IsEmpty || double.IsNaN(e.NewSize.Height) || e.NewSize.Height <= 0)
            {
                this.imageCanvas.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                this.saveImageButton.IsEnabled = false;
                this.selectedRegion.OuterRect = Rect.Empty;
                this.selectedRegion.ResetCorner(0, 0, 0, 0);
            }
            else
            {
                this.imageCanvas.Visibility = Windows.UI.Xaml.Visibility.Visible;
                this.saveImageButton.IsEnabled = true;
                var size = e.NewSize.Height > e.NewSize.Width ? e.NewSize.Width : e.NewSize.Height;
                this.imageCanvas.Height = e.NewSize.Height;
                this.imageCanvas.Width = e.NewSize.Width;
                this.selectedRegion.OuterRect = new Rect(0, 0, e.NewSize.Width, e.NewSize.Height);

                if (e.PreviousSize.IsEmpty || double.IsNaN(e.PreviousSize.Height) || e.PreviousSize.Height <= 0)
                {
                    this.selectedRegion.ResetCorner(0, 0, size, size);
                }
                else
                {
                    double scale = e.NewSize.Height / e.PreviousSize.Height;
                    this.selectedRegion.ResizeSelectedRect(scale);
                    UpdatePreviewImage();
                }

            }


        }

        /// <summary> 
        /// Show the select region information. 
        /// </summary> 
        void selectedRegion_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            double widthScale = imageCanvas.Width / this.sourceImagePixelWidth;
            double heightScale = imageCanvas.Height / this.sourceImagePixelHeight;

            this.selectInfoInBitmapText.Text = string.Format("Start Point: ({0},{1})  Size: {2}*{3}",
                Math.Floor(this.selectedRegion.SelectedRect.X / widthScale),
                Math.Floor(this.selectedRegion.SelectedRect.Y / heightScale),
                Math.Floor(this.selectedRegion.SelectedRect.Width / widthScale),
                Math.Floor(this.selectedRegion.SelectedRect.Height / heightScale));
        }

        /// <summary> 
        /// Save the cropped image. 
        /// </summary> 
        async private void saveImageButton_Click(object sender, RoutedEventArgs e)
        {
            bool unsnapped = ((ApplicationView.Value != ApplicationViewState.Snapped) || ApplicationView.TryUnsnap());
            if (!unsnapped)
            {
                NotifyUser("Cannot unsnap the sample.");
                return;
            }

            double widthScale = imageCanvas.Width / this.sourceImagePixelWidth;
            double heightScale = imageCanvas.Height / this.sourceImagePixelHeight;

            FileSavePicker savePicker = new FileSavePicker {SuggestedStartLocation = PickerLocationId.PicturesLibrary};

            savePicker.FileTypeChoices.Add("JPG file", new List<string>() { ".jpg" });
            savePicker.FileTypeChoices.Add("JPEG file", new List<string>() { ".jpeg" });
            savePicker.FileTypeChoices.Add("PNG file", new List<string>() { ".png" });
            savePicker.FileTypeChoices.Add("BMP file", new List<string>() { ".bmp" });


            savePicker.SuggestedFileName = string.Format("{0}_{1}x{2}{3}", sourceImageFile.DisplayName, (int)Math.Floor(this.selectedRegion.SelectedRect.Width / widthScale), (int)Math.Floor(this.selectedRegion.SelectedRect.Height / heightScale), sourceImageFile.FileType);
            StorageFile croppedImageFile = await savePicker.PickSaveFileAsync();

            if (croppedImageFile != null)
            {
                await CropBitmap.SaveCroppedBitmapAsync(
                    sourceImageFile,
                    croppedImageFile,
                    new Point(this.selectedRegion.SelectedRect.X / widthScale, this.selectedRegion.SelectedRect.Y / heightScale),
                    new Size(this.selectedRegion.SelectedRect.Width / widthScale, this.selectedRegion.SelectedRect.Height / heightScale));
                var fileUploader = new FileUploader(new NotificationSender());
                var result = await fileUploader.UploadImage(croppedImageFile);
                if (result)
                    this.NotifyUser("New profile picture saved successfuly");
                else
                    this.NotifyUser("An error has occured");
            }
        }

        #endregion

        #region Select Region methods 

        /// <summary> 
        /// The user manipulates the selectRegion. The manipulation includes 
        /// 1. Translate 
        /// 2. Scale 
        /// </summary> 
        void selectRegion_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            this.selectedRegion.UpdateSelectedRect(e.Delta.Scale, e.Delta.Translation.X, e.Delta.Translation.Y);
            e.Handled = true;
        }

        /// <summary> 
        /// The manipulation is completed, and then update the preview image 
        /// </summary> 
        void selectRegion_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            UpdatePreviewImage();
        }

        /// <summary> 
        /// Update preview image. 
        /// </summary> 
        async void UpdatePreviewImage()
        {
            double sourceImageWidthScale = imageCanvas.Width / this.sourceImagePixelWidth;
            double sourceImageHeightScale = imageCanvas.Height / this.sourceImagePixelHeight;


            Size previewImageSize = new Size(this.selectedRegion.SelectedRect.Width / sourceImageWidthScale, this.selectedRegion.SelectedRect.Height / sourceImageHeightScale);

            double previewImageScale = 1;

            if (previewImageSize.Width <= imageCanvas.Width && previewImageSize.Height <= imageCanvas.Height)
                this.previewImage.Stretch = Windows.UI.Xaml.Media.Stretch.None;
            else
            {
                this.previewImage.Stretch = Windows.UI.Xaml.Media.Stretch.Uniform;

                previewImageScale = Math.Min(imageCanvas.Width / previewImageSize.Width, imageCanvas.Height / previewImageSize.Height);
            }
            this.previewImage.Source = await CropBitmap.GetCroppedBitmapAsync(this.sourceImageFile,new Point(this.selectedRegion.SelectedRect.X / sourceImageWidthScale, this.selectedRegion.SelectedRect.Y / sourceImageHeightScale),previewImageSize,previewImageScale);
        }

        #endregion

        public void BackButton_clicked(object sender, RoutedEventArgs e)
        {
            App.MyNavigationService.GoBack();
        }

        #region Common methods 

        public async void NotifyUser(string message)
        {
            var messageDialog = new MessageDialog(message);

            await messageDialog.ShowAsync();
        }

        #endregion
    }
}
