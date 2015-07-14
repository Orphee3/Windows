using System.IO;
using Midi;
using MidiDotNet.ExportModule;
using MidiDotNet.ExportModule.Interfaces;
using NUnit.Framework;
using Orphee.CreationShared;
using Orphee.CreationShared.Interfaces;

namespace Orphee.UnitTests.ExportModuleTests.NoteMessageWriterTests
{
    public class WhenNoteMessageWriterIsCalled : ExportModuleTestsBase
    {
        protected INoteMessageWriter NoteMessageWriter;
        protected IOrpheeTrack OrpheeTrack;

        public WhenNoteMessageWriterIsCalled()
        {
            this.OrpheeTrack = new OrpheeTrack(0, Channel.Channel1);
            this.NoteMessageWriter = new NoteMessageWriter();
            var result = InitializeFile("NoteMessageTests.orph").Result;
        }
    }

    [TestFixture]
    public class ItShouldWriteTheNoteMessage : WhenNoteMessageWriterIsCalled
    {
        [SetUp]
        public void Init()
        {
            using (this.Writer = new BinaryWriter(this.File.OpenStreamForWriteAsync().Result))
            {
               // foreach ()
               // this.NoteMessageWriter.WriteNoteMessage();
            }
        }
    }
}
