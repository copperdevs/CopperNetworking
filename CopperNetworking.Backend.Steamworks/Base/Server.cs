using Steamworks;
using Steamworks.Data;

namespace CopperNetworking.Backend.Steamworks.Base;

public class Server : SocketManager
{
    public override void OnConnecting(Connection connection, ConnectionInfo data)
    {
        base.OnConnecting(connection, data);//The base class will accept the connection
        Log.Info("SocketManager OnConnecting");
    }

    public override void OnConnected(Connection connection, ConnectionInfo data)
    {
        base.OnConnected(connection, data);
        Log.Info("New player connecting");
    }

    public override void OnDisconnected(Connection connection, ConnectionInfo data)
    {
        base.OnDisconnected(connection, data);
        Log.Info("Player disconnected");
    }

    public override void OnMessage(Connection connection, NetIdentity identity, IntPtr data, int size, long messageNum, long recvTime, int channel)
    {
        // Socket server received message, forward on message to all members of socket server
        SteamManager.Instance.RelaySocketMessageReceived(data, size, connection.Id);
        Log.Info("Socket message received");
    }
}