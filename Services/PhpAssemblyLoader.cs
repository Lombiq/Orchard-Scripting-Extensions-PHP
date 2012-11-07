using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OrchardHUN.Scripting.EventHandlers;

namespace OrchardHUN.Scripting.Php.Services
{
    public class PhpAssemblyLoader : IScriptingEventHandler
    {
        public void BeforeExecution(BeforeExecutionContext context)
        {
            if (context.Engine != "PHP") return;

            context.Scope.LoadAssembly(typeof(PhpShape).Assembly);
        }

        public void AfterExecution(AfterExecutionContext context)
        {
        }
    }
}