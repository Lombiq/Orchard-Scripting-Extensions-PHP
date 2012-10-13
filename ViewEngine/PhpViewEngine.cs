using System.Web.Mvc;
using OrchardHUN.Scripting.Services;
using Orchard.Environment;
using OrchardHUN.Scripting.Php.Services;
using Orchard.Environment.Extensions;

namespace OrchardHUN.Scripting.Php.ViewEngine
{
    [OrchardFeature("OrchardHUN.Scripting.Php.ViewEngine")]
    public class PhpViewEngine : VirtualPathProviderViewEngine
    {
        private readonly Work<IPhpRuntime> _phpRuntimeWork;

        public PhpViewEngine(Work<IPhpRuntime> phpRuntimeWork)
        {
            _phpRuntimeWork = phpRuntimeWork;
            FileExtensions = new[] { ".php" };
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            return new PhpView(_phpRuntimeWork.Value, viewPath, masterPath);
        }

        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            return new PhpView(_phpRuntimeWork.Value, partialPath, "");
        }
    }
}