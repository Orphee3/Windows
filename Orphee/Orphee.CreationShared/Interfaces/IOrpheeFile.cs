using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace Orphee.CreationShared.Interfaces
{
    /// <summary>
    /// OrpheeFile interface
    /// </summary>
    public interface IOrpheeFile
    {
        // Properties
        [JsonProperty(PropertyName = "_id")]
        string Id { get; set; }
        /// <summary>Parameters of the orphee file </summary>
        [JsonConverter(typeof(ConcreteConverter<OrpheeFileParameters>))]
        IOrpheeFileParameters OrpheeFileParameters { get; set; }
        /// <summary>List of track contained in the current file </summary>
        [JsonConverter(typeof(ConcreteConverter<ObservableCollection<OrpheeTrack>>))]
        ObservableCollection<IOrpheeTrack> OrpheeTrackList { get; set; }
        /// <summary>Name of the current file </summary>
        [JsonProperty(PropertyName = "FileName")]
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
