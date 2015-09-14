using Newtonsoft.Json.Linq;
using Orphee.RestApiManagement.Models.Interfaces;

namespace Orphee.RestApiManagement.Models
{
    public class Comment : IComment
    {
        public string Id { get; set; }
        public string Message { get; set; }
        public string CreatorId { get; set; }
        public JArray Child { get; set; }
    }
}
