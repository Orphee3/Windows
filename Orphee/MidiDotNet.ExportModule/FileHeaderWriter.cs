using System.IO;
using System.Text;
using MidiDotNet.ExportModule.Interfaces;
using MidiDotNet.Shared.Interfaces;
using Orphee.CreationShared.Interfaces;

namespace MidiDotNet.ExportModule
{
    /// <summary>
    /// This class writes the MIDI file header
    /// in the MIDI file.
    /// </summary>
    public class FileHeaderWriter : IFileHeaderWriter
    {
        private readonly ISwapManager _swapManager;

        /// <summary>
        /// Constructor of the class initializing the 
        /// swapManager variable through dependency injection.
        /// </summary>
        /// <param name="swapManager">Instance of the SwapManager class used to swap the position of the bytes contained in the value it's given</param>
        public FileHeaderWriter(ISwapManager swapManager)
        {
            this._swapManager = swapManager;
        }

        /// <summary>
        /// Function writting the file header int the 
        /// MIDI file.
        /// </summary>
        /// <param name="writer">Instance of the BinaryWriter class writting the noteMessage in the MIDI file</param>
        /// <param name="orpheeFileParameters">Instance of the OrpheeFileParameter class containing the parameters of the MIDI file</param>
        /// <returns>Returns true if the message has been written and false if it hasn't</returns>
        public bool WriteFileHeader(BinaryWriter writer, IOrpheeFileParameters orpheeFileParameters)
        {
            if (writer == null)
                return false;
            writer.Write(Encoding.UTF8.GetBytes("MThd"));
            writer.Write(this._swapManager.SwapUInt32(6));
            writer.Write(this._swapManager.SwapUInt16(orpheeFileParameters.OrpheeFileType));
            writer.Write(this._swapManager.SwapUInt16(orpheeFileParameters.NumberOfTracks));
            writer.Write(this._swapManager.SwapUInt16(orpheeFileParameters.DeltaTicksPerQuarterNote));
            return true;
        }
    }
}
