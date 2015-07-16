using System.IO;
using MidiDotNet.ImportModule;
using MidiDotNet.ImportModule.Interfaces;
using NUnit.Framework;

namespace MidiDotNet.ImportModuleUnitTests.ImportModuleTests.TempoMessageReaderTests
{
    public class WhenTempoMessageReaderIsCalled : ImportModuleTestsBase
    {
        protected ITempoMessageReader TempoMessageReader;

        public WhenTempoMessageReaderIsCalled()
        {
            this.TempoMessageReader = new TempoMessageReader();
            var result = GetFile("TempoMessageTest.test").Result;
        }
    }

    [TestFixture]
    public class ItShouldReturnTrueWhenTheTempoMessageIsCorrect : WhenTempoMessageReaderIsCalled
    {
        private bool _result;

        [SetUp]
        public void Init()
        {
            ReWriteTheFile(new byte[] {0x00, 0xFF, 0x51, 0x03, 0x07, 0xA1, 0x20});
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
                this._result = this.TempoMessageReader.ReadTempoMessage(this.Reader);
        }

        [Test]
        public void ResultShouldBeTrue()
        {
            Assert.IsTrue(this._result);
        }
    }

    [TestFixture]
    public class ItShouldHaveATempoValueOfAHundredAndTwenty : WhenTempoMessageReaderIsCalled
    {
        private bool _result;
        private int _expectedTempo;

        [SetUp]
        public void Init()
        {
            this._expectedTempo = 120;
            ReWriteTheFile(new byte[] { 0x00, 0xFF, 0x51, 0x03, 0x07, 0xA1, 0x20 });
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
                this._result = this.TempoMessageReader.ReadTempoMessage(this.Reader);
        }

        [Test]
        public void ResultShouldBeTrue()
        {
            Assert.IsTrue(this._result);
        }

        [Test]
        public void TempoMessageReaderTempoShouldBeEqualToExpectedTempo()
        {
            Assert.AreEqual(this._expectedTempo, this.TempoMessageReader.Tempo);
        }
    }

    [TestFixture]
    public class ItShouldReturnFalseWhenTheTempoMessageDeltaTimeIsNotEqualToZero : WhenTempoMessageReaderIsCalled
    {
        private bool _result;

        [SetUp]
        public void Init()
        {
            ReWriteTheFile(new byte[] { 0x01, 0xFF, 0x51, 0x03, 0x07, 0xA1, 0x20 });
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
                this._result = this.TempoMessageReader.ReadTempoMessage(this.Reader);
        }

        [Test]
        public void ResultShouldBeFalse()
        {
            Assert.IsFalse(this._result);
        }
    }

    [TestFixture]
    public class ItShouldReturnFalseWhenTheTempoMessageMetaMessageIsNoteEqualToFf : WhenTempoMessageReaderIsCalled
    {
        private bool _result;

        [SetUp]
        public void Init()
        {
            ReWriteTheFile(new byte[] { 0x01, 0xF4, 0x51, 0x03, 0x07, 0xA1, 0x20 });
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
                this._result = this.TempoMessageReader.ReadTempoMessage(this.Reader);
        }

        [Test]
        public void ResultShouldBeFalse()
        {
            Assert.IsFalse(this._result);
        }
    }

    [TestFixture]
    public class ItShouldReturnFalseWhenTheTempoMessageCodeIsNotAsExpected : WhenTempoMessageReaderIsCalled
    {
        private bool _result;

        [SetUp]
        public void Init()
        {
            ReWriteTheFile(new byte[] { 0x01, 0xFF, 0x50, 0x03, 0x07, 0xA1, 0x20 });
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
                this._result = this.TempoMessageReader.ReadTempoMessage(this.Reader);
        }

        [Test]
        public void ResultShouldBeFalse()
        {
            Assert.IsFalse(this._result);
        }
    }

    [TestFixture]
    public class ItShouldReturnFalseWhenTheTempoMessageTempoIsLessThanForty : WhenTempoMessageReaderIsCalled
    {
        private bool _result;

        [SetUp]
        public void Init()
        {
            ReWriteTheFile(new byte[] { 0x01, 0xFF, 0x50, 0x03, 0x16, 0xE3, 0x59 });
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
                this._result = this.TempoMessageReader.ReadTempoMessage(this.Reader);
        }

        [Test]
        public void ResultShouldBeFalse()
        {
            Assert.IsFalse(this._result);
        }
    }

    [TestFixture]
    public class ItShouldReturnFalseWhenTheTempoMessageTempoIsMoreThanFourHundred: WhenTempoMessageReaderIsCalled
    {
        private bool _result;

        [SetUp]
        public void Init()
        {
            ReWriteTheFile(new byte[] { 0x01, 0xFF, 0x50, 0x03, 0x02, 0x49, 0xF1 });
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
                this._result = this.TempoMessageReader.ReadTempoMessage(this.Reader);
        }

        [Test]
        public void ResultShouldBeFalse()
        {
            Assert.IsFalse(this._result);
        }
    }

    [TestFixture]
    public class ItShouldReturnFalseWhenTheBinaryReaderIsNull : WhenTempoMessageReaderIsCalled
    {
        private bool _result;

        [SetUp]
        public void Init()
        {
            this._result = this.TempoMessageReader.ReadTempoMessage(this.Reader);
        }

        [Test]
        public void ResultShouldBeFalse()
        {
            Assert.IsFalse(this._result);
        }
    }

    [TestFixture]
    public class ItShouldReturnFalseWhenTheBinaryReaderIsEmpty : WhenTempoMessageReaderIsCalled
    {
        private bool _result;

        [SetUp]
        public void Init()
        {
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
            {
                for (var iterator = this.Reader.BaseStream.Length; iterator >= 1; iterator--)
                    this.Reader.ReadByte();
                this._result = this.TempoMessageReader.ReadTempoMessage(this.Reader);
            }
        }

        [Test]
        public void ResultShouldBeFalse()
        {
            Assert.IsFalse(this._result);
        }
    }

    [TestFixture]
    public class ItShouldReturnFalseWhenTheBinaryReaderContainsLessThanSevenBytes : WhenTempoMessageReaderIsCalled
    {
        private bool _result;

        [SetUp]
        public void Init()
        {
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
            {
                this.Reader.ReadByte();
                this._result = this.TempoMessageReader.ReadTempoMessage(this.Reader);
            }
        }

        [Test]
        public void ResultShouldBeFalse()
        {
            Assert.IsFalse(this._result);
        }
    }
}
