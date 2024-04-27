using Reloaded.Mod.Interfaces;
using System.Drawing;

namespace p5rpc.flowscriptframework.Utils;
internal static class Logger
{
    internal static ILogger? logger { get; set; }
    internal static bool Debug { get; set; }

    private static void LogAsync(string prefix, string message, Color color)
    {
        logger.WriteAsync($"{prefix} {message}", color);
    }
    private static void Log(string prefix, string message, Color color)
    {
        logger.Write($"{prefix} {message}", color);
    }
    internal static void Log(string message, Color color)
    {
        LogAsync("[FlowscriptFramework]", message, color);
    }

    internal static void Log(string message)
    {
        Log(message, Color.LightGray);
    }

    internal static void DebugLog(string message, Color color)
    {
        if (Debug)
            Log("\n[FlowscriptFramework DEBUG]", message, color);
    }

    internal static void DebugLog(string message)
    {
        DebugLog(message, Color.LightCyan);
    }
}
