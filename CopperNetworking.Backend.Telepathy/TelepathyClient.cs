namespace CopperNetworking.Backend.Telepathy;

public class TelepathyClient : Base.Client, ICopperClient
{
    public TelepathyClient(int maxMessageSize) : base(maxMessageSize) { }
    public TelepathyClient() : base(16 * 1024) { }
    public new Action<ArraySegment<byte>> OnData { get => base.OnData; set => base.OnData = value; }
    public Action OnConnect { get => base.OnConnected; set => base.OnConnected = value; }
    public Action OnDisconnect { get => base.OnDisconnected; set => base.OnDisconnected = value; }
    public bool SendData(ArraySegment<byte> data) => Send(data);
    public void Update() => Tick(100);
    public new void Disconnect() => base.Disconnect();
}