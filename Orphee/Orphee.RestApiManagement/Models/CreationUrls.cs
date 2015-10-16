using Orphee.RestApiManagement.Models.Interfaces;

namespace Orphee.RestApiManagement.Models
{
    /// <summary>
    /// Class containing the AWS S3 
    /// get and put urls
    /// </summary>
    public class CreationUrls : ICreationUrls
    {
        /// <summary>Get url of the related file </summary>
        public string GetUrl { get; set; }
        /// <summary>Put url of the related file </summary>
        public string PutUrl { get; set; }
    }
}
