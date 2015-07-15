using MidiDotNet.ExportModule;
using MidiDotNet.ExportModule.Interfaces;

namespace Orphee.UnitTests.ExportModuleTests.DeltaTimeWriterTests
{
    public class WhenDeltaTimeWriterIsCalledWithAOneByteLongDeltaTime : ExportModuleTestsBase
    {
        protected IDeltaTimeWriter DeltaTimeWriter;

        public WhenDeltaTimeWriterIsCalledWithAOneByteLongDeltaTime()
        {
            this.DeltaTimeWriter = new DeltaTimeWriter();
        }
    }
}
