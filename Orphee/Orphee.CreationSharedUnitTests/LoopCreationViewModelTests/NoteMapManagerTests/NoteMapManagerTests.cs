﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Midi;
using Moq;
using NUnit.Framework;
using Orphee.CreationShared;
using Orphee.CreationShared.Interfaces;

namespace Orphee.CreationSharedUnitTests.LoopCreationViewModelTests.NoteMapManagerTests
{
    public class NoteMapTestsBase
    {
        protected IOrpheeFile OrpheeFile;
        protected IList<ObservableCollection<IToggleButtonNote>> NoteMap;
        protected Mock<IOrpheeTrackUI> OrpheeTrackUIMock;
        protected NoteMapManager NoteMapManager;

        public NoteMapTestsBase()
        {
            this.OrpheeTrackUIMock = new Mock<IOrpheeTrackUI>();
            this.NoteMapManager = new NoteMapManager();
            this.OrpheeFile = new OrpheeFile(new OrpheeTrack(this.OrpheeTrackUIMock.Object, this.NoteMapManager));
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
            this.NoteMap = this.OrpheeFile.OrpheeTrackList[0].NoteMap;
        }

        [Test]
        public void NoteMapShouldBeEmptyButNotNull()
        {
            Assert.NotNull(this.NoteMap);
        }

        [Test]
        public void NoteMapShoudContainFiftyColumns()
        {
            Assert.AreEqual(50, this.NoteMap[0].Count);
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
                var expectedNote = this.NoteMapManager.NoteNameListManager.NoteNameList[this.NoteMapManager.NoteNameListManager.NoteNameList.Keys.ElementAt((lineIndex + (3 * 12)))];
                foreach (var actualNote in this.NoteMap[lineIndex].Select(toggleButtonNote => toggleButtonNote.Note))
                {
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
            this.NoteMapManager.AddColumnsToThisNoteMap(this.OrpheeFile.OrpheeTrackList[0].NoteMap);
            this.NoteMap = this.OrpheeFile.OrpheeTrackList[0].NoteMap;
        }

        [Test]
        public void TheNoteMapShouldHaveAHundredColumns()
        {
            Assert.AreEqual(100, this.NoteMap[0].Count);
        }
    }
    [TestFixture]
    public class TheNoteMapShouldNotExceedTwoHundredColumns : WhenYouAddColumnsToNoteMap
    {
        [SetUp]
        public void Init()
        {
            for (var counter = 0; counter < 20; counter++)
                this.NoteMapManager.AddColumnsToThisNoteMap(this.OrpheeFile.OrpheeTrackList[0].NoteMap);
            this.NoteMap = this.OrpheeFile.OrpheeTrackList[0].NoteMap;
        }

        [Test]
        public void TheResultShouldBeEqualToOneThousandFifty()
        {
            Assert.AreEqual(1050, this.NoteMap[0].Count);
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
            this.NoteMapManager.RemoveAColumnFromThisNoteMap(this.OrpheeFile.OrpheeTrackList[0].NoteMap);
            this.NoteMap = this.OrpheeFile.OrpheeTrackList[0].NoteMap;
        }

        [Test]
        public void TheNoteMapShouldHaveFortyNineColumns()
        {
            Assert.AreEqual(49, this.NoteMap[0].Count);
        }
    }
    [TestFixture]
    public class TheNoteMapShouldNotHaveLessThanAColumn : WhenYouRemoveAColumnFromTheNoteMap
    {
        [SetUp]
        public void Init()
        {
            for (var counter = 0; counter < 50; counter++)
                this.NoteMapManager.RemoveAColumnFromThisNoteMap(this.OrpheeFile.OrpheeTrackList[0].NoteMap);
            this.NoteMap = this.OrpheeFile.OrpheeTrackList[0].NoteMap;
        }

        [Test]
        public void TheRestultShouldBeOne()
        {
            Assert.AreEqual(1, this.NoteMap[0].Count);
        }
    }

    public class WhenYouCallConvertNoteMapToOrpheeMessageListWithAnEmptyNoteMap
    {
        protected IList<ObservableCollection<IToggleButtonNote>> NoteMap;
        protected NoteMapManager NoteMapManager;

        public WhenYouCallConvertNoteMapToOrpheeMessageListWithAnEmptyNoteMap()
        {
            this.NoteMapManager = new NoteMapManager();
            this.NoteMap = this.NoteMapManager.GenerateNoteMap(4);
        }
    }

    [TestFixture]
    public class ItShouldReturnAnEmptyListOfOrpheeNoteMessage : WhenYouCallConvertNoteMapToOrpheeMessageListWithAnEmptyNoteMap
    {
        private IList<IOrpheeNoteMessage> _opheeNoteMessageList;
        private uint _trackLength;

        [SetUp]
        public void Init()
        {
            this._opheeNoteMessageList = this.NoteMapManager.ConvertNoteMapToOrpheeNoteMessageList(this.NoteMap, (int)Channel.Channel1, ref this._trackLength);
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

    public class WhenYouCallConvertNoteMapToOrpheeMessageListWithAnNonEmptyNoteMap
    {
        protected IList<ObservableCollection<IToggleButtonNote>> NoteMap;
        protected NoteMapManager NoteMapManager;

        public WhenYouCallConvertNoteMapToOrpheeMessageListWithAnNonEmptyNoteMap()
        {
            this.NoteMapManager = new NoteMapManager();
            this.NoteMap = this.NoteMapManager.GenerateNoteMap(4);
            this.NoteMap[0][0].Note = Note.C4;
            this.NoteMap[0][0].ColumnIndex = 0;
            this.NoteMap[0][0].LineIndex = 0;
            this.NoteMap[0][0].IsChecked = 100;
        }
    }

    [TestFixture]
    public class ItShouldReturnANonEmptyListOfOrpheeNoteMessage : WhenYouCallConvertNoteMapToOrpheeMessageListWithAnNonEmptyNoteMap
    {
        private IList<IOrpheeNoteMessage> _orpheeNoteMessageList;
        private uint _trackLength;

        [SetUp]
        public void Init()
        {
            this._orpheeNoteMessageList = this.NoteMapManager.ConvertNoteMapToOrpheeNoteMessageList(this.NoteMap, (int)Channel.Channel1, ref this._trackLength);
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
        public void OrpheNoteMessageShouldContainTwoMessages()
        {
            Assert.AreEqual(2, this._orpheeNoteMessageList.Count);
        }
    }

    [TestFixture]
    public class ItShouldReturnANonEmptyListOfOrpheeNoteMessageContainingTheExpectedNoteMessages : WhenYouCallConvertNoteMapToOrpheeMessageListWithAnNonEmptyNoteMap
    {
        private IList<IOrpheeNoteMessage> _orpheeNoteMessageList;
        private IOrpheeNoteMessage _orpheeNoteOnMessage;
        private IOrpheeNoteMessage _orpheeNoteOffMessage;
        private IOrpheeNoteMessage _expectedNoteOnMessage;
        private IOrpheeNoteMessage _expectedNoteOffMessage;
        private uint _trackLength;

        [SetUp]
        public void Init()
        {
            this._expectedNoteOnMessage = new OrpheeNoteMessage()
            {
                Channel = 0,
                DeltaTime = 0,
                MessageCode = 0x90,
                Note = Note.C4,
                Velocity = 76
            };
            this._expectedNoteOffMessage = new OrpheeNoteMessage()
            {
                Channel = 0,
                DeltaTime = 96,
                MessageCode = 0x80,
                Note = Note.C4,
                Velocity = 0
            };
            this._orpheeNoteMessageList = this.NoteMapManager.ConvertNoteMapToOrpheeNoteMessageList(this.NoteMap, (int)Channel.Channel1, ref this._trackLength);
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
        public void OrpheNoteMessageShouldContainTwoMessages()
        {
            Assert.AreEqual(2, this._orpheeNoteMessageList.Count);
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
        public void OrpheeNoteMessageMessageCodeShouldBeEqualToExpectedNoteOffMessageMessageCode()
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

    [TestFixture]
    public class ItShouldReturnANonEmptyListOfOrpheeNoteMessageContainingTheExpectedNoteMessagesWithAHighDeltaTime : WhenYouCallConvertNoteMapToOrpheeMessageListWithAnNonEmptyNoteMap
    {
        private IList<IOrpheeNoteMessage> _orpheeNoteMessageList; 
        private IOrpheeNoteMessage _orpheeNoteOnMessage;
        private IOrpheeNoteMessage _orpheeNoteOffMessage;
        private IOrpheeNoteMessage _expectedNoteOnMessage;
        private IOrpheeNoteMessage _expectedNoteOffMessage;
        private uint _trackLength;

        [SetUp]
        public void Init()
        {
            this.NoteMap[0][9].Note = Note.C4;
            this.NoteMap[0][9].ColumnIndex = 9;
            this.NoteMap[0][9].LineIndex = 0;
            this.NoteMap[0][9].IsChecked = 100;
            this._expectedNoteOnMessage = new OrpheeNoteMessage()
            {
                Channel = 4,
                DeltaTime = 384,
                MessageCode = 0x90,
                Note = Note.C4,
                Velocity = 76
            };
            this._expectedNoteOffMessage = new OrpheeNoteMessage()
            {
                Channel = 4,
                DeltaTime = 96,
                MessageCode = 0x80,
                Note = Note.C4,
                Velocity = 0,
            };

            this._orpheeNoteMessageList = this.NoteMapManager.ConvertNoteMapToOrpheeNoteMessageList(this.NoteMap, (int)Channel.Channel5, ref this._trackLength);
            this._orpheeNoteOnMessage = this._orpheeNoteMessageList[2];
            this._orpheeNoteOffMessage = this._orpheeNoteMessageList[3];
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
        public void OrpheNoteMessageShouldContainFourMessages()
        {
            Assert.AreEqual(4, this._orpheeNoteMessageList.Count);
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

    [TestFixture]
    public class ItShouldReturnANonEmptyListOfOrpheeNoteMessageContainingTheExpectedNoteMessagesWithTheSameDeltaTime : WhenYouCallConvertNoteMapToOrpheeMessageListWithAnNonEmptyNoteMap
    {
        private IList<IOrpheeNoteMessage> _orpheeNoteMessageList;
        private IOrpheeNoteMessage _orpheeFirstNoteOnMessage;
        private IOrpheeNoteMessage _orpheeSecondNoteOnMessage;
        private IOrpheeNoteMessage _orpheeFirstNoteOffMessage;
        private IOrpheeNoteMessage _orpheeSecondNoteOffMessage;
        private IOrpheeNoteMessage _expectedFirstNoteOnMessage;
        private IOrpheeNoteMessage _expectedSecondNoteOnMessage;
        private IOrpheeNoteMessage _expectedFirstNoteOffMessage;
        private IOrpheeNoteMessage _expectedSecondNoteOffMessage;
        private uint _trackLength;

        [SetUp]
        public void Init()
        {
            InitNoteMap();
            InitExptectedMessages();
            this._orpheeNoteMessageList = this.NoteMapManager.ConvertNoteMapToOrpheeNoteMessageList(this.NoteMap, (int)Channel.Channel6, ref this._trackLength);
            this._orpheeFirstNoteOnMessage = this._orpheeNoteMessageList[2];
            this._orpheeSecondNoteOnMessage = this._orpheeNoteMessageList[3];
            this._orpheeFirstNoteOffMessage = this._orpheeNoteMessageList[4];
            this._orpheeSecondNoteOffMessage = this._orpheeNoteMessageList[5];
        }

        private void InitNoteMap()
        {
            this.NoteMap[0][2].Note = Note.C4;
            this.NoteMap[0][2].ColumnIndex = 2;
            this.NoteMap[0][2].LineIndex = 0;
            this.NoteMap[0][2].IsChecked = 100;

            this.NoteMap[1][2].Note = Note.C4;
            this.NoteMap[1][2].ColumnIndex = 2;
            this.NoteMap[1][2].LineIndex = 1;
            this.NoteMap[1][2].IsChecked = 100;
        }

        private void InitExptectedMessages()
        {
            this._expectedFirstNoteOnMessage = new OrpheeNoteMessage()
            {
                Channel = 5,
                DeltaTime = 48,
                MessageCode = 0x90,
                Note = Note.C4,
                Velocity = 76
            };
            this._expectedSecondNoteOnMessage = new OrpheeNoteMessage()
            {
                Channel = 5,
                DeltaTime = 0,
                MessageCode = 0x90,
                Note = Note.C4,
                Velocity = 76,
            };

            this._expectedFirstNoteOffMessage = new OrpheeNoteMessage()
            {
                Channel = 5,
                DeltaTime = 96,
                MessageCode = 0x80,
                Note = Note.C4,
                Velocity = 0
            };

            this._expectedSecondNoteOffMessage = new OrpheeNoteMessage()
            {
                Channel = 5,
                DeltaTime = 0,
                MessageCode = 0x80,
                Note = Note.C4,
                Velocity = 0,
            };
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
        public void OrpheNoteMessageShouldContainSixMessages()
        {
            Assert.AreEqual(6, this._orpheeNoteMessageList.Count);    
        }

        [Test]
        public void OrpheeFirstNoteOnMessageChannelShouldBeEqualToExpectedNoteOnMessageChannel()
        {
            Assert.AreEqual(this._expectedFirstNoteOnMessage.Channel, this._orpheeFirstNoteOnMessage.Channel);
        }

        [Test]
        public void OrpheeFirstNoteOnMessageDeltaTimeShouldBeEqualToExpectedNoteOnMessageDeltaTime()
        {
            Assert.AreEqual(this._expectedFirstNoteOnMessage.DeltaTime, this._orpheeFirstNoteOnMessage.DeltaTime);
        }

        [Test]
        public void OrpheeFirstNoteOnMessageMessageCodeShouldBeEqualToExpectedFirstNoteOnMessageMessageCode()
        {
            Assert.AreEqual(this._expectedFirstNoteOnMessage.MessageCode, this._orpheeFirstNoteOnMessage.MessageCode);
        }

        [Test]
        public void OrpheeFirstNoteOnMessageNoteShouldBeEqualToExpectedFirstNoteOnMessageNote()
        {
            Assert.AreEqual(this._expectedFirstNoteOnMessage.Note, this._orpheeFirstNoteOnMessage.Note);
        }

        [Test]
        public void OrpheeFirstNoteOnMessageVelocityShouldBeEqualToExpectedFirstNoteOnMessageVelocity()
        {
            Assert.AreEqual(this._expectedFirstNoteOnMessage.Velocity, this._orpheeFirstNoteOnMessage.Velocity);
        }

        [Test]
        public void OrpheeSecondNoteOnMessageChannelShouldBeEqualToExpectedSecondNoteOnMessageChannel()
        {
            Assert.AreEqual(this._expectedSecondNoteOnMessage.Channel, this._orpheeSecondNoteOnMessage.Channel);
        }

        [Test]
        public void OrpheeSecondNoteOnMessageDeltaTimeShouldBeEqualToExpectedSecondNoteOnMessageDeltaTime()
        {
            Assert.AreEqual(this._expectedSecondNoteOnMessage.DeltaTime, this._orpheeSecondNoteOnMessage.DeltaTime);
        }

        [Test]
        public void OrpheeSecondNoteOnMessageMessageCodeShouldBeEqualToExpectedSecondNoteOnMessageMessageCode()
        {
            Assert.AreEqual(this._expectedSecondNoteOnMessage.MessageCode, this._orpheeSecondNoteOnMessage.MessageCode);
        }

        [Test]
        public void OrpheeSecondNoteOnMessageNoteShouldBeEqualToExpectedSecondNoteOnMessageNote()
        {
            Assert.AreEqual(this._expectedSecondNoteOnMessage.Note, this._orpheeSecondNoteOnMessage.Note);
        }

        [Test]
        public void OrpheeSecondNoteOnMessageVelocityShouldBeEqualToExpectedSecondNoteOnMessageVelocity()
        {
            Assert.AreEqual(this._expectedSecondNoteOnMessage.Velocity, this._orpheeSecondNoteOnMessage.Velocity);
        }

        [Test]
        public void OrpheeFirstNoteOffMessageChannelShouldBeEqualToExpectedNoteOffMessageChannel()
        {
            Assert.AreEqual(this._expectedFirstNoteOffMessage.Channel, this._orpheeFirstNoteOffMessage.Channel);
        }

        [Test]
        public void OrpheeFirstNoteOffMessageDeltaTimeShouldBeEqualToExpectedNoteOffMessageDeltaTime()
        {
            Assert.AreEqual(this._expectedFirstNoteOffMessage.DeltaTime, this._orpheeFirstNoteOffMessage.DeltaTime);
        }

        [Test]
        public void OrpheeFirstNoteOffMessageMessageCodeShouldBeEqualToExpectedFirstNoteOffMessageMessageCode()
        {
            Assert.AreEqual(this._expectedFirstNoteOffMessage.MessageCode, this._orpheeFirstNoteOffMessage.MessageCode);
        }

        [Test]
        public void OrpheeFirstNoteOffMessageNoteShouldBeEqualToExpectedFirstNoteOffMessageNote()
        {
            Assert.AreEqual(this._expectedFirstNoteOffMessage.Note, this._orpheeFirstNoteOffMessage.Note);
        }

        [Test]
        public void OrpheeFirstNoteOffMessageVelocityShouldBeEqualToExpectedFirstNoteOffMessageVelocity()
        {
            Assert.AreEqual(this._expectedFirstNoteOffMessage.Velocity, this._orpheeFirstNoteOffMessage.Velocity);
        }

        [Test]
        public void OrpheeSecondNoteOffMessageChannelShouldBeEqualToExpectedSecondNoteOffMessageChannel()
        {
            Assert.AreEqual(this._expectedSecondNoteOffMessage.Channel, this._orpheeSecondNoteOffMessage.Channel);
        }

        [Test]
        public void OrpheeSecondNoteOffMessageDeltaTimeShouldBeEqualToExpectedSecondNoteOffMessageDeltaTime()
        {
            Assert.AreEqual(this._expectedSecondNoteOffMessage.DeltaTime, this._orpheeSecondNoteOffMessage.DeltaTime);
        }

        [Test]
        public void OrpheeSecondNoteOffMessageMessageCodeShouldBeEqualToExpectedSecondNoteOffMessageMessageCode()
        {
            Assert.AreEqual(this._expectedSecondNoteOffMessage.MessageCode, this._orpheeSecondNoteOffMessage.MessageCode);
        }

        [Test]
        public void OrpheeSecondNoteOffMessageNoteShouldBeEqualToExpectedSecondNoteOffMessageNote()
        {
            Assert.AreEqual(this._expectedSecondNoteOffMessage.Note, this._orpheeSecondNoteOffMessage.Note);
        }

        [Test]
        public void OrpheeSecondNoteOffMessageVelocityShouldBeEqualToExpectedSecondNoteOffMessageVelocity()
        {
            Assert.AreEqual(this._expectedSecondNoteOffMessage.Velocity, this._orpheeSecondNoteOffMessage.Velocity);
        }
    }

    public class WhenYouCallConvertOrpheeNoteMessageListToNoteMap
    {
        protected IList<IOrpheeNoteMessage> OrpheeNoteMessageList;
        protected NoteMapManager NoteMapManager;

        public WhenYouCallConvertOrpheeNoteMessageListToNoteMap()
        {
            this.NoteMapManager = new NoteMapManager();
            this.OrpheeNoteMessageList = new List<IOrpheeNoteMessage>();
        }
    }

    [TestFixture]
    public class ItShouldReturnANonEmptyNoteMap : WhenYouCallConvertOrpheeNoteMessageListToNoteMap
    {
        private IList<ObservableCollection<IToggleButtonNote>> _result;

        [SetUp]
        public void Init()
        {
            this.OrpheeNoteMessageList.Add(new OrpheeNoteMessage()
            {
                Channel = 0,
                DeltaTime = 0,
                MessageCode = 0x90,
                Note = Note.B4,
                Velocity = 76,
            });
            this.OrpheeNoteMessageList.Add(new OrpheeNoteMessage()
            {
                Channel = 0,
                DeltaTime = 48,
                MessageCode = 0x70,
                Note = Note.B4,
                Velocity = 0,
            });
            this._result = this.NoteMapManager.ConvertOrpheeMessageListToNoteMap(this.OrpheeNoteMessageList);
        }

        [Test]
        public void ResultShouldNotBeNull()
        {
            Assert.IsNotNull(this._result);
        }

        [Test]
        public void ResultShouldNotBeEmpty()
        {
            Assert.IsNotEmpty(this._result);
        }
    }
}
