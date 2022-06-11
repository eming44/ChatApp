using System;
using System.IO;
using System.Text;

namespace TCPClientWPF
{
    public class PackageBuilder
    {
        private MemoryStream stream;

        public PackageBuilder()
        {
            this.stream = new MemoryStream();
        }

        public void WriteOpCode(byte opcode)
        {
            this.stream.WriteByte(opcode);
        }

        public void WriteMessage(string msg)
        {
            this.stream.Write(BitConverter.GetBytes(msg.Length));
            this.stream.Write(Encoding.ASCII.GetBytes(msg));
        }

        public byte[] GetPackageBytes()
        {
            return this.stream.ToArray();
        }
    }
}
