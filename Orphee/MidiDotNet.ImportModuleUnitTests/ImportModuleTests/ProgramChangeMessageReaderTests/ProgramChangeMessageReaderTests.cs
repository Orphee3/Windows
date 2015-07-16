using System.IO;
using MidiDotNet.ImportModule;
using MidiDotNet.ImportModule.Interfaces;
using NUnit.Framework;

namespace MidiDotNet.ImportModuleUnitTests.ImportModuleTests.ProgramChangeMessageReaderTests
{
    public class WhenProgramChangeMessageReaderIsCalled : ImportModuleTestsBase
    {
        protected IProgramChangeMessageReader ProgramChangeMessageReder;

        public WhenProgramChangeMessageReaderIsCalled()
        {
            this.ProgramChangeMessageReder = new ProgramChangeMessageReader();
            var result = GetFile("ProgramChangeTests.test").Result;
        }
    }

    [TestFixture]
    public class ItShouldReturnTrueWhenTheProgamChangeMessageIsCorrect : WhenProgramChangeMessageReaderIsCalled
    {
        private bool _result;

        [SetUp]
        public void Init()
        {
            ReWriteTheFile(new byte[]{0x00, 0xC0, 0x00});
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
                this._result = this.ProgramChangeMessageReder.ReadProgramChangeMessage(this.Reader);
        }

        [Test]
        public void ResultShouldBeTrue()
        {
            Assert.IsTrue(this._result);
        }
    }

    [TestFixture]
    public class ItShouldReturnFalseWhenTheProgamChangeMessageDeltaTimeIsNotEqualToZero : WhenProgramChangeMessageReaderIsCalled
    {
        private bool _result;

        [SetUp]
        public void Init()
        {
            ReWriteTheFile(new byte[] { 0x01, 0xC5, 0x00 });
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
                this._result = this.ProgramChangeMessageReder.ReadProgramChangeMessage(this.Reader);
        }

        [Test]
        public void ResultShouldBeFalse()
        {
            Assert.IsFalse(this._result);
        }
    }

    [TestFixture]
    public class ItShouldReturnFalseWhenTheProgamChangeMessageIsNoCorrect : WhenProgramChangeMessageReaderIsCalled
    {
        private bool _result;

        [SetUp]
        public void Init()
        {
            ReWriteTheFile(new byte[] { 0x01, 0xF0, 0x00 });
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
                this._result = this.ProgramChangeMessageReder.ReadProgramChangeMessage(this.Reader);
        }

        [Test]
        public void ResultShouldBeFalse()
        {
            Assert.IsFalse(this._result);
        }
    }

    [TestFixture]
    public class ItShouldReturnFalseWhenTheProgamChangeMessageInstrumentIndexIsMoreThan127 : WhenProgramChangeMessageReaderIsCalled
    {
        private bool _result;

        [SetUp]
        public void Init()
        {
            ReWriteTheFile(new byte[] { 0x01, 0xF0, 0x80 });
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
                this._result = this.ProgramChangeMessageReder.ReadProgramChangeMessage(this.Reader);
        }

        [Test]
        public void ResultShouldBeFalse()
        {
            Assert.IsFalse(this._result);
        }
    }

    [TestFixture]
    public class ItShouldReturnFalseWhenReaderIsNull : WhenProgramChangeMessageReaderIsCalled
    {
        private bool _result;

        [SetUp]
        public void Init()
        {
            this._result = this.ProgramChangeMessageReder.ReadProgramChangeMessage(this.Reader);
        }

        [Test]
        public void ResultShouldBeFalse()
        {
            Assert.IsFalse(this._result);
        }
    }

    [TestFixture]
    public class ItShouldReturnFalseWhenReaderIsContainsLessThanThreeBytes : WhenProgramChangeMessageReaderIsCalled
    {
        private bool _result;

        [SetUp]
        public void Init()
        {
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
            {
                this.Reader.ReadByte();
                this._result = this.ProgramChangeMessageReder.ReadProgramChangeMessage(this.Reader);
            }
        }

        [Test]
        public void ResultShouldBeFalse()
        {
            Assert.IsFalse(this._result);
        }
    }

    [TestFixture]
    public class ItShouldReturnFalseWhenReaderIsEmpty : WhenProgramChangeMessageReaderIsCalled
    {
        private bool _result;

        [SetUp]
        public void Init()
        {
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
            {
                for (var iterator = this.Reader.BaseStream.Length; iterator >= 1; iterator--)
                    this.Reader.ReadByte();
                this._result = this.ProgramChangeMessageReder.ReadProgramChangeMessage(this.Reader);
            }
        }

        [Test]
        public void ResultShouldBeFalse()
        {
            Assert.IsFalse(this._result);
        }
    }
}
