using System;
using System.Threading.Tasks;
using Windows.Storage;
using Midi;
using MidiDotNet.ExportModule;
using MidiDotNet.ExportModule.Interfaces;
using MidiDotNet.Shared;
using Moq;
using NUnit.Framework;
using Orphee.CreationShared;
using Orphee.CreationShared.Interfaces;
using Orphee.RestApiManagement;
using Orphee.RestApiManagement.Senders;

namespace MidiDotNet.ExportModuleUnitTests.ExportModuleTests.OrpheeFileExporterTests
{
    public class WhenFileExporterIsCalled
    {
        protected IOrpheeFileExporter OrpheeFileExporter;
        protected IOrpheeTrack OrpheeTrack;
        protected IOrpheeFile OrpheeFile;
        protected IPlayerParameters PlayerParameters;
        protected Mock<IOrpheeTrackUI> OrpheeTrackUIMock;
        protected Mock<IFilePickerManager> FilePickerManagerMock;
        protected INoteMapGenerator NoteMapGenerator;

        public WhenFileExporterIsCalled()
        {
            InitFilePickerManagerMock();
            this.NoteMapGenerator = new NoteMapManager();
            this.OrpheeTrackUIMock = new Mock<IOrpheeTrackUI>();
            this.PlayerParameters = new PlayerParameters()
            {
                TimeSignatureNominator = 4,
                TimeSignatureDenominator = 4,
                TimeSignatureClocksPerBeat = 24,
                Tempo = 120,
                TimeSignatureNumberOf32ThNotePerBeat = 4
            };
            this.OrpheeTrack = new OrpheeTrack(this.OrpheeTrackUIMock.Object, this.NoteMapGenerator)
            {
                CurrentInstrument = Instrument.OrchestralHarp
            };
            this.OrpheeTrack.Init(0, Channel.Channel1, true);
            this.OrpheeFile = new OrpheeFile(new OrpheeTrack(this.OrpheeTrackUIMock.Object, new NoteMapManager()))
            {
                FileName = "testFile.mid"
            };
            this.OrpheeFileExporter = new OrpheeFileExporter(new ChunckWriters(new FileHeaderWriter(new SwapManager()), new NoteMessageWriter(new ProgramChangeMessageWriter(), new EndOfTrackMessageWriter()), new TrackHeaderWriter(new TimeSignatureMessageWriter(), new TempoMessageWriter(), new SwapManager())),  new FileUploader(new NotificationSender()), new NoteMapManager(), this.FilePickerManagerMock.Object);
        }

        private async void InitFilePickerManagerMock()
        {
            this.FilePickerManagerMock = new Mock<IFilePickerManager>();
            this.FilePickerManagerMock.Setup(fpm => fpm.GetTheSaveFilePicker(It.IsAny<IOrpheeFile>())).Returns(Task.FromResult(await KnownFolders.MusicLibrary.CreateFileAsync("testFile.mid", CreationCollisionOption.ReplaceExisting)));
        }
    }

    [TestFixture]
    public class ItShouldSaveATrackProperly : WhenFileExporterIsCalled
    {
        private StorageFile _createdFile;

        [SetUp]
        public void Init()
        {
            this.OrpheeFileExporter.SaveOrpheeFile(this.OrpheeFile);
            var result = RetrieveCreatedFile().Result;
        }

        private async Task<bool> RetrieveCreatedFile()
        {
            var folder = KnownFolders.MusicLibrary;
            this._createdFile = await folder.GetFileAsync(this.OrpheeFile.FileName);
            return true;
        }

        [Test]
        public void TheCreatedFileShouldNotBeNull()
        {
            Assert.IsNotNull(this._createdFile);
        }
    }
}
