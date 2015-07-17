using MidiDotNet.ImportModule;
using MidiDotNet.ImportModule.Interfaces;
using MidiDotNet.Shared;
using NUnit.Framework;
using Orphee.CreationShared.Interfaces;

namespace MidiDotNet.ImportModuleUnitTests.ImportModuleTests.OrpheeFileImporterTests
{
    public class WhenOrpheeFileImporterIsCalled : ImportModuleTestsBase
    {
        protected IOrpheeFileImporter OrpheeFileImporter;

        protected WhenOrpheeFileImporterIsCalled()
        {
            this.OrpheeFileImporter = new OrpheeFileImporter(new FileHeaderReader(new SwapManager()), new TrackHeaderReader(new SwapManager(), new TimeSignatureMessageReader(), new TempoMessageReader(), new ProgramChangeMessageReader()), new NoteMessageReader(new DeltaTimeReader(), new EndOfTrackMessageReader()));
            var result = GetFile("loop1.loop").Result;
            this.OrpheeFileImporter.StorageFile = this.File;
        }
    }
}
