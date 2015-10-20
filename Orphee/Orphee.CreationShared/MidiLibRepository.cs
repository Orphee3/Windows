using System;
using System.Collections.Generic;
using Midi;
using Orphee.CreationShared.Interfaces;

namespace Orphee.CreationShared
{
    /// <summary>
    /// Class that manages the sound emission
    /// </summary>
    public class MidiLibRepository : IMidiLibRepository
    {
        /// <summary>Represents the parameters on the player </summary>
        public IPlayerParameters PlayerParameters { get; set; }
        private Clock _clock;
        private OutputDevice _outputDevice;
        private readonly int _velocity;

        /// <summary>
        /// Constructor
        /// </summary>
        public MidiLibRepository()
        {
            this.PlayerParameters = new PlayerParameters()
            {
                TimeSignatureNominator = 4,
                TimeSignatureDenominator = 4,
                TimeSignatureClocksPerBeat = 24,
                TimeSignatureNumberOf32ThNotePerBeat = 4,
                Tempo = 120,
            };
            this._clock = new Clock(this.PlayerParameters.Tempo);
            try
            {
                this._outputDevice = OutputDevice.InstalledDevices[0];
            }
            catch (Exception e)
            {
                throw new Exception("", e);
            }
            this._velocity = 75;

            this._outputDevice.Open();
            this._clock.Start();
        }

        /// <summary>
        /// Function playing the given note
        /// </summary>
        /// <param name="note">Value representing the note to play</param>
        /// <param name="channel">Value representing the channel in which the note will be played</param>
        public void PlayNote(Note note, Channel channel)
        {
            var noteOn = new NoteOnMessage(this._outputDevice, channel, note, this._velocity, this._clock.BeatTime);
            var noteOff = new NoteOffMessage(this._outputDevice, channel, note, this._velocity, this._clock.BeatTime + 1);

            this._clock.Schedule(noteOn);
            this._clock.Schedule(noteOff);
        }

        /// <summary>
        /// Function updating the current instrument
        /// </summary>
        /// <param name="newPlayingInstrument">Instrument replacing the current instrument</param>
        /// <param name="channel">Channel for which the instrument is going to be changed</param>
        public void UpdatePlayingInstrument(Channel channel, Instrument newPlayingInstrument)
        {
            this._outputDevice.SendProgramChange(channel, newPlayingInstrument);
        }

        /// <summary>
        /// Function changing the tempo value
        /// </summary>
        /// <param name="tempo">New tempo value</param>
        public void UpdateTempo(uint tempo)
        {
            this.PlayerParameters.Tempo = tempo;
            this._outputDevice.Close();
            this._clock = new Clock(tempo);
            this._outputDevice = OutputDevice.InstalledDevices[0];
            this._outputDevice.Open();
            this._clock.Start();
        }

        /// <summary>
        /// Function setting the PlayerParameter
        /// </summary>
        /// <param name="playerParameters">PlayerParameter used to set the current PlayerParameter</param>
        public void SetPlayerParameters(IPlayerParameters playerParameters)
        {
            this.PlayerParameters = playerParameters;
        }

        /// <summary>
        /// Function playing the notes contained in a track
        /// </summary>
        /// <param name="noteMessageList">Contains all the track's note to be played</param>
        /// <param name="instrument">Represents the instrument that's going to play the notes</param>
        /// <param name="channel">Channel in which the notes are going to be played</param>
        public void PlayTrack(IList<IOrpheeNoteMessage> noteMessageList, Instrument instrument, Channel channel)
        {
            UpdatePlayingInstrument(channel, instrument);
            var beatTime = 0;
            foreach (var note in noteMessageList)
            {
                beatTime += note.DeltaTime / 48;
                if ((note.MessageCode & 0x90) == 0x90)
                    this._clock.Schedule(new NoteOnMessage(this._outputDevice, (Channel)note.Channel, note.Note, note.Velocity, this._clock.BeatTime + beatTime));
                else
                    this._clock.Schedule(new NoteOffMessage(this._outputDevice, (Channel)note.Channel, note.Note, note.Velocity, this._clock.BeatTime + beatTime));
            }
        }
    }
}
