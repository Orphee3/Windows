using System.Threading.Tasks;
using MidiDotNet.ExportModule.Interfaces;
using MidiDotNet.ImportModule.Interfaces;
using Orphee.CreationShared;
using Orphee.CreationShared.Interfaces;
using Orphee.Models.Interfaces;

namespace Orphee.Models
{
    public class OrpheeFileManager : IOrpheeFileManager
    {
        public IOrpheeFileExporter OrpheeFileExporter { get; private set; }
        private readonly IOrpheeFileImporter _orpheeFileImporter;

        public OrpheeFileManager(IOrpheeFileExporter orpheeFileExporter, IOrpheeFileImporter orpheeFileImporter)
        {
            this.OrpheeFileExporter = orpheeFileExporter;
            this._orpheeFileImporter = orpheeFileImporter;
        }

        public async Task<IOrpheeFile> ImportOrpheeFile()
        {
            return await this._orpheeFileImporter.ImportFile();
        }

        public async Task<string> ExportOrpheeFile(IOrpheeFile fileToSave)
        {
            return await this.OrpheeFileExporter.SaveOrpheeFile(fileToSave);
        }

        public void InitOrpheeFileWithImportedOrpheeFile(IOrpheeFile importedOrpheeFile, IOrpheeFile fileToInitialize)
        {
            fileToInitialize.OrpheeTrackList.Clear();
            fileToInitialize.OrpheeFileParameters = importedOrpheeFile.OrpheeFileParameters;
            foreach (var track in importedOrpheeFile.OrpheeTrackList)
            {
                var newTrack = new OrpheeTrack(new OrpheeTrackUI(new ColorManager()), new NoteMapManager());
                newTrack.Init(track);
                fileToInitialize.AddNewTrack(newTrack);
            }
            fileToInitialize.FileName = importedOrpheeFile.FileName;
        }
    }
}
