using CopperNetworking;
using CopperNetworking.Backend.Telepathy;
using CopperNetworking.Messages;

namespace CopperChat.Api;

public class CopperChatClient
{
    public CopperClient<TelepathyClient> Client;

    public CopperChatClient(string targetIp = "127.0.0.1", int targetPort = 7777)
    {
        Client = new CopperClient<TelepathyClient>(targetIp, targetPort);
        Client.AddMessageHandler((int)MessageIds.ClientJoined, ClientJoinedHandler);
        Client.AddMessageHandler((int)MessageIds.ClientLeft, ClientLeftHandler);
    }

    private void ClientJoinedHandler(Message? message)
    {
        Log.Info(message!.Data);
    }
    
    private void ClientLeftHandler(Message? message)
    {
        Log.Info(message!.Data);
    }

    public void Stop()
    {
        Client.StopClient();
    }
}