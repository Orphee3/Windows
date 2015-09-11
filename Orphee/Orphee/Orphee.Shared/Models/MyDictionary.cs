using System;
using Windows.UI.Xaml;

namespace Orphee.Models
{
    public class MyDictionary
    {
        public Visibility MyMessageVisibility { get; private set; }
        public Visibility ItsMessageVisibility { get; private set; }
        public string Message { get; private set; }
        public string Hour { get; set; }

        public MyDictionary(Visibility myMessageVisibility, Visibility itsMessageVisibility, string message, DateTime date)
        {
            this.MyMessageVisibility = myMessageVisibility;
            this.ItsMessageVisibility = itsMessageVisibility;
            this.Message = message;
            this.Hour = date.ToString("HH:mm");
        }
    }
}
