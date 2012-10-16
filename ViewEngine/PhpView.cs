using System.IO;
using System.Web.Mvc;
using Orchard.Environment.Extensions;
using OrchardHUN.Scripting.Models;
using OrchardHUN.Scripting.Php.Services;
using System.Collections.Generic;

namespace OrchardHUN.Scripting.Php.ViewEngine
{
    [OrchardFeature("OrchardHUN.Scripting.Php.ViewEngine")]
    public class PhpView : IView
    {
        private readonly IPhpRuntime _phpRuntime;

        public string ViewPath { get; private set; }
        public string MasterPath { get; private set; }


        public PhpView(IPhpRuntime phpRuntime, string viewPath, string masterPath)
        {
            _phpRuntime = phpRuntime;
            ViewPath = viewPath;
            MasterPath = masterPath;
        }


        public void Render(ViewContext context, TextWriter writer)
        {
            var filename = context.HttpContext.Server.MapPath(ViewPath);

            var scope = new ScriptScopeImpl(ViewPath);
            var orchardGlobal = new Dictionary<string, dynamic>();
            orchardGlobal["VIEW_CONTEXT"] = context;
            scope.SetVariable("_ORCHARD", orchardGlobal);

            var output = _phpRuntime.ExecuteFile(filename, scope);
            writer.Write(output);
        }


        private class ScriptScopeImpl : ScriptScope
        {
            public ScriptScopeImpl(string name)
                : base(name)
            {
            }
        }
    }
}