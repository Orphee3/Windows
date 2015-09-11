﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.RestApiManagement;
using Orphee.RestApiManagement.Interfaces;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    public class ProfileEditionPageViewModel : ViewModel, IProfileEditionPageViewModel
    {
        public DelegateCommand GoBackCommand { get; }
        public DelegateCommand ChangePictureCommand { get; }
        private readonly IFileUploader _fileUploader;

        public ProfileEditionPageViewModel(IFileUploader fileUploader)
        {
            this._fileUploader = fileUploader;
            this.GoBackCommand = new DelegateCommand(() => App.MyNavigationService.GoBack());
            this.ChangePictureCommand = new DelegateCommand(ChangePictureCommandExec);
        }

        private async void ChangePictureCommandExec()
        {
            string picturePath;
            if ((picturePath = await OpenFilePicker()) != "")
                RestApiManagerBase.Instance.UserData.User.PicturePath = picturePath;
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
            var pictureFile = await openPicker.PickSingleFileAsync();
            if (pictureFile != null)
                if (await this._fileUploader.UploadImage(pictureFile))
                    return pictureFile.Path;
            return "";
        }
    }
}