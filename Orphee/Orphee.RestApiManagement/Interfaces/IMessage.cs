namespace Orphee.RestApiManagement.Interfaces
{
    public interface IMessage
    {
        string ReceivedMessage { get; set; }
        User User { get; set; }
    }
}
