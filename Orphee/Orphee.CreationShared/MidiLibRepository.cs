using System;
using System.Collections.Generic;
using Midi;
using Orphee.CreationShared.Interfaces;

namespace Orphee.CreationShared
{
    public class MidiLibRepository : IMidiLibRepository
    {
        public IPlayerParameters PlayerParameters { get; set; }
        private Clock _clock;
       // private OutputDevice _outputDevice;
        private readonly int _velocity;

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
               // this._outputDevice = OutputDevice.InstalledDevices[0];
            }
            catch (Exception e)
            {
                throw new Exception("", e);
            }
            this._velocity = 75;

           // this._outputDevice.Open();
            this._clock.Start();
        }

        public void PlayNote(Note note, Channel channel)
        {
            //var noteOn = new NoteOnMessage(this._outputDevice, channel, note, this._velocity, this._clock.BeatTime);
            //var noteOff = new NoteOffMessage(this._outputDevice, channel, note, this._velocity, this._clock.BeatTime + 1);

            //this._clock.Schedule(noteOn);
            //this._clock.Schedule(noteOff);
        }

        public void UpdatePlayingInstrument(Channel channel, Instrument newPlayingInstrument)
        {
            //this._outputDevice.SendProgramChange(channel, newPlayingInstrument);
        }

        public void UpdateTempo(uint tempo)
        {
            this.PlayerParameters.Tempo = tempo;
           // this._outputDevice.Close();
            this._clock = new Clock(tempo);
           // this._outputDevice = OutputDevice.InstalledDevices[0];
           // this._outputDevice.Open();
            this._clock.Start();
        }

        public void SetPlayerParameters(IPlayerParameters playerParameters)
        {
            this.PlayerParameters = playerParameters;
        }

        public void PlayTrack(IList<IOrpheeNoteMessage> noteMessageList, Instrument instrument, Channel channel)
        {
            //UpdatePlayingInstrument(channel, instrument);
            //var beatTime = 0;
            //foreach (var note in noteMessageList)
            //{
            //    beatTime += note.DeltaTime / 48;
            //    if ((note.MessageCode & 0x90) == 0x90)
            //        this._clock.Schedule(new NoteOnMessage(this._outputDevice, (Channel)note.Channel, note.Note, note.Velocity, this._clock.BeatTime + beatTime));
            //    else
            //        this._clock.Schedule(new NoteOffMessage(this._outputDevice, (Channel)note.Channel, note.Note, note.Velocity, this._clock.BeatTime + beatTime));
            //}
        }
    }
}
