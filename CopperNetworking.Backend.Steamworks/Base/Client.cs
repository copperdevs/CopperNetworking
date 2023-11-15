using System.Diagnostics;
using Steamworks;
using Steamworks.Data;

namespace CopperNetworking.Backend.Steamworks.Base;

public class Client : ConnectionManager
{
    public override void OnConnected(ConnectionInfo info)
    {
        base.OnConnected(info);
        Log.Info("ConnectionOnConnected");
    }

    public override void OnConnecting(ConnectionInfo info)
    {
        base.OnConnecting(info);
        Log.Info("ConnectionOnConnecting");
    }

    public override void OnDisconnected(ConnectionInfo info)
    {
        base.OnDisconnected(info);
        Log.Info("ConnectionOnDisconnected");
    }

    public override void OnMessage(IntPtr data, int size, long messageNum, long recvTime, int channel)
    {
        // Message received from socket server, delegate to method for processing
        SteamManager.Instance.ProcessMessageFromSocketServer(data, size);
        Log.Info("Connection Got A Message");
    }
}