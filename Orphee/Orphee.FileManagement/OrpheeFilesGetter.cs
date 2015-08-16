using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Orphee.FileManagement.Interfaces;

namespace Orphee.FileManagement
{
    public class OrpheeFilesGetter : IOrpheeFilesGetter
    {
        public async Task<List<IStorageFile>> RetrieveOrpheeFiles()
        {
            var folder = KnownFolders.MusicLibrary;
            var files = await folder.GetFilesAsync();

            return GetRidOfNonOrpheeFiles(files);
        }

        private List<IStorageFile> GetRidOfNonOrpheeFiles(IReadOnlyList<IStorageFile> musicFiles)
        {
            return musicFiles.Where(mf => mf.FileType == ".mid").ToList();
        } 
    }
}
