using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Moq;
using NUnit.Framework;
using Orphee.CreationShared;
using Orphee.CreationShared.Interfaces;
using Orphee.ViewModels;
using Orphee.ViewModels.Interfaces;

namespace Orphee.UnitTests.CreationSharedTests.LoopCreationViewModelTests.NoteMapTests
{
    public class NoteMapTestsBase
    {
        protected ILoopCreationViewModel LoopCreationViewModel;
        protected IList<ObservableCollection<IToggleButtonNote>> NoteMap;
        protected Mock<IInstrumentManager> InstrumentManagerMock;
        protected Mock<ISoundPlayer> SoundPlayerMock;

        public NoteMapTestsBase()
        {
            this.SoundPlayerMock = new Mock<ISoundPlayer>();
            this.InstrumentManagerMock = new Mock<IInstrumentManager>();
            this.LoopCreationViewModel =new LoopCreationViewModel(this.SoundPlayerMock.Object, this.InstrumentManagerMock.Object);
        }
    }
    public class WhenYouCreateLoopCreationViewModel : NoteMapTestsBase
    {

    }

    [TestFixture]
    public class AnNoteMapShouldBeCreated : WhenYouCreateLoopCreationViewModel
    {
        [SetUp]
        public void Init()
        {
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

    public class WhenYouAddColumnsToNoteMap : NoteMapTestsBase
    {
    }

    [TestFixture]
    public class TheNoteMapShouldGetTenMoreColumns : WhenYouAddColumnsToNoteMap
    {
        [SetUp]
        public void Init()
        {
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

    public class WhenYouRemoveAColumnFromTheNoteMap : NoteMapTestsBase
    {
  
    }

    [TestFixture]
    public class TheNoteMapShouldHaveAColumnLess : WhenYouRemoveAColumnFromTheNoteMap
    {
        [SetUp]
        public void Init()
        {
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
