using System;
using Windows.UI.Xaml;

namespace Orphee.Models
{
    public class MyDictionary
    {
        public Visibility MyMessageVisibility { get; private set; }
        public Visibility ItsMessageVisibility { get; private set; }
        public string Message { get; private set; }
        public string Hour { get; private set; }

        public MyDictionary(Visibility myMessageVisibility, Visibility itsMessageVisibility, string message)
        {
            this.MyMessageVisibility = myMessageVisibility;
            this.ItsMessageVisibility = itsMessageVisibility;
            this.Message = message;
            this.Hour = DateTime.Now.ToString("HH:mm");
        }
    }
}
