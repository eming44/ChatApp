using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Server
{
    public class Client
    {
        private PackageReader packageReader;

        public Client(TcpClient client)
        {
            this.ClientSocket = client;
            this.UID = Guid.NewGuid();
            this.packageReader = new PackageReader(client.GetStream());
            byte opcode = this.packageReader.ReadByte();
            this.UserName = this.packageReader.ReadMessage();

            Console.WriteLine($"[{DateTime.Now}]: {this.UserName} has connected");

            Task.Run(this.ProcessReceivedData);
        }

        public string UserName { get; set; }

        public Guid UID { get; set; }

        public TcpClient ClientSocket { get; set; }

        private void ProcessReceivedData()
        {
            while (true)
            {
                try
                {
                    byte opcode = this.packageReader.ReadByte();
                    string message = this.packageReader.ReadMessage();

                    if (opcode == 3)
                    {
                        Console.WriteLine($"[{DateTime.Now}] {this.UserName}: {message}");
                        Program.BroadcastMessage($"[{DateTime.Now}] {this.UserName}: {message}");
                    }

                    if (opcode == 5)
                    {
                        Console.WriteLine($"[{DateTime.Now}] {this.UserName}: changed status to [{message}]");
                        Program.BroadCastActivityStatusChanged($"[{DateTime.Now}] {this.UserName}: {message}");
                    }
                }
                catch (Exception)
                {
                    ClientSocket.Close();
                    Console.WriteLine($"[{DateTime.Now}] ({this.UID}){this.UserName} has disconnected");
                    Program.BroadcastDisconnection(this.UID.ToString());
                    break;
                }
            }
        }
    }
}
