namespace MidiDotNet.ExportModule.Interfaces
{
    public interface IChunckWriters
    {
        IFileHeaderWriter FileHeaderWriter { get; }
        ITrackHeaderWriter TrackHeaderWriter { get; }
        INoteMessageWriter NoteMessageWriter { get; }
    }
}
