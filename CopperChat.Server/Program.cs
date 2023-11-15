using CopperChat.Api;
using CopperNetworking;
using CopperNetworking.Common;

namespace CopperChat.Server;

public static class Program
{
    public static CopperChatServer? Server;
    
    public static async Task Main()
    {
        Server = new CopperChatServer(7777);
        await ProgramUtils.KeepAlive(TimeSpan.FromSeconds(5), () => Log.Info("Keep Alive"));
    }
}