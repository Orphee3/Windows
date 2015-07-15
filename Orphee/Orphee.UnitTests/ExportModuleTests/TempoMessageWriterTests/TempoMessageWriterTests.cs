using System;
using System.IO;
using MidiDotNet.ExportModule;
using MidiDotNet.ExportModule.Interfaces;
using NUnit.Framework;
using Orphee.CreationShared;
using Orphee.CreationShared.Interfaces;

namespace Orphee.UnitTests.ExportModuleTests.TempoMessageWriterTests
{
    public class WhenTempoMessageWriterIsCalled : ExportModuleTestsBase
    {
        protected ITempoMessageWriter TempoMessageWriter;
        protected IPlayerParameters PlayerParameters;

        public WhenTempoMessageWriterIsCalled()
        {
            this.PlayerParameters = new PlayerParameters()
            {
                Tempo = 120
            };
            this.TempoMessageWriter = new TempoMessageWriter();
            var result = InitializeFile("TempoMessageTest.test").Result;
        }

    }

    [TestFixture]
    public class ItShouldWriteTheTempoMessage : WhenTempoMessageWriterIsCalled
    {
        private byte _tempoDeltaTime;
        private byte _metaEventCode;
        private byte _tempoMessageCode;
        private int _tempo;

        [SetUp]
        public void Init()
        {
            using (this.Writer = new BinaryWriter(this.File.OpenStreamForWriteAsync().Result))
            {
                this.TempoMessageWriter.WriteTempoMessage(this.Writer, this.PlayerParameters.Tempo);
            }
            ReadTempoMessageFromFile();
        }

        private void ReadTempoMessageFromFile()
        {
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
            {
                this._tempoDeltaTime = this.Reader.ReadByte();
                this._metaEventCode = this.Reader.ReadByte();
                this._tempoMessageCode = this.Reader.ReadByte();
                RetrieveTempo();
               
            }
        }

        private void RetrieveTempo()
        {
            var dataSize = this.Reader.ReadByte();
            var data = new byte[dataSize + 1];

            for (var pos = 0; pos < 4 - dataSize; pos++)
                data[pos] = 0;
            for (var pos = 4 - dataSize; pos < 4; pos++)
                data[pos] = this.Reader.ReadByte();

            Array.Reverse(data);
            this._tempo = 60000000 / BitConverter.ToInt32(data, 0);
        }

        [Test]
        public void TempoDeltaTimeShouldBeEqualToZero()
        {
            Assert.AreEqual(0, this._tempoDeltaTime);        
        }

        [Test]
        public void TempoMetaEventCodeShoulBeAsExpected()
        {
            Assert.AreEqual(0xFF, this._metaEventCode);
        }

        [Test]
        public void TempoMessageCodeShouldBeAsExpected()
        {
            Assert.AreEqual(0x51, this._tempoMessageCode);
        }

        [Test]
        public void TempoShouldBeEqualToPlayerParametersTempo()
        {
            Assert.AreEqual(this.PlayerParameters.Tempo, this._tempo);
        }

    }
}
