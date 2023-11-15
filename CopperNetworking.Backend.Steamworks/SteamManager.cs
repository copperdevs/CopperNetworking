using System.Diagnostics;
using CopperNetworking.Backend.Steamworks.Base;
using CopperNetworking.Common;
using Steamworks;
using Steamworks.Data;

namespace CopperNetworking.Backend.Steamworks;

public class SteamManager : Singleton<SteamManager>
{
    private bool ShouldQuit = false;

    private Server steamSocketManager;
    private Client steamConnectionManager;
    private bool activeSteamSocketServer;
    private bool activeSteamSocketConnection;

    public bool IsHost;

    public SteamManager()
    {
        SteamNetworkingUtils.InitRelayNetworkAccess();
        Task.Run(UpdateTask);
    }

    private async Task UpdateTask()
    {
        while (!ShouldQuit)
        {
            UpdateSteamworks();
            await Task.Delay(TimeSpan.FromSeconds(0.02));
        }
    }

    private void UpdateSteamworks()
    {
        SteamClient.RunCallbacks();
        try
        {
            if (activeSteamSocketServer)
            {
                steamSocketManager.Receive();
            }

            if (activeSteamSocketConnection)
            {
                steamConnectionManager.Receive();
            }
        }
        catch
        {
            Log.Info("Error receiving data on socket/connection");
        }
    }

    private void CreateSteamSocketServer()
    {
        steamSocketManager = SteamNetworkingSockets.CreateRelaySocket<Server>(0);
        // Host needs to connect to own socket server with a ConnectionManager to send/receive messages
        // Relay Socket servers are created/connected to through SteamIds rather than "Normal" Socket Servers which take IP addresses
        steamConnectionManager = SteamNetworkingSockets.ConnectRelay<Client>(SteamClient.SteamId);
        activeSteamSocketServer = true;
        activeSteamSocketConnection = true;
    }

    private void JoinSteamSocketServer(SteamId targetSteamId)
    {
        if (!IsHost)
        {
            Log.Info("joining socket server");
            steamConnectionManager = SteamNetworkingSockets.ConnectRelay<Client>(targetSteamId, 0);
            activeSteamSocketServer = false;
            activeSteamSocketConnection = true;
        }
    }

    private void LeaveSteamSocketServer()
    {
        activeSteamSocketServer = false;
        activeSteamSocketConnection = false;
        try
        {
            // Shutdown connections/sockets. I put this in try block because if player 2 is leaving they don't have a socketManager to close, only connection
            steamConnectionManager.Close();
            steamSocketManager.Close();
        }
        catch
        {
            Log.Error("Error closing socket server / connection manager");
        }
    }

    public void RelaySocketMessageReceived(IntPtr message, int size, uint connectionSendingMessageId)
    {
        try
        {
            // Loop to only send messages to socket server members who are not the one that sent the message
            for (int i = 0; i < steamSocketManager.Connected.Count; i++)
            {
                if (steamSocketManager.Connected[i].Id != connectionSendingMessageId)
                {
                    Result success = steamSocketManager.Connected[i].SendMessage(message, size);
                    if (success != Result.OK)
                    {
                        Result retry = steamSocketManager.Connected[i].SendMessage(message, size);
                    }
                }
            }
        }
        catch
        {
            Log.Error("Unable to relay socket server message");
        }
    }

    public bool SendMessageToSocketServer(byte[] messageToSend)
    {
        try
        {
            // Convert string/byte[] message into IntPtr data type for efficient message send / garbage management
            int sizeOfMessage = messageToSend.Length;
            IntPtr intPtrMessage = System.Runtime.InteropServices.Marshal.AllocHGlobal(sizeOfMessage);
            System.Runtime.InteropServices.Marshal.Copy(messageToSend, 0, intPtrMessage, sizeOfMessage);
            Result success =
                steamConnectionManager.Connection.SendMessage(intPtrMessage, sizeOfMessage, SendType.Reliable);
            if (success == Result.OK)
            {
                System.Runtime.InteropServices.Marshal.FreeHGlobal(intPtrMessage); // Free up memory at pointer
                return true;
            }
            else
            {
                // RETRY
                Result retry =
                    steamConnectionManager.Connection.SendMessage(intPtrMessage, sizeOfMessage, SendType.Reliable);
                System.Runtime.InteropServices.Marshal.FreeHGlobal(intPtrMessage); // Free up memory at pointer
                if (retry == Result.OK)
                {
                    return true;
                }

                return false;
            }
        }
        catch (Exception e)
        {
            Log.Error($"Unable to send message to socket server - {e.Message}");
            return false;
        }
    }

    public void ProcessMessageFromSocketServer(IntPtr messageIntPtr, int dataBlockSize)
    {
        try
        {
            byte[] message = new byte[dataBlockSize];
            System.Runtime.InteropServices.Marshal.Copy(messageIntPtr, message, 0, dataBlockSize);
            string messageString = System.Text.Encoding.UTF8.GetString(message);

            // Do something with received message
        }
        catch
        {
            Log.Info("Unable to process message from socket server");
        }
    }
}