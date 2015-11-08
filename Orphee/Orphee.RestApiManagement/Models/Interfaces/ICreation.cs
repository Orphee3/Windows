using System.Collections.Generic;
using Windows.UI.Xaml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Orphee.RestApiManagement.Models.Interfaces
{
    /// <summary>
    /// Creation interface
    /// </summary>
    public interface ICreation
    {
        /// <summary>Creation id </summary>
        [JsonProperty(PropertyName = "_id")]
        string Id { get; set; }
        /// <summary>Creation name</summary>
        [JsonProperty(PropertyName = "name")]
        string Name { get; set; }
        /// <summary>True if the creation is private. False otherwise </summary>
        [JsonProperty(PropertyName = "isPrivate")]
        bool IsPrivate { get; set; }
        /// <summary>Comments related to the creation </summary>
        [JsonProperty(PropertyName = "comments")]
        JArray Comments { get; set; }
        /// <summary>Creation creator </summary>
        [JsonProperty(PropertyName = "creator")]
        JArray Creator { get; set; }
        /// <summary>AWS S3 creation get url</summary>
        [JsonProperty(PropertyName = "url")]
        string GetUrl { get; set; }
        /// <summary>Number of comments related to the creation</summary>
        [JsonProperty(PropertyName = "nbComments")]
        int NumberOfComment { get; set; }
        /// <summary>Number of likes related to the creation </summary>
        [JsonProperty(PropertyName = "nbLikes")]
        int NumberOfLike { get; set; }
        /// <summary>List of the creators of the creation</summary>
        List<User> CreatorList { get; set; }

        /// <summary> 
        /// Updates the number of likes and comments of the creation
        /// </summary> 
        void UpdateNumberOfLikeAndCommentValue();
    }
}
