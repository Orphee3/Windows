using System;
using System.IO;
using MidiDotNet.ExportModule;
using MidiDotNet.ExportModule.Interfaces;
using NUnit.Framework;
using Orphee.CreationShared;
using Orphee.CreationShared.Interfaces;

namespace MidiDotNet.ExportModuleUnitTests.ExportModuleTests.TimeSignatureMessageWriterTests
{
    public class WhenTimeSignatureMessageWriterTestIsCalled : ExportModuleTestsBase
    {
        protected ITimeSignatureMessageWriter TimeSignatureMessageWriter;
        protected IPlayerParameters PlayerParameters;

        public WhenTimeSignatureMessageWriterTestIsCalled()
        {
            this.PlayerParameters = new PlayerParameters()
            {
                TimeSignatureNominator = 4,
                TimeSignatureDenominator = 4,
                TimeSignatureClocksPerBeat= 24,
                TimeSignatureNumberOf32ThNotePerBeat = 4
            };
            this.TimeSignatureMessageWriter = new TimeSignatureMessageWriter();
            var result = InitializeFile("TimeSignatureTests.test").Result;
        }
    }

    public class ItShouldWriteTheTimeSignatureMessage : WhenTimeSignatureMessageWriterTestIsCalled
    {
        private byte _deltaTime;
        private byte _metaEventCode;
        private byte _messageCode;
        private byte _numerator;
        private int _denominator;
        private byte _clocksPerBeat;
        private int _numberOf32ThNotesPerBeat;

        [SetUp]
        public void Init()
        {
            using (this.Writer = new BinaryWriter(this.File.OpenStreamForWriteAsync().Result))
            {
                this.TimeSignatureMessageWriter.WriteTimeSignatureMessage(this.Writer, this.PlayerParameters);
            }
            ReadTimeSignatureMessageFromUnitTestFile();
        }

        private void ReadTimeSignatureMessageFromUnitTestFile()
        {
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
            {
                this._deltaTime = this.Reader.ReadByte();
                this._metaEventCode = this.Reader.ReadByte();
                this._messageCode = this.Reader.ReadByte();
                var dataSize = this.Reader.ReadByte();
                var data = new byte[dataSize];

                for (var pos = 0; pos < dataSize; pos++)
                    data[pos] = Reader.ReadByte();

                this._numerator = data[0];
                this._denominator =  (int)Math.Pow(2, Convert.ToDouble(data[1]));
                this._clocksPerBeat = data[2];
                this._numberOf32ThNotesPerBeat = data[3] / 2;
            }
        }

        [Test]
        public void TimeSignatureDeltaTimeShouldBeZero()
        {
            Assert.AreEqual(0, this._deltaTime);
        }

        [Test]
        public void TimeSignatureMetaEventCodeShouldBeAsExpected()
        {
            Assert.AreEqual(0xFF, this._metaEventCode);
        }

        [Test]
        public void TimeSignatureMessageCodeShouldBeAsExpected()
        {
            Assert.AreEqual(0x58, this._messageCode);
        }

        [Test]
        public void TimeSignatureNominatorShouldBeEqualToPlayerParametersTimeSignatureNumerator()
        {
            Assert.AreEqual(this.PlayerParameters.TimeSignatureNominator, this._numerator);
        }

        [Test]
        public void TimeSignatureDenominatorShouldBeEqualToPlayerParametersTimeSignatureDenominator()
        {
            Assert.AreEqual(this.PlayerParameters.TimeSignatureDenominator, this._denominator);
        }

        [Test]
        public void TimeSignatureNumberOf32ThNotesPerBeatShouldBeEqualToPlayerParametersNumberOf32TnNotesPerBeat()
        {
            Assert.AreEqual(this.PlayerParameters.TimeSignatureNumberOf32ThNotePerBeat, this._numberOf32ThNotesPerBeat);
        }

        [Test]
        public void TimeSignatureClocksPerBeatShouldBeEqualToPlayerParametersClocksPerBeat()
        {
            Assert.AreEqual(this.PlayerParameters.TimeSignatureClocksPerBeat, this._clocksPerBeat);
        }
    }
}
