namespace CopperNetworking.Backend;

public interface ICopperServer : ICopperPeer
{
    public Action<int, ArraySegment<byte>> OnData { get; set; }
    public Action<int> OnConnect { get; set; }
    public Action<int> OnDisconnect { get; set; }
    public void Start(int targetPort);
    public bool SendData(int targetClient, ArraySegment<byte> data);
    public void Update();
    public void Stop();
}