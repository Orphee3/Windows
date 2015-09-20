using System;
using Orphee.RestApiManagement.Models.Interfaces;

namespace Orphee.RestApiManagement.Models
{
    public class News : INews
    {
        public string Id { get; set; }
        public User Creator { get; set; }
        public Creation Creation { get; set; }
        public string NewsType { get; set; }
        public DateTime DateCreation { get; set; }
        public bool HasBeenViews { get; set; }
    }
}
