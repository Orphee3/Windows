using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Orphee.RestApiManagement.Models.Interfaces;

namespace Orphee.RestApiManagement.Models
{
    public class Creation : ICreation
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsPrivate { get; set; }
        public JArray Comments { get; set; }
        public List<User> Creator { get; set; }
        public string GetUrl { get; set; }
        public int NumberOfComment { get; set; }
        public int NumberOfLike { get; set; }
        public void UpdateNumberOfLikeAndCommentValue()
        {
            if (this.Comments != null)
                this.NumberOfComment = Comments.Count;
            this.NumberOfLike = 0;
        }
    }
}
