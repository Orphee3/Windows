using System;
using System.IO;
using Windows.Storage;
using Midi;
using MidiDotNet.ExportModule;
using MidiDotNet.ExportModule.Interfaces;
using Moq;
using NUnit.Framework;
using Orphee.CreationShared;
using Orphee.CreationShared.Interfaces;

namespace Orphee.UnitTests.ExportModuleTests.TrackHeaderWriterTests
{
    public class WhenTrackHeaderWriterIsCalled
    {
        protected Mock<IProgramMessageWriter> ProgramMessageWriterMock;
        protected Mock<ITempoMessageWriter> TempoMessageWriterMock;
        protected Mock<ITimeSignatureMessageWriter> TimeSignatureMessageWriterMock;
        protected ITrackHeaderWriter TrackHeaderWriter;
        protected BinaryWriter Writer;
        protected BinaryReader Reader;
        protected StorageFile OrpheeFile;
        protected IOrpheeTrack OrpheeTrack;
        protected IPlayerParameters PlayerParameters;

        public WhenTrackHeaderWriterIsCalled()
        {
            this.OrpheeTrack = new OrpheeTrack(0, Channel.Channel1)
            {
                PlayerParameters = new PlayerParameters()
            };
            this.ProgramMessageWriterMock = new Mock<IProgramMessageWriter>();
            this.TempoMessageWriterMock = new Mock<ITempoMessageWriter>();
            this.TimeSignatureMessageWriterMock = new Mock<ITimeSignatureMessageWriter>();
            this.TrackHeaderWriter = new TrackHeaderWriter(this.TimeSignatureMessageWriterMock.Object , this.TempoMessageWriterMock.Object, this.ProgramMessageWriterMock.Object);
            InitializeWriter();
        }

        private async void InitializeWriter()
        {
            var folder = KnownFolders.MusicLibrary;
            this.OrpheeFile = await folder.CreateFileAsync("UnitTest.orph", CreationCollisionOption.ReplaceExisting);
        }
    }

    [TestFixture]
    public class TheWriteFunctionShouldReturnTrue : WhenTrackHeaderWriterIsCalled
    {
        private bool _result;

        [SetUp]
        public void Init()
        {
            using (this.Writer = new BinaryWriter(this.OrpheeFile.OpenStreamForWriteAsync().Result))
            {
                this._result = this.TrackHeaderWriter.WriteTrackHeader(this.Writer, this.OrpheeTrack.PlayerParameters);
            }
        }

        [Test]
        public void TheResultShouldBeTrue()
        {
            Assert.IsTrue(this._result);
        }
    }
}
