namespace p5rpc.flowscriptframework;
internal class FlowFunction
{
    internal string FunctionName { get; set; }
    internal int ArgCount { get; set; }
    internal List<object> Args;
    internal Func<List<object>, object> Function;
    internal object ReturnValue;

    internal FlowFunction(string functionName, int argCount, Func<List<object>, object> function)
    {
        FunctionName = functionName;
        ArgCount = argCount;
        Function = function;
    }

    internal void Invoke()
    {
        ReturnValue = Function.Invoke(Args);
    }
}
