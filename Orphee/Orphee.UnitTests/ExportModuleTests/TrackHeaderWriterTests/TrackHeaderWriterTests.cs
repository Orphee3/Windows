using System;
using System.IO;
using System.Text;
using Midi;
using MidiDotNet.ExportModule;
using MidiDotNet.ExportModule.Interfaces;
using MidiDotNet.Shared;
using MidiDotNet.Shared.Interfaces;
using Moq;
using NUnit.Framework;
using Orphee.CreationShared;
using Orphee.CreationShared.Interfaces;

namespace Orphee.UnitTests.ExportModuleTests.TrackHeaderWriterTests
{
    public class WhenTrackHeaderWriterIsCalled : ExportModuleTestsBase
    {
        protected Mock<ITempoMessageWriter> TempoMessageWriterMock;
        protected Mock<ITimeSignatureMessageWriter> TimeSignatureMessageWriterMock;
        protected ITrackHeaderWriter TrackHeaderWriter;
        protected IOrpheeTrack OrpheeTrack;
        protected IPlayerParameters PlayerParameters;
        protected ISwapManager SwapManager;

        public WhenTrackHeaderWriterIsCalled()
        {
            this.OrpheeTrack = new OrpheeTrack(0, Channel.Channel1)
            {
                PlayerParameters = new PlayerParameters()
            };
            this.SwapManager = new SwapManager();
            this.TempoMessageWriterMock = new Mock<ITempoMessageWriter>();
            this.TimeSignatureMessageWriterMock = new Mock<ITimeSignatureMessageWriter>();
            this.TrackHeaderWriter = new TrackHeaderWriter(this.TimeSignatureMessageWriterMock.Object , this.TempoMessageWriterMock.Object, this.SwapManager);
            var result = InitializeFile("TrackHeaderTests.orph").Result;
        }
    }

    [TestFixture]
    public class TheWriteFunctionShouldReturnTrue : WhenTrackHeaderWriterIsCalled
    {
        private bool _result;

        [SetUp]
        public void Init()
        {
            using (this.Writer = new BinaryWriter(this.File.OpenStreamForWriteAsync().Result))
            {
                this._result = this.TrackHeaderWriter.WriteTrackHeader(this.Writer, this.OrpheeTrack.PlayerParameters, this.OrpheeTrack.TrackLength);
            }
        }

        [Test]
        public void TheResultShouldBeTrue()
        {
            Assert.IsTrue(this._result);
        }
    }

    [TestFixture]
    public class TheFirstTrackHeaderShouldBeWrittenInTheNewFile : WhenTrackHeaderWriterIsCalled
    {
        private bool _result;
        private string _trackHeaderCode;
        private uint _trackLength;

        [SetUp]
        public void WriteDataInTheUnitTestFile()
        {
            using (this.Writer = new BinaryWriter(this.File.OpenStreamForWriteAsync().Result))
            {
                this._result = this.TrackHeaderWriter.WriteTrackHeader(this.Writer, this.PlayerParameters, this.OrpheeTrack.TrackLength);
            }
            ReadTheUnitTestFile();
        }

        private void ReadTheUnitTestFile()
        {
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
            {
                this._trackHeaderCode = Encoding.UTF8.GetString(this.Reader.ReadBytes(4), 0, 4);
                this._trackLength = this.SwapManager.SwapUInt32(this.Reader.ReadUInt32());
            }
        }

        [Test]
        public void ResultShouldBeTrue()
        {
            Assert.IsTrue(this._result);
        }

        [Test]
        public void TrackHeaderShouldBeMTrk()
        {
            Assert.AreEqual("MTrk", this._trackHeaderCode);
        }

        [Test]
        public void TrackLengthShouldBeEqualToOrpheTrackTrackLength()
        {
            Assert.AreEqual(this.OrpheeTrack.TrackLength, this._trackLength);
        }
    }
}
