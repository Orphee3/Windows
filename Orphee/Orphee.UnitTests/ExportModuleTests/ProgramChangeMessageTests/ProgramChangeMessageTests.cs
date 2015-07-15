using System.IO;
using Midi;
using MidiDotNet.ExportModule;
using MidiDotNet.ExportModule.Interfaces;
using NUnit.Framework;
using Orphee.CreationShared;
using Orphee.CreationShared.Interfaces;

namespace Orphee.UnitTests.ExportModuleTests.ProgramChangeMessageTests
{
    public class WhenProgramChangeMessageIsCalled : ExportModuleTestsBase
    {
        protected IProgramChangeMessageWriter ProgramChangeMessageWriter;
        protected IOrpheeTrack OrpheeTrack;

        public WhenProgramChangeMessageIsCalled()
        {
            this.OrpheeTrack = new OrpheeTrack(0, Channel.Channel1)
            {
                CurrentInstrument = Instrument.Banjo
            };
            this.ProgramChangeMessageWriter = new ProgramChangeMessageWriter();
            var result = InitializeFile("ProgramChangeTests.orph").Result;
        }
    }

    [TestFixture]
    public class ItShouldWriteTHeProgramChangeMessage : WhenProgramChangeMessageIsCalled
    {
        private byte _deltaTime;
        private byte _eventCode;
        private byte _channel;
        private byte _instrumentIndex;

        [SetUp]
        public void Init()
        {
            using (this.Writer = new BinaryWriter(this.File.OpenStreamForWriteAsync().Result))
            {
                this.ProgramChangeMessageWriter.WriteProgramChangeMessage(this.Writer, (int)this.OrpheeTrack.Channel, this.OrpheeTrack.CurrentInstrument);
            }
            ReadProgramChangeMessage();
        }

        private void ReadProgramChangeMessage()
        {
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
            {
                this._deltaTime = this.Reader.ReadByte();
                var eventCodeByte = this.Reader.ReadByte();
                this._eventCode = (byte) (eventCodeByte & 0xC0);
                this._channel = (byte) (eventCodeByte ^ 0xC0);
                this._instrumentIndex = this.Reader.ReadByte();
            }
        }

        [Test]
        public void ProgramChangeMessageDeltaTimeShouldBeZero()
        {
            Assert.AreEqual(0, this._deltaTime);
        }

        [Test]
        public void ProgramChangeEventCodeShouldBeAsExpected()
        {
            Assert.AreEqual(0xC0, this._eventCode);
        }

        [Test]
        public void ProgramChangeMessageChannelShouldBeEqualToOrpheeTrackChannel()
        {
            Assert.AreEqual((int)this.OrpheeTrack.Channel, this._channel);
        }

        [Test]
        public void ProgramChangerMessageInstrumentShouldBeEqualToOrpheeTrackInstrument()
        {
            var instrumentManager = new InstrumentManager();
            Assert.AreEqual(this.OrpheeTrack.CurrentInstrument, instrumentManager.InstrumentList[this._instrumentIndex]);
        }
    }
}
