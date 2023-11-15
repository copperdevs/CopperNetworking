using System.Numerics;
using ImGuiNET;
using Raylib_cs;

namespace CopperChat.Client;

public static class ClientUi
{
    private static string ClientMessageInput = "";
    public static void Update()
    {
        WindowUpdate();
        // ImGui.ShowDemoWindow();
    }

    private static void WindowUpdate()
    {
        ImGui.SetNextWindowPos(new Vector2(0, 0));
        ImGui.SetNextWindowSize(new Vector2(Raylib.GetScreenWidth(), Raylib.GetScreenHeight()));
        
        const ImGuiWindowFlags windowFlags = ImGuiWindowFlags.NoDecoration | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoResize;
        
        if (ImGui.Begin("ImGui Window", windowFlags))
        {
            WindowContentUpdate();
            ImGui.End();
        }
    }

    private static void WindowContentUpdate()
    {
        ImGui.BeginGroup();
        ImGui.InputText("Input", ref ClientMessageInput, 32);
        ImGui.EndGroup();
            
        ImGui.BeginGroup();
        ImGui.InputText("Input", ref ClientMessageInput, 32);
        ImGui.EndGroup();
    }
}