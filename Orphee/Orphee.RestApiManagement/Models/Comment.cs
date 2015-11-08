using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;
using Orphee.RestApiManagement.Models.Interfaces;

namespace Orphee.RestApiManagement.Models
{
    /// <summary>
    /// Class containing all the comment
    /// related data
    /// </summary>
    [DataContract]
    public class Comment : IComment
    {
        /// <summary>Comment id </summary>
        [DataMember]
        public string Id { get; set; }
        /// <summary>Comment </summary>
        [DataMember]
        public string Message { get; set; }
        /// <summary>Comment creator </summary>
        [DataMember]
        public User Creator { get; set; }
        /// <summary>Comment repsonses</summary>
        public JArray Child { get; set; }
        /// <summary>Creation id related to the comment </summary>
        [DataMember]
        public string CreationId { get; set; }
    }
}
