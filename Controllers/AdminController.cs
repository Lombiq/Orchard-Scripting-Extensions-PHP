using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orchard.UI.Admin;
using Orchard;
using PHP.Core;
using System.IO;
using OrchardHUN.Scripting.Php.ViewModels;
using Orchard.UI.Notify;
using Orchard.Localization;
using System.Reflection;

namespace OrchardHUN.Scripting.Php.Controllers
{
    [Admin]
    public class AdminController : Controller
    {
        private readonly IWorkContextAccessor _workContextAccessor;
        private readonly IOrchardServices _orchardServices;

        public Localizer T { get; set; }


        public AdminController(
            IWorkContextAccessor workContextAccessor,
            IOrchardServices orchardServices)
        {
            _workContextAccessor = workContextAccessor;
            _orchardServices = orchardServices;

            T = NullLocalizer.Instance;
        }


        public ActionResult TestBed()
        {
            return View((object)new TestBedViewModel());
        }

        [HttpPost/*, ValidateInput(false)*/]
        public ActionResult TestBed(TestBedViewModel viewModel)
        {
            /*
             * Some test code here
             * 
             * echo "Yes, this is PHP from Orchard.";
             * echo $_ORCHARD['WORK_CONTEXT']->CurrentSite->SiteName;
             * echo str_replace("i", "k", "iiiiii");
             */

            if (!String.IsNullOrEmpty(viewModel.Code))
            {
                try
                {
                    var scriptName = "testbed.php";
                    var workContext = _workContextAccessor.GetContext();
                    using (var requestContext = RequestContext.Initialize(ApplicationContext.Default, workContext.HttpContext.ApplicationInstance.Context))
                    {
                        var scriptContext = requestContext.ScriptContext;
                        using (scriptContext.OutputStream = new MemoryStream())
                        {
                            using (scriptContext.Output = new StreamWriter(scriptContext.OutputStream))
                            {
                                var orchardGlobal = new Dictionary<string, object>();
                                orchardGlobal["WORK_CONTEXT"] = workContext;
                                orchardGlobal["ORCHARD_SERVICES"] = _orchardServices;
                                orchardGlobal["OUTPUT"] = "";

                                dynamic globals = scriptContext.Globals;
                                globals._ORCHARD = orchardGlobal;

                                // Setting globals like this only works for primitive types.
                                //Operators.SetVariable(scriptContext, null, "test", "");

                                DynamicCode.Eval(
                                    viewModel.Code,
                                    false,/*phalanger internal stuff*/
                                    scriptContext,
                                    null,/*local variables*/
                                    null,/*reference to "$this"*/
                                    null,/*current class context*/
                                    scriptName,/*file name, used for debug and cache key*/
                                    1, 1,/*position in the file used for debug and cache key*/
                                    -1,/*something internal*/
                                    null/*current namespace, used in CLR mode*/
                                );

                                scriptContext.Output.Flush();
                                scriptContext.OutputStream.Position = 0;
                                using (var streamReader = new StreamReader(scriptContext.OutputStream))
                                {
                                    viewModel.Output = streamReader.ReadToEnd();
                                }
                            }
                        }
                    }
                }
                catch (PhpException ex)
                {
                    _orchardServices.Notifier.Error(T("There was a glitch with your code: {0}", ex.Message));
                }
            }

            return View((object)viewModel);
        }
    }
}