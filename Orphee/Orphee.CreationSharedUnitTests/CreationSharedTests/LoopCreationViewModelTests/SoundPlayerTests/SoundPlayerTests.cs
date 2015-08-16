using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Midi;
using MidiDotNet.ExportModule.Interfaces;
using MidiDotNet.ImportModule.Interfaces;
using Moq;
using NUnit.Framework;
using Orphee.CreationShared;
using Orphee.CreationShared.Interfaces;
using Orphee.ViewModels;
using Orphee.ViewModels.Interfaces;

namespace Orphee.CreationSharedUnitTests.CreationSharedTests.LoopCreationViewModelTests.SoundPlayerTests
{
    public class WhenAToggleButtonNoteIsClicked
    {
        protected ILoopCreationPageViewModel LoopCreationViewModel;
        protected Mock<IMidiLibRepository> MidiLibRepositoryMock;
        protected Mock<IInstrumentManager> InstrumentManagerMock;
        protected Mock<IOrpheeFileExporter> OrpheeFileExporterMock;
        protected Mock<IOrpheeFileImporter> OrpheeFileImporterMock;
        protected IToggleButtonNote ToggleButtonNote;
        protected ISoundPlayer SoundPlayer;

        public WhenAToggleButtonNoteIsClicked()
        {
            this.OrpheeFileImporterMock = new Mock<IOrpheeFileImporter>();
            this.OrpheeFileExporterMock = new Mock<IOrpheeFileExporter>();
            this.InstrumentManagerMock = new Mock<IInstrumentManager>();
            this.MidiLibRepositoryMock = new Mock<IMidiLibRepository>();
            this.SoundPlayer = new SoundPlayer(this.MidiLibRepositoryMock.Object);
            this.LoopCreationViewModel = new LoopCreationPageViewModel(this.SoundPlayer, this.InstrumentManagerMock.Object, this.OrpheeFileExporterMock.Object, this.OrpheeFileImporterMock.Object);
        }
    }

    [TestFixture]
    public class ItShouldPassThroughThePlayNoteFunctionOfTheSoundPlayer : WhenAToggleButtonNoteIsClicked
    {
        [SetUp]
        public void Init()
        {
            this.MidiLibRepositoryMock.Setup(mlr => mlr.PlayNote(It.IsAny<Note>()));
            this.SoundPlayer.PlayNote(Note.C2);
        }

        [Test]
        public void TheResultShouldBeTrue()
        {
            this.MidiLibRepositoryMock.Verify(mlr => mlr.PlayNote(It.IsAny<Note>()), Times.Once());
        }
    }
}
