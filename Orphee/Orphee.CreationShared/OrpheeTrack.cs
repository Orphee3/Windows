using System.Collections.Generic;
using System.Collections.ObjectModel;
using Midi;
using Orphee.CreationShared.Interfaces;

namespace Orphee.CreationShared
{
    public class OrpheeTrack : IOrpheeTrack
    {
        public IList<ObservableCollection<IToggleButtonNote>> NoteMap { get; private set; }
        public IList<IOrpheeNoteMessage> OrpheeNoteMessageList { get; set; } 
        public Instrument CurrentInstrument { get; set; }
        public IPlayerParameters PlayerParameters { get; set; }
        public Channel Channel { get; private set; }
        public int TrackPos { get; set; }
        public uint TrackLength { get; set; }

        public OrpheeTrack(int trackPos, Channel channel)
        {
            this.NoteMap = NoteMapManager.Instance.GenerateNoteMap();
            this.Channel = channel;
            this.TrackPos = trackPos;
            if (this.TrackPos == 0)
            {
                this.PlayerParameters = new PlayerParameters();
                this.TrackLength = 22;
            }
            else
                this.TrackLength = 7;
        }
    }
}
