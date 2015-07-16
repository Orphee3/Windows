using System.IO;
using MidiDotNet.ImportModule;
using MidiDotNet.ImportModule.Interfaces;
using NUnit.Framework;

namespace MidiDotNet.ImportModuleUnitTests.ImportModuleTests.DeltaTimeReaderTests
{
    public class WhenDeltaTimeRetrieverIsCalled : ImportModuleTestsBase
    {
        protected IDeltaTimeReader DeltaTimeRetriever;

        public WhenDeltaTimeRetrieverIsCalled()
        {
            this.DeltaTimeRetriever = new DeltaTimeReader();
            var result = InitializeFile("DeltaTimeRetrieverTests.test").Result;
        }
    }

    [TestFixture]
    public class ItShouldReadTheExpectedResultForAOneByteDeltaTime : WhenDeltaTimeRetrieverIsCalled
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
            Assert.AreEqual(this._expectedResult, this._actualResult);
        }
    }

    [TestFixture]
    public class ItShouldReadTheExpectedResultForATwoBytesDeltaTime : WhenDeltaTimeRetrieverIsCalled
    {
        private int _expectedResult;
        private int _actualResult;

        [SetUp]
        public void Init()
        {
            this._expectedResult = 16383;
            using (this.Writer = new BinaryWriter(this.File.OpenStreamForWriteAsync().Result))
            {
                this.Writer.Write((byte) 0xFF);
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
            Assert.AreEqual(this._expectedResult, this._actualResult);
        }
    }

    [TestFixture]
    public class ItShouldReadTheExpectedResultForAThreeBytesDeltaTime : WhenDeltaTimeRetrieverIsCalled
    {
        private int _expectedResult;
        private int _actualResult;

        [SetUp]
        public void Init()
        {
            this._expectedResult = 2097151;
            using (this.Writer = new BinaryWriter(this.File.OpenStreamForWriteAsync().Result))
            {
                this.Writer.Write((byte) 0xFF);
                this.Writer.Write((byte) 0xFF);
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
            Assert.AreEqual(this._expectedResult, this._actualResult);
        }
    }
}
