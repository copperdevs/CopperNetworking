using CopperNetworking.Backend;
using CopperNetworking.Backend.Telepathy;
using CopperNetworking.Common;
using CopperNetworking.Data;
using CopperNetworking.Messages;

namespace CopperNetworking.Testing.Server;

public static class Program
{
    private static CopperServer<TelepathyServer> Server;
    
    public static async Task Main()
    {
        
        CopperLogger.Initialize(CopperLogger.Info, CopperLogger.Warning, CopperLogger.Error);
        Server = new CopperServer<TelepathyServer>();
        await KeepAlive();
    }

    private static async Task KeepAlive()
    {
        while (!Server.ShouldQuit)
        {
            var currentTime = DateTime.Now.ToLongTimeString();
            Log.Info(currentTime);
            Server.SendMessage(MessageType.FromServerToAllClients, new Message(0, currentTime));
            await Task.Delay(TimeSpan.FromSeconds(1));
        }
    }
}