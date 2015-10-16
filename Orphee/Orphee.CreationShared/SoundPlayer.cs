using System.Collections.Generic;
using Midi;
using Orphee.CreationShared.Interfaces;

namespace Orphee.CreationShared
{
    /// <summary>
    /// Class that plays the role of interface between the program and 
    /// the MidiLibRepository class
    /// </summary>
    public class SoundPlayer : ISoundPlayer
    {
        private readonly IMidiLibRepository _midiLibRepository;

        /// <summary>
        /// Constructor initializing the midiLibRepository class
        /// through dependency injection
        /// </summary>
        /// <param name="midiLibRepository"></param>
        public SoundPlayer(IMidiLibRepository midiLibRepository)
        {
            this._midiLibRepository = midiLibRepository;
        }


        /// <summary>
        /// Function playing the given note
        /// </summary>
        /// <param name="note">Value representing the note to play</param>
        /// <param name="channel">Value representing the channel in which the note will be played</param>
        public void PlayNote(Note note, Channel channel)
        {
            this._midiLibRepository.PlayNote(note, channel);
        }

        /// <summary>
        /// Function returning the sound player playerParameter
        /// </summary>
        /// <returns>Returns the actual playerParameter</returns>
        public IPlayerParameters GetPlayerParameters()
        {
            return this._midiLibRepository.PlayerParameters;
        }

        /// <summary>
        /// Function updating the current instrument
        /// </summary>
        /// <param name="currentInstrument">Instrument replacing the current instrument</param>
        /// <param name="channel">Channel for which the instrument is going to be changed</param>
        public void UpdateCurrentInstrument(Instrument currentInstrument, Channel channel)
        {
            this._midiLibRepository.UpdatePlayingInstrument(channel, currentInstrument);
        }

        /// <summary>
        /// Function changing the tempo value
        /// </summary>
        /// <param name="tempo">New tempo value</param>
        public void UpdateTempo(uint tempo)
        {
            this._midiLibRepository.UpdateTempo(tempo);
        }

        /// <summary>
        /// Function setting the PlayerParameter
        /// </summary>
        /// <param name="playerParameters">PlayerParameter used to set the current PlayerParameter</param>
        public void SetPlayerParameters(IPlayerParameters playerParameters)
        {
            this._midiLibRepository.SetPlayerParameters(playerParameters);
        }

        /// <summary>
        /// Function playing the notes contained in a track
        /// </summary>
        /// <param name="noteMessageList">Contains all the track's note to be played</param>
        /// <param name="instrument">Represents the instrument that's going to play the notes</param>
        /// <param name="channel">Channel in which the notes are going to be played</param>
        public void PlayTrack(IList<IOrpheeNoteMessage> noteMessageList, Instrument instrument, Channel channel)
        {
            this._midiLibRepository.PlayTrack(noteMessageList, instrument, channel);
        }
    }
}
