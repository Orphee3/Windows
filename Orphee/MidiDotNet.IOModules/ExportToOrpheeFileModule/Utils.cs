using System.Collections;
using System.IO;

namespace MidiDotNet.IOModules.ExportToOrpheeFileModule
{
    public class Utils
    {
        public static Utils Instance
        {
            get
            {
                if (_utils == null)
                    _utils = new Utils();
                return _utils;
            }
        }

        private static Utils _utils;
        private Utils()
        {
         
        }

        public uint SwapUInt32(uint i)
        {
            return ((i & 0xFF000000) >> 24) | ((i & 0x00FF0000) >> 8) | ((i & 0x0000FF00) << 8) |
                   ((i & 0x000000FF) << 24);
        }

        public ushort SwapUInt16(ushort i)
        {
            return (ushort) (((i & 0xFF00) >> 8) | ((i & 0x00FF) << 8));
        }

        public void WriteDeltaTime(int deltaTime, BinaryWriter writer, ref int trackLength)
        {
            var pos = 0;
            var buffer = new byte[4];

            do
            {
                buffer[pos++] = (byte) (deltaTime & 0x7F);
                deltaTime >>= 7;
                trackLength++;
            } while (deltaTime > 0);

            while (pos > 0)
            {
                pos--;
                if (pos > 0)
                    writer.Write((byte) (buffer[pos] | 0x80));
                else
                    writer.Write(buffer[pos]);
            }
        }

        public int RetreiveDeltaTime(byte[] deltaTime)
        {
            var retreivedDeltaTime = 0;

            if (deltaTime[0] > 0)
            {
                var bitArray = new BitArray(deltaTime);
                var removedBit = 0;

                for (var bitBytePos = 0; (bitBytePos < bitArray.Length && bitBytePos < 32); bitBytePos++)
                {
                    if ((bitBytePos + 1)%8 == 0)
                        removedBit++;
                    if (bitArray[bitBytePos] && (bitBytePos + 1)%8 != 0)
                        retreivedDeltaTime |= (1 << bitBytePos - removedBit);
                }
            }
            return retreivedDeltaTime;
        }
    }
}
