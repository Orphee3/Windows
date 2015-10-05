using System.IO;
using Midi;
using MidiDotNet.ExportModule;
using MidiDotNet.ExportModule.Interfaces;
using MidiDotNet.ImportModule;
using MidiDotNet.ImportModule.Interfaces;
using MidiDotNet.Shared.Interfaces;
using Moq;
using NUnit.Framework;
using Orphee.CreationShared;
using Orphee.CreationShared.Interfaces;
using Orphee.RestApiManagement.Senders.Interfaces;

namespace MidiDotNet.ExportModuleUnitTests.ExportModuleTests.NoteMessageWriterTests
{
    public class WhenNoteMessageWriterIsCalled : ExportModuleTestsBase
    {
        protected INoteMessageWriter NoteMessageWriter;
        protected IOrpheeFile OrpheeFile;
        protected IOrpheeTrack OrpheeTrack;
        protected IDeltaTimeReader DeltaTimeRetriever;
        protected Mock<IProgramChangeMessageWriter> ProgramChangeMessageWriterMock;
        protected Mock<IEndOfTrackMessageWriter> EndOfTrackMessageWriterMock;
        protected Mock<ISwapManager> SwapManagerMock;
        protected Mock<ITrackHeaderWriter> TrackHeaderWriterMock;
        protected Mock<INoteMessageWriter> NoteMessageWriterMock;
        protected Mock<IFileHeaderWriter> FileHeaderWriterMock;
        protected Mock<IFileUploader> FileUploaderMock;
        protected IOrpheeFileExporter OrpheeFileExporter;

        public WhenNoteMessageWriterIsCalled()
        {
            this.FileUploaderMock = new Mock<IFileUploader>();
            this.FileHeaderWriterMock = new Mock<IFileHeaderWriter>();
            this.NoteMessageWriterMock = new Mock<INoteMessageWriter>();
            this.SwapManagerMock = new Mock<ISwapManager>();
            this.TrackHeaderWriterMock = new Mock<ITrackHeaderWriter>();
            this.OrpheeFileExporter = new OrpheeFileExporter(this.FileHeaderWriterMock.Object, this.TrackHeaderWriterMock.Object, this.NoteMessageWriterMock.Object, this.FileUploaderMock.Object);
            this.EndOfTrackMessageWriterMock = new Mock<IEndOfTrackMessageWriter>();
            this.ProgramChangeMessageWriterMock = new Mock<IProgramChangeMessageWriter>();
            this.DeltaTimeRetriever = new DeltaTimeReader();
            this.OrpheeFile = new OrpheeFile();
            this.OrpheeTrack = new OrpheeTrack(0, Channel.Channel5)
            {
                CurrentInstrument = Instrument.AcousticGuitarSteel,
            };
            this.OrpheeTrack.NoteMap[0][0].IsChecked = true;
            this.OrpheeTrack.NoteMap[0][0].ColumnIndex = 0;
            this.OrpheeTrack.NoteMap[0][0].LineIndex = 0;
            this.OrpheeTrack.NoteMap[0][0].Note = Note.C4;
            this.OrpheeFile.AddNewTrack(this.OrpheeTrack);
            this.NoteMessageWriter = new NoteMessageWriter(this.ProgramChangeMessageWriterMock.Object, this.EndOfTrackMessageWriterMock.Object);
            var result = InitializeFile("NoteMessageTests.test").Result;
        }
    }

    [TestFixture]
    public class ItShouldWriteTheNoteMessageAsExpected : WhenNoteMessageWriterIsCalled
    {
        private IOrpheeNoteMessage _orpheeNoteOnMessage;
        private IOrpheeNoteMessage _orpheeNoteOffMessage;
        private int _noteOnMessageDeltaTime;
        private byte _noteOnMessageCode;
        private byte _noteOnMessageChannel;
        private byte _noteOnMessageNote;
        private byte _noteOnMessageVelocity;
        private int _noteOffMessageDeltaTime;
        private byte _noteOffMessageCode;
        private byte _noteOffMessageChannel;
        private byte _noteOffMessageNote;
        private byte _noteOffMessageVelocity;

        [SetUp]
        public void Init()
        {
            this.OrpheeFileExporter.ConvertTracksNoteMapToOrpheeNoteMessageList(this.OrpheeFile);
            this._orpheeNoteOnMessage = this.OrpheeTrack.OrpheeNoteMessageList[0];
            this._orpheeNoteOffMessage = this.OrpheeTrack.OrpheeNoteMessageList[1];
            using (this.Writer = new BinaryWriter(this.File.OpenStreamForWriteAsync().Result))
            {
              foreach (var orpheeTrack in this.OrpheeFile.OrpheeTrackList)
                this.NoteMessageWriter.WriteNoteMessages(Writer, orpheeTrack.OrpheeNoteMessageList, (int)orpheeTrack.Channel, orpheeTrack.CurrentInstrument);
            }
            ReadNoteOnMessageFromFile();
        }

        private void ReadNoteOnMessageFromFile()
        {
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
            {
                this._noteOnMessageDeltaTime = this.DeltaTimeRetriever.GetIntDeltaTime(this.Reader);
                var messageCodeByte = this.Reader.ReadByte();
                this._noteOnMessageCode = (byte) (messageCodeByte & 0x90);
                this._noteOnMessageChannel = (byte) (messageCodeByte ^ 0x90);
                this._noteOnMessageNote = this.Reader.ReadByte();
                this._noteOnMessageVelocity = this.Reader.ReadByte();
                ReadNoteOffFromFile();
            }
        }

        private void ReadNoteOffFromFile()
        {
            this._noteOffMessageDeltaTime = this.DeltaTimeRetriever.GetIntDeltaTime(this.Reader);
            var messageCodeByte = this.Reader.ReadByte();
            this._noteOffMessageCode = (byte)(messageCodeByte & 0x80);
            this._noteOffMessageChannel = (byte)(messageCodeByte ^ 0x80);
            this._noteOffMessageNote = this.Reader.ReadByte();
            this._noteOffMessageVelocity = this.Reader.ReadByte();
        }

        [Test]
        public void NoteOnMessageDeltaTimeShouldBeEqualToOrpheeNoteOnMessageDeltaTime()
        {
            Assert.AreEqual(this._noteOnMessageDeltaTime, this._orpheeNoteOnMessage.DeltaTime);
        }

        [Test]
        public void NoteOnMessageMessageCodeShouldBeEqualToOrpheeNoteOnMessageEventCode()
        {
            Assert.AreEqual(this._noteOnMessageCode, this._orpheeNoteOnMessage.MessageCode);
        }

        [Test]
        public void NoteOnMessageChannelShouldBeEqualToOrpheeNoteOnMessageChannel()
        {
            Assert.AreEqual(this._noteOnMessageChannel, this._orpheeNoteOnMessage.Channel);
        }

        [Test]
        public void NoteOnMessageNoteShouldBeEqualToOrpheeNoteOnMessageNote()
        {
            Assert.AreEqual(this._noteOnMessageNote, (int)this._orpheeNoteOnMessage.Note);
        }

        [Test]
        public void NoteOnMessageVelocityShouldBeEqualToOrpheeNoteOnMessageVelocity()
        {
            Assert.AreEqual(this._noteOnMessageVelocity, this._orpheeNoteOnMessage.Velocity);
        }

        [Test]
        public void NoteOffMessageDeltaTimeShouldBeEqualToOrpheeNoteOffMessageDeltaTime()
        {
            Assert.AreEqual(this._noteOffMessageDeltaTime, this._orpheeNoteOffMessage.DeltaTime);
        }

        [Test]
        public void NoteOffMessageMessageCodeShouldBeEqualToOrpheeNoteOffMessageEventCode()
        {
            Assert.AreEqual(this._noteOffMessageCode, this._orpheeNoteOffMessage.MessageCode);
        }

        [Test]
        public void NoteOffMessageChannelShouldBeEqualToOrpheeNoteOffMessageChannel()
        {
            Assert.AreEqual(this._noteOffMessageChannel, this._orpheeNoteOffMessage.Channel);
        }

        [Test]
        public void NoteOffMessageNoteShouldBeEqualToOrpheeNoteOffMessageNote()
        {
            Assert.AreEqual(this._noteOffMessageNote, (int)this._orpheeNoteOffMessage.Note);
        }

        [Test]
        public void NoteOffMessageVelocityShouldBeEqualToOrpheeNoteOffMessageVelocity()
        {
            Assert.AreEqual(this._noteOffMessageVelocity, this._orpheeNoteOffMessage.Velocity);
        }
    }
}
