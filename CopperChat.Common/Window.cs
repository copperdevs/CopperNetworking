using ImGuiNET;
using Raylib_cs;
using rlImGui_cs;
using static Raylib_cs.Raylib;

namespace CopperChat.Common;

public class Window
{
    public Action WindowStarted;
    public Action WindowUpdate;
    public Action WindowStopped;

    private int WindowWidth = 800;
    private int WindowHeight = 650;
    private string WindowTitle = "Window";
    
    public Window()
    {
        Task.Run(WindowTask);
    }
    
    public Window(string title = "Window")
    {
        WindowTitle = title;
        Task.Run(WindowTask);
    }
    
    public Window(int width = 800, int height = 650, string title = "Window")
    {
        WindowWidth = width;
        WindowHeight = height;
        WindowTitle = title;
        Task.Run(WindowTask);
    }

    private void StartWindow()
    {
        SetConfigFlags(ConfigFlags.FLAG_MSAA_4X_HINT | ConfigFlags.FLAG_VSYNC_HINT | ConfigFlags.FLAG_WINDOW_RESIZABLE);
        InitWindow(WindowWidth, WindowHeight, WindowTitle);
        SetTargetFPS(144);
        rlImGui.Setup();
        WindowStarted.Invoke();
    }
    
    private Task WindowTask()
    {
        StartWindow();
        
        while (!WindowShouldClose())
        {
            BeginDrawing();
            ClearBackground(Color.DARKGRAY);

            rlImGui.Begin();
            WindowUpdate.Invoke();
            rlImGui.End();

            EndDrawing();
        }

        CloseWindow();
        return Task.CompletedTask;
    }

    private void CloseWindow()
    {
        WindowStopped.Invoke();
        rlImGui.Shutdown();
        Raylib.CloseWindow();
    }
}