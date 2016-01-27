using System.Threading.Tasks;
using MidiDotNet.ExportModule.Interfaces;
using Orphee.CreationShared.Interfaces;

namespace Orphee.Models.Interfaces
{
    public interface IOrpheeFileManager
    {
        Task<string> ExportOrpheeFile(IOrpheeFile fileToSave);
        Task<IOrpheeFile> ImportOrpheeFile();
        void InitOrpheeFileWithImportedOrpheeFile(IOrpheeFile importedFile, IOrpheeFile fileToInitialize);
        IOrpheeFileExporter OrpheeFileExporter { get; }
    }
}
