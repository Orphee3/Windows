using Midi;
using Moq;
using NUnit.Framework;
using Orphee.Models;
using Orphee.Models.Interfaces;
using Orphee.ViewModels;
using Orphee.ViewModels.Interfaces;

namespace Orphee.UnitTests.ViewModelTests.LoopCreationViewModelTests
{
    public class WhenAToggleButtonNoteIsClicked
    {
        protected ILoopCreationViewModel LoopCreationViewModel;
        protected Mock<IOrpheeTrack> OrpheeTrackMock;
        protected ISoundPlayer SoundPlayer;
        protected Mock<IMidiLibRepository> MidiLibRepositoryMock;
        protected IToggleButtonNote ToggleButtonNote;
    }

    [TestFixture]
    public class ItShouldPassThroughThePlayNoteFunctionOfTheSoundPlayer : WhenAToggleButtonNoteIsClicked
    {
        [SetUp]
        public void Init()
        {
            this.MidiLibRepositoryMock = new Mock<IMidiLibRepository>();
            this.MidiLibRepositoryMock.Setup(mlr => mlr.PlayNote(It.IsAny<Note>()));
            this.SoundPlayer = new SoundPlayer(this.MidiLibRepositoryMock.Object);
            this.SoundPlayer.PlayNote(Note.C2);
        }

        [Test]
        public void TheResultShouldBeTrue()
        {
            this.MidiLibRepositoryMock.Verify(mlr => mlr.PlayNote(It.IsAny<Note>()), Times.Once());
        }
    }

    [TestFixture]
    public class ItShoudEmitASoundIfTheTheToggleButtonIsNotChecked : WhenAToggleButtonNoteIsClicked
    {
        private Mock<ISoundPlayer> _soundPlayerMock;

        [SetUp]
        public void Init()
        {
            this._soundPlayerMock = new Mock<ISoundPlayer>();
            this.OrpheeTrackMock = new Mock<IOrpheeTrack>();
            this.LoopCreationViewModel = new LoopCreationViewModel(this.OrpheeTrackMock.Object, this._soundPlayerMock.Object);
            this.ToggleButtonNote = new ToggleButtonNote() { LineIndex = 0, ColumnIndex = 0, Note = Note.A1, IsChecked = false };
            this.LoopCreationViewModel.ToggleButtonNoteExec(this.ToggleButtonNote);
        }

        [Test]
        public void TheToggleButtonShouldBeChecked()
        {
            Assert.IsTrue(this.ToggleButtonNote.IsChecked);
        }
    }

    [TestFixture]
    public class ItShoudNotEmitASoundIfTheTheToggleButtonIsChecked : WhenAToggleButtonNoteIsClicked
    {
        private Mock<ISoundPlayer> _soundPlayerMock;

        [SetUp]
        public void Init()
        {
            this._soundPlayerMock = new Mock<ISoundPlayer>();
            this.OrpheeTrackMock = new Mock<IOrpheeTrack>();
            this.LoopCreationViewModel = new LoopCreationViewModel(this.OrpheeTrackMock.Object, this._soundPlayerMock.Object);
            this.ToggleButtonNote = new ToggleButtonNote() { LineIndex = 0, ColumnIndex = 0, Note = Note.A1, IsChecked = true };
            this.LoopCreationViewModel.ToggleButtonNoteExec(this.ToggleButtonNote);
        }

        [Test]
        public void TheToggleButtonShouldNotBeChecked()
        {
            Assert.IsFalse(this.ToggleButtonNote.IsChecked);
        }
    }
}
