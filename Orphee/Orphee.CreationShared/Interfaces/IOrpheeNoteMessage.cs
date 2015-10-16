using Midi;

namespace Orphee.CreationShared.Interfaces
{
    /// <summary>
    /// OrpheeNoteMessage interface
    /// </summary>
    public interface IOrpheeNoteMessage
    {
        // Properties

        /// <summary>Delta time of the current noteMessage </summary>
        int DeltaTime { get; set; }
        /// <summary>Code of the current messageCode </summary>
        byte MessageCode { get; set; }
        /// <summary>Channel related to the current noteMessage </summary>
        int Channel { get; set; }
        /// <summary>Note related to the current noteMessage </summary>
        Note Note { get; set; }
        /// <summary>Velocity related to the current noteMessage </summary>
        int Velocity { get; set; }

        // Methods

    }
}
