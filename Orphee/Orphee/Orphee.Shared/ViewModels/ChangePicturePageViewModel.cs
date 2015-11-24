using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    /// <summary>
    /// ProfileEditionPage view model
    /// </summary>
    public class ChangePicturePageViewModel : ViewModel, IChangePicturePageViewModel
    {
        /// <summary>Redirects to the previous page </summary>
        public DelegateCommand GoBackCommand { get; private set; }
        /// <summary>Enables the posibility to change the user's picture </summary>
        public DelegateCommand ChangePictureCommand { get; private set; }

        /// <summary>
        /// Constructor initializing fileUploader
        /// through dependency injection
        /// </summary>
        /// <param name="fileUploader"></param>
        public ChangePicturePageViewModel()
        {
            this.GoBackCommand = new DelegateCommand(() => App.MyNavigationService.GoBack());
            this.ChangePictureCommand = new DelegateCommand(ChangePictureCommandExec);
        }

        private async void ChangePictureCommandExec()
        {
            var result = await OpenFilePicker();
            App.MyNavigationService.Navigate("ProfilePictureEdition", result);
        }

        private async Task<StorageFile> OpenFilePicker()
        {
            var openPicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".bmp");
            openPicker.FileTypeFilter.Add(".png");

#if WINDOWS_PHONE_APP
            StorageFile pictureFile = null;
            openPicker.PickSingleFileAndContinue();
#else
            var pictureFile = await openPicker.PickSingleFileAsync();
#endif
            if (pictureFile != null)
                return pictureFile;
            return null;
        }
    }
}
