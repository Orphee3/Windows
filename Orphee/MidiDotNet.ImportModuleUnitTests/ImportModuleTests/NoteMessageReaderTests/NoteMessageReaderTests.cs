using System.IO;
using MidiDotNet.ImportModule;
using MidiDotNet.ImportModule.Interfaces;
using NUnit.Framework;

namespace MidiDotNet.ImportModuleUnitTests.ImportModuleTests.NoteMessageReaderTests
{
    public class WhenNoteMessageReader : ImportModuleTestsBase
    {
        protected INoteMessageReader NoteMessageReader;

        public WhenNoteMessageReader()
        {
            this.NoteMessageReader = new NoteMessageReader(new DeltaTimeReader());
            var result = GetFile("NoteMessageTests.test").Result;
        }
    }

    [TestFixture]
    public class ItShouldReturnTrueWhenTheNoteOnMessageIsCorrect : WhenNoteMessageReader
    {
        private bool _result;

        [SetUp]
        public void Init()
        {
            ReWriteTheFile(new byte[] {0x00, 0x94, 0x3C, 0x4C});
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
                this._result = this.NoteMessageReader.ReadNoteMessage(this.Reader);
        }

        [Test]
        public void ResultShouldBeTrue()
        {
            Assert.IsTrue(this._result);
        }
    }

    [TestFixture]
    public class ItShouldReturnTrueWhenTheNoteOffMessageIsCorrect : WhenNoteMessageReader
    {
        private bool _result;

        [SetUp]
        public void Init()
        {
            ReWriteTheFile(new byte[] { 0x00, 0x84, 0x3C, 0x4C });
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
                this._result = this.NoteMessageReader.ReadNoteMessage(this.Reader);
        }

        [Test]
        public void ResultShouldBeTrue()
        {
            Assert.IsTrue(this._result);
        }
    }

    [TestFixture]
    public class ItShouldHaveTheExpectedNoteIndex : WhenNoteMessageReader
    {
        private bool _result;
        private byte _expectedNoteIndex;

        [SetUp]
        public void Init()
        {
            this._expectedNoteIndex = 0x3C;
            ReWriteTheFile(new byte[] { 0x00, 0x94, 0x3C, 0x4C });
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
                this._result = this.NoteMessageReader.ReadNoteMessage(this.Reader);
        }

        [Test]
        public void ResultShouldBeTrue()
        {
            Assert.IsTrue(this._result);
        }

        [Test]
        public void NoteMessageReaderNoteIndexShouldBeEqualToExpectednoteIndex()
        {
            Assert.AreEqual(this._expectedNoteIndex, this.NoteMessageReader.NoteIndex);
        }
    }

    [TestFixture]
    public class ItShouldReturnFalseIfTheNoteIndexIsHigherThanTheSetLimit : WhenNoteMessageReader
    {
        private bool _result;

        [SetUp]
        public void Init()
        {
            ReWriteTheFile(new byte[] { 0x00, 0x94, 0x80, 0x4C });
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
                this._result = this.NoteMessageReader.ReadNoteMessage(this.Reader);
        }

        [Test]
        public void ResultShouldBeFalse()
        {
            Assert.IsFalse(this._result);
        }
    }

    [TestFixture]
    public class ItShouldReturnTrueIfTheNoteIndexIsOnePointLowerThanTheSetLimit : WhenNoteMessageReader
    {
        private bool _result;

        [SetUp]
        public void Init()
        {
            ReWriteTheFile(new byte[] { 0x00, 0x94, 0x7F, 0x4C });
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
                this._result = this.NoteMessageReader.ReadNoteMessage(this.Reader);
        }

        [Test]
        public void ResultShouldBeTrue()
        {
            Assert.IsTrue(this._result);
        }
    }


    [TestFixture]
    public class ItShouldHaveTheExpectedDeltaTime : WhenNoteMessageReader
    {
        private bool _result;
        private byte _expectedDeltaTime;

        [SetUp]
        public void Init()
        {
            this._expectedDeltaTime = 96;
            ReWriteTheFile(new byte[] { 0x60, 0x94, 0x3C, 0x4C });
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
                this._result = this.NoteMessageReader.ReadNoteMessage(this.Reader);
        }

        [Test]
        public void ResultShouldBeTrue()
        {
            Assert.IsTrue(this._result);
        }

        [Test]
        public void NoteMessageReaderDeltaTimeShouldBeEqualToExpectedDeltaTime()
        {
            Assert.AreEqual(this._expectedDeltaTime, this.NoteMessageReader.DeltaTime);
        }
    }

    [TestFixture]
    public class ItShouldReturnFalseIfTheDeltaTimeIsHigherThanTheSetLimit : WhenNoteMessageReader
    {
        private bool _result;

        [SetUp]
        public void Init()
        {
            ReWriteTheFile(new byte[] { 0x81, 0x80, 0x80, 0x00, 0x94, 0x3C, 0x4C });
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
                this._result = this.NoteMessageReader.ReadNoteMessage(this.Reader);
        }

        [Test]
        public void ResultShouldBeFalse()
        {
            Assert.IsFalse(this._result);
        }
    }

    [TestFixture]
    public class ItShouldReturnTrueIfTheDeltaTimeIsOnePointLowerThanTheSettedLimit : WhenNoteMessageReader
    {
        private bool _result;

        [SetUp]
        public void Init()
        {
            ReWriteTheFile(new byte[] { 0xFF, 0xFF, 0x7F, 0x94, 0x3C, 0x4C });
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
                this._result = this.NoteMessageReader.ReadNoteMessage(this.Reader);
        }

        [Test]
        public void ResultShouldBeTrue()
        {
            Assert.IsTrue(this._result);
        }
    }

    [TestFixture]
    public class ItShouldHaveTheExpectedVelocity: WhenNoteMessageReader
    {
        private bool _result;
        private byte _expectedVelocity;

        [SetUp]
        public void Init()
        {
            this._expectedVelocity = 76;
            ReWriteTheFile(new byte[] { 0x60, 0x94, 0x3C, 0x4C });
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
                this._result = this.NoteMessageReader.ReadNoteMessage(this.Reader);
        }

        [Test]
        public void ResultShouldBeTrue()
        {
            Assert.IsTrue(this._result);
        }

        [Test]
        public void NoteMessageReaderVelocityShouldBeEqualToExpectedVelocity()
        {
            Assert.AreEqual(this._expectedVelocity, this.NoteMessageReader.Velocity);
        }
    }

    [TestFixture]
    public class ItShouldReturnFalseIfTheBinaryReaderIsNull : WhenNoteMessageReader
    {
        private bool _result;

        [SetUp]
        public void Init()
        {
            this._result = this.NoteMessageReader.ReadNoteMessage(this.Reader);
        }

        [Test]
        public void ResultShouldBeFalse()
        {
            Assert.IsFalse(this._result);
        }
    }

    [TestFixture]
    public class ItShouldReturnFalseIfTheBinaryReaderIsEmpty : WhenNoteMessageReader
    {
        private bool _result;

        [SetUp]
        public void Init()
        {
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
            {
                for (var iterator = this.Reader.BaseStream.Length; iterator >= 1; iterator--)
                    this.Reader.ReadByte();
                this._result = this.NoteMessageReader.ReadNoteMessage(this.Reader);
            }
        }

        [Test]
        public void ResultShouldBeFalse()
        {
            Assert.IsFalse(this._result);
        }
    }

    [TestFixture]
    public class ItShouldReturnFalseIfTheBinaryReaderIsLessThanFourBytes : WhenNoteMessageReader
    {
        private bool _result;

        [SetUp]
        public void Init()
        {
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
            {
                this.Reader.ReadByte();
                this._result = this.NoteMessageReader.ReadNoteMessage(this.Reader);
            }
        }

        [Test]
        public void ResultShouldBeFalse()
        {
            Assert.IsFalse(this._result);
        }
    }
}
