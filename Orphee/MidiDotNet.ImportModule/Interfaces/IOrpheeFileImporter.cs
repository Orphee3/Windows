using System.Threading.Tasks;
using Windows.Storage;
using Orphee.CreationShared.Interfaces;

namespace MidiDotNet.ImportModule.Interfaces
{
    public interface IOrpheeFileImporter
    {
        // Properties
        string FileName { get; set; }
        IOrpheeFile OrpheeFile { get; }
        IStorageFile StorageFile { get; set; }
        
        // Methods
        Task<IOrpheeFile> ImportFile(string fileType);
    }
}
