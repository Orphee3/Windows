using System;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Orphee.RestApiManagement.Interfaces;

namespace Orphee.RestApiManagement
{
    public class Message : IMessage
    {
        public string ReceivedMessage { get; set; }
        public User User { get; set; }
        public DateTime Date { get; set; }
        public SolidColorBrush BackgroundMessageColor { get; private set;}
        public HorizontalAlignment MessageHorizontalAlignment { get; private set; }
        public string Hour { get; private set; }
        public string UserPictureSource { get; private set; }
        public GridLength ColumnZeroWidth { get; private set; }
        public GridLength ColumnOneWidth { get; private set; }
        public int ElipseColumnNumber { get; private set; }
        public int MessageColumnNumber { get; private set; }
        public PointCollection PolygonPoints { get; private set; }

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
