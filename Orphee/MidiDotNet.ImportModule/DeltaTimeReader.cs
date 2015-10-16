using System.Collections;
using System.IO;
using MidiDotNet.ImportModule.Interfaces;

namespace MidiDotNet.ImportModule
{
    /// <summary>
    /// Class containing all the needed functions in order to
    /// read and to convert the delta time from a byte array
    /// to an int
    /// </summary>
    public class DeltaTimeReader : IDeltaTimeReader
    {
        /// <summary>
        /// Function reading and converting the deltaTime from a byte array
        /// to an int.
        /// </summary>
        /// <param name="reader">Instance of the BinaryReader class read the noteMessage in the MIDI file</param>
        /// <returns>Returns the converted int value of the delta time</returns>
        public int GetIntDeltaTime(BinaryReader reader)
        {
            var byteArray = new byte[4];
            var arrayTrueLength = 0;
            byteArray[0] = reader.ReadByte();

            while ((byteArray[arrayTrueLength] & 0x80) == 0x80)
                byteArray[++arrayTrueLength] = reader.ReadByte();
            var definitiveByteArray = new byte[arrayTrueLength + 1];

            for (var pos = 0; pos <= arrayTrueLength; pos++)
                definitiveByteArray[pos] = byteArray[arrayTrueLength - pos];
            return (RetreiveDeltaTime(definitiveByteArray));
        }

        private int RetreiveDeltaTime(byte[] deltaTime)
        {
            var retreivedDeltaTime = 0;

            if (deltaTime[deltaTime.Length - 1] > 0)
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
