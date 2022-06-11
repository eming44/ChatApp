using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    class Program
    {
        private static TcpListener listener;
        private static List<Client> users;

        public static void Main(string[] args)
        {
            users = new List<Client>();
            listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 3000);
            listener.Start();
            Console.WriteLine("Server is waiting for connection...");

            while(true)
            {
                Client client = new Client(listener.AcceptTcpClient());
                users.Add(client);
                BroadcastConnection();
            }
        }

        public static void BroadcastMessage(string message)
        {
            foreach (Client user in users)
            {
                PackageBuilder broadcastPackage = new PackageBuilder();
                broadcastPackage.WriteOpCode(3);
                broadcastPackage.WriteMessage(message);
                user.ClientSocket.Client.Send(broadcastPackage.GetPackageBytes());
            }
        }

        public static void BroadcastDisconnection(string uid)
        {
            Client disconnectedClient = users.Where(user => user.UID.ToString() == uid).FirstOrDefault();
            users.Remove(disconnectedClient);

            foreach (Client user in users)
            {
                PackageBuilder broadcastPackage = new PackageBuilder();
                broadcastPackage.WriteOpCode(4);
                broadcastPackage.WriteMessage(uid);
                user.ClientSocket.Client.Send(broadcastPackage.GetPackageBytes());
            }

            BroadcastMessage($"[{DateTime.Now}] ({disconnectedClient.UID}){disconnectedClient.UserName} has disconnected");
        }

        private static void BroadcastConnection()
        {
            foreach (Client user in users)
            {
                foreach (Client usr in users)
                {
                    PackageBuilder broadcastPackage = new PackageBuilder();
                    broadcastPackage.WriteOpCode(1);
                    broadcastPackage.WriteMessage(usr.UserName);
                    broadcastPackage.WriteMessage(usr.UID.ToString());
                    user.ClientSocket.Client.Send(broadcastPackage.GetPackageBytes());
                }
            }
        }
    }
}
