using System.Collections.ObjectModel;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Input;

namespace Client.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private const int ActivityStatusTimerDuration = 600000;

        private readonly NavigationStore navigationStore;

        private Server server;
        private string message;
        private User mainUser;
        private ActivityStatus activityStatus;
        private ActivityStatus lastStatusBeforeChangedInternally;
        private Visibility mainUserContainerVisibility;
        private Visibility contactsListContainerVisibility;

        public MainViewModel(NavigationStore navigationStore)
        {
            this.navigationStore = navigationStore;

            this.Users = new ObservableCollection<User>();
            this.Messages = new ObservableCollection<Message>();
            this.server = new Server();
            this.server.UserConnectedEvent += this.OnUserConnected;
            this.server.MessageReceivedEvent += this.OnMessageReceived;
            this.server.UserDisconnectedEvent += this.OnUserDisconnected;
            this.server.UserActivityChangedEvent += OnActivityStatusChanged;
            this.ConnectToServerCommand = new RelayCommand(o => this.server.ConnectToServer(MainClientUsername), o => !string.IsNullOrEmpty(MainClientUsername));
            this.SendMessageCommand = new RelayCommand(o => this.SendMessageExecute(o));
            this.MainUserContainerVisibility = Visibility.Hidden;
            this.ContactsListContainerVisibility = Visibility.Hidden;
        }

        public static string MainClientUsername { get; set; }

        public ViewModelBase CurrentViewModel
        {
            get => this.navigationStore.CurrentViewModel;
        }

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

        public User MainUser
        {
            get
            {
                return this.mainUser;
            }
            set
            {
                if (this.mainUser != value)
                {
                    this.mainUser = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public ActivityStatus ActivityStatus
        {
            get
            {
                return this.activityStatus;
            }
            set
            {
                if (this.activityStatus != value)
                {
                    this.activityStatus = value;
                    this.OnPropertyChanged();
                    this.server.SendActivityStatusChanged(this.ActivityStatus);
                }
            }
        }

        public ObservableCollection<User> Users { get; set; }

        public ObservableCollection<Message> Messages { get; set; }

        public RelayCommand ConnectToServerCommand { get; set; }

        public RelayCommand SendMessageCommand { get; set; }

        public Visibility MainUserContainerVisibility
        {
            get
            {
                return this.mainUserContainerVisibility;
            }
            set
            {
                if (this.mainUserContainerVisibility != value)
                {
                    this.mainUserContainerVisibility = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public Visibility ContactsListContainerVisibility
        {
            get
            {
                return this.contactsListContainerVisibility;
            }
            set
            {
                if (this.contactsListContainerVisibility != value)
                {
                    this.contactsListContainerVisibility = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public void OnPreviewMouseClick(object sender, MouseButtonEventArgs e)
        {
            this.ResetActivityTimer();
        }

        public void OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            this.ResetActivityTimer();
        }

        public void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            this.ResetActivityTimer();
        }

        private void OnUserConnected()
        {
            User usr = new User
            {
                Username = this.server.PackageReader.ReadMessage(),
                UID = this.server.PackageReader.ReadMessage()
            };

            //////////////////////////////////////////////////////////////////////////////////
            //This should not work in this way. Refactor the logic into the server in order to send firstly connection successful and then only the new users.
            //whiout sending all users to all users.
            //////////////////////////////////////////////////////////////////////////////////
            if (!this.Users.Any(user => user.UID == usr.UID))
            {
                Application.Current.Dispatcher.Invoke(() => 
                {
                    if (usr.Username == MainClientUsername)
                    {
                        if (this.MainUser == null)
                        {
                            this.MainUser = usr;
                            this.MainUser.Status = this.ActivityStatus;
                            this.MainUser.StatusTimer = new Timer(ActivityStatusTimerDuration);
                            this.MainUser.StatusTimer.Elapsed += StatusTimer_Elapsed;
                            this.MainUser.StatusTimer.Start();
                            this.MainUserContainerVisibility = Visibility.Visible;
                        }

                        return;
                    }
                    else
                    {
                        if (this.Users.Count == 0)
                        {
                            this.ContactsListContainerVisibility = Visibility.Visible;
                        }

                        this.Users.Add(usr);
                    }
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

        private void OnActivityStatusChanged()
        {
            string message = this.server.PackageReader.ReadMessage();
            ActivityStatusMessage statusMessage = MessageService.CreateActivityStatusMessage(message);
            User usr = this.Users.First(usr => usr.Username == statusMessage.Author);
            usr.Status = statusMessage.Status;
        }

        private void SendMessageExecute(object o)
        {
            if (!string.IsNullOrEmpty(this.Message))
            {
                this.server.SendMessageToServer(this.Message);
                this.Message = string.Empty;
            }
        }

        private void StatusTimer_Elapsed(object sender, ElapsedEventArgs e)
         {
            this.lastStatusBeforeChangedInternally = this.ActivityStatus;
            this.ActivityStatus = ActivityStatus.Away;

            Timer timer = (Timer)sender;
            timer.Stop();
            
            this.server.SendActivityStatusChanged(this.ActivityStatus);
        }

        private void ResetActivityTimer()
        {
            if (this.MainUser != null)
            {
                Timer activityTimer = this.MainUser.StatusTimer;

                if (activityTimer.Enabled)
                {
                    activityTimer.Stop();
                }
                else
                {
                    this.ActivityStatus = this.lastStatusBeforeChangedInternally;

                    this.server.SendActivityStatusChanged(this.ActivityStatus);
                }

                activityTimer.Start();
            }
        }
    }
}
