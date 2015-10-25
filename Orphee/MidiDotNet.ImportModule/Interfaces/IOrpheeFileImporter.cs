using System.Threading.Tasks;
using Windows.Storage;
using Orphee.CreationShared.Interfaces;

namespace MidiDotNet.ImportModule.Interfaces
{
    /// <summary>
    /// OrpheeFileImporter interface.
    /// </summary>
    public interface IOrpheeFileImporter
    {
        // Properties

        /// <summary>String representing the name of the actual MIDI file</summary>
        string FileName { get; set; }
        /// <summary>File representing the actual MIDI file in the program </summary>
        IOrpheeFile OrpheeFile { get; }
        /// <summary>File representing the actual MIDI file in the device </summary>
        IStorageFile StorageFile { get; set; }
        
        // Methods

        /// <summary>
        /// Function importing the MIDI file and converting
        /// it so it can be used in the program
        /// </summary>
        /// <param name="fileType">Value representing the actual MIDI file type</param>
        /// <returns>Returns the imported MIDI file or null if a problem occured</returns>
        Task<IOrpheeFile> ImportFile(string fileType);

        Task<IOrpheeFile> ImportFileFromNet(string filePath, string fileName);
    }
}
