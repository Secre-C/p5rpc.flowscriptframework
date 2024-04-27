using Reloaded.Hooks.Definitions;
using System.Runtime.InteropServices;

namespace p5rpc.flowscriptframework.structs;
internal unsafe struct FlowFunctionInfo
{
    nint FunctionPointer;
    long ArgCount;
    nint FunctionName;

    internal FlowFunctionInfo(FlowFunction flowFunction)
    {
        FunctionName = Marshal.StringToHGlobalAnsi(flowFunction.FunctionName);
        ArgCount = flowFunction.ArgCount;
        FunctionPointer = 0;
    }
}
