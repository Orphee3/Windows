using Orphee.CreationShared.Interfaces;

namespace Orphee.CreationShared
{
    public class OrpheeFileParameters : IOrpheeFileParameters
    {
        public ushort OrpheeFileType { get; set; }
        public ushort NumberOfTracks { get; set; }
        public ushort DeltaTicksPerQuarterNote { get; set; }

        public OrpheeFileParameters()
        {
            this.NumberOfTracks = 1;
            this.OrpheeFileType = 1;
            this.DeltaTicksPerQuarterNote = 60;
        }
    }
}
