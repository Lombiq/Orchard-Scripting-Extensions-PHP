using System;
using System.Collections;
using System.IO;
using System.Web.Mvc;
using PHP.Core;
using PHP.Core.Reflection;
using OrchardHUN.Scripting.Php.Services;
using OrchardHUN.Scripting.Models;
using Orchard.Environment.Extensions;

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
            scope.SetVariable("_VIEW_CONTEXT", context);

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