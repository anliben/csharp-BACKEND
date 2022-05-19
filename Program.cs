using System.Net;
using System;
using System.Net.Sockets;
using System.Text;

namespace SportPlayer
{
    internal class Program
    {
        private static TcpListener _listener;

        [Obsolete]
        static void Main(string[] args)
        {
            _listener = new TcpListener(IPAddress.Any, 5000);
            _listener.Start();
            while (true)
            {
                TcpClient client = _listener.AcceptTcpClient();

                NetworkStream stream = client.GetStream();
                Task.Run(() => {

                while (true)
                {
                    if (stream.DataAvailable)
                    {
                        byte[] receivedBytes = ReadToEnd(stream);
                        string receivedText = Encoding.UTF8.GetString(receivedBytes);

                        Send(stream, "connect");
                        Console.WriteLine("Data available: " + receivedText);
                    }else{
                        Thread.Sleep(1);
                    }
                }
            });
            }
        }

        private static byte[] ReadToEnd(NetworkStream stream)
        {
            List<byte> receivedBytes = new List<byte>();
            while (stream.DataAvailable)
            {
                byte[] buffer = new byte[1024];

                stream.Read(buffer, 0, buffer.Length);
                receivedBytes.AddRange(buffer);
                receivedBytes.RemoveAll(b => b == 0);
            }
                return receivedBytes.ToArray();
        }

        private static void Send(NetworkStream stream, string text)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(text);
            stream.Write(buffer, 0, buffer.Length);
        }
    }  
}