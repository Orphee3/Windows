using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Newtonsoft.Json;

namespace Orphee.RestApiManagement.Models.Interfaces
{
    public interface IMessage
    {
        [JsonProperty(PropertyName = "message")]
        string ReceivedMessage { get; set; }
        [JsonProperty(PropertyName = "creator")]
        User User { get; set; }
        [JsonProperty(PropertyName = "dateCreation")]
        DateTime Date { get; set; }
        
        SolidColorBrush BackgroundMessageColor { get; }
        HorizontalAlignment MessageHorizontalAlignment { get; }
        string Hour { get; }
        string UserPictureSource { get; }
        void SetProperties();
        GridLength ColumnZeroWidth { get; }
        GridLength ColumnOneWidth { get; }
        int ElipseColumnNumber { get; }
        int MessageColumnNumber { get; }
        PointCollection PolygonPoints { get; }
    }
}
