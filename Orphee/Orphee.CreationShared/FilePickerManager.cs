using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Orphee.CreationShared.Interfaces;

namespace Orphee.CreationShared
{
    public class FilePickerManager : IFilePickerManager
    {
        public async Task<StorageFile> GetTheSaveFilePicker(IOrpheeFile orpheeFile)
        {
            var savePicker = new FileSavePicker()
            {
                SuggestedStartLocation = PickerLocationId.MusicLibrary,
                SuggestedFileName = orpheeFile.FileName,
            };
            savePicker.FileTypeChoices.Add("Plain Text", new List<string>() { ".mid" });
            return await savePicker.PickSaveFileAsync();
        }
    }
}
