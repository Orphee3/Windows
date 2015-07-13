namespace Orphee.CreationShared.Interfaces
{
    public interface IPlayerParameters
    {
        // Properties
        uint TimeSignatureNominator { get; set; }
        uint TimeSignatureDenominator { get; set; }
        uint TimeSignatureClocksPerBeat { get; set; }
        uint TimeSignatureNumberOf32ThNotePerBeat { get; set; }
        uint Tempo { get; set; }

        // Methods

    }
}
