using MidiDotNet.IOModules.Interfaces;

namespace MidiDotNet.IOModules
{
    public class OrpheeFileParameters : IOrpheeFileParameters
    {
        public ushort OrpheeFileType { get; set; }
        public ushort NumberOfTracks { get; set; }
        public ushort DeltaTicksPerQuarterNote { get; set; }
    }
}
