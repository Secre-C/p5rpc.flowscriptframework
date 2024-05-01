using p5rpc.flowscriptframework.interfaces;
using Reloaded.Mod.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowscriptFrameworkTest;
internal class FlowscriptFrameworkTest
{
    private ILogger _logger;
    internal FlowscriptFrameworkTest(IFlowFramework flowFramework, ILogger logger)
    {
        _logger = logger;
        var id = flowFramework.Register("SQUARE_NUMBER", 1, () =>
        {
            var api = flowFramework.GetFlowApi();
            var num = api.GetIntArg(0);
            api.SetReturnValue(num * num);

            return FlowStatus.SUCCESS;
        });
    }
}
