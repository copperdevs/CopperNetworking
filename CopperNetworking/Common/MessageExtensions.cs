using System.Text.Json;
using CopperNetworking.Messages;
using Extensions = CopperNetworking.Common.Extensions;

namespace CopperNetworking.Common;

public static class MessageExtensions
{
    public static bool TryGetMessage(this IEnumerable<byte> messageData, out Message? message)
    {
        try
        {
            var data = new string(messageData.Select(Convert.ToChar).ToArray());
            message = JsonSerializer.Deserialize<Message>(data);
            return true;
        }
        catch (Exception e)
        {
            Log.Error($"Error getting message - {e}");
            message = null;
            return false;
        }
    }
}