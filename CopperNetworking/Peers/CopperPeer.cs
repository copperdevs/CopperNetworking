using CopperNetworking.Backend;
using CopperNetworking.Messages;

namespace CopperNetworking;

public abstract class CopperPeer
{
    public bool ShouldQuit { get; protected set; } = false;
    protected readonly int TargetClientUpdateFps = 50;

    private Dictionary<int, Action<Message?>> MessageHandlers = new();

    public void AddMessageHandler(int messageId, Action<Message?> messageHandler)
    {
        MessageHandlers.Add(messageId, messageHandler);
    }

    protected void HandleMessage(Message? message)
    {
        MessageHandlers[message!.MessageId].Invoke(message);
    }
    
}