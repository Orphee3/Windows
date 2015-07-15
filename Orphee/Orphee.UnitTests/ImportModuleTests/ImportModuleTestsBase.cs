using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace Orphee.UnitTests.ImportModuleTests
{
    public class ImportModuleTestsBase
    {
        protected BinaryReader Reader;
        protected BinaryWriter Writer;
        protected IStorageFile File;

        public async Task<bool> InitializeFile(string fileName)
        {
            var folder = KnownFolders.MusicLibrary;
            this.File = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            return true;
        }
    }
}
