using p5rpc.flowscriptframework.interfaces;
using p5rpc.flowscriptframework.structs;
using p5rpc.flowscriptframework.Utils;
using Reloaded.Hooks.Definitions;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace p5rpc.flowscriptframework;
internal class FlowscriptFramework
{
    private Dictionary<ushort, FlowFunction> Functions;
    private List<AsmHookWrapper> FunctionCallHook;
    private IReloadedHooks _hooks;

    private delegate FlowStatus d_flowFunction();

    internal unsafe FlowscriptFramework(IReloadedHooks hooks)
    {
        _hooks = hooks;
        Functions = new Dictionary<ushort, FlowFunction>();
        FunctionCallHook = new List<AsmHookWrapper>();

        Scanner.SigScan("48 8B 00 FF D0", "ExecuteFlowFunction", (result) => // 0x1416e7b3f
        {
            string[] asm =
            {
                "use64",
                "sub rsp, 0x20",
                Memory.PushCallerRegisters,
                "mov rcx, rsi",
                "mov rdx, rbp",
                "mov r8, rax",
                hooks.Utilities.GetAbsoluteCallMnemonics(GetFlowFunction, out var reverseWrapper),
                Memory.PopCallerRegisters,
                "add rsp, 0x20",
            };

            FunctionCallHook.Add(new AsmHookWrapper(
                hooks.CreateAsmHook(asm, result, Reloaded.Hooks.Definitions.Enums.AsmHookBehaviour.ExecuteFirst).Activate(),
                reverseWrapper
            ));
        });

        Scanner.SigScan("8B 47 ?? 29 43 ?? 8B 43", "ExecuteFlowFunction2", (result) => // 0x1416e7b7a
        {
            string[] asm =
            {
                "use64",
                "sub rsp, 0x20",
                Memory.PushCallerRegisters,
                "mov rcx, rsi",
                "mov rdx, rbp",
                "mov r8, rdi",
                hooks.Utilities.GetAbsoluteCallMnemonics(GetFlowFunction, out var reverseWrapper),
                Memory.PopCallerRegisters,
                "mov rdi, rax",
                "add rsp, 0x20",
            };

            FunctionCallHook.Add(new AsmHookWrapper(
                hooks.CreateAsmHook(asm, result, Reloaded.Hooks.Definitions.Enums.AsmHookBehaviour.ExecuteFirst).Activate(),
                reverseWrapper
            ));
        });
    }

    internal ushort Register(string functionName, int argCount, Func<FlowStatus> function)
    {
        var index = GenerateFunctionIndex(functionName);
        var flowFunction = new FlowFunction(functionName, argCount, function);
        d_flowFunction func = flowFunction.Invoke;
        flowFunction.FunctionInvokeWrapper = _hooks.CreateReverseWrapper(func);

        Functions.TryAdd(index, flowFunction);
        Logger.Log($"Registered Function {functionName} with {argCount} args at index 0x{index:X8}");
        return index;
    }

    private static ushort GenerateFunctionIndex(string functionName)
    {
        byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(functionName));
        ushort hashValue = BitConverter.ToUInt16(bytes, 0);
        if (hashValue < 0x6000)
            hashValue += (ushort)(0x6000 - (hashValue % 0x1000 * 0x1000));
        return hashValue;
    }

    private unsafe FlowFunctionInfo* GetFlowFunction(ushort scriptTableSectionId, ushort scriptFunctionIndex, FlowFunctionInfo* p_flowFunctionInfo)
    {
        ushort scriptFunctionId = (ushort)(scriptTableSectionId * 0x1000 + scriptFunctionIndex);

        if (Functions.TryGetValue(scriptFunctionId, out var flowFunction))
        {
            Logger.DebugLog($"Calling Function 0x{scriptFunctionId:X} - {flowFunction.FunctionName}");
            var flowFunctionInfo = new FlowFunctionInfo(flowFunction);
            p_flowFunctionInfo = &flowFunctionInfo;
        }

        return p_flowFunctionInfo;
    }
}
