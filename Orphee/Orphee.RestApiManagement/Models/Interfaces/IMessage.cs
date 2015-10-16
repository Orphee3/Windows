using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Newtonsoft.Json;

namespace Orphee.RestApiManagement.Models.Interfaces
{
    /// <summary>
    /// Message interface
    /// </summary>
    public interface IMessage
    {
        /// <summary>Message received</summary>
        [JsonProperty(PropertyName = "message")]
        string ReceivedMessage { get; set; }
        /// <summary>User that created the message </summary>
        [JsonProperty(PropertyName = "creator")]
        User User { get; set; }
        /// <summary>Message creation date </summary>
        [JsonProperty(PropertyName = "dateCreation")]
        DateTime Date { get; set; }

        /// <summary>Background color of the message bubble</summary>
        SolidColorBrush BackgroundMessageColor { get; }
        /// <summary>Right if the message if from you and left otherwise </summary>
        HorizontalAlignment MessageHorizontalAlignment { get; }
        /// <summary>Message reception time </summary>
        string Hour { get; }
        /// <summary>User picture source</summary>
        string UserPictureSource { get; }
        /// <summary>Width of the column </summary>
        GridLength ColumnZeroWidth { get; }
        /// <summary>Width of the column </summary>
        GridLength ColumnOneWidth { get; }
        /// <summary>Column number of the elipse containing the user's picture </summary>
        int ElipseColumnNumber { get; }
        /// <summary>Column number of the message </summary>
        int MessageColumnNumber { get; }
        /// <summary>Points needed to create le triangle for each message bubble </summary>
        PointCollection PolygonPoints { get; }

        /// <summary>
        /// Sets the class properties accordingly to the received message creator
        /// </summary>
        void SetProperties();
    }
}
