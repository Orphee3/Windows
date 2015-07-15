using System.IO;
using MidiDotNet.ImportModule;
using MidiDotNet.ImportModule.Interfaces;
using NUnit.Framework;

namespace Orphee.UnitTests.ImportModuleTests.DeltaTimeRetrieverTests
{
    public class WhenDeltaTimeRetrieverIsCalled : ImportModuleTestsBase
    {
        protected IDeltaTimeRetriever DeltaTimeRetriever;

        public WhenDeltaTimeRetrieverIsCalled()
        {
            this.DeltaTimeRetriever = new DeltaTimeRetriever();
            var result = InitializeFile("DeltaTimeRetrieverTests.test").Result;
        }
    }

    [TestFixture]
    public class ItShouldRetrieveTheExpectedResultForAOneByteDeltaTime : WhenDeltaTimeRetrieverIsCalled
    {
        private int _expectedResult;
        private int _actualResult;

        [SetUp]
        public void Init()
        {
            this._expectedResult = 127;
            using (this.Writer = new BinaryWriter(this.File.OpenStreamForWriteAsync().Result))
            {
                this.Writer.Write((byte) 0x7F);
            }
            ReadDeltaTimeFromFile();
        }

        private void ReadDeltaTimeFromFile()
        {
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
                this._actualResult = this.DeltaTimeRetriever.GetIntDeltaTime(this.Reader);
        }

        [Test]
        public void ActualResultShouldBeEqualToExpectedResult()
        {
            Assert.AreEqual(this._expectedResult, this._expectedResult);
        }
    }

    [TestFixture]
    public class ItShouldRetrieveTheExpectedResultForATwoBytesDeltaTime : WhenDeltaTimeRetrieverIsCalled
    {
        private int _expectedResult;
        private int _actualResult;

        [SetUp]
        public void Init()
        {
            this._expectedResult = 255;
            using (this.Writer = new BinaryWriter(this.File.OpenStreamForWriteAsync().Result))
            {
                this.Writer.Write((byte) 0x81);
                this.Writer.Write((byte) 0x7F);
            }
            ReadDeltaTimeFromFile();
        }

        private void ReadDeltaTimeFromFile()
        {
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
                this._actualResult = this.DeltaTimeRetriever.GetIntDeltaTime(this.Reader);
        }

        [Test]
        public void ActualResultShouldBeEqualToExpectedResult()
        {
            Assert.AreEqual(this._expectedResult, this._expectedResult);
        }
    }

    [TestFixture]
    public class ItShouldRetrieveTheExpectedResultForAThreeBytesDeltaTime : WhenDeltaTimeRetrieverIsCalled
    {
        private int _expectedResult;
        private int _actualResult;

        [SetUp]
        public void Init()
        {
            this._expectedResult = 32768;
            using (this.Writer = new BinaryWriter(this.File.OpenStreamForWriteAsync().Result))
            {
                this.Writer.Write((byte) 0x82);
                this.Writer.Write((byte) 0x80);
                this.Writer.Write((byte) 0x00);
            }
            ReadDeltaTimeFromFile();
        }

        private void ReadDeltaTimeFromFile()
        {
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
                this._actualResult = this.DeltaTimeRetriever.GetIntDeltaTime(this.Reader);
        }

        [Test]
        public void ActualResultShouldBeEqualToExpectedResult()
        {
            Assert.AreEqual(this._expectedResult, this._expectedResult);
        }
    }
}
