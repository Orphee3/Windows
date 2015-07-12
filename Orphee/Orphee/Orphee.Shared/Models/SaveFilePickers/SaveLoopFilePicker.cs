using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Orphee.Models.Interfaces;

namespace Orphee.Models.SaveFilePickers
{
    public class SaveLoopFilePicker
    {
        public async Task<bool> SaveLoop(IOrpheeTrack orpheeTrack)
        {
            FileSavePicker savePicker = new FileSavePicker();
            savePicker.SuggestedStartLocation = PickerLocationId.MusicLibrary;
            savePicker.FileTypeChoices.Add("Plain Text", new List<string>() { ".orph" });
            savePicker.SuggestedFileName = "New Loop";
            StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                CachedFileManager.DeferUpdates(file);
                // Call Save Function of MidiExport
                await FileIO.WriteTextAsync(file, file.Name);
                return true;
            }
            return false;
        }
    }
}
