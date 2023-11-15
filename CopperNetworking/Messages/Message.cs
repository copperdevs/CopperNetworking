using System.Text.Json.Serialization;
using CopperNetworking.Common;

namespace CopperNetworking.Messages;

public class Message
{
    [JsonConstructor]
    public Message() { }
    
    public Message(int messageId, object data)
    {
        MessageId = messageId;
        Data = data.ToJson();
    }
    
    public Message(int messageId, string data)
    {
        MessageId = messageId;
        Data = data;
    }

    public string Data { get; set; }
    public int MessageId { get; set; }
    /// <summary>
    /// -1 for server, positive for clients
    /// </summary>
    public int MessageSender { get; set; }
}