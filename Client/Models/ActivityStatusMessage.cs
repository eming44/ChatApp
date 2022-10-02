using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class ActivityStatusMessage
    {
        public string Author { get; set; }

        public ActivityStatus Status { get; set; }

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
