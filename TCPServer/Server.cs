using System.Text;
using System.Threading;
using System;
using System.Net;
using System.Collections.Generic;
using System.Net.Sockets;

namespace TCPServer
{
    public class Server
    {
        private static object _lock = new object();

        private readonly TcpListener server;

        private int clientCount = 1;

        private static Dictionary<int, TcpClient> clients = new Dictionary<int, TcpClient>();

        public Server(string ip, int port)
        {
            IPAddress localAddress = IPAddress.Parse(ip);
            server = new TcpListener(localAddress, port);
            server.Start();
            StartListener();
        }

        public void StartListener()
        {
            try
            {
                while(true)
                {  
                    Console.WriteLine("Waiting for client connections...");
                    TcpClient client = server.AcceptTcpClient();

                    lock(_lock)
                    {
                        clients.Add(clientCount, client);
                    }

                    Console.WriteLine("ClientId {clientCount} is connected and added to our clients...");
                    Thread thread = new Thread(HandleClientConnection);
                    thread.Start(clientCount);

                    clientCount++;
                }
            }
            catch(SocketException e)
            {
                Console.WriteLine($"SSocketException: {e}");
                server.Stop();
            }
        }

        public void HandleClientConnection(Object obj)
        {
            int clientId = (int)obj;

            TcpClient client = null;

            lock(_lock)
            {
                client = clients[clientId];
            }

            while(true)
            {
                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[1024];
                int byteCount = stream.Read(buffer, 0, buffer.Length);

                if(byteCount == 0)
                {
                    break;
                }

                string data = Encoding.ASCII.GetString(buffer, 0, byteCount);
                // send data to all clients
                Broacast(data);
                Console.WriteLine($"ClientId {clientId} is broacasting: {data}");
            }

            lock(_lock)
            {
                clients.Remove(clientId);
            }

            client.Client.Shutdown(SocketShutdown.Both);
            client.Close();
        }

        public static void Broacast(string message)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(message + Environment.NewLine);

            lock(_lock)
            {
                foreach(TcpClient client in clients.Values)
                {
                    NetworkStream stream = client.GetStream();
                    stream.Write(buffer, 0, buffer.Length);
                }
            }
        }
    }
}