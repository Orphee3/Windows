using Newtonsoft.Json.Linq;
using Orphee.RestApiManagement.Models.Interfaces;

namespace Orphee.RestApiManagement.Models
{
    public class Comment : IComment
    {
        public string Id { get; set; }
        public string Message { get; set; }
        public User Creator { get; set; }
        public JArray Child { get; set; }
        public string CreationId { get; set; }
    }
}
