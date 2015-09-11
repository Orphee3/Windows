using Newtonsoft.Json.Linq;
using Orphee.RestApiManagement.Interfaces;

namespace Orphee.RestApiManagement
{
    public class Comment : IComment
    {
        public string Id { get; set; }
        public string Message { get; set; }
        public string CreatorId { get; set; }
        public JArray Child { get; set; }
    }
}
