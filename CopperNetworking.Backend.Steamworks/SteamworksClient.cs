namespace CopperNetworking.Backend.Steamworks;

public class SteamworksClient : Base.Client, ICopperClient
{
    public Action<ArraySegment<byte>> OnData { get; set; }
    public Action OnConnect { get; set; }
    public Action OnDisconnect { get; set; }
    public void Connect(string targetIp, int targetPort)
    {
        throw new NotImplementedException();
    }

    public bool SendData(ArraySegment<byte> data)
    {
        throw new NotImplementedException();
    }

    public void Update()
    {
        throw new NotImplementedException();
    }

    public void Disconnect()
    {
        throw new NotImplementedException();
    }
}