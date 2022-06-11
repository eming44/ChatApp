using System;
using System.Windows;

namespace TCPClientWPF
{
    public class Message
    {
        public string Author { get; set; }

        public Visibility IsAuthorVisible { get; set; } = Visibility.Visible;

        public string FullText { get; set; }

        public string NamedText { get; set; }

        public string Text { get; set; }

        public DateTime SentTime { get; set; }

        public string Time
        {
            get
            {
                return this.SentTime.ToString("HH:mm");
            }
        }
    }
}
