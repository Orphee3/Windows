using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace MidiDotNet.ExportModuleUnitTests.ExportModuleTests
{
    public class ExportModuleTestsBase
    {
        protected BinaryWriter Writer;
        protected BinaryReader Reader;
        protected IStorageFile File;

        public async Task<bool> InitializeFile(string fileName)
        {
            var folder = KnownFolders.MusicLibrary;
            this.File = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            return true;
        }
    }
}
