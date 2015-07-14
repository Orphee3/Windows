using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Midi;
using Moq;
using NUnit.Framework;
using Orphee.CreationShared;
using Orphee.CreationShared.Interfaces;
using Orphee.ViewModels;
using Orphee.ViewModels.Interfaces;

namespace Orphee.UnitTests.CreationSharedTests.LoopCreationViewModelTests.NoteMapManagerTests
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

    public class WhenYouCallConvertNoteMapToOrpheeMessageList
    {
        protected IList<ObservableCollection<IToggleButtonNote>> NoteMap;

        public WhenYouCallConvertNoteMapToOrpheeMessageList()
        {
            this.NoteMap = NoteMapManager.Instance.GenerateNoteMap();
        }
    }

    [TestFixture]
    public class ItShouldReturnAnEmptyListOfOrpheeNoteMessage : WhenYouCallConvertNoteMapToOrpheeMessageList
    {
        private IList<IOrpheeNoteMessage> _opheeNoteMessageList;
        
        [SetUp]
        public void Init()
        {
            this._opheeNoteMessageList = NoteMapManager.Instance.ConvertNoteMapToOrpheeMessageList(this.NoteMap, (int)Channel.Channel1);
        }

        [Test]
        public void OrpheeNoteMessageListShouldNoteBeNull()
        {
            Assert.IsNotNull(this._opheeNoteMessageList);
        }

        [Test]
        public void OrpheeNoteMessageShouldBeEmpty()
        {
           Assert.IsEmpty(this._opheeNoteMessageList);
        }
    }

    [TestFixture]
    public class ItShouldReturnANonEmptyListOfOrpheeNoteMessage : WhenYouCallConvertNoteMapToOrpheeMessageList
    {
        private IList<IOrpheeNoteMessage> _orpheeNoteMessageList;
        private IOrpheeNoteMessage _orpheeNoteOnMessage;
        private IOrpheeNoteMessage _orpheeNoteOffMessage;
        private IOrpheeNoteMessage _expectedNoteOnMessage;
        private IOrpheeNoteMessage _expectedNoteOffMessage;

        [SetUp]
        public void Init()
        {
            this._expectedNoteOnMessage = new OrpheeNoteMessage()
            {
                Channel = 0,
                DeltaTime = 48,
                MessageCode = 0x90,
                Note = Note.C4,
                Velocity = 76
            };
            this._expectedNoteOffMessage = new OrpheeNoteMessage()
            {
                Channel = 0,
                DeltaTime = 48,
                MessageCode = 0x80,
                Note = Note.C4,
                Velocity = 0,
            };
            this.NoteMap[0][1].Note = Note.C4;
            this.NoteMap[0][1].ColumnIndex = 0;
            this.NoteMap[0][1].LineIndex = 0;
            this.NoteMap[0][1].IsChecked = true;
            this._orpheeNoteMessageList = NoteMapManager.Instance.ConvertNoteMapToOrpheeMessageList(this.NoteMap, (int)Channel.Channel1);
            this._orpheeNoteOnMessage = this._orpheeNoteMessageList[0];
            this._orpheeNoteOffMessage = this._orpheeNoteMessageList[1];
        }

        [Test]
        public void OrpheeNoteMessageListShouldNotBeNull()
        {
            Assert.IsNotNull(this._orpheeNoteMessageList);
        }

        [Test]
        public void OrpheeNoteMessageListShouldNotBeEmpty()
        {
            Assert.IsNotEmpty(this._orpheeNoteMessageList);
        }

        [Test]
        public void OrpheeNoteMessageChannelShouldBeEqualToExpectedNoteOnMessageChannel()
        {
            Assert.AreEqual(this._expectedNoteOnMessage.Channel, this._orpheeNoteOnMessage.Channel);
        }

        [Test]
        public void OrpheeNoteMessageDeltaTimeShouldBeEqualToExpectedNoteOnMessageDeltaTime()
        {
            Assert.AreEqual(this._expectedNoteOnMessage.DeltaTime, this._orpheeNoteOnMessage.DeltaTime);
        }

        [Test]
        public void OrpheeNoteMessageMessageCodeShouldBeEqualToExpectedNoteOnMessageMessageCode()
        {
            Assert.AreEqual(this._expectedNoteOnMessage.MessageCode, this._orpheeNoteOnMessage.MessageCode);
        }

        [Test]
        public void OrpheeNoteMessageNoteShouldBeEqualToExpectedNoteOnMessageNote()
        {
            Assert.AreEqual(this._expectedNoteOnMessage.Note, this._orpheeNoteOnMessage.Note);
        }

        [Test]
        public void OrpheeNoteMessageVelocityShouldBeEqualToExpectedNoteOnMessageVelocity()
        {
            Assert.AreEqual(this._expectedNoteOnMessage.Velocity, this._orpheeNoteOnMessage.Velocity);
        }
        [Test]
        public void OrpheeNoteMessageChannelShouldBeEqualToExpectedNoteOffMessageChannel()
        {
            Assert.AreEqual(this._expectedNoteOffMessage.Channel, this._orpheeNoteOffMessage.Channel);
        }

        [Test]
        public void OrpheeNoteMessageDeltaTimeShouldBeEqualToExpectedNoteOffMessageDeltaTime()
        {
            Assert.AreEqual(this._expectedNoteOffMessage.DeltaTime, this._orpheeNoteOffMessage.DeltaTime);
        }

        [Test]
        public void OrpheeNoteMessageMessageCodeShouldBeEqualToExpectedNoteOOffnMessageMessageCode()
        {
            Assert.AreEqual(this._expectedNoteOffMessage.MessageCode, this._orpheeNoteOffMessage.MessageCode);
        }

        [Test]
        public void OrpheeNoteMessageNoteShouldBeEqualToExpectedNoteOffMessageNote()
        {
            Assert.AreEqual(this._expectedNoteOffMessage.Note, this._orpheeNoteOffMessage.Note);
        }

        [Test]
        public void OrpheeNoteMessageVelocityShouldBeEqualToExpectedNoteOffMessageVelocity()
        {
            Assert.AreEqual(this._expectedNoteOffMessage.Velocity, this._orpheeNoteOffMessage.Velocity);
        }
    }
}
