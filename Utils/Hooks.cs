using Reloaded.Hooks.Definitions;

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
