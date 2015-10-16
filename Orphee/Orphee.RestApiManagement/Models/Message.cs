using System;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Orphee.RestApiManagement.Models.Interfaces;

namespace Orphee.RestApiManagement.Models
{
    /// <summary>
    /// Class containing all the 
    /// message related data
    /// </summary>
    public class Message : IMessage
    {
        /// <summary>Message received</summary>
        public string ReceivedMessage { get; set; }
        /// <summary>User that created the message </summary>
        public User User { get; set; }
        /// <summary>Message creation date </summary>
        public DateTime Date { get; set; }
        /// <summary>Background color of the message bubble</summary>
        public SolidColorBrush BackgroundMessageColor { get; private set;}
        /// <summary>Right if the message if from you and left otherwise </summary>
        public HorizontalAlignment MessageHorizontalAlignment { get; private set; }
        /// <summary>Message reception time </summary>
        public string Hour { get; private set; }
        /// <summary>User picture source</summary>
        public string UserPictureSource { get; private set; }
        /// <summary>Width of the column </summary>
        public GridLength ColumnZeroWidth { get; private set; }
        /// <summary>Width of the column </summary>
        public GridLength ColumnOneWidth { get; private set; }
        /// <summary>Column number of the elipse containing the user's picture </summary>
        public int ElipseColumnNumber { get; private set; }
        /// <summary>Column number of the message </summary>
        public int MessageColumnNumber { get; private set; }
        /// <summary>Points needed to create le triangle for each message bubble </summary>
        public PointCollection PolygonPoints { get; private set; }

        /// <summary>
        /// Sets the class properties accordingly to the received message creator
        /// </summary>
        public void SetProperties()
        {
            this.MessageHorizontalAlignment = this.User.Id == RestApiManagerBase.Instance.UserData.User.Id ? HorizontalAlignment.Right : HorizontalAlignment.Left;
            this.BackgroundMessageColor = new SolidColorBrush(this.MessageHorizontalAlignment == HorizontalAlignment.Right ? Color.FromArgb(0xFF, 0x78, 0xC7, 0xF9) : Color.FromArgb(0xFF, 0xBD, 0xBE, 0xC0));
            this.ColumnZeroWidth = this.MessageHorizontalAlignment == HorizontalAlignment.Right ? new GridLength(1, GridUnitType.Star): new GridLength(60);
            this.ColumnOneWidth = this.MessageHorizontalAlignment == HorizontalAlignment.Right ? new GridLength(60) : new GridLength(1, GridUnitType.Star);
            this.ElipseColumnNumber = this.MessageHorizontalAlignment == HorizontalAlignment.Right ? 1 : 0;
            this.MessageColumnNumber = this.MessageHorizontalAlignment == HorizontalAlignment.Right ? 0 : 1;
            this.PolygonPoints = this.MessageHorizontalAlignment == HorizontalAlignment.Right ? new PointCollection {new Point(0, 0), new Point(8, 5), new Point(0, 10)} : new PointCollection {new Point(8, 0), new Point(0, 5), new Point(8, 10)};
            this.Hour = this.Date.ToString("HH:mm");
            this.UserPictureSource = this.User.Picture ?? "/Assets/defaultUser.png";
        }
    }
}
