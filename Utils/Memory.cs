using System.Diagnostics;

namespace p5rpc.flowscriptframework.Utils;
internal static class Memory
{
    internal static readonly long baseAddress = Process.GetCurrentProcess().MainModule.BaseAddress.ToInt64();
    internal static readonly string PushCallerRegisters = "push rcx\npush rdx\npush r8\npush r9";
    internal static readonly string PopCallerRegisters = "pop r9\npop r8\npop rdx\npop rcx";

    internal unsafe static nint GetAddressFromGlobalRef(nint instructionAdr, byte length, string name = "")
    {
        int opd = *(int*)(instructionAdr + length - 4);
        nint result = instructionAdr + opd + length;

        if (name == string.Empty)
            Logger.DebugLog($"Found Global ref at 0x{result:X} from 0x{instructionAdr:X}");
        else
            Logger.DebugLog($"Found Global ref {name} at 0x{result:X} from 0x{instructionAdr:X}");

        return result;
    }
}
