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
        private readonly IOrchardServices _orchardServices;

        public Localizer T { get; set; }


        public AdminController(
            IScriptingManager scriptingManager,
            IOrchardServices orchardServices)
        {
            _scriptingManager = scriptingManager;
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
            if (!String.IsNullOrEmpty(viewModel.Code))
            {
                try
                {
                    using (var scope = _scriptingManager.CreateScope("testbed"))
                    {
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