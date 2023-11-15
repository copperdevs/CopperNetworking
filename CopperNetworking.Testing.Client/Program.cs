using CopperNetworking.Backend.Telepathy;
using CopperNetworking.Messages;

namespace CopperNetworking.Testing.Client;

public static class Program
{
    private static CopperClient<TelepathyClient> Client;
    
    public static async Task Main()
    {
        CopperLogger.Initialize(CopperLogger.Info, CopperLogger.Warning, CopperLogger.Error);
        Client = new CopperClient<TelepathyClient>();
        Client.AddMessageHandler(0, TimeMessageReceived);
        await KeepAlive();
    }

    private static void TimeMessageReceived(Message? message)
    {
        Log.Info(message!.Data);
    }

    private static async Task KeepAlive()
    {
        while (!Client.ShouldQuit)
        {
            Log.Info("Keep alive");
            await Task.Delay(TimeSpan.FromSeconds(5));
        }
    }
}