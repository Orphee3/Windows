using MidiDotNet.ImportModule;
using MidiDotNet.ImportModule.Interfaces;
using NUnit.Framework;

namespace MidiDotNet.ImportModuleUnitTests.ImportModuleTests.FileHeaderReaderTests
{
    public class WhenFileHeaderReaderIsCalled : ImportModuleTestsBase
    {
        protected IFileHeaderReader FileHeaderReader;

        public WhenFileHeaderReaderIsCalled()
        {
            this.FileHeaderReader = new FileHeaderReader();
            var result = GetFile("FileHeaderWriterTests.test");
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
