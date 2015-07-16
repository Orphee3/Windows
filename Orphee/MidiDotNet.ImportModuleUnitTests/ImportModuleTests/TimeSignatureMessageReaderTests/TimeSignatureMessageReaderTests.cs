using System.IO;
using MidiDotNet.ImportModule;
using MidiDotNet.ImportModule.Interfaces;
using NUnit.Framework;

namespace MidiDotNet.ImportModuleUnitTests.ImportModuleTests.TimeSignatureMessageReaderTests
{
    public class WhenTimeSignatureMessageReaderIsCalled : ImportModuleTestsBase
    {
        protected ITimeSignatureMessageReader TimeSignatureMessageReader;

        public WhenTimeSignatureMessageReaderIsCalled()
        {
            this.TimeSignatureMessageReader = new TimeSignatureMessageReader();
            var result = GetFile("TimeSignatureTests.test").Result;
        }
    }

    [TestFixture]
    public class ItShouldReturnTrueWhenTheTimeSignatureMessageIsCorrect : WhenTimeSignatureMessageReaderIsCalled
    {
        private bool _result;

        [SetUp]
        public void Init()
        {
            ReWriteTheFile(new byte[] {0x00, 0xFF, 0x58, 0x04, 0x04, 0x02, 0x18, 0x08});
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
                this._result = this.TimeSignatureMessageReader.ReadTimeSignatureMessage(this.Reader);
        }

        [Test]
        public void ResultShouldBeTrue()
        {
            Assert.IsTrue(this._result);
        }
    }

    [TestFixture]
    public class ItShouldHaveTheCorrectValues : WhenTimeSignatureMessageReaderIsCalled
    {
        private bool _result;
        private int _expectedNominator;
        private int _expectedDenominator;
        private int _expectedClocksPerBeat;
        private int _expectedNumberOf32ThNotePerBeat;

        [SetUp]
        public void Init()
        {
            this._expectedNominator = 4;
            this._expectedDenominator = 4;
            this._expectedClocksPerBeat = 24;
            this._expectedNumberOf32ThNotePerBeat = 8;
            ReWriteTheFile(new byte[] { 0x00, 0xFF, 0x58, 0x04, 0x04, 0x02, 0x18, 0x08 });
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
                this._result = this.TimeSignatureMessageReader.ReadTimeSignatureMessage(this.Reader);
        }

        [Test]
        public void ResultShouldBeTrue()
        {
            Assert.IsTrue(this._result);
        }

        [Test]
        public void TimeSignatureMessageReaderNominatorShouldBeEqualToExpectedNominator()
        {
            Assert.AreEqual(this._expectedNominator, this.TimeSignatureMessageReader.Nominator);
        }

        [Test]
        public void TimeSignatureMessageReaderDenominatorShouldBeEqualToExpectedDenominator()
        {
            Assert.AreEqual(this._expectedDenominator, this.TimeSignatureMessageReader.Denominator);
        }

        [Test]
        public void TimeSignatureMessageReaderClocksPerBeatShouldBeEqualToExpectedClocksPerBeat()
        {
            Assert.AreEqual(this._expectedClocksPerBeat, this.TimeSignatureMessageReader.ClocksPerBeat);
        }

        [Test]
        public void TimeSignatureMessageReaderNumberOf32ThNotePerBeatShouldBeEqualToExpectedNumberOf32ThNotePerBeat()
        {
            Assert.AreEqual(this._expectedNumberOf32ThNotePerBeat, this.TimeSignatureMessageReader.NumberOf32ThNotePerBeat);
        }
    }

    [TestFixture]
    public class ItShouldReturnFalseWhenTheTimeSignatureMessageDeltaTimeIsNotEqualToZero : WhenTimeSignatureMessageReaderIsCalled
    {
        private bool _result;

        [SetUp]
        public void Init()
        {
            ReWriteTheFile(new byte[] { 0x01, 0xFF, 0x58, 0x04, 0x04, 0x02, 0x18, 0x08 });
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
                this._result = this.TimeSignatureMessageReader.ReadTimeSignatureMessage(this.Reader);
        }

        [Test]
        public void ResultShouldBeFalse()
        {
            Assert.IsFalse(this._result);
        }
    }

    [TestFixture]
    public class ItShouldReturnFalseWhenTheTimeSignatureMessageMetaCodeIsNotFf : WhenTimeSignatureMessageReaderIsCalled
    {
        private bool _result;

        [SetUp]
        public void Init()
        {
            ReWriteTheFile(new byte[] { 0x00, 0x0F, 0x58, 0x04, 0x04, 0x02, 0x18, 0x08 });
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
                this._result = this.TimeSignatureMessageReader.ReadTimeSignatureMessage(this.Reader);
        }

        [Test]
        public void ResultShouldBeFalse()
        {
            Assert.IsFalse(this._result);
        }
    }

    [TestFixture]
    public class ItShouldReturnFalseWhenTheTimeSignatureMessageCodeIsNotAsExpected : WhenTimeSignatureMessageReaderIsCalled
    {
        private bool _result;

        [SetUp]
        public void Init()
        {
            ReWriteTheFile(new byte[] { 0x00, 0xFF, 0x50, 0x04, 0x04, 0x02, 0x18, 0x08 });
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
                this._result = this.TimeSignatureMessageReader.ReadTimeSignatureMessage(this.Reader);
        }

        [Test]
        public void ResultShouldBeFalse()
        {
            Assert.IsFalse(this._result);
        }
    }

    [TestFixture]
    public class ItShouldReturnFalseWhenTheReaderIsNull : WhenTimeSignatureMessageReaderIsCalled
    {
        private bool _result;

        [SetUp]
        public void Init()
        {
            this._result = this.TimeSignatureMessageReader.ReadTimeSignatureMessage(this.Reader);
        }

        [Test]
        public void ResultShouldBeFalse()
        {
            Assert.IsFalse(this._result);
        }
    }

    [TestFixture]
    public class ItShouldReturnFalseWhenTheReaderIsEmpty : WhenTimeSignatureMessageReaderIsCalled
    {
        private bool _result;

        [SetUp]
        public void Init()
        {
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
            {
                for (var iterator = this.Reader.BaseStream.Length; iterator >= 1; iterator--)
                    this.Reader.ReadByte();
                this._result = this.TimeSignatureMessageReader.ReadTimeSignatureMessage(this.Reader);
            }
        }

        [Test]
        public void ResultShouldBeFalse()
        {
            Assert.IsFalse(this._result);
        }
    }

    [TestFixture]
    public class ItShouldReturnFalseWhenTheReaderContainsLessThanEightBytes : WhenTimeSignatureMessageReaderIsCalled
    {
        private bool _result;

        [SetUp]
        public void Init()
        {
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
            {
                for (var iterator = this.Reader.BaseStream.Length; iterator >= 1; iterator--)
                    this.Reader.ReadByte();
                this._result = this.TimeSignatureMessageReader.ReadTimeSignatureMessage(this.Reader);
            }
        }

        [Test]
        public void ResultShouldBeFalse()
        {
            Assert.IsFalse(this._result);
        }
    }
}
