using System.Numerics;
using CopperChat.Api;
using CopperChat.Common;
using CopperNetworking;
using ImGuiNET;
using static Raylib_cs.Raylib;

namespace CopperChat.Client;

public static class Program
{
    private static CopperChatClient? Client;
    private static Window? Window;
    private static bool WindowShouldClose = false;

    
    public static async Task Main()
    {
        Client = new CopperChatClient("127.0.0.1", 7777);
        
        Window = new Window("Chat Client");
        Window.WindowStarted += WindowStarted;
        Window.WindowUpdate += UpdateWindow;
        Window.WindowStopped += StopClient;
        
        while (!WindowShouldClose)
        {
            Log.Info("Keep Alive");
            await Task.Delay((int)TimeSpan.FromSeconds(5).TotalMilliseconds);
        }
    }

    private static void WindowStarted()
    {
        ImGui.GetIO().ConfigWindowsMoveFromTitleBarOnly = true;
    }
    
    private static void UpdateWindow()
    {
        ClientUi.Update();
    }
    
    private static void StopClient()
    {
        WindowShouldClose = true;
        Client?.Stop();
    }
}