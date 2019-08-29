# TCPServerAndClient

This project contains examples for creating a TCP Server in .NET Core, a TCP Client in .NET Core, and also a TCP Client for Unity3d projects.

Examples listed below:


### TCP Server:

To Start a server:

```csharp
  Thread serverThread = new Thread(() => new Server("127.0.0.1" , 13000));
  serverThread.Start();
            
```

### Unity TCP Client:

To create a new client connection from Unity:

```csharp
    new Thread(() => {
        Thread.CurrentThread.IsBackground = true;
        ConnectClient(serverIPAddress, port, clientId, $"ClientId: {clientId} sending a message...");
    }).Start();
        
```

To Dispatch events / messages to the main thread:

```csharp
  Dispatcher.Instance.Enqueue(() => Message(clientId, $"Sent: {message}"));      
```

### Console TCP Client

```csharp
  new Thread(() => {
      Thread.CurrentThread.IsBackground = true;
      ConnectClient("127.0.0.1", 13000, $"ClientId: {1} sending a message...");
  }).Start();
```
