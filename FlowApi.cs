using p5rpc.flowscriptframework.interfaces;
using p5rpc.flowscriptframework.Utils;
using Reloaded.Hooks.Definitions;
using System.Runtime.InteropServices;

namespace p5rpc.flowscriptframework;
internal unsafe class FlowApi : IFlowApi
{
    private static FlowApi? Instance;
    private FlowInstanceData* _flowData { get; set; }

    private delegate int d_GetIntArg(int index);
    private d_GetIntArg _getIntArg;

    private delegate float d_GetFloatArg(int index);
    private d_GetFloatArg _getFloatArg;

    internal static FlowApi Initialize(IReloadedHooks hooks)
    {
        if (Instance == null)
            Instance = new FlowApi(hooks);

        return Instance;
    }

    internal static FlowApi? GetInstance() => Instance;

    private FlowApi(IReloadedHooks hooks)
    {
        Scanner.SigScan("48 89 1D ?? ?? ?? ?? 33 FF", "FlowData", (result) =>
        {
            _flowData = (FlowInstanceData*)Memory.GetAddressFromGlobalRef(result, 7, "FlowData");
        });

        Scanner.SigScan("4C 8B 05 ?? ?? ?? ?? 41 8B 50 ?? 29 CA", "GetIntArg", (result) =>
        {
            _getIntArg = hooks.CreateWrapper<d_GetIntArg>(result, out _);
        });

        Scanner.SigScan("4C 8B 05 ?? ?? ?? ?? 41 8B 50 ?? 2B D1 8D 42 ?? 83 F8 2F 77", "GetFloatArg", (result) =>
        {
            _getFloatArg = hooks.CreateWrapper<d_GetFloatArg>(result, out _);
        });
    }

    public int GetIntArg(int index)
    {
        var result = _getIntArg(index);
        Logger.DebugLog($"Arg index {index} contains {result}");
        return result;
    }

    public float GetFloatArg(int index)
    {
        var result = _getFloatArg(index);
        Logger.DebugLog($"Arg index {index} contains {result}");
        return result;
    }

    public string GetStringArg(int index)
    {
        int argDataIndex = _flowData->commandData->unk2c - index - 1;

        if (argDataIndex > 0x2f)
        {
            argDataIndex = -1;
        }
        if (argDataIndex < 0 || _flowData->commandData->ArgTypes[argDataIndex] != 5)
        {
            return string.Empty;
        }

        return Marshal.PtrToStringAnsi((nint)_flowData->commandData->ArgData[argDataIndex]);
    }

    public void SetReturnValue(int value)
    {
        _flowData->commandData->ReturnValue = value;
        _flowData->commandData->ReturnType = FlowReturnType.Int;
    }

    public void SetReturnValue(float value)
    {
        *(float*)&_flowData->commandData->ReturnValue = value;
        _flowData->commandData->ReturnType = FlowReturnType.Float;
    }

    [StructLayout(LayoutKind.Explicit)]
    internal struct FlowInstanceData
    {
        [FieldOffset(0x0)]
        internal FlowCommandData* commandData;
    }

    [StructLayout(LayoutKind.Explicit)]
    internal struct FlowCommandData
    {
        [FieldOffset(0x2c)]
        internal int unk2c;

        [FieldOffset(0x30)]
        internal fixed byte ArgTypes[0x2f];

        [FieldOffset(0x5f)]
        internal FlowReturnType ReturnType;

        [FieldOffset(0x60)]
        internal fixed long ArgData[0x2f];

        [FieldOffset(0x1d8)]
        internal long ReturnValue;
    }

    internal enum FlowReturnType : byte
    {
        Int,
        Float
    }
}
