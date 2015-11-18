using Midi;
using MidiDotNet.ExportModule.Interfaces;
using MidiDotNet.ImportModule.Interfaces;
using Moq;
using NUnit.Framework;
using Orphee.CreationShared;
using Orphee.CreationShared.Interfaces;
using Orphee.ViewModels;
using Orphee.ViewModels.Interfaces;

namespace Orphee.CreationSharedUnitTests.LoopCreationViewModelTests.SoundPlayerTests
{
    public class WhenAToggleButtonNoteIsClicked
    {
        protected ICreationPageViewModel LoopCreationViewModel;
        protected Mock<IMidiLibRepository> MidiLibRepositoryMock;
        protected Mock<IOrpheeFileExporter> OrpheeFileExporterMock;
        protected Mock<IOrpheeFileImporter> OrpheeFileImporterMock;
        protected Mock<IOrpheeFile> OrpheeFileMock;
        protected Mock<INoteMapManager> NoteMapManagerMock;
        protected IToggleButtonNote ToggleButtonNote;
        protected ISoundPlayer SoundPlayer;

        public WhenAToggleButtonNoteIsClicked()
        {
            this.OrpheeFileMock = new Mock<IOrpheeFile>();
            this.NoteMapManagerMock = new Mock<INoteMapManager>();
            this.OrpheeFileImporterMock = new Mock<IOrpheeFileImporter>();
            this.OrpheeFileExporterMock = new Mock<IOrpheeFileExporter>();
            this.MidiLibRepositoryMock = new Mock<IMidiLibRepository>();
            this.SoundPlayer = new SoundPlayer(this.MidiLibRepositoryMock.Object);
            this.LoopCreationViewModel = new CreationPageViewModel(this.SoundPlayer,  this.OrpheeFileExporterMock.Object, this.OrpheeFileImporterMock.Object, this.NoteMapManagerMock.Object, this.OrpheeFileMock.Object);
        }
    }

    [TestFixture]
    public class ItShouldPassThroughThePlayNoteFunctionOfTheSoundPlayer : WhenAToggleButtonNoteIsClicked
    {
        [SetUp]
        public void Init()
        {
            this.MidiLibRepositoryMock.Setup(mlr => mlr.PlayNote(It.IsAny<Note>(), It.IsAny<Channel>()));
            this.SoundPlayer.PlayNote(Note.C2, Channel.Channel1);
        }

        [Test]
        public void TheResultShouldBeTrue()
        {
            this.MidiLibRepositoryMock.Verify(mlr => mlr.PlayNote(It.IsAny<Note>(), It.IsAny<Channel>()), Times.Once());
        }
    }
}
