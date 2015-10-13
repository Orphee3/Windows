using System.Threading.Tasks;
using Orphee.CreationShared.Interfaces;

namespace MidiDotNet.ExportModule.Interfaces
{
    public interface IOrpheeFileExporter
    {
        // Properties

        // Methods
        void ConvertTracksNoteMapToOrpheeNoteMessageList(IOrpheeFile orpheeFile);
        Task<bool> SaveOrpheeFile(IOrpheeFile orpheeFile);
    }
}
