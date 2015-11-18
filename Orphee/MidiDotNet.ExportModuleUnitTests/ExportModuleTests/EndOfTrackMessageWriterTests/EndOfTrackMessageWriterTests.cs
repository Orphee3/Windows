using System.IO;
using MidiDotNet.ExportModule;
using MidiDotNet.ExportModule.Interfaces;
using NUnit.Framework;

namespace MidiDotNet.ExportModuleUnitTests.ExportModuleTests.EndOfTrackMessageWriterTests
{
    public class WhenEndOfTrackMessageWriterIsCalled : ExportModuleTestsBase
    {
        protected IEndOfTrackMessageWriter EndOfTrackMessageWriter;

        public WhenEndOfTrackMessageWriterIsCalled()
        {
            this.EndOfTrackMessageWriter = new EndOfTrackMessageWriter();
            var result = InitializeFile("EndOfTrackMessageWriterTests.test").Result;
        }
    }

    public class ItShouldWriteTheEndOfTrackMessageProperly : WhenEndOfTrackMessageWriterIsCalled
    {
        private byte _endOfTrackMessageDeltaTime;
        private byte _metaMessageCode;
        private byte _endOfTrackMessageCode;
        private byte _endOfTrackMessageData;

        [SetUp]
        public void Init()
        {
            using (this.Writer = new BinaryWriter(this.File.OpenStreamForWriteAsync().Result))
                this.EndOfTrackMessageWriter.WriteEndOfTrackMessage(this.Writer);
            ReadEndOfTrackMessage();
        }

        private void ReadEndOfTrackMessage()
        {
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
            {
                this._endOfTrackMessageDeltaTime = this.Reader.ReadByte();
                this._metaMessageCode = this.Reader.ReadByte();
                this._endOfTrackMessageCode = this.Reader.ReadByte();
                this._endOfTrackMessageData = this.Reader.ReadByte();
            }
        }

        [Test]
        public void EndOfTrackMessageDeltaTimeShouldBeEqualToZero()
        {
            Assert.AreEqual(0, this._endOfTrackMessageDeltaTime);
        }

        [Test]
        public void EndOfTrackMetaMessageCodeShoudBeEqualToFf()
        {
            Assert.AreEqual(0xFF, this._metaMessageCode);
        }

        [Test]
        public void EndOfTrackMessageCodeShouldBeAsExpected()
        {
            Assert.AreEqual(0x2F, this._endOfTrackMessageCode);
        }

        [Test]
        public void EndOfTrackMessageDataShouldBeEqualToZero()
        {
            Assert.AreEqual(0, this._endOfTrackMessageData);
        }
    }
}
