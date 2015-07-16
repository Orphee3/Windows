using System.IO;
using MidiDotNet.ImportModule.Interfaces;

namespace MidiDotNet.ImportModule
{
    public class FileHeaderReader : IFileHeaderReader
    {
        private readonly string _expectedHeader;
        private readonly byte _expectedHeaderLength;
        private readonly byte _expectedFileType;

        public bool ReadFileHeader(BinaryReader reader)
        {
            return false;
        }
    }
}
