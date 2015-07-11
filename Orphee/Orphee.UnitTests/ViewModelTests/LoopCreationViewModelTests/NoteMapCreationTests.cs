using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NUnit.Framework;
using Orphee.Models;
using Orphee.Models.Interfaces;
using Orphee.OrpheeMidiConverter;
using Orphee.ViewModels;
using Orphee.ViewModels.Interfaces;

namespace Orphee.UnitTests.ViewModelTests.LoopCreationViewModelTests
{
    public class WhenYouCreateLoopCreationViewModel
    {
        protected ILoopCreationViewModel LoopCreationViewModel;
        protected IList<ObservableCollection<IToggleButtonNote>> NoteMap;
    }

    public class AnNoteMapShouldAppear : WhenYouCreateLoopCreationViewModel
    {
        [SetUp]
        public void Init()
        {
            this.LoopCreationViewModel = new LoopCreationViewModel(new OrpheeTrack());
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
    }

    public class TheNoteMapShouldGetTenMoreColumns : WhenYouAddColumnsToNoteMap
    {
        [SetUp]
        public void Init()
        {
            this.LoopCreationViewModel = new LoopCreationViewModel(new OrpheeTrack());
            NoteMapManager.Instance.AddColumnsToThisNoteMap(this.LoopCreationViewModel.DisplayedTrack.NoteMap);
            this.NoteMap = this.LoopCreationViewModel.DisplayedTrack.NoteMap;
        }

        [Test]
        public void TheNoteMapShouldHave20Columns()
        {
            Assert.AreEqual(20, this.NoteMap[0].Count);
        }
    }

    public class TheNoteMapShouldNotGetMoreThanTwoHundredColumns : WhenYouAddColumnsToNoteMap
    {
        [SetUp]
        public void Init()
        {
            this.LoopCreationViewModel = new LoopCreationViewModel(new OrpheeTrack());
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
    }

    public class TheNoteMapShouldHaveAColumnLess : WhenYouRemoveAColumnFromTheNoteMap
    {
        [SetUp]
        public void Init()
        {
            this.LoopCreationViewModel = new LoopCreationViewModel(new OrpheeTrack());
            NoteMapManager.Instance.RemoveAColumnFromThisNoteMap(this.LoopCreationViewModel.DisplayedTrack.NoteMap);
            this.NoteMap = this.LoopCreationViewModel.DisplayedTrack.NoteMap;
        }

        [Test]
        public void TheNoteMapShouldHaveNineColumns()
        {
            Assert.AreEqual(9, this.NoteMap[0].Count);
        }
    }

    public class TheNoteMapShouldNotHaveLessThanAColumn : WhenYouRemoveAColumnFromTheNoteMap
    {
        [SetUp]
        public void Init()
        {
            this.LoopCreationViewModel =new LoopCreationViewModel(new OrpheeTrack());
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
