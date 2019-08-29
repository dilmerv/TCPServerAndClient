using System.Threading;
using System.Net.Sockets;
using System;

namespace ConsoleTCPClient
{
    class Program
    {
        static void Main(string[] args)
        {
            new Thread(() => {
                Thread.CurrentThread.IsBackground = true;
                ConnectClient("127.0.0.1", 13000, $"ClientId: {1} sending a message...");
            }).Start();

            new Thread(() => {
                Thread.CurrentThread.IsBackground = true;
                ConnectClient("127.0.0.1", 13000, $"ClientId: {2} sending a message...");
            }).Start();

            new Thread(() => {
                Thread.CurrentThread.IsBackground = true;
                ConnectClient("127.0.0.1", 13000, $"ClientId: {3} sending a message...");
            }).Start();

            new Thread(() => {
                Thread.CurrentThread.IsBackground = true;
                ConnectClient("127.0.0.1", 13000, $"ClientId: {4} sending a message...");
            }).Start();

            Console.ReadLine();
        }

        private static void ConnectClient(string server, int port, string message)
        {
            try
            {
                TcpClient client = new TcpClient(server, port);
                NetworkStream stream = client.GetStream();
                int count = 0;

                while(count++ < 3)
                {
                    byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
                    stream.Write(data, 0, data.Length);
                    Console.WriteLine($"Sent: {message}");

                    data = new byte[256];
                    int bytes = stream.Read(data, 0, data.Length);
                    string response = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                    
                    Console.WriteLine($"Received: {response}");

                    Thread.Sleep(2000);
                }

                stream.Close();
                client.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine($"Exception : {e}");
            }

            Console.Read();
        }
    }
}
