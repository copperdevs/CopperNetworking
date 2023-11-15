using CopperNetworking;
using CopperNetworking.Backend.Telepathy;
using CopperNetworking.Data;
using CopperNetworking.Messages;

namespace CopperChat.Api;

public class CopperChatServer
{
    private CopperServer<TelepathyServer> Server;

    public CopperChatServer(int targetPort = 7777)
    {
        Server = new CopperServer<TelepathyServer>(targetPort);
        Server.ClientJoined += ClientJoinedHandler;
        Server.ClientLeft += ClientLeftHandler;
    }

    private void ClientJoinedHandler(int client)
    {
        Server.SendMessage(MessageType.FromServerToAllClientsButOne, new Message((int)MessageIds.ClientJoined, $"Client Joined - [{client}]"), client);
    }
    
    private void ClientLeftHandler(int client)
    {
        Server.SendMessage(MessageType.FromServerToAllClients, new Message((int)MessageIds.ClientLeft, $"Client Left - [{client}]"));
    }

    public void Stop()
    {
        Server.StopServer();
    }
}