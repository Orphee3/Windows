﻿using System.IO;
using System.Text;
using MidiDotNet.ExportModule;
using MidiDotNet.ExportModule.Interfaces;
using MidiDotNet.Shared;
using MidiDotNet.Shared.Interfaces;
using NUnit.Framework;
using Orphee.CreationShared;
using Orphee.CreationShared.Interfaces;

namespace MidiDotNet.ExportModuleUnitTests.ExportModuleTests.FileHeaderWriterTests
{
    public class WhenFileHeaderWriterIsCalled : ExportModuleTestsBase
    {
        protected IFileHeaderWriter FileHeaderWriter;
        protected ISwapManager SwapManager;
        protected IOrpheeFileParameters OrpheeFileParameters;

        public WhenFileHeaderWriterIsCalled()
        {
            this.OrpheeFileParameters = new OrpheeFileParameters()
            {
                DeltaTicksPerQuarterNote = 60,
                NumberOfTracks = 1,
                OrpheeFileType = 0
            };
            this.SwapManager = new SwapManager();
            this.FileHeaderWriter = new FileHeaderWriter(this.SwapManager);
            var result = InitializeFile("FileHeaderWriterTests.test").Result;
        }
    }

    [TestFixture]
    public class TheWriteFunctionShouldReturnTrue : WhenFileHeaderWriterIsCalled
    {
        private bool _result;

        [SetUp]
        public void Init()
        {
            using (this.Writer = new BinaryWriter(this.File.OpenStreamForWriteAsync().Result))
            {
                this._result = this.FileHeaderWriter.WriteFileHeader(this.Writer, this.OrpheeFileParameters);
            }
        }

        [Test]
        public void TheResultShouldBeTrue()
        {
            Assert.IsTrue(this._result);
        }
    }

    [TestFixture]
    public class TheHeaderShoudBeWrittenInTheNewFile : WhenFileHeaderWriterIsCalled
    {
        private string _fileHeader;
        private uint _fileHeaderLength;
        private bool _result;
        private uint _orpheeFileType;
        private uint _numberOfTracks;
        private int _deltaTimePerQuarterNote;

        [SetUp]
        public void WriteDataInTheUnitTestFile()
        {
            using (this.Writer = new BinaryWriter(this.File.OpenStreamForWriteAsync().Result))
            {
                this._result = this.FileHeaderWriter.WriteFileHeader(this.Writer, this.OrpheeFileParameters);
            }
            ReadTheUnitTestFile();
        }

        public void ReadTheUnitTestFile()
        {
            using (this.Reader = new BinaryReader(this.File.OpenStreamForReadAsync().Result))
            {
                this._fileHeader = Encoding.UTF8.GetString(this.Reader.ReadBytes(4), 0, 4);
                this._fileHeaderLength = this.SwapManager.SwapUInt32(this.Reader.ReadUInt32());
                this._orpheeFileType = this.SwapManager.SwapUInt16(this.Reader.ReadUInt16());
                this._numberOfTracks = this.SwapManager.SwapUInt16(this.Reader.ReadUInt16());
                this._deltaTimePerQuarterNote = this.SwapManager.SwapUInt16(this.Reader.ReadUInt16());
            }
        }

        [Test]
        public void ResultShouldBeTrue()
        {
            Assert.IsTrue(this._result);
        }

        [Test]
        public void TheFileHeaderContentShouldBeMTdh()
        {
            Assert.AreEqual("MThd", this._fileHeader);
        }

        [Test]
        public void TheFileHeaderLengthShouldBeSix()
        {
            Assert.AreEqual(6, this._fileHeaderLength);
        }

        [Test]
        public void TheOrpheeFileTypeShouldBeZero()
        {
            Assert.AreEqual(0, this._orpheeFileType);
        }

        [Test]
        public void TheFileHeaderNumberOfTrackShouldBeOne()
        {
            Assert.AreEqual(1, this._numberOfTracks);
        }

        [Test]
        public void TheFileHeaderDeltaTicksPerQuarterNoteShouldBeSixty()
        {
            Assert.AreEqual(60, this._deltaTimePerQuarterNote);
        }
    }
}
