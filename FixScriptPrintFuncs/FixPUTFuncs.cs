using p5rpc.flowscriptframework.interfaces;
using Reloaded.Mod.Interfaces;
using System.Drawing;

namespace p5rpc.flowscriptframework.FixScriptPrintFuncs;
internal class FixPUTFuncs
{
    private ILogger _logger;
    internal FixPUTFuncs(IModLoader modLoader, ILogger logger)
    {
        _logger = logger;
        if (!modLoader.GetController<IFlowFramework>().TryGetTarget(out var flowFramework))
        {
            throw new Exception("Failed to get IFlowFramework Controller");
        }

        flowFramework.Register("PUT", 1, () =>
        {
            var api = flowFramework.GetFlowApi();
            Log(api.GetIntArg(0).ToString());
            return FlowStatus.SUCCESS;
        }, 2);

        flowFramework.Register("PUTS", 1, () =>
        {
            var api = flowFramework.GetFlowApi();
            Log(api.GetStringArg(0));
            return FlowStatus.SUCCESS;
        }, 3);

        flowFramework.Register("PUTF", 1, () =>
        {
            var api = flowFramework.GetFlowApi();
            Log(api.GetFloatArg(0).ToString());
            return FlowStatus.SUCCESS;
        }, 4);
    }

    private void Log(string message)
    {
        _logger.WriteLineAsync(message, Color.LightGreen);
    }
}
