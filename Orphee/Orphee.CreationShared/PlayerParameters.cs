using System.Runtime.Serialization;
using Orphee.CreationShared.Interfaces;

namespace Orphee.CreationShared
{
    /// <summary>
    /// Class representing the PlayerParameter model
    /// containing the data needed to set the MIDI lib parameters
    /// in order to play the MIDI messages accordingly
    /// </summary>
    [DataContract]
    public class PlayerParameters : IPlayerParameters
    {
        /// <summary>Value representing the TimeSignatureMessage nominator </summary>
        public uint TimeSignatureNominator { get; set; }
        /// <summary>Value representing the TimeSignatureMessage denominator </summary>
        public uint TimeSignatureDenominator { get; set; }
        /// <summary>Value representing the TimeSignatureMessage clock per beat </summary>
        public uint TimeSignatureClocksPerBeat { get; set; }
        /// <summary>Value representing the TimeSignatureMessage number of 32th note per beat </summary>
        public uint TimeSignatureNumberOf32ThNotePerBeat { get; set; }
        /// <summary>Value representing the tempo </summary>
        [DataMember]
        public uint Tempo { get; set; }
    }
}
