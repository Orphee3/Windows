using MidiDotNet.Shared.Interfaces;

namespace MidiDotNet.Shared
{
    /// <summary>
    /// Class containing all the needed functions in order to
    /// swap the bytes contained in the given values
    /// </summary>
    public class SwapManager : ISwapManager
    {
        /// <summary>
        /// Function swaping all the bytes of the given
        /// uint.
        /// </summary>
        /// <param name="i">Value to swap</param>
        /// <returns>Returns the swaped value</returns>
        public uint SwapUInt32(uint i)
        {
            return ((i & 0xFF000000) >> 24) | ((i & 0x00FF0000) >> 8) | ((i & 0x0000FF00) << 8) | ((i & 0x000000FF) << 24);
        }

        /// <summary>
        /// Function swaping all the bytes of the given
        /// ushort.
        /// </summary>
        /// <param name="i">Value to swap</param>
        /// <returns>Returns the swaped value</returns>
        public ushort SwapUInt16(ushort i)
        {
            return (ushort)(((i & 0xFF00) >> 8) | ((i & 0x00FF) << 8));
        }
    }
}
