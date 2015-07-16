using System.IO;
using MidiDotNet.ImportModule;
using MidiDotNet.ImportModule.Interfaces;
using NUnit.Framework;

namespace MidiDotNet.ImportModuleUnitTests.ImportModuleTests.EndOfTrackMessageReaderTests
{
    public class WhenEndOfTrackReaderIsCalled : ImportModuleTestsBase
    {
        protected IEndOfTrackMessageReader EndOfTrackMessageReader;

        public WhenEndOfTrackReaderIsCalled()
        {
            this.EndOfTrackMessageReader = new EndOfTrackMessageReader();
            var result = GetFile("EndOfTrackMessageWriterTests.test").Result;
        }
    }

    [TestFixture]
    public class IsShouldReadTheEndOfTrackMessageAsExpected : WhenEndOfTrackReaderIsCalled
    {
        private bool _result;

        [SetUp]
        public void Init()
        {
            ReWriteTheFile(new byte[] {0x00, 0xFF, 0x2F, 0x00});
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
                this._result = this.EndOfTrackMessageReader.ReadEndOfTrackMessage(this.Reader);
        }

        [Test]
        public void ResultShouldBeTrue()
        {
            Assert.IsTrue(this._result);
        }
    }

    [TestFixture]
    public class IsShouldReturnFalseIfTheBinaryReaderIsNull : WhenEndOfTrackReaderIsCalled
    {
        private bool _result;

        [SetUp]
        public void Init()
        {
            this._result = this.EndOfTrackMessageReader.ReadEndOfTrackMessage(this.Reader);
        }

        [Test]
        public void ResultShouldBeFalse()
        {
            Assert.IsFalse(this._result);
        }
    }

    [TestFixture]
    public class IsShouldReturnFalseIfTheBinaryReaderIsEmpty : WhenEndOfTrackReaderIsCalled
    {
        private bool _result;

        [SetUp]
        public void Init()
        {
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
            {
                for (var iterator = this.Reader.BaseStream.Length; iterator >= 1; iterator--)
                    this.Reader.ReadByte();
                this._result = this.EndOfTrackMessageReader.ReadEndOfTrackMessage(this.Reader);
            }
        }

        [Test]
        public void ResultShouldBeFalse()
        {
            Assert.IsFalse(this._result);
        }
    }

    [TestFixture]
    public class IsShouldReturnFalseIfTheBinaryReaderIsLessThanFourBytesLong : WhenEndOfTrackReaderIsCalled
    {
        private bool _result;

        [SetUp]
        public void Init()
        {
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
            {
                this.Reader.ReadByte();
                this._result = this.EndOfTrackMessageReader.ReadEndOfTrackMessage(this.Reader);
            }
        }

        [Test]
        public void ResultShouldBeFalse()
        {
            Assert.IsFalse(this._result);
        }
    }

    [TestFixture]
    public class IsShouldReturnFalseIfTheEndOfTrackMessageDeltaTimeIsNotAsExpected : WhenEndOfTrackReaderIsCalled
    {
        private bool _result;

        [SetUp]
        public void Init()
        {
            ReWriteTheFile(new byte[] { 0x01, 0xFF, 0x2F, 0x00 });
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
                this._result = this.EndOfTrackMessageReader.ReadEndOfTrackMessage(this.Reader);
        }

        [Test]
        public void ResultShouldBeFalse()
        {
            Assert.IsFalse(this._result);
        }
    }

    [TestFixture]
    public class IsShouldReturnFalseIfTheEndOfTrackMetaCodeIsNotAsExpected : WhenEndOfTrackReaderIsCalled
    {
        private bool _result;

        [SetUp]
        public void Init()
        {
            ReWriteTheFile(new byte[] { 0x00, 0xF0, 0x2F, 0x00 });
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
                this._result = this.EndOfTrackMessageReader.ReadEndOfTrackMessage(this.Reader);
        }

        [Test]
        public void ResultShouldBeFalse()
        {
            Assert.IsFalse(this._result);
        }
    }

    [TestFixture]
    public class IsShouldReturnFalseIfTheEndOfTrackMessageCodeIsNotAsExpected : WhenEndOfTrackReaderIsCalled
    {
        private bool _result;

        [SetUp]
        public void Init()
        {
            ReWriteTheFile(new byte[] { 0x00, 0xFF, 0x20, 0x00 });
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
                this._result = this.EndOfTrackMessageReader.ReadEndOfTrackMessage(this.Reader);
        }

        [Test]
        public void ResultShouldBeFalse()
        {
            Assert.IsFalse(this._result);
        }
    }

    [TestFixture]
    public class IsShouldReturnFalseIfTheEndOfTrackDataIsNotAsExpected : WhenEndOfTrackReaderIsCalled
    {
        private bool _result;

        [SetUp]
        public void Init()
        {
            ReWriteTheFile(new byte[] { 0x00, 0xFF, 0x2F, 0x04 });
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
                this._result = this.EndOfTrackMessageReader.ReadEndOfTrackMessage(this.Reader);
        }

        [Test]
        public void ResultShouldBeFalse()
        {
            Assert.IsFalse(this._result);
        }
    }
}
