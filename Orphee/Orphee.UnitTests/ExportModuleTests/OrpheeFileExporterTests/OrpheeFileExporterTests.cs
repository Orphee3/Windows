using System;
using System.Threading.Tasks;
using Windows.Storage;
using Midi;
using MidiDotNet.ExportModule;
using MidiDotNet.ExportModule.Interfaces;
using MidiDotNet.Shared;
using NUnit.Framework;
using Orphee.CreationShared;
using Orphee.CreationShared.Interfaces;
using Orphee.RestApiManagement;

namespace MidiDotNet.ExportModuleUnitTests.ExportModuleTests.OrpheeFileExporterTests
{
    public class WhenFileExporterIsCalled
    {
        protected IOrpheeFileExporter OrpheeFileExporter;
        protected IOrpheeTrack OrpheeTrack;
        protected IOrpheeFile OrpheeFile;
        protected IPlayerParameters PlayerParameters;

        public WhenFileExporterIsCalled()
        {
            this.PlayerParameters = new PlayerParameters()
            {
                TimeSignatureNominator = 4,
                TimeSignatureDenominator = 4,
                TimeSignatureClocksPerBeat = 24,
                Tempo = 120,
                TimeSignatureNumberOf32ThNotePerBeat = 4
            };
            this.OrpheeTrack = new OrpheeTrack(0, Channel.Channel1)
            {
                PlayerParameters = this.PlayerParameters,
                CurrentInstrument = Instrument.OrchestralHarp
            };
            this.OrpheeFile = new OrpheeFile();
            this.OrpheeFile.AddNewTrack(this.OrpheeTrack);
            this.OrpheeFileExporter = new OrpheeFileExporter(new FileHeaderWriter(new SwapManager()), new TrackHeaderWriter(new TimeSignatureMessageWriter(), new TempoMessageWriter(), new SwapManager()), new NoteMessageWriter(new ProgramChangeMessageWriter(), new EndOfTrackMessageWriter()), new FileUploader());
        }
    }

    [TestFixture]
    public class ItShouldSaveATrackProperly : WhenFileExporterIsCalled
    {
        private StorageFile _createdFile;

        [SetUp]
        public void Init()
        {
            this.OrpheeFileExporter.SaveOrpheeTrack(this.OrpheeTrack);
            var result = RetrieveCreatedFile().Result;
        }

        private async Task<bool> RetrieveCreatedFile()
        {
            var folder = KnownFolders.MusicLibrary;
            this._createdFile = await folder.GetFileAsync("loop1.loop");
            return true;
        }

        [Test]
        public void TheCreatedFileShouldNotBeNull()
        {
            Assert.IsNotNull(this._createdFile);
        }
    }
}
