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
using OrchardHUN.Scripting.Services;
using OrchardHUN.Scripting.Exceptions;
using OrchardHUN.Scripting.Models;
using Orchard.Environment;
using System.Diagnostics;

namespace OrchardHUN.Scripting.Php.Controllers
{
    [Admin]
    public class AdminController : Controller
    {
        private readonly IScriptingManager _scriptingManager;
        private readonly IWorkContextAccessor _workContextAccessor;
        private readonly IOrchardServices _orchardServices;

        public Localizer T { get; set; }


        public AdminController(
            IScriptingManager scriptingManager,
            IWorkContextAccessor workContextAccessor,
            IOrchardServices orchardServices)
        {
            _scriptingManager = scriptingManager;
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
             * echo str_replace(array("don't", "ridiculous"), array("do", "awesome"), "I don't want to run PHP from Orchard, this is ridiculous.");
             */

            if (!String.IsNullOrEmpty(viewModel.Code))
            {
                var sw = Stopwatch.StartNew();
                try
                {
                    using (var scope = _scriptingManager.CreateScope("testbed"))
                    {
                        var orchardGlobal = new Dictionary<string, object>();
                        orchardGlobal["WORK_CONTEXT"] = _workContextAccessor.GetContext();
                        orchardGlobal["ORCHARD_SERVICES"] = _orchardServices;
                        orchardGlobal["OUTPUT"] = "";

                        scope.SetVariable("_ORCHARD", orchardGlobal);

                        viewModel.Output = _scriptingManager.ExecuteExpression("PHP", viewModel.Code, scope);
                    }
                }
                catch (ScriptRuntimeException ex)
                {
                    _orchardServices.Notifier.Error(
                        T("There was a glitch with your code: {0}" 
                        + Environment.NewLine + Environment.NewLine 
                        + "Details:" + Environment.NewLine + "{1}", ex.Message, ex.InnerException.Message));
                }

                sw.Stop();
                var s = sw.Elapsed;
            }

            return View((object)viewModel);
        }
    }
}