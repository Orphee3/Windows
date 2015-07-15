using System.IO;
using MidiDotNet.ExportModule.Interfaces;

namespace MidiDotNet.ExportModule
{
    public class DeltaTimeWriter : IDeltaTimeWriter
    {
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
