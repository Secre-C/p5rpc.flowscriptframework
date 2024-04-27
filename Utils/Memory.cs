using System.Diagnostics;

namespace p5rpc.flowscriptframework.Utils;
internal static class Memory
{
    internal static readonly long baseAddress = Process.GetCurrentProcess().MainModule.BaseAddress.ToInt64();
    internal static readonly string PushCallerRegisters = "push rcx\npush rdx\npush r8\npush r9";
    internal static readonly string PopCallerRegisters = "pop r9\npop r8\npop rdx\npop rcx";
}
