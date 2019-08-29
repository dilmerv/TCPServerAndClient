using System;
using System.Net.Sockets;
using System.Threading;
using TMPro;
using UnityEngine;

public class TCPClient : MonoBehaviour
{
    [SerializeField]
    private string serverIPAddress = "127.0.0.1";

    [SerializeField]
    private int port = 13000;

    [SerializeField]
    private TextMeshProUGUI outputText;

    void Awake()
    {
        // create singleton object before threads are created
        Dispatcher dispatcher = Dispatcher.Instance;
    }

    void Start()
    {
        // first client id
        int clientId = 1;

        new Thread(() => {
            Thread.CurrentThread.IsBackground = true;
            ConnectClient(serverIPAddress, port, clientId, $"ClientId: {clientId} sending a message...");
        }).Start();

        // second client id
        clientId++;

        new Thread(() => {
            Thread.CurrentThread.IsBackground = true;
            ConnectClient(serverIPAddress, port, clientId, $"ClientId: {clientId} sending a message...");
        }).Start();
    }

    private void Message(int clientId, string message)
    {
        Debug.Log($"ClientId : {clientId}");
        Debug.Log($"{message}");

        string clientConnected = $"ClientId : {clientId}";
        string clientMessage = $"{message}";
        outputText.text += $"{clientConnected}\n{clientMessage}\n\n";
    }

    private void ConnectClient(string server, int port, int clientId, string message)
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

                Dispatcher.Instance.Enqueue(() => Message(clientId, $"Sent: {message}"));

                data = new byte[256];
                int bytes = stream.Read(data, 0, data.Length);
                string response = System.Text.Encoding.ASCII.GetString(data, 0, bytes);

                Dispatcher.Instance.Enqueue(() => Message(clientId, $"Received: {response}"));

                Thread.Sleep(2000);
            }

            stream.Close();
            client.Close();
        }
        catch(Exception e)
        {
            Console.WriteLine($"Exception : {e}");
            Dispatcher.Instance.Enqueue(() => Message(clientId, $"Exception: {e.Message}"));
        }

        Console.Read();
    }
}
