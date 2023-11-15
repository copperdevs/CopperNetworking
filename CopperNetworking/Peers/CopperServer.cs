using System.Text;
using System.Text.Json;
using CopperNetworking.Backend;
using CopperNetworking.Common;
using CopperNetworking.Data;
using CopperNetworking.Messages;

namespace CopperNetworking;

public class CopperServer<T> : CopperPeer where T : ICopperServer, new()
{
    private T Server;
    private Dictionary<int, ConnectedClient> ConnectedClients = new();
    public Action<ArraySegment<byte>> ByteDataReceived;
    public Action<int> ClientJoined;
    public Action<int> ClientLeft;

    public CopperServer(int targetPort = 7777)
    {
        CopperLogger.Initialize(CopperLogger.Info, CopperLogger.Warning, CopperLogger.Error);

        Log.Info($"Starting server");
        
        Server = new T();
        Server.Start(targetPort);
        Server.OnData += DataReceived;
        Server.OnConnect += ClientJoinedHandler;
        Server.OnDisconnect += ClientLeftHandler;
        
        Log.Info($"Started server");
        
        Task.Run(UpdateServer);
    }

    private void DataReceived(int client, ArraySegment<byte> bytes)
    {
        Log.Info($"Received Data - [ {string.Join(" ", bytes)} ]");
        var messageData = bytes;
        
        if (bytes.TryGetMessage(out var message))
        {
            Log.Info("Handling message");
            message!.MessageSender = client;
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
    
    public void SendMessage(MessageType type, Message message, int targetClient)
    {
        SendData(type, message.ToJson().ToByteArray(), targetClient);
    }

    public void SendData(MessageType type, byte[] bytes)
    {
        if (type == MessageType.FromClientToServer)
        {
            Log.Warning("Message being sent is from the wrong source. Message not being sent.");
            return;
        }

        if (type == MessageType.FromServerToClient)
        {
            Log.Warning("Message being sent to a client has not specified a client. Message not being sent.");
            return;
        }

        if (type == MessageType.FromServerToAllClientsButOne)
        {
            Log.Warning("Message being sent to all clients but one has not specified a client. Message not being sent.");
            return;
        }

        Log.Info($"Sending data to all clients - [ {string.Join(" ", bytes)} ]");

        foreach (var clients in GetConnectedClients())
        {
            Log.Info($"Sending data to client {clients.ClientId}");
            Server.SendData(clients.ClientId, bytes);
        }
    }

    public void SendData(MessageType type, byte[] bytes, int targetClient)
    {
        switch (type)
        {
            case MessageType.FromClientToServer:
                Log.Warning("Message being sent is from the wrong source. Message not being sent.");
                return;
            case MessageType.FromServerToAllClients:
                Log.Warning("Message being sent to all clients has specified a client. Sending to all clients.");
                SendData(MessageType.FromServerToAllClients, bytes);
                return;
            case MessageType.FromServerToClient:
                Server.SendData(targetClient, bytes);
                return;
            case MessageType.FromServerToAllClientsButOne:
                foreach (var clients in GetConnectedClients())
                {
                    if (clients.ClientId == targetClient)
                        continue;

                    Log.Info($"Sending data to client {clients.ClientId}");
                    Server.SendData(clients.ClientId, bytes);
                }
                return;
            default:
                Log.Error("No message type specified. Message not being sent.");
                return;
        }
        
    }
    private async Task UpdateServer()
    {
        while (!ShouldQuit)
        {
            Server.Update();
            await Task.Delay(TargetClientUpdateFps.ConvertFpsToMilliseconds());
        }
    }


    private void ClientJoinedHandler(int client)
    {
        Log.Info($"New client has joined - {client}");
        ConnectedClients.Add(client, new ConnectedClient(client));
        ClientJoined.Invoke(client);
    }

    private void ClientLeftHandler(int client)
    {
        Log.Info($"A client has left - {client}");
        ConnectedClients.Remove(client);
        ClientLeft.Invoke(client);
    }

    private List<ConnectedClient> GetConnectedClients()
    {
        return ConnectedClients.Values.ToList();
    }

    public void StopServer()
    {
        ShouldQuit = true;
        Server.Stop();
    }
}