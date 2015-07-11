using System;
using Midi;
using Orphee.Models.Interfaces;

namespace Orphee.Models
{
    public class MidiLibRepository : IMidiLibRepository
    {
        private readonly Clock _clock;
        private readonly OutputDevice _outputDevice;
        private readonly int _velocity;

        public MidiLibRepository()
        {
            this._clock = new Clock(120);
            try
            {
                this._outputDevice = OutputDevice.InstalledDevices[0];
            }
            catch (Exception e)
            {
                throw new Exception();
            }
            this._velocity = 75;

            this._outputDevice.Open();
            this._clock.Start();
        }

        public void PlayNote(Note note)
        {
            var noteOn = new NoteOnMessage(this._outputDevice, Channel.Channel1, note, this._velocity, this._clock.BeatTime);
            var noteOff = new NoteOffMessage(this._outputDevice, Channel.Channel1, note, this._velocity, this._clock.BeatTime + 1);

            this._clock.Schedule(noteOn);
            this._clock.Schedule(noteOff);
        }
    }
}
