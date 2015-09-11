using System;
using Orphee.RestApiManagement.Interfaces;

namespace Orphee.RestApiManagement
{
    public class Message : IMessage
    {
        public string ReceivedMessage { get; set; }
        public User User { get; set; }
        public DateTime Date { get; set; }
    }
}
