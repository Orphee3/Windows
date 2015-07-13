﻿namespace MidiDotNet.IOModules.Interfaces
{
    public interface IOrpheeFileParameters
    {
        // Properties
        ushort NumberOfTracks { get; set; }
        ushort OrpheeFileType { get; set; }
        ushort DeltaTicksPerQuarterNote { get; set; }
        // Methods
    }
}
