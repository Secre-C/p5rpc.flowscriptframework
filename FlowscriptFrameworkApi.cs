namespace p5rpc.flowscriptframework;
internal class FlowscriptFrameworkApi
{
    private FlowscriptFramework _flowscriptFramework;
    internal FlowscriptFrameworkApi(FlowscriptFramework flowscriptFramework)
    {
        _flowscriptFramework = flowscriptFramework;
    }

    internal ushort Register(string functionName, int argCount, Func<List<object>, object> function)
        => Register(functionName, argCount, function);
}
