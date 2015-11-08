using System.Collections.ObjectModel;

namespace Orphee.CreationShared.Interfaces
{
    /// <summary>
    /// OrpheeFile interface
    /// </summary>
    public interface IOrpheeFile
    {
        // Properties
     
        /// <summary>Parameters of the orphee file </summary>
        IOrpheeFileParameters OrpheeFileParameters { get; set; }
        /// <summary>List of track contained in the current file </summary>
        ObservableCollection<IOrpheeTrack> OrpheeTrackList { get; set; }
        /// <summary>Name of the current file </summary>
        string FileName { get; set; }
        bool HasBeenSent { get; set; }

        // Methods

        /// <summary>
        /// Adds a new track to the OrpheeTrackList
        /// </summary>
        /// <param name="orpheeTrack">OrpheeTrack to add</param>
        void AddNewTrack(IOrpheeTrack orpheeTrack);
    }
}
