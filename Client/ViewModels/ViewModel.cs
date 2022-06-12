using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Client
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private Server server;
        private string message;

        public MainViewModel()
        {
            this.Users = new ObservableCollection<User>();
            this.Messages = new ObservableCollection<Message>();
            this.server = new Server();
            this.server.UserConnectedEvent += this.OnUserConnected;
            this.server.MessageReceivedEvent += this.OnMessageReceived;
            this.server.UserDisconnectedEvent += this.OnUserDisconnected;
            this.ConnectToServerCommand = new RelayCommand(o => this.server.ConnectToServer(MainClientUsername), o => !string.IsNullOrEmpty(MainClientUsername));
            this.SendMessageCommand = new RelayCommand(o => this.SendMessageExecute(o));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public static string MainClientUsername { get; set; }

        public string Message
        {
            get
            {
                return this.message;
            }
            set
            {
                if (this.message != value)
                {
                    this.message = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<User> Users { get; set; }

        public ObservableCollection<Message> Messages { get; set; }

        public RelayCommand ConnectToServerCommand { get; set; }

        public RelayCommand SendMessageCommand { get; set; }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void OnUserConnected()
        {
            User usr = new User
            {
                Username = this.server.PackageReader.ReadMessage(),
                UID = this.server.PackageReader.ReadMessage()
            };

            if (!this.Users.Any(user => user.UID == usr.UID))
            {
                Application.Current.Dispatcher.Invoke(() => 
                {
                    this.Users.Add(usr);
                });
            }
        }

        private void OnMessageReceived()
        {
            string message = this.server.PackageReader.ReadMessage();
            Message msg = MessageService.CreateMessage(message);
            if (this.Messages.Count > 0 && this.Messages[this.Messages.Count - 1].Author == msg.Author)
            {
                msg.IsAuthorVisible = Visibility.Collapsed;
            }
            Application.Current.Dispatcher.Invoke(() => this.Messages.Add(msg));
        }

        private void OnUserDisconnected()
        {
            string uid = this.server.PackageReader.ReadMessage();
            User disconnectedUser = this.Users.Where(usr => usr.UID == uid).FirstOrDefault();
            Application.Current.Dispatcher.Invoke(() =>  this.Users.Remove(disconnectedUser));
        }

        private void SendMessageExecute(object o)
        {
            if (!string.IsNullOrEmpty(this.Message))
            {
                this.server.SendMessageToServer(this.Message);
                this.Message = string.Empty;
            }
        }
    }
}
