namespace CopperNetworking.Backend;

public interface ICopperClient : ICopperPeer
{
    public Action<ArraySegment<byte>> OnData { get; set; }
    public Action OnConnect { get; set; }
    public Action OnDisconnect { get; set; }
    public void Connect(string targetIp, int targetPort);
    public bool SendData(ArraySegment<byte> data);
    public void Update();
    public void Disconnect();
}