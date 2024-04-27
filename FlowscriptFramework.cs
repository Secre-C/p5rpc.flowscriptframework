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
    AsmHookWrapper FunctionCallHook;

    internal unsafe FlowscriptFramework(IReloadedHooks hooks)
    {
        Functions = new Dictionary<ushort, FlowFunction>();
        Scanner.SigScan("48 8B 00 FF D0", "ExecuteFlowFunction", (result) =>
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

            FunctionCallHook = new AsmHookWrapper(
                hooks.CreateAsmHook(asm, result, Reloaded.Hooks.Definitions.Enums.AsmHookBehaviour.ExecuteFirst).Activate(),
                reverseWrapper
            );
            
        });
    }

    internal ushort Register(string functionName, int argCount, Func<List<object>, object> function)
    {
        var index = GenerateFunctionIndex(functionName, argCount);
        Functions.TryAdd(index, new FlowFunction(functionName, argCount, function));
        Logger.Log($"Registered Function {functionName} with {argCount} args at index 0x{index:X8}");
        return index;
    }

    private static ushort GenerateFunctionIndex(string functionName, int argCount)
    {
        string combinedString = functionName + argCount.ToString();
        byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(combinedString));
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
            var flowFunctionInfo = new FlowFunctionInfo(flowFunction);
            p_flowFunctionInfo = &flowFunctionInfo;
        }

        Logger.DebugLog($"section {scriptTableSectionId} index {scriptFunctionIndex} id 0x{scriptFunctionId:X}");
        return p_flowFunctionInfo;
    }
}
