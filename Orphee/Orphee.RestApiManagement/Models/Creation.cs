using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Orphee.RestApiManagement.Models.Interfaces;

namespace Orphee.RestApiManagement.Models
{
    /// <summary>
    /// Class containing all the creation
    /// related data
    /// </summary>
    public class Creation : ICreation
    {
        /// <summary>Creation id </summary>
        public string Id { get; set; }
        /// <summary>Creation name</summary>
        public string Name { get; set; }
        /// <summary>True if the creation is private. False otherwise </summary>
        public bool IsPrivate { get; set; }
        /// <summary>Comments related to the creation </summary>
        public JArray Comments { get; set; }
        /// <summary>Creation creator </summary>
        public JArray Creator { get; set; }
        /// <summary>AWS S3 creation get url</summary>
        public string GetUrl { get; set; }
        /// <summary>Number of comments related to the creation</summary>
        public int NumberOfComment { get; set; }
        /// <summary>Number of likes related to the creation </summary>
        public int NumberOfLike { get; set; }
        /// <summary>List of the creators of the creation</summary>
        public List<User> CreatorList { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public Creation()
        {
            this.CreatorList = new List<User>();
        }

        /// <summary>
        /// Updates the number of likes and comments of the creation
        /// </summary>
        public void UpdateNumberOfLikeAndCommentValue()
        {
            if (this.Comments != null)
                this.NumberOfComment = Comments.Count;
            this.NumberOfLike = 0;
        }
    }
}
