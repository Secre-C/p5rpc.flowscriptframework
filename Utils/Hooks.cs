using Reloaded.Hooks.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace p5rpc.flowscriptframework.Utils;

internal struct AsmHookWrapper
{
    internal IAsmHook AsmHook;
    internal IReverseWrapper ReverseWrapper;

    internal AsmHookWrapper(IAsmHook asmHook, IReverseWrapper reverseWrapper)
    {
        AsmHook = asmHook;
        ReverseWrapper = reverseWrapper;
    }
}
