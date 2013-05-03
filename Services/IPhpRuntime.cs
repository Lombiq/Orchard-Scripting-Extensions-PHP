using OrchardHUN.Scripting.Services;

namespace OrchardHUN.Scripting.Php.Services
{
    public interface IPhpRuntime : IScriptingRuntime
    {
        dynamic ExecuteFile(string path, ScriptScope scope);
    }
}
