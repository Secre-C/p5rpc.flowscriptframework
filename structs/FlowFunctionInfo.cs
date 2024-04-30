using System.Runtime.InteropServices;

namespace p5rpc.flowscriptframework.structs;

[StructLayout(LayoutKind.Explicit)]
internal unsafe class FlowFunctionInfo
{
    [FieldOffset(0)]
    nint FunctionPointer;
    [FieldOffset(8)]
    int ArgCount;
    [FieldOffset(0x10)]
    nint FunctionName;

    internal FlowFunctionInfo(FlowFunction flowFunction)
    {
        FunctionName = Marshal.StringToHGlobalAnsi(flowFunction.FunctionName);
        ArgCount = flowFunction.ArgCount;
        FunctionPointer = flowFunction.FunctionInvokeWrapper.NativeFunctionPtr;
    }
}
