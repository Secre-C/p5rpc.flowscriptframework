using p5rpc.flowscriptframework.interfaces;

namespace p5rpc.flowscriptframework;
internal class FlowscriptFrameworkApi : IFlowFramework
{
    private FlowscriptFramework _flowscriptFramework;
    private FlowApi _flowApi;
    internal FlowscriptFrameworkApi(FlowscriptFramework flowscriptFramework, FlowApi flowApi)
    {
        _flowscriptFramework = flowscriptFramework;
        _flowApi = flowApi;
    }

    public ushort Register(string functionName, int argCount, Func<FlowStatus> function, ushort idOverride = 0xffff)
        => _flowscriptFramework.Register(functionName, argCount, function, idOverride);

    public IFlowApi GetFlowApi() => _flowApi;
}
