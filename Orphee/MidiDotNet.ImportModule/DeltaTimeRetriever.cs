using System.Collections;
using MidiDotNet.ImportModule.Interfaces;

namespace MidiDotNet.ImportModule
{
    public class DeltaTimeRetriever : IDeltaTimeRetriever
    {
        public int RetreiveDeltaTime(byte[] deltaTime)
        {
            var retreivedDeltaTime = 0;

            if (deltaTime[0] > 0)
            {
                var bitArray = new BitArray(deltaTime);
                var removedBit = 0;

                for (var bitBytePos = 0; (bitBytePos < bitArray.Length && bitBytePos < 32); bitBytePos++)
                {
                    if ((bitBytePos + 1) % 8 == 0)
                        removedBit++;
                    if (bitArray[bitBytePos] && (bitBytePos + 1) % 8 != 0)
                        retreivedDeltaTime |= (1 << bitBytePos - removedBit);
                }
            }
            return retreivedDeltaTime;
        }
    }
}
