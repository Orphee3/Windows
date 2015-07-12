using System;
using System.Threading.Tasks;
using Windows.Storage.Pickers;

namespace Orphee.Models.LoadFilePickers
{
    public class LoadLoopFilePicker
    {
        public async Task<bool> LoadLoop()
        {
            var openPicker = new FileOpenPicker()
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.MusicLibrary,
            };
            openPicker.FileTypeFilter.Add(".orph");
            var file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                // Application now has read/write access to the picked file
                return true;
            }
            return false;
        }
    }
}
