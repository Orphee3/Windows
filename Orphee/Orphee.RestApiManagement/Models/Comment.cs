using Newtonsoft.Json.Linq;
using Orphee.RestApiManagement.Models.Interfaces;

namespace Orphee.RestApiManagement.Models
{
    /// <summary>
    /// Class containing all the comment
    /// related data
    /// </summary>
    public class Comment : IComment
    {
        /// <summary>Comment id </summary>
        public string Id { get; set; }
        /// <summary>Comment </summary>
        public string Message { get; set; }
        /// <summary>Comment creator </summary>
        public User Creator { get; set; }
        /// <summary>Comment repsonses</summary>
        public JArray Child { get; set; }
        /// <summary>Creation id related to the comment </summary>
        public string CreationId { get; set; }
    }
}
