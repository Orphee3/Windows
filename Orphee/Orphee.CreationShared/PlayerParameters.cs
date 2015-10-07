using Midi;
using Orphee.CreationShared.Interfaces;

namespace Orphee.CreationShared
{
    public class PlayerParameters : IPlayerParameters
    {
        public uint TimeSignatureNominator { get; set; }
        public uint TimeSignatureDenominator { get; set; }
        public uint TimeSignatureClocksPerBeat { get; set; }
        public uint TimeSignatureNumberOf32ThNotePerBeat { get; set; }
        public uint Tempo { get; set; }
    }
}
