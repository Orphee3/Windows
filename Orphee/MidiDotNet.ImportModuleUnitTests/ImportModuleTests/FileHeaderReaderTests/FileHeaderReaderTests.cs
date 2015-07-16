using System.IO;
using MidiDotNet.ImportModule;
using MidiDotNet.ImportModule.Interfaces;
using MidiDotNet.Shared;
using NUnit.Framework;

namespace MidiDotNet.ImportModuleUnitTests.ImportModuleTests.FileHeaderReaderTests
{
    public class WhenFileHeaderReaderIsCalled : ImportModuleTestsBase
    {
        protected IFileHeaderReader FileHeaderReader;

        public WhenFileHeaderReaderIsCalled()
        {
            var swapManager = new SwapManager();
            this.FileHeaderReader = new FileHeaderReader(swapManager);
            var result = GetFile("FileHeaderWriterTests.test").Result;
        }
    }

    [TestFixture]
    public class ItShouldReturnTrueWhenTheFileHeaderIsCorrect : WhenFileHeaderReaderIsCalled
    {
        private bool _result;
        [SetUp]
        public void Init()
        {
            ReWriteTheFile(new byte[] {0x4D, 0x54, 0x68, 0x64, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00, 0x01, 0x00, 0x3C});
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
                this._result = this.FileHeaderReader.ReadFileHeader(this.Reader);
        }

        [Test]
        public void ResultShouldBeTrue()
        {
            Assert.IsTrue(this._result);
        }
    }

    [TestFixture]
    public class ItShouldReturnFakseWhenTheFileHeaderMThdIsFalse : WhenFileHeaderReaderIsCalled
    {
        private bool _result;
        [SetUp]
        public void Init()
        {
            ReWriteTheFile(new byte[] { 0x4D, 0x0, 0x68, 0x64, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00, 0x01, 0x00, 0x3C });
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
                this._result = this.FileHeaderReader.ReadFileHeader(this.Reader);
        }

        [Test]
        public void ResultShouldBeFalse()
        {
            Assert.IsFalse(this._result);
        }
    }

    [TestFixture]
    public class ItShouldReturnFakseWhenTheFileHeaderLengthIsFalse : WhenFileHeaderReaderIsCalled
    {
        private bool _result;
        [SetUp]
        public void Init()
        {
            ReWriteTheFile(new byte[] { 0x4D, 0x54, 0x68, 0x64, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x01, 0x00, 0x3C });
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
                this._result = this.FileHeaderReader.ReadFileHeader(this.Reader);
        }

        [Test]
        public void ResultShouldBeFalse()
        {
            Assert.IsFalse(this._result);
        }
    }

    [TestFixture]
    public class ItShouldReturnFakseWhenTheFileHeaderTypeIsMoreThanTwo : WhenFileHeaderReaderIsCalled
    {
        private bool _result;
        [SetUp]
        public void Init()
        {
            ReWriteTheFile(new byte[] { 0x4D, 0x54, 0x68, 0x64, 0x00, 0x00, 0x00, 0x06, 0x00, 0x03, 0x00, 0x00, 0x00, 0x3C });
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
                this._result = this.FileHeaderReader.ReadFileHeader(this.Reader);
        }

        [Test]
        public void ResultShouldBeFalse()
        {
            Assert.IsFalse(this._result);
        }
    }

    [TestFixture]
    public class ItShouldReturnFalseIfTheBinaryReaderIsEmptyNull : WhenFileHeaderReaderIsCalled
    {
        private bool _result;

        [SetUp]
        public void Init()
        {
           this._result = this.FileHeaderReader.ReadFileHeader(this.Reader);
        }

        [Test]
        public void ResultShouldBeFalse()
        {
            Assert.IsFalse(this._result);
        }
    }
}
