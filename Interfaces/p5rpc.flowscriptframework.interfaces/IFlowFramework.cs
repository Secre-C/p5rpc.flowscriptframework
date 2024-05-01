namespace p5rpc.flowscriptframework.interfaces;

public interface IFlowFramework
{
    /// <summary>
    /// Registers a method as a flowscript function.
    /// </summary>
    /// <typeparam name="TFunction">A delegate with the desired method assigned to it.</typeparam>
    /// <param name="functionName">The name of the function</param>
    /// <param name="argCount">The number of args the function uses.</param>
    /// <param name="function">The contents of the custom functions.</param>
    /// <param name="idOverride">Optional value to override vanilla flow functions. Only values 0x5007 and below are accepted. This should only be used to overwrite existing flow functions.</param>
    /// <returns>The id of the function to be used in your Functions.json</returns>
    public ushort Register(string functionName, int argCount, Func<FlowStatus> function, ushort idOverride = 0xffff);

    /// <summary>
    /// Returns an instance of the FlowApi containing useful methods to use in flowscript functions.
    /// </summary>
    /// <returns></returns>
    public IFlowApi GetFlowApi();
}

public interface IFlowApi
{
    /// <summary>
    /// Gets an integer arugment at a given index.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public int GetIntArg(int index);

    /// <summary>
    /// Gets a float argument at a given index.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public float GetFloatArg(int index);

    /// <summary>
    /// Gets a string argument at a given index.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public string GetStringArg(int index);

    /// <summary>
    /// Sets the functions return value.
    /// </summary>
    /// <param name="value"></param>
    public void SetReturnValue(int value);

    /// <summary>
    /// Sets the functions return value.
    /// </summary>
    /// <param name="value"></param>
    public void SetReturnValue(float value);
}

public enum FlowStatus : byte
{
    FAILURE = 0,
    SUCCESS = 1,
}