using System;
using System.IO;
using Orchard;
using Orchard.Environment;
using OrchardHUN.Scripting.Exceptions;
using OrchardHUN.Scripting.Models;
using PHP.Core;
using PHP.Library;
using System.Collections.Generic;

namespace OrchardHUN.Scripting.Php.Services
{
    public class PhpRuntime : IPhpRuntime
    {
        private readonly Work<IWorkContextAccessor> _wcaWork;

        public string Engine
        {
            get { return "PHP"; }
        }


        public PhpRuntime(Work<IWorkContextAccessor> wcaWork)
        {
            _wcaWork = wcaWork;
        }

        public dynamic ExecuteExpression(string expression, ScriptScope scope)
        {
            return
                RunPhpExecutor(
                    requestContext =>
                    {
                        DynamicCode.Eval(
                            expression,
                            false,      // phalanger internal stuff
                            requestContext.ScriptContext,
                            null,       // local variables
                            null,       // reference to "$this"
                            null,       // current class context
                            scope.Name, // file name, used for debug and cache key
                            1, 1,       // position in the file used for debug and cache key
                            -1,         // something internal
                            null        // current namespace, used in CLR mode
                        );
                    },
                    scope);
        }

        public dynamic ExecuteFile(string path, ScriptScope scope)
        {
            return
                RunPhpExecutor(
                    requestContext =>
                    {
                        requestContext.ScriptContext.Include(path, true);
                    },
                    scope);
        }

        private dynamic RunPhpExecutor(Action<RequestContext> executor, ScriptScope scope)
        {
            try
            {
                // Just selecting a class to access the assembly.
                // Other .NET assemblies can be loaded the same way, so PHP code can access them.
                ApplicationContext.Default.AssemblyLoader.Load(typeof(PhpHash).Assembly, null);
                var workContext = _wcaWork.Value.GetContext();
                using (var requestContext = RequestContext.Initialize(ApplicationContext.Default, workContext.HttpContext.ApplicationInstance.Context))
                {
                    var scriptContext = requestContext.ScriptContext;
                    using (scriptContext.OutputStream = new MemoryStream())
                    {
                        using (scriptContext.Output = new StreamWriter(scriptContext.OutputStream))
                        {
                            var orchardGlobal = new Dictionary<string, object>();
                            orchardGlobal["WORK_CONTEXT"] = workContext;
                            orchardGlobal["ORCHARD_SERVICES"] = workContext.Resolve<IOrchardServices>();

                            scope.SetVariable("_ORCHARD", orchardGlobal);

                            foreach (var item in scope.Variables)
                            {
                                scriptContext.Globals.TrySetMember(item.Key, item.Value);
                            }

                            // Setting globals like this only works for primitive types.
                            // Operators.SetVariable(scriptContext, null, "test", "");

                            executor(requestContext);

                            foreach (var variable in scriptContext.GlobalVariables)
                            {
                                scope.SetVariable(variable.Key.ToString(), variable.Value);
                            }

                            scriptContext.Output.Flush();
                            scriptContext.OutputStream.Position = 0;
                            using (var streamReader = new StreamReader(scriptContext.OutputStream))
                            {
                                return streamReader.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch (PhpException ex)
            {
                throw new ScriptRuntimeException("The PHP script could not be executed.", ex);
            }
        }
    }
}