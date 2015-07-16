using System.IO;
using MidiDotNet.ImportModule;
using MidiDotNet.ImportModule.Interfaces;
using MidiDotNet.Shared;
using NUnit.Framework;

namespace MidiDotNet.ImportModuleUnitTests.ImportModuleTests.TrackHeaderReaderTests
{
    public class WhenTrackHeaderReaderIsCalled : ImportModuleTestsBase
    {
        protected ITrackHeaderReader TrackHeaderReader;

        public WhenTrackHeaderReaderIsCalled()
        {
            this.TrackHeaderReader = new TrackHeaderReader(new SwapManager());
            var result = GetFile("TrackHeaderTests.test").Result;
        }
    }

    [TestFixture]
    public class ItShouldReturnTrueWhenTheTrackHeaderIsCorrect : WhenTrackHeaderReaderIsCalled
    {
        private bool _result;

        [SetUp]
        public void Init()
        {
            ReWriteTheFile(new byte[] {0x4D, 0x54, 0x72, 0x6B, 0x00, 0x00, 0x00, 0x16});
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
                this._result = this.TrackHeaderReader.ReadTrackHeader(this.Reader);
        }

        [Test]
        public void ResultShouldBeTrue()
        {
            Assert.IsTrue(this._result);
        }
    }

    [TestFixture]
    public class ItShouldReturnFalseWhenTheTrackHeaderIsNotAsExpected : WhenTrackHeaderReaderIsCalled
    {
        private bool _result;

        [SetUp]
        public void Init()
        {
            ReWriteTheFile(new byte[] { 0x4A, 0x54, 0x72, 0x6B, 0x00, 0x00, 0x00, 0x16 });
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
                this._result = this.TrackHeaderReader.ReadTrackHeader(this.Reader);
        }

        [Test]
        public void ResultShouldBeFalse()
        {
            Assert.IsFalse(this._result);
        }
    }

    [TestFixture]
    public class ItShouldReturnFalseWhenTheBinaryReaderIsNull : WhenTrackHeaderReaderIsCalled
    {
        private bool _result;

        [SetUp]
        public void Init()
        {
            this._result = this.TrackHeaderReader.ReadTrackHeader(this.Reader);
        }

        [Test]
        public void ResultShouldBeFalse()
        {
            Assert.IsFalse(this._result);
        }
    }

    [TestFixture]
    public class ItShouldReturnFalseWhenTheBinaryReaderIsEmpty : WhenTrackHeaderReaderIsCalled
    {
        private bool _result;

        [SetUp]
        public void Init()
        {
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
            {
                for (var iterator = this.Reader.BaseStream.Length; iterator >= 1; iterator--)
                    this.Reader.ReadByte();
                this._result = this.TrackHeaderReader.ReadTrackHeader(this.Reader);
            }
        }

        [Test]
        public void ResultShouldBeFalse()
        {
            Assert.IsFalse(this._result);
        }
    }

    [TestFixture]
    public class ItShouldReturnFalseWhenTheBinaryReaderContainsLessThanEightBytes: WhenTrackHeaderReaderIsCalled
    {
        private bool _result;

        [SetUp]
        public void Init()
        {
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
            {
                this.Reader.ReadByte();
                this._result = this.TrackHeaderReader.ReadTrackHeader(this.Reader);
            }
        }

        [Test]
        public void ResultShouldBeFalse()
        {
            Assert.IsFalse(this._result);
        }
    }
}
