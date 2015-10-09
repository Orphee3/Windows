using Orphee.CreationShared.Interfaces;

namespace MidiDotNet.ExportModule.Interfaces
{
    public interface IOrpheeFileExporter
    {
        // Properties

        // Methods
        void ConvertTracksNoteMapToOrpheeNoteMessageList(IOrpheeFile orpheeFile);
        void SaveOrpheeFile(IOrpheeFile orpheeFile);
    }
}
