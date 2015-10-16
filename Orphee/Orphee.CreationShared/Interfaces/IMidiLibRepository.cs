using System.Collections.Generic;
using Midi;

namespace Orphee.CreationShared.Interfaces
{
    /// <summary>
    /// MidiLibRepository interface
    /// </summary>
    public interface IMidiLibRepository
    {
        // Properties

        /// <summary>Represents the parameters on the player </summary>
        IPlayerParameters PlayerParameters { get; set; }

        // Methods

        /// <summary>
        /// Function playing the given note
        /// </summary>
        /// <param name="note">Value representing the note to play</param>
        /// <param name="channel">Value representing the channel in which the note will be played</param>
        void PlayNote(Note note, Channel channel);

        /// <summary>
        /// Function updating the current instrument
        /// </summary>
        /// <param name="newPlayingInstrument">Instrument replacing the current instrument</param>
        /// <param name="channel">Channel for which the instrument is going to be changed</param>
        void UpdatePlayingInstrument(Channel channel, Instrument newPlayingInstrument);

        /// <summary>
        /// Function changing the tempo value
        /// </summary>
        /// <param name="tempo">New tempo value</param>
        void UpdateTempo(uint tempo);

        /// <summary>
        /// Function setting the PlayerParameter
        /// </summary>
        /// <param name="playerParameters">PlayerParameter used to set the current PlayerParameter</param>
        void SetPlayerParameters(IPlayerParameters playerParameters);

        /// <summary>
        /// Function playing the notes contained in a track
        /// </summary>
        /// <param name="noteMessageList">Contains all the track's note to be played</param>
        /// <param name="instrument">Represents the instrument that's going to play the notes</param>
        /// <param name="channel">Channel in which the notes are going to be played</param>
        void PlayTrack(IList<IOrpheeNoteMessage> noteMessageList, Instrument instrument, Channel channel);
    }
}
