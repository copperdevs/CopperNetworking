namespace CopperNetworking;

public static class Log
{
    public static void Info(string message) => CopperLogger.InfoLog(message);
    public static void Warning(string message) => CopperLogger.WarningLog(message);
    public static void Error(string message) => CopperLogger.ErrorLog(message);
}