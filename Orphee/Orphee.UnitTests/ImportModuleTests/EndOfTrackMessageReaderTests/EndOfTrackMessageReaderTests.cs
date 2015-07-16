using System.IO;
using MidiDotNet.ImportModule;
using MidiDotNet.ImportModule.Interfaces;
using NUnit.Framework;

namespace Orphee.UnitTests.ImportModuleTests.EndOfTrackMessageReaderTests
{
    public class WhenEndOfTrackReaderIsCalled : ImportModuleTestsBase
    {
        protected IEndOfTrackMessageReader EndOfTrackMessageReader;

        public WhenEndOfTrackReaderIsCalled()
        {
            this.EndOfTrackMessageReader = new EndOfTrackMessageReader();
            var result = GetFile("EndOfTrackMessageWriterTests.test");
        }
    }

    [TestFixture]
    public class IsShouldReturnFalseIfTheTrackIsEmpty : WhenEndOfTrackReaderIsCalled
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
    public class IsShouldReadTheEndOfTrackMessageAsExpected : WhenEndOfTrackReaderIsCalled
    {
        private bool _result;

        [SetUp]
        public void Init()
        {
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
                this._result = this.EndOfTrackMessageReader.ReadEndOfTrackMessage(this.Reader);
        }

        [Test]
        public void ResultShouldBeTrue()
        {
            Assert.IsTrue(this._result);
        }
    }
}
