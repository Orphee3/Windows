namespace MidiDotNet.Shared.Interfaces
{
    /// <summary>
    /// SwapManager interface
    /// </summary>
    public interface ISwapManager
    {
        // Properties

        // Methods

        /// <summary>
        /// Function swaping all the bytes of the given
        /// uint.
        /// </summary>
        /// <param name="i">Value to swap</param>
        /// <returns>Returns the swaped value</returns>
        uint SwapUInt32(uint i);
        /// <summary>
        /// Function swaping all the bytes of the given
        /// ushort.
        /// </summary>
        /// <param name="i">Value to swap</param>
        /// <returns>Returns the swaped value</returns>
        ushort SwapUInt16(ushort i);
    }
}
