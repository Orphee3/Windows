using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Core;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.UI.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
using Assert = NUnit.Framework.Assert;

namespace MidiDotNet.ExportModuleUnitTests.ExportModuleTests.NoteMessageWriterTests
{
    public class UITestMethodAttribute : TestMethodAttribute
    {
        public override TestResult[] Execute(ITestMethod testMethod)
        {
            if (testMethod.GetAttributes<AsyncStateMachineAttribute>(false).Length != 0)
            {
                throw new NotSupportedException("async TestMethod with UITestMethodAttribute are not supported. Either remove async or use TestMethodAttribute.");
            }

            TestResult result = null;

            CoreApplication.MainView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                result = testMethod.Invoke(new object[0]);
            }).AsTask().GetAwaiter().GetResult();

            return new TestResult[] { result };
        }
    }
    public class WhenNoteMessageWriterIsCalled : ExportModuleTestsBase
    {
        protected INoteMessageWriter NoteMessageWriter;
        protected IOrpheeFile OrpheeFile;
        protected IDeltaTimeReader DeltaTimeRetriever;
        protected Mock<IProgramChangeMessageWriter> ProgramChangeMessageWriterMock;
        protected Mock<IEndOfTrackMessageWriter> EndOfTrackMessageWriterMock;
        protected Mock<ISwapManager> SwapManagerMock;
        protected Mock<IChunckWriters> ChunkWritersMock;
        protected Mock<IFilePickerManager> FilePickerManagerMock;
        protected Mock<IFileUploader> FileUploaderMock;
        protected Mock<IOrpheeTrackUI> OrpheeTrackUIMock;
        protected INoteMapGenerator NoteMapGenerator;
        protected IOrpheeFileExporter OrpheeFileExporter;

        public WhenNoteMessageWriterIsCalled()
        {
            this.ChunkWritersMock = new Mock<IChunckWriters>();
            this.FilePickerManagerMock = new Mock<IFilePickerManager>();
            this.NoteMapGenerator = new NoteMapManager();
            this.FileUploaderMock = new Mock<IFileUploader>();
            this.SwapManagerMock = new Mock<ISwapManager>();
            this.OrpheeFileExporter = new OrpheeFileExporter(this.ChunkWritersMock.Object, this.FileUploaderMock.Object, this.NoteMapGenerator, this.FilePickerManagerMock.Object);
            this.EndOfTrackMessageWriterMock = new Mock<IEndOfTrackMessageWriter>();
            this.ProgramChangeMessageWriterMock = new Mock<IProgramChangeMessageWriter>();
            this.OrpheeTrackUIMock = new Mock<IOrpheeTrackUI>();
            this.OrpheeTrackUIMock.Setup(otu => otu.InitProperties(It.IsAny<int>()));
            this.DeltaTimeRetriever = new DeltaTimeReader();
            this.OrpheeFile = new OrpheeFile(new OrpheeTrack(this.OrpheeTrackUIMock.Object, this.NoteMapGenerator) {CurrentInstrument = Instrument.AcousticGuitarSteel});
            this.OrpheeFile.OrpheeTrackList[0].NoteMap[0][0].IsChecked = 100;
            this.OrpheeFile.OrpheeTrackList[0].NoteMap[0][0].ColumnIndex = 0;
            this.OrpheeFile.OrpheeTrackList[0].NoteMap[0][0].LineIndex = 0;
            this.OrpheeFile.OrpheeTrackList[0].NoteMap[0][0].Note = Note.C4;
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
            this._orpheeNoteOnMessage = this.OrpheeFile.OrpheeTrackList[0].OrpheeNoteMessageList[0];
            this._orpheeNoteOffMessage = this.OrpheeFile.OrpheeTrackList[0].OrpheeNoteMessageList[1];
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
