using p5rpc.flowscriptframework.interfaces;
using p5rpc.flowscriptframework.structs;
using Reloaded.Hooks.Definitions;

namespace p5rpc.flowscriptframework;
internal unsafe class FlowFunction
{
    internal string FunctionName { get; set; }
    internal int ArgCount { get; set; }
    internal IFlowApi Api;
    internal Func<FlowStatus> Function;
    internal IReverseWrapper FunctionInvokeWrapper;
    internal FlowFunctionInfo NativeStruct;

    internal FlowFunction(string functionName, int argCount, Func<FlowStatus> function)
    {
        FunctionName = functionName;
        ArgCount = argCount;
        Function = function;
    }

    internal FlowStatus Invoke()
    {
        return Function.Invoke();
    }
}
