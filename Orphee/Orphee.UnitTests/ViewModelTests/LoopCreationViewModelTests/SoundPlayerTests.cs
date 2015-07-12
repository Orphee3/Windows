﻿using Midi;
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
        protected Mock<IMidiLibRepository> MidiLibRepositoryMock;
        protected Mock<IInstrumentManager> InstrumentManagerMock;
        protected IToggleButtonNote ToggleButtonNote;
        protected ISoundPlayer SoundPlayer;

        public WhenAToggleButtonNoteIsClicked()
        {
            this.InstrumentManagerMock = new Mock<IInstrumentManager>();
            this.OrpheeTrackMock = new Mock<IOrpheeTrack>();
            this.MidiLibRepositoryMock = new Mock<IMidiLibRepository>();
            this.SoundPlayer = new SoundPlayer(this.MidiLibRepositoryMock.Object);
            this.LoopCreationViewModel = new LoopCreationViewModel(this.OrpheeTrackMock.Object, this.SoundPlayer, this.InstrumentManagerMock.Object);
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
    [TestFixture]
    public class ItShoudEmitASoundIfTheTheToggleButtonIsNotChecked : WhenAToggleButtonNoteIsClicked
    {
 
        [SetUp]
        public void Init()
        {
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
        [SetUp]
        public void Init()
        {
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
