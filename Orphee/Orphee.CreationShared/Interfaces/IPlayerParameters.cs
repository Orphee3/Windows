namespace Orphee.CreationShared.Interfaces
{
    /// <summary>
    /// PlayerParameters interface
    /// </summary>
    public interface IPlayerParameters
    {
        // Properties

        /// <summary>Value representing the TimeSignatureMessage nominator </summary>
        uint TimeSignatureNominator { get; set; }
        /// <summary>Value representing the TimeSignatureMessage denominator </summary>
        uint TimeSignatureDenominator { get; set; }
        /// <summary>Value representing the TimeSignatureMessage clock per beat </summary>
        uint TimeSignatureClocksPerBeat { get; set; }
        /// <summary>Value representing the TimeSignatureMessage number of 32th note per beat </summary>
        uint TimeSignatureNumberOf32ThNotePerBeat { get; set; }
        /// <summary>Value representing the tempo </summary>
        uint Tempo { get; set; }

        // Methods

    }
}
