using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Moq;
using NUnit.Framework;
using Orphee.Models;
using Orphee.Models.Interfaces;
using Orphee.ViewModels;
using Orphee.ViewModels.Interfaces;

namespace Orphee.UnitTests.ViewModelTests.LoopCreationViewModelTests
{
    public class WhenYouCreateLoopCreationViewModel
    {
        protected ILoopCreationViewModel LoopCreationViewModel;
        protected IList<ObservableCollection<IToggleButtonNote>> NoteMap;
        protected Mock<ISoundPlayer> SoundPlayerMock;
    }

    [TestFixture]
    public class AnNoteMapShouldBeCreated : WhenYouCreateLoopCreationViewModel
    {
        [SetUp]
        public void Init()
        {
            this.SoundPlayerMock = new Mock<ISoundPlayer>();
            this.LoopCreationViewModel = new LoopCreationViewModel(new OrpheeTrack(), SoundPlayerMock.Object);
            this.NoteMap = this.LoopCreationViewModel.DisplayedTrack.NoteMap;
        }

        [Test]
        public void NoteMapShouldBeEmptyButNotNull()
        {
            Assert.NotNull(this.NoteMap);
        }

        [Test]
        public void NoteMapShoudContainTenColumns()
        {
            Assert.AreEqual(10, this.NoteMap[0].Count);
        }

        [Test]
        public void NoteMapShouldContainTwelveLines()
        {
            Assert.AreEqual(12, this.NoteMap.Count);
        }

        [Test]
        public void EachLineShouldContainToggleButtonsWithADifferentNote()
        {
            for (var lineIndex = 0; lineIndex < 12; lineIndex++)
            {
                var expectedNote = NoteMapManager.Instance.NoteNameListManager.NoteNameList[NoteMapManager.Instance.NoteNameListManager.NoteNameList.Keys.ElementAt(lineIndex)];
                foreach (var toggleButtonNote in this.NoteMap[lineIndex])
                {
                    var actualNote = toggleButtonNote.Note;
                    Assert.AreEqual(expectedNote, actualNote);
                }
            }
        }
    }

    public class WhenYouAddColumnsToNoteMap
    {
        protected ILoopCreationViewModel LoopCreationViewModel;
        protected IList<ObservableCollection<IToggleButtonNote>> NoteMap;
        protected Mock<ISoundPlayer> SoundPlayerMock;
    }

    [TestFixture]
    public class TheNoteMapShouldGetTenMoreColumns : WhenYouAddColumnsToNoteMap
    {
        [SetUp]
        public void Init()
        {
            this.SoundPlayerMock = new Mock<ISoundPlayer>();
            this.LoopCreationViewModel = new LoopCreationViewModel(new OrpheeTrack(), this.SoundPlayerMock.Object);
            NoteMapManager.Instance.AddColumnsToThisNoteMap(this.LoopCreationViewModel.DisplayedTrack.NoteMap);
            this.NoteMap = this.LoopCreationViewModel.DisplayedTrack.NoteMap;
        }

        [Test]
        public void TheNoteMapShouldHave20Columns()
        {
            Assert.AreEqual(20, this.NoteMap[0].Count);
        }
    }
    [TestFixture]
    public class TheNoteMapShouldNotExceedTwoHundredColumns : WhenYouAddColumnsToNoteMap
    {
        [SetUp]
        public void Init()
        {
            this.SoundPlayerMock = new Mock<ISoundPlayer>();
            this.LoopCreationViewModel = new LoopCreationViewModel(new OrpheeTrack(), this.SoundPlayerMock.Object);
            for (var counter = 0; counter < 20; counter++)
                NoteMapManager.Instance.AddColumnsToThisNoteMap(this.LoopCreationViewModel.DisplayedTrack.NoteMap);
            this.NoteMap = this.LoopCreationViewModel.DisplayedTrack.NoteMap;
        }

        [Test]
        public void TheResultShouldBeEqualToTwoHundred()
        {
            Assert.AreEqual(200, this.NoteMap[0].Count);
        }
    }

    public class WhenYouRemoveAColumnFromTheNoteMap
    {
        protected ILoopCreationViewModel LoopCreationViewModel;
        protected IList<ObservableCollection<IToggleButtonNote>> NoteMap;
        protected Mock<ISoundPlayer> SoundPlayerMock;
    }

    [TestFixture]
    public class TheNoteMapShouldHaveAColumnLess : WhenYouRemoveAColumnFromTheNoteMap
    {
        [SetUp]
        public void Init()
        {
            this.SoundPlayerMock = new Mock<ISoundPlayer>();
            this.LoopCreationViewModel = new LoopCreationViewModel(new OrpheeTrack(), this.SoundPlayerMock.Object);
            NoteMapManager.Instance.RemoveAColumnFromThisNoteMap(this.LoopCreationViewModel.DisplayedTrack.NoteMap);
            this.NoteMap = this.LoopCreationViewModel.DisplayedTrack.NoteMap;
        }

        [Test]
        public void TheNoteMapShouldHaveNineColumns()
        {
            Assert.AreEqual(9, this.NoteMap[0].Count);
        }
    }
    [TestFixture]
    public class TheNoteMapShouldNotHaveLessThanAColumn : WhenYouRemoveAColumnFromTheNoteMap
    {
        [SetUp]
        public void Init()
        {
            this.SoundPlayerMock = new Mock<ISoundPlayer>();
            this.LoopCreationViewModel =new LoopCreationViewModel(new OrpheeTrack(), this.SoundPlayerMock.Object);
            for (var counter = 0; counter < 11; counter++)
                NoteMapManager.Instance.RemoveAColumnFromThisNoteMap(this.LoopCreationViewModel.DisplayedTrack.NoteMap);
            this.NoteMap = this.LoopCreationViewModel.DisplayedTrack.NoteMap;
        }

        [Test]
        public void TheRestultShouldBeOne()
        {
            Assert.AreEqual(1, this.NoteMap[0].Count);
        }
    }
}
