using System.IO;
using MidiDotNet.ExportModule.Interfaces;

namespace MidiDotNet.ExportModule
{
    /// <summary>
    /// This class writes the delta time of every MIDI message
    /// in the MIDI file.
    /// </summary>
    public class DeltaTimeWriter : IDeltaTimeWriter
    {
        /// <summary>
        /// Function writting the delta time of each MIDI message
        /// in the MIDI file.
        /// </summary>
        /// <param name="writer">Instance of the BinaryWriter class writting the noteMessage in the MIDI file</param>
        /// <param name="deltaTime">Delta time value to write</param>
        public void WriteDeltaTime(BinaryWriter writer, int deltaTime)
        {
            var pos = 0;
            var buffer = new byte[4];

            do
            {
                buffer[pos++] = (byte) (deltaTime & 0x7F);
                deltaTime >>= 7;
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
    }
}
