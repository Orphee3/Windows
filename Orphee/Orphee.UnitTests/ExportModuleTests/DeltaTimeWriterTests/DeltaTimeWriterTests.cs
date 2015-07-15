using System.IO;
using MidiDotNet.ExportModule;
using MidiDotNet.ExportModule.Interfaces;
using NUnit.Framework;

namespace Orphee.UnitTests.ExportModuleTests.DeltaTimeWriterTests
{
    public class WhenDeltaTimeWriterIsCalled : ExportModuleTestsBase
    {
        protected IDeltaTimeWriter DeltaTimeWriter;

        public WhenDeltaTimeWriterIsCalled()
        {
            this.DeltaTimeWriter = new DeltaTimeWriter();
            var result = InitializeFile("DeltaTimeTests.test").Result;
        }
    }

    [TestFixture]
    public class ItShouldWriteTheOneByteLongDeltaTime : WhenDeltaTimeWriterIsCalled
    {
        private byte[] _expectedResult;
        private byte[] _actualResult;

        [SetUp]
        public void Init()
        {
            this._expectedResult = new byte[]
            {
                0x7F,
            };
            using (this.Writer = new BinaryWriter(this.File.OpenStreamForWriteAsync().Result))
                this.DeltaTimeWriter.WriteDeltaTime(Writer, 0x7F);
            ReadDeltaTimeFromFile();
        }

        private void ReadDeltaTimeFromFile()
        {
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
            {
                this._actualResult = new byte[this.Reader.BaseStream.Length];
                for (int iterator = 0; iterator < this.Reader.BaseStream.Length; iterator++)
                    this._actualResult[iterator] = this.Reader.ReadByte();
            }
        }

        [Test]
        public void ActualResultShouldBeAsLongAsExpectedResult()
        {
            Assert.AreEqual(this._expectedResult.Length, this._actualResult.Length);
        }

        [Test]
        public void ActualResultFirstByteShouldContainTheSameByteValueContainedInExpectedResultFirstByte()
        {
            Assert.AreEqual(this._expectedResult[0], this._actualResult[0]);
        }
    }

    [TestFixture]
    public class ItShouldWriteTheTwoBytesLongDeltaTime : WhenDeltaTimeWriterIsCalled
    {
        private byte[] _expectedResult;
        private byte[] _actualResult;

        [SetUp]
        public void Init()
        {
            this._expectedResult = new byte[]
            {
                0x81,
                0x7F,
            };
            using (this.Writer = new BinaryWriter(this.File.OpenStreamForWriteAsync().Result))
                this.DeltaTimeWriter.WriteDeltaTime(Writer, 255);
            ReadDeltaTimeFromFile();
        }

        private void ReadDeltaTimeFromFile()
        {
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
            {
                this._actualResult = new byte[this.Reader.BaseStream.Length];
                for (int iterator = 0; iterator < this.Reader.BaseStream.Length; iterator++)
                    this._actualResult[iterator] = this.Reader.ReadByte();
            }
        }

        [Test]
        public void ActualResultShouldBeAsLongAsExpectedResult()
        {
            Assert.AreEqual(this._expectedResult.Length, this._actualResult.Length);
        }

        [Test]
        public void ActualResultFirstByteShouldContainTheSameByteValueContainedInExpectedResultFirstByte()
        {
            Assert.AreEqual(this._expectedResult[0], this._actualResult[0]);
        }

        [Test]
        public void ActualResultSecondByteShouldContainTheSameByteValueContainedInExpectedResultSecondByte()
        {
            Assert.AreEqual(this._expectedResult[1], this._actualResult[1]);
        }
    }

    [TestFixture]
    public class ItShouldWriteTheThreeBytesLongDeltaTime : WhenDeltaTimeWriterIsCalled
    {
        private byte[] _expectedResult;
        private byte[] _actualResult;

        [SetUp]
        public void Init()
        {
            this._expectedResult = new byte[]
            {
                0x82,
                0x80,
                0x00
            };
            using (this.Writer = new BinaryWriter(this.File.OpenStreamForWriteAsync().Result))
                this.DeltaTimeWriter.WriteDeltaTime(Writer, 32768);
            ReadDeltaTimeFromFile();
        }

        private void ReadDeltaTimeFromFile()
        {
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
            {
                this._actualResult = new byte[this.Reader.BaseStream.Length];
                for (int iterator = 0; iterator < this.Reader.BaseStream.Length; iterator++)
                    this._actualResult[iterator] = this.Reader.ReadByte();
            }
        }

        [Test]
        public void ActualResultShouldBeAsLongAsExpectedResult()
        {
            Assert.AreEqual(this._expectedResult.Length, this._actualResult.Length);
        }

        [Test]
        public void ActualResultFirstByteShouldContainTheSameByteValueContainedInExpectedResultFirstByte()
        {
            Assert.AreEqual(this._expectedResult[0], this._actualResult[0]);
        }

        [Test]
        public void ActualResultSecondByteShouldContainTheSameByteValueContainedInExpectedResultSecondByte()
        {
            Assert.AreEqual(this._expectedResult[1], this._actualResult[1]);
        }

        [Test]
        public void ActualResultThirdByteShouldContainTheSameByteValueContainedInExpectedResultThirdByte()
        {
            Assert.AreEqual(this._expectedResult[2], this._actualResult[2]);
        }
    }
}
