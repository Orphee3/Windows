using System.IO;
using Orphee.CreationShared.Interfaces;

namespace MidiDotNet.ExportModule.Interfaces
{
    /// <summary>
    ///  FileHeaderWriter interface.
    /// </summary>
    public interface IFileHeaderWriter
    {
        // Properties

        // Methods
        /// <summary>
        /// Function writting the file header int the 
        /// MIDI file.
        /// </summary>
        /// <param name="writer">Instance of the BinaryWriter class writting the noteMessage in the MIDI file</param>
        /// <param name="orpheeFileParameters">Instance of the OrpheeFileParameter class containing the parameters of the MIDI file</param>
        /// <returns>Returns true if the message has been written and false if it hasn't</returns>
        bool WriteFileHeader(BinaryWriter writer, IOrpheeFileParameters orpheeFileParameters);
    }
}
