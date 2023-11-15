namespace CopperNetworking.Common;

public static class ProgramUtils
{
    public static async Task KeepAlive(TimeSpan delay, Action tickEvent)
    {
        var keepAlive = true;
        while (keepAlive)
        {
            tickEvent.Invoke();
            await Task.Delay((int)delay.TotalMilliseconds);
        }
    }
}