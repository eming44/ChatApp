using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Client
{
    public class Server
    {
        private TcpClient client;

        public Server()
        {
            this.client = new TcpClient();
        }

        public PackageReader PackageReader { get; set; }

        public event Action UserConnectedEvent;
        public event Action MessageReceivedEvent;
        public event Action UserDisconnectedEvent;
        public event Action UserActivityChangedEvent;

        public void ConnectToServer(string username)
        {
            if (!this.client.Connected)
            {
                this.client.Connect("127.0.0.1", 3000);
                this.PackageReader = new PackageReader(this.client.GetStream());

                if (!string.IsNullOrEmpty(username))
                {
                    PackageBuilder connectPackage = new PackageBuilder();
                    connectPackage.WriteOpCode(0);
                    connectPackage.WriteMessage(username);
                    this.client.Client.Send(connectPackage.GetPackageBytes());
                }

                this.ReadPackages();
            }
        }

        public void SendMessageToServer(string message)
        {
            PackageBuilder messagePackage = new PackageBuilder();
            messagePackage.WriteOpCode(3);
            messagePackage.WriteMessage(message);
            this.client.Client.Send(messagePackage.GetPackageBytes());
        }

        public void SendActivityStatusChanged(ActivityStatus status)
        {
            PackageBuilder messagePackage = new PackageBuilder();
            messagePackage.WriteOpCode(5);
            messagePackage.WriteMessage(status.ToString());
            this.client.Client.Send(messagePackage.GetPackageBytes());
        }

        private void ReadPackages()
        {
            Task.Run(() => 
            {
                while(true)
                {
                    byte opcode = this.PackageReader.ReadByte();

                    switch(opcode)
                    {
                        case 1:
                            this.UserConnectedEvent?.Invoke();
                            break;
                        case 3:
                            this.MessageReceivedEvent?.Invoke();
                            break;
                        case 4:
                            this.UserDisconnectedEvent?.Invoke();
                            break;
                        case 5:
                            this.UserActivityChangedEvent?.Invoke();
                            break;
                        default:
                            Console.WriteLine("Package OPCODE not found");
                            break;
                    }
                }
            });
        }
    }
}
