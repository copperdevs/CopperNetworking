using CopperNetworking.Backend;
using CopperNetworking.Common;
using CopperNetworking.Data;
using CopperNetworking.Messages;

namespace CopperNetworking;

public class CopperClient<T> : CopperPeer where T : ICopperClient, new()
{
    private ICopperClient Client;
    public Action<ArraySegment<byte>> ByteDataReceived;
    
    public CopperClient(string targetIp = "localhost", int targetPort = 7777)
    {
        CopperLogger.Initialize(CopperLogger.Info, CopperLogger.Warning, CopperLogger.Error);
        
        Log.Info($"Starting client");
        
        Client = new T();
        Client.Connect(targetIp, targetPort);
        Client.OnData += DataReceived;
        Client.OnConnect += ClientJoinedHandler;
        Client.OnDisconnect += ClientLeftHandler;

        Log.Info($"Started client");
        
        Task.Run(UpdateClient);
    }

    private void DataReceived(ArraySegment<byte> bytes)
    {
        Log.Info($"Received new data - [{string.Join(",", bytes)}]");
        var messageData = bytes;
        
        if (bytes.TryGetMessage(out var message))
        {
            Log.Info("Handling message");
            message!.MessageSender = -1;
            HandleMessage(message);
        }
        else
        {
            Log.Info("Byte data received");
            ByteDataReceived.Invoke(bytes);
        }
    }

    public void SendMessage(MessageType type, Message message)
    {
        SendData(type, message.ToJson().ToByteArray());
    }
    
    public void SendData(MessageType type, byte[] bytes)
    {
        switch (type)
        {
            case MessageType.FromServerToAllClients:
                Log.Warning("Message being sent is to the client, but we are a client. Message not being sent.");
                return;
            case MessageType.FromServerToClient:
                Log.Warning("Message being sent is to all clients, but we are a client. Message not being sent.");
                SendData(MessageType.FromServerToAllClients, bytes);
                return;
        }
        
        Client.SendData(bytes);
    }

    private async Task UpdateClient()
    {
        while (!ShouldQuit)
        {
            Client.Update();
            await Task.Delay(TargetClientUpdateFps.ConvertFpsToMilliseconds());
        }
    }

    private void ClientJoinedHandler()
    {
        Log.Info("Joined Server");
    }

    private void ClientLeftHandler()
    {
        Log.Info("Left Server");
    }

    public void StopClient()
    {
        ShouldQuit = true;
        Client.Disconnect();
    }
}