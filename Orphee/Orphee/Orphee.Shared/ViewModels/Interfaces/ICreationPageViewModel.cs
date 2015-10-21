using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Microsoft.Practices.Prism.Commands;
using Orphee.CreationShared;
using Orphee.CreationShared.Interfaces;

namespace Orphee.ViewModels.Interfaces
{
    /// <summary>
    /// CreationPageViewModel instance
    /// </summary>
    public interface ICreationPageViewModel
    {
        // Methods

        /// <summary>
        /// Emits the sound associated with the given toggleButtonNote
        /// </summary>
        /// <param name="toggleButtonNote"></param>
        void ToggleButtonNoteExec(IToggleButtonNote toggleButtonNote);

        // Properties

        /// <summary>Index of the current tempo </summary>
        int CurrentTempoIndex { get; set; }
        /// <summary>List of int containing tempo values from 40 to 400 </summary>
        List<uint> TempoValues { get; }
        int CurrentTrackPos { get; set; }
        /// <summary>OrpheeFile displayed at the screen </summary>
        IOrpheeFile OrpheeFile { get; }
        /// <summary>Adds columns to the current track's note map </summary>
        DelegateCommand AddColumnsCommand { get; }
        /// <summary>Removes columns from the current track's note map </summary>
        DelegateCommand RemoveAColumnCommand { get; }
        /// <summary>Saves the current OrpheeFile in a MIDI file </summary>
        DelegateCommand SaveButtonCommand { get; }
        /// <summary>Loads a MIDI file</summary>
        DelegateCommand LoadButtonCommand { get; }
        /// <summary>Redirects to the previous page </summary>
        DelegateCommand BackButtonCommand { get; }
        /// <summary>Changes the track displayed </summary>
        DelegateCommand<SelectionChangedEventArgs> SelectedTrackCommand { get; }
        /// <summary>Add one higher octave to the actual track's note map </summary>
        DelegateCommand AddOneHigherOctaveCommand { get; }
        /// <summary>Adds one lower octave to the current track's note map </summary>
        DelegateCommand AddOneLowerOctaveCommand { get; }
        /// <summary>Add a new track to the OrpheeFile </summary>
        DelegateCommand AddNewTrackCommand { get; }
        /// <summary>Play the notes contained in each track of the OrpheeFile </summary>
        DelegateCommand PlayCommand { get; }
        DelegateCommand ItemSelectedCommand { get; }
        DelegateCommand<OrpheeTrack> HoldTrackCommand { get; }
    }
}
