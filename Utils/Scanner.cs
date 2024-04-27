using Reloaded.Memory.SigScan.ReloadedII.Interfaces;
using static p5rpc.flowscriptframework.Utils.Logger;
using static p5rpc.flowscriptframework.Utils.Memory;

namespace p5rpc.flowscriptframework.Utils;
internal static class Scanner
{
    internal static IStartupScanner scanner { get; set; }
    internal static void SigScan(string pattern, Action<nint> action) => SigScan(pattern, string.Empty, action);

    internal static void SigScan(string pattern, string name, Action<nint> action)
    {
        scanner.AddMainModuleScan(pattern, (result) =>
        {
            if (!result.Found)
            {
                if (name == string.Empty)
                    name = pattern;

                throw new Exception($"Could not find address for {name}");
            }

            DebugLog($"Found {name} at 0x{baseAddress + result.Offset:X}");
            action.Invoke((nint)(baseAddress + result.Offset));
        });
    }
}
