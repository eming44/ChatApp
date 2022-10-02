using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Timers;

namespace Client
{
    public class User : INotifyPropertyChanged
    {
        private ActivityStatus status;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Username { get; set; }

        public string UID { get; set; }

        public bool IsMainUser { get; set; } = false;

        public ActivityStatus Status
        {
            get
            {
                return this.status;
            }
            set
            {
                if (this.status != value)
                {
                    this.status = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public Timer StatusTimer { get; set; }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
