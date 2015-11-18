using MidiDotNet.ExportModule.Interfaces;

namespace MidiDotNet.ExportModule
{
    public class ChunckWriters : IChunckWriters
    {
        public IFileHeaderWriter FileHeaderWriter { get; }
        public ITrackHeaderWriter TrackHeaderWriter { get; }
        public INoteMessageWriter NoteMessageWriter { get; }

        public ChunckWriters(IFileHeaderWriter fileHeaderWriter, INoteMessageWriter noteMessageWriter, ITrackHeaderWriter trackHeaderWriter)
        {
            this.FileHeaderWriter = fileHeaderWriter;
            this.NoteMessageWriter = noteMessageWriter;
            this.TrackHeaderWriter = trackHeaderWriter;
        }
    }
}
