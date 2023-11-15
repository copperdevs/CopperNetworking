using CopperNetworking.Backend.Telepathy.Base;

namespace CopperNetworking.Backend.Telepathy;

public class TelepathyServer : Server, ICopperServer
{
    public TelepathyServer(int maxMessageSize) : base(maxMessageSize) { }
    public TelepathyServer() : base(16 * 1024) { }
    public new Action<int, ArraySegment<byte>> OnData { get => base.OnData; set => base.OnData = value; }
    public Action<int> OnConnect { get => OnConnected; set => OnConnected = value; }
    public Action<int> OnDisconnect { get => OnDisconnected; set => OnDisconnected = value; }
    public new void Start(int targetPort) => base.Start(targetPort);
    public bool SendData(int targetClient, ArraySegment<byte> data) => Send(targetClient, data);
    public void Update() => Tick(100);
    public new void Stop() => base.Stop();
}