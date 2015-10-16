﻿using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.RestApiManagement.Senders.Interfaces;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    /// <summary>
    /// ProfileEditionPage view model
    /// </summary>
    public class ProfileEditionPageViewModel : ViewModel, IProfileEditionPageViewModel
    {
        /// <summary>Redirects to the previous page </summary>
        public DelegateCommand GoBackCommand { get; }
        /// <summary>Enables the posibility to change the user's picture </summary>
        public DelegateCommand ChangePictureCommand { get; }
        private readonly IFileUploader _fileUploader;

        /// <summary>
        /// Constructor initializing fileUploader
        /// through dependency injection
        /// </summary>
        /// <param name="fileUploader"></param>
        public ProfileEditionPageViewModel(IFileUploader fileUploader)
        {
            this._fileUploader = fileUploader;
            this.GoBackCommand = new DelegateCommand(() => App.MyNavigationService.GoBack());
            this.ChangePictureCommand = new DelegateCommand(ChangePictureCommandExec);
        }

        private async void ChangePictureCommandExec()
        {
            var result = await OpenFilePicker();
        }

        private async Task<string> OpenFilePicker()
        {
            var openPicker = new FileOpenPicker()
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary,
            };
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".png");

#if WINDOWS_PHONE_APP
            StorageFile pictureFile = null;
            openPicker.PickSingleFileAndContinue();
#else
            var pictureFile = await openPicker.PickSingleFileAsync();
#endif
            if (pictureFile != null)
                if (await this._fileUploader.UploadImage(pictureFile))
                    return pictureFile.Path;
            return "";
        }
    }
}
