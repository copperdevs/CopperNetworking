namespace CopperNetworking.Backend.Steamworks;

public class SteamworksServer : Base.Server, ICopperServer
{
    public Action<int, ArraySegment<byte>> OnData { get; set; }
    public Action<int> OnConnect { get; set; }
    public Action<int> OnDisconnect { get; set; }
    public void Start(int targetPort)
    {
        throw new NotImplementedException();
    }

    public bool SendData(int targetClient, ArraySegment<byte> data)
    {
        throw new NotImplementedException();
    }

    public void Update()
    {
        throw new NotImplementedException();
    }

    public void Stop()
    {
        throw new NotImplementedException();
    }
}