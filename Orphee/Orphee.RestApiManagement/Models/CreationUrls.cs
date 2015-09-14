using Orphee.RestApiManagement.Models.Interfaces;

namespace Orphee.RestApiManagement.Models
{
    public class CreationUrls : ICreationUrls
    {
        public string GetUrl { get; set; }
        public string PutUrl { get; set; }
    }
}
