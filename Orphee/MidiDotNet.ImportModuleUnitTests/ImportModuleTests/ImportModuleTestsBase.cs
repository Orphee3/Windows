using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace MidiDotNet.ImportModuleUnitTests.ImportModuleTests
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

        public async Task<bool> GetFile(string fileName)
        {
            var folder = KnownFolders.MusicLibrary;
            this.File = await folder.GetFileAsync(fileName);
            return true;
        }

        public void ReWriteTheFile(byte[] byteArray)
        {
            using (this.Writer = new BinaryWriter(this.File.OpenStreamForWriteAsync().Result))
            {
                this.Writer.BaseStream.Position = 0;
                foreach (var byteToWrite in byteArray)
                    this.Writer.Write(byteToWrite);   
            }
        }
    }
}
