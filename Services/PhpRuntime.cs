using System.IO;
using System.Reflection;
using Orchard;
using Orchard.Environment;
using OrchardHUN.Scripting.Exceptions;
using OrchardHUN.Scripting.Models;
using OrchardHUN.Scripting.Services;
using PHP.Core;

namespace OrchardHUN.Scripting.Php.Services
{
    public class PhpRuntime : IScriptingRuntime
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
            try
            {
                ApplicationContext.Default.AssemblyLoader.Load(Assembly.Load(new AssemblyName("PhpNetClassLibrary")), null);
                var workContext = _wcaWork.Value.GetContext();
                using (var requestContext = RequestContext.Initialize(ApplicationContext.Default, workContext.HttpContext.ApplicationInstance.Context))
                {
                    var scriptContext = requestContext.ScriptContext;
                    using (scriptContext.OutputStream = new MemoryStream())
                    {
                        using (scriptContext.Output = new StreamWriter(scriptContext.OutputStream))
                        {
                            foreach (var item in scope.Variables)
                            {
                                scriptContext.Globals.TrySetMember(item.Key, item.Value);
                            }

                            // Setting globals like this only works for primitive types.
                            //Operators.SetVariable(scriptContext, null, "test", "");

                            DynamicCode.Eval(
                                expression,
                                false,/*phalanger internal stuff*/
                                scriptContext,
                                null,/*local variables*/
                                null,/*reference to "$this"*/
                                null,/*current class context*/
                                scope.Name,/*file name, used for debug and cache key*/
                                1, 1,/*position in the file used for debug and cache key*/
                                -1,/*something internal*/
                                null/*current namespace, used in CLR mode*/
                            );

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