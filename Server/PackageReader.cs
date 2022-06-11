using System.IO;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    public class PackageReader : BinaryReader
    {
        private NetworkStream stream;

        public PackageReader(NetworkStream stream) : base(stream)
        {
            this.stream = stream;
        }

        public string ReadMessage()
        {
            int length = ReadInt32();
            byte[] msgBuffer = new byte[length];
            this.stream.Read(msgBuffer, 0, length);
            return Encoding.ASCII.GetString(msgBuffer);
        }
    }
}
