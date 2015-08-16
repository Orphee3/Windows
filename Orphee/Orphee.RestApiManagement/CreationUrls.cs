using Orphee.RestApiManagement.Interfaces;

namespace Orphee.RestApiManagement
{
    public class CreationUrls : ICreationUrls
    {
        public string GetUrl { get; set; }
        public string PutUrl { get; set; }
    }
}
