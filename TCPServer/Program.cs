using System.Threading;
using System;

namespace TCPServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread serverThread = new Thread(() => new Server("127.0.0.1" , 13000));
            serverThread.Start();
            
            // server is going to be launch from here
            Console.WriteLine("Server Started...");
        }
    }
}
