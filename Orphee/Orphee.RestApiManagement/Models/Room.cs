using System.Collections.Generic;
using Orphee.RestApiManagement.Models.Interfaces;

namespace Orphee.RestApiManagement.Models
{
    public class Room : IRoom
    {
        public List<string> People { get; set; }
        public string Id { get; set; }
        public string Type { get; set; }
        public string Host { get; set; }
    }
}
