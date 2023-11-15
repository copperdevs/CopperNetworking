using CopperNetworking.Common;
using static System.Console;
using System;

namespace CopperNetworking;

public static class CopperLogger
{
    private static bool initialized = false;
    public delegate void LogMethod(string message);

    public static LogMethod InfoLog = Info;
    public static LogMethod WarningLog = Warning;
    public static LogMethod ErrorLog = Error;
    
    public static void Initialize(LogMethod info, LogMethod warning, LogMethod error)
    {
        if (initialized)
            return;
        
        initialized = true;

        InfoLog = info;
        WarningLog = warning;
        ErrorLog = error;
        
        Log.Info($"Initialized {nameof(CopperLogger)}");
    }
    
    
    public static void Info(string message) => BaseLog(message, ConsoleColor.DarkGray);
    public static void Warning(string message) => BaseLog(message, ConsoleColor.DarkYellow);
    public static void Error(string message) => BaseLog(message, ConsoleColor.DarkRed);

    private static void BaseLog(string message, ConsoleColor color)
    {
        ForegroundColor = color;
        WriteLine(message);
        ResetColor();
    }
}