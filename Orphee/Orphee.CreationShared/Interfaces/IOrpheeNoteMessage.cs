using Midi;

namespace Orphee.CreationShared.Interfaces
{
    public interface IOrpheeNoteMessage
    {
        // Properties
        int DeltaTime { get; set; }
        byte MessageCode { get; set; }
        int Channel { get; set; }
        Note Note { get; set; }
        int Velocity { get; set; }

        // Methods

    }
}
