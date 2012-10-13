using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Orchard;
using Orchard.Localization;
using Orchard.UI.Admin;
using Orchard.UI.Notify;
using OrchardHUN.Scripting.Exceptions;
using OrchardHUN.Scripting.Php.ViewModels;
using OrchardHUN.Scripting.Services;

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

        public ActionResult Test()
        {
            return View();
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
                try
                {
                    using (var scope = _scriptingManager.CreateScope("testbed"))
                    {
                        var orchardGlobal = new Dictionary<string, object>();
                        orchardGlobal["WORK_CONTEXT"] = _workContextAccessor.GetContext();
                        orchardGlobal["ORCHARD_SERVICES"] = _orchardServices;

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
            }

            return View((object)viewModel);
        }
    }
}