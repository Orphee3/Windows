using System.Collections.Generic;
using System.IO;
using MidiDotNet.ImportModule;
using MidiDotNet.ImportModule.Interfaces;
using Moq;
using NUnit.Framework;
using Orphee.CreationShared.Interfaces;

namespace MidiDotNet.ImportModuleUnitTests.ImportModuleTests.NoteMessageReaderTests
{
    public class WhenNoteMessageReader : ImportModuleTestsBase
    {
        protected INoteMessageReader NoteMessageReader;

        public WhenNoteMessageReader()
        {
            var endOfTrackMessageReaderMock = new Mock<IEndOfTrackMessageReader>();
            endOfTrackMessageReaderMock.Setup(eotmrm => eotmrm.ReadEndOfTrackMessage(It.IsAny<BinaryReader>())).Returns(true);
            this.NoteMessageReader = new NoteMessageReader(new DeltaTimeReader(), endOfTrackMessageReaderMock.Object);
            var result = GetFile("NoteMessageTests.test").Result;
        }
    }

    [TestFixture]
    public class ItShouldReturnTrueWhenTheNoteOnMessageIsCorrect : WhenNoteMessageReader
    {
        private bool _result;
        private IList<IOrpheeNoteMessage> _orpheeNoteMessageList;

        [SetUp]
        public void Init()
        {
            ReWriteTheFile(new byte[] {0x00, 0x94, 0x3C, 0x4C});
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
                this._result = this.NoteMessageReader.ReadNoteMessage(this.Reader, 4);
            this._orpheeNoteMessageList = this.NoteMessageReader.OrpheeNoteMessageList;
        }

        [Test]
        public void ResultShouldBeTrue()
        {
            Assert.IsTrue(this._result);
        }

        [Test]
        public void NoteMessageReaderOrpheeNoteMessageListShouldNotBeNull()
        {
            Assert.IsNotNull(this._orpheeNoteMessageList);
        }

        [Test]
        public void NoteMessageReaderOrpheeNoteMessageListShouldNotBeEmpty()
        {
            Assert.IsNotEmpty(this._orpheeNoteMessageList);
        }
    }

    [TestFixture]
    public class ItShouldReturnTrueWhenTheNoteOffMessageIsCorrect : WhenNoteMessageReader
    {
        private bool _result;
        private IList<IOrpheeNoteMessage> _orpheeNoteMessageList;

        [SetUp]
        public void Init()
        {
            ReWriteTheFile(new byte[] { 0x00, 0x84, 0x3C, 0x4C });
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
                this._result = this.NoteMessageReader.ReadNoteMessage(this.Reader, 4);
            this._orpheeNoteMessageList = this.NoteMessageReader.OrpheeNoteMessageList;
        }

        [Test]
        public void ResultShouldBeTrue()
        {
            Assert.IsTrue(this._result);
        }

        [Test]
        public void NoteMessageReaderOrpheeNoteMessageListShouldNotBeNull()
        {
            Assert.IsNotNull(this._orpheeNoteMessageList);
        }

        [Test]
        public void NoteMessageReaderOrpheeNoteMessageListShouldNotBeEmpty()
        {
            Assert.IsNotEmpty(this._orpheeNoteMessageList);
        }
    }

    [TestFixture]
    public class ItShouldHaveTheExpectedNoteIndex : WhenNoteMessageReader
    {
        private bool _result;
        private byte _expectedNoteIndex;
        private byte _actualNoteIndex;
        private IList<IOrpheeNoteMessage> _orpheeNoteMessageList;

        [SetUp]
        public void Init()
        {
            this._expectedNoteIndex = 0x3C;
            ReWriteTheFile(new byte[] { 0x00, 0x94, 0x3C, 0x4C });
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
                this._result = this.NoteMessageReader.ReadNoteMessage(this.Reader, 4);
            this._actualNoteIndex = (byte) this.NoteMessageReader.OrpheeNoteMessageList[0].Note;
            this._orpheeNoteMessageList = this.NoteMessageReader.OrpheeNoteMessageList;
        }

        [Test]
        public void ResultShouldBeTrue()
        {
            Assert.IsTrue(this._result);
        }

        [Test]
        public void NoteMessageReaderNoteIndexShouldBeEqualToExpectednoteIndex()
        {
            Assert.AreEqual(this._expectedNoteIndex, this._actualNoteIndex);
        }

        [Test]
        public void NoteMessageReaderOrpheeNoteMessageListShouldNotBeNull()
        {
            Assert.IsNotNull(this._orpheeNoteMessageList);
        }

        [Test]
        public void NoteMessageReaderOrpheeNoteMessageListShouldNotBeEmpty()
        {
            Assert.IsNotEmpty(this._orpheeNoteMessageList);
        }
    }

    [TestFixture]
    public class ItShouldReturnFalseIfTheNoteIndexIsHigherThanTheSetLimit : WhenNoteMessageReader
    {
        private bool _result;
        private IList<IOrpheeNoteMessage> _orpheeNoteMessageList;

        [SetUp]
        public void Init()
        {
            ReWriteTheFile(new byte[] { 0x00, 0x94, 0x80, 0x4C });
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
                this._result = this.NoteMessageReader.ReadNoteMessage(this.Reader, 4);
            this._orpheeNoteMessageList = this.NoteMessageReader.OrpheeNoteMessageList;
        }

        [Test]
        public void ResultShouldBeFalse()
        {
            Assert.IsFalse(this._result);
        }

        [Test]
        public void NoteMessageReaderOrpheeNoteMessageListShouldNotBeNull()
        {
            Assert.IsNotNull(this._orpheeNoteMessageList);
        }

        [Test]
        public void NoteMessageReaderOrpheeNoteMessageListShouldBeEmpty()
        {
            Assert.IsEmpty(this._orpheeNoteMessageList);
        }
    }

    [TestFixture]
    public class ItShouldReturnTrueIfTheNoteIndexIsOnePointLowerThanTheSetLimit : WhenNoteMessageReader
    {
        private bool _result;
        private IList<IOrpheeNoteMessage> _orpheeNoteMessageList;

        [SetUp]
        public void Init()
        {
            ReWriteTheFile(new byte[] { 0x00, 0x94, 0x7F, 0x4C });
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
                this._result = this.NoteMessageReader.ReadNoteMessage(this.Reader, 4);
            this._orpheeNoteMessageList = this.NoteMessageReader.OrpheeNoteMessageList;
        }

        [Test]
        public void ResultShouldBeTrue()
        {
            Assert.IsTrue(this._result);
        }

        [Test]
        public void NoteMessageReaderOrpheeNoteMessageListShouldNotBeNull()
        {
            Assert.IsNotNull(this._orpheeNoteMessageList);
        }

        [Test]
        public void NoteMessageReaderOrpheeNoteMessageListShouldNotBeEmpty()
        {
            Assert.IsNotEmpty(this._orpheeNoteMessageList);
        }
    }


    [TestFixture]
    public class ItShouldHaveTheExpectedDeltaTime : WhenNoteMessageReader
    {
        private bool _result;
        private byte _expectedDeltaTime;
        private byte _actualDeltaTime;
        private IList<IOrpheeNoteMessage> _orpheeNoteMessageList;

        [SetUp]
        public void Init()
        {
            this._expectedDeltaTime = 96;
            ReWriteTheFile(new byte[] { 0x60, 0x94, 0x3C, 0x4C });
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
                this._result = this.NoteMessageReader.ReadNoteMessage(this.Reader, 4);
            this._actualDeltaTime = (byte) this.NoteMessageReader.OrpheeNoteMessageList[0].DeltaTime;
            this._orpheeNoteMessageList = this.NoteMessageReader.OrpheeNoteMessageList;
        }

        [Test]
        public void ResultShouldBeTrue()
        {
            Assert.IsTrue(this._result);
        }

        [Test]
        public void NoteMessageReaderDeltaTimeShouldBeEqualToExpectedDeltaTime()
        {
            Assert.AreEqual(this._expectedDeltaTime, this._actualDeltaTime);
        }

        [Test]
        public void NoteMessageReaderOrpheeNoteMessageListShouldNotBeNull()
        {
            Assert.IsNotNull(this._orpheeNoteMessageList);
        }

        [Test]
        public void NoteMessageReaderOrpheeNoteMessageListShouldNotBeEmpty()
        {
            Assert.IsNotEmpty(this._orpheeNoteMessageList);
        }
    }

    [TestFixture]
    public class ItShouldHaveTheExpectedVelocity: WhenNoteMessageReader
    {
        private bool _result;
        private byte _expectedVelocity;
        private byte _actualVelocity;
        private IList<IOrpheeNoteMessage> _orpheeNoteMessageList;
        
        [SetUp]
        public void Init()
        {
            this._expectedVelocity = 76;
            ReWriteTheFile(new byte[] { 0x60, 0x94, 0x3C, 0x4C });
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
                this._result = this.NoteMessageReader.ReadNoteMessage(this.Reader, 4);
            this._orpheeNoteMessageList = this.NoteMessageReader.OrpheeNoteMessageList;
            this._actualVelocity = (byte) this.NoteMessageReader.OrpheeNoteMessageList[0].Velocity;
        }

        [Test]
        public void ResultShouldBeTrue()
        {
            Assert.IsTrue(this._result);
        }

        [Test]
        public void NoteMessageReaderOrpheeNoteMessageListShouldNotBeNull()
        {
            Assert.IsNotNull(this._orpheeNoteMessageList);
        }

        [Test]
        public void NoteMessageReaderOrpheeNoteMessageListShouldNotBeEmpty()
        {
            Assert.IsNotEmpty(this._orpheeNoteMessageList);
        }

        [Test]
        public void NoteMessageReaderVelocityShouldBeEqualToExpectedVelocity()
        {
            Assert.AreEqual(this._expectedVelocity, this._actualVelocity);
        }
    }

    [TestFixture]
    public class ItShouldReturnTrueIfTheDeltaTimeIsOnePointLowerThanTheSetLimit : WhenNoteMessageReader
    {
        private bool _result;
        private IList<IOrpheeNoteMessage> _orpheeNoteMessageList;

        [SetUp]
        public void Init()
        {
            ReWriteTheFile(new byte[] { 0xFF, 0xFF, 0x7F, 0x94, 0x3C, 0x4C });
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
                this._result = this.NoteMessageReader.ReadNoteMessage(this.Reader, 6);
            this._orpheeNoteMessageList = this.NoteMessageReader.OrpheeNoteMessageList;
        }

        [Test]
        public void ResultShouldBeTrue()
        {
            Assert.IsTrue(this._result);
        }

        [Test]
        public void NoteMessageReaderOrpheeNoteMessageListShouldNotBeNull()
        {
            Assert.IsNotNull(this._orpheeNoteMessageList);
        }

        [Test]
        public void NoteMessageReaderOrpheeNoteMessageListShouldNotBeEmpty()
        {
            Assert.IsNotEmpty(this._orpheeNoteMessageList);
        }
    }

    [TestFixture]
    public class ItShouldReturnFalseIfTheDeltaTimeIsHigherThanTheSetLimit : WhenNoteMessageReader
    {
        private bool _result;
        private IList<IOrpheeNoteMessage> _orpheeNoteMessageList;

        [SetUp]
        public void Init()
        {
            ReWriteTheFile(new byte[] { 0x81, 0x80, 0x80, 0x00, 0x94, 0x3C, 0x4C });
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
                this._result = this.NoteMessageReader.ReadNoteMessage(this.Reader, 7);
            this._orpheeNoteMessageList = this.NoteMessageReader.OrpheeNoteMessageList;
        }

        [Test]
        public void ResultShouldBeFalse()
        {
            Assert.IsFalse(this._result);
        }

        [Test]
        public void NoteMessageReaderOrpheeNoteMessageListShouldNotBeNull()
        {
            Assert.IsNotNull(this._orpheeNoteMessageList);
        }

        [Test]
        public void NoteMessageReaderOrpheeNoteMessageListShouldBeEmpty()
        {
            Assert.IsEmpty(this._orpheeNoteMessageList);
        }
    }

    [TestFixture]
    public class ItShouldReturnFalseIfTheBinaryReaderIsNull : WhenNoteMessageReader
    {
        private bool _result;
        private IList<IOrpheeNoteMessage> _orpheeNoteMessageList;

        [SetUp]
        public void Init()
        {
            this._result = this.NoteMessageReader.ReadNoteMessage(this.Reader, 0);
            this._orpheeNoteMessageList = this.NoteMessageReader.OrpheeNoteMessageList;
        }

        [Test]
        public void ResultShouldBeFalse()
        {
            Assert.IsFalse(this._result);
        }

        [Test]
        public void NoteMessageReaderOrpheeNoteMessageListShouldNotBeNull()
        {
            Assert.IsNotNull(this._orpheeNoteMessageList);
        }

        [Test]
        public void NoteMessageReaderOrpheeNoteMessageListShouldBeEmpty()
        {
            Assert.IsEmpty(this._orpheeNoteMessageList);
        }
    }

    [TestFixture]
    public class ItShouldReturnFalseIfTheBinaryReaderIsEmpty : WhenNoteMessageReader
    {
        private bool _result;
        private IList<IOrpheeNoteMessage> _orpheeNoteMessageList;

        [SetUp]
        public void Init()
        {
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
            {
                for (var iterator = this.Reader.BaseStream.Length; iterator >= 1; iterator--)
                    this.Reader.ReadByte();
                this._result = this.NoteMessageReader.ReadNoteMessage(this.Reader, (uint)(this.Reader.BaseStream.Length - this.Reader.BaseStream.Position));
            }
            this._orpheeNoteMessageList = this.NoteMessageReader.OrpheeNoteMessageList;
        }

        [Test]
        public void ResultShouldBeFalse()
        {
            Assert.IsFalse(this._result);
        }

        [Test]
        public void NoteMessageReaderOrpheeNoteMessageListShouldNotBeNull()
        {
            Assert.IsNotNull(this._orpheeNoteMessageList);
        }

        [Test]
        public void NoteMessageReaderOrpheeNoteMessageListShouldBeEmpty()
        {
            Assert.IsEmpty(this._orpheeNoteMessageList);
        }
    }

    [TestFixture]
    public class ItShouldReturnFalseIfTheBinaryReaderIsLessThanFourBytes : WhenNoteMessageReader
    {
        private bool _result;
        private IList<IOrpheeNoteMessage> _orpheeNoteMessageList;

        [SetUp]
        public void Init()
        {
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
            {
                this.Reader.ReadByte();
                this._result = this.NoteMessageReader.ReadNoteMessage(this.Reader, (uint)(this.Reader.BaseStream.Length - this.Reader.BaseStream.Position));
            }
            this._orpheeNoteMessageList = this.NoteMessageReader.OrpheeNoteMessageList;
        }

        [Test]
        public void ResultShouldBeFalse()
        {
            Assert.IsFalse(this._result);
        }

        [Test]
        public void NoteMessageReaderOrpheeNoteMessageListShouldNotBeNull()
        {
            Assert.IsNotNull(this._orpheeNoteMessageList);
        }

        [Test]
        public void NoteMessageReaderOrpheeNoteMessageListShouldBeEmpty()
        {
            Assert.IsEmpty(this._orpheeNoteMessageList);
        }
    }
}
