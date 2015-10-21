using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Midi;

namespace Orphee.CreationShared.Interfaces
{
    /// <summary>
    /// OrpheeTrack interface
    /// </summary>
    public interface IOrpheeTrack
    {
        // Properties

        /// <summary>Rectangle map represented on the CreationPage screen </summary>
        ObservableCollection<ObservableCollection<IToggleButtonNote>> NoteMap { get; }
        SolidColorBrush TrackColor { get; set; }
        /// <summary>List of noteMessage representation of the NoteMap </summary>
        IList<IOrpheeNoteMessage> OrpheeNoteMessageList { get; set; }
        /// <summary>Current instrument </summary>
        Instrument CurrentInstrument { get; set; }
        /// <summary>Player parameters </summary>
        IPlayerParameters PlayerParameters { get; set; }
        /// <summary>Channel assigned to the track </summary>
        Channel Channel { get; set; }
        /// <summary>Represents the Visibility of the track on the CreationPage screen </summary>
        Visibility TrackVisibility { get; set; }
        /// <summary>Graphical position of the track </summary>
        int TrackPos { get; set; }
        /// <summary>Length of the track </summary>
        uint TrackLength { get; set; }
        /// <summary>Name of the track </summary>
        string TrackName { get; set; }
        /// <summary>Represents the isCheck event of the rectangle associated to the toggleButtonNote</summary>
        bool IsChecked { get; set; }
        bool IsMuted { get; set; }
        bool IsSolo{ get; set; }
        ObservableCollection<MyRectangle> ColumnMap { get; set; } 


        // Methods

        /// <summary>
        /// Copies the given track info to the current track
        /// </summary>
        /// <param name="orpheeTrack">OrpheeTrack to be copied</param>
        //void UpdateOrpheeTrack(IOrpheeTrack orpheeTrack);

        /// <summary>
        /// Sets the current instrument value
        /// </summary>
        /// <param name="instrument">Instrument value to set to the current instrument</param>
        void UpdateCurrentInstrument(Instrument instrument);

        /// <summary>
        /// Converts the NoteMap variable to a list of NoteMessage
        /// </summary>
        void ConvertNoteMapToOrpheeMessage();
    }
}
