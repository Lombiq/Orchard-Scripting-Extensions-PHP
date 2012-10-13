using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OrchardHUN.Scripting.Services;
using OrchardHUN.Scripting.Models;

namespace OrchardHUN.Scripting.Php.Services
{
    public interface IPhpRuntime : IScriptingRuntime
    {
        dynamic ExecuteFile(string path, ScriptScope scope);
    }
}
