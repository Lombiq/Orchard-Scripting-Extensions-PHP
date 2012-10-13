using System.Web.Mvc;

namespace OrchardHUN.Scripting.Php.PhpViewEngine
{
    public class PhpViewEngine : BuildManagerViewEngine
    {
        public PhpViewEngine()
        {
            FileExtensions = new[] { "php" };
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            return new PhpView(viewPath, masterPath);
        }

        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            return new PhpView(partialPath, "");
        }
    }
}