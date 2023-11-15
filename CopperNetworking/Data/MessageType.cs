namespace CopperNetworking.Data;

public enum MessageType : uint
{
    FromServerToClient,
    FromServerToAllClients,
    FromClientToServer,
    
    // TODO: Implement this
    FromServerToAllClientsButOne
}