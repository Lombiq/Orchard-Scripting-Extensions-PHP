﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Orchard.DisplayManagement.Descriptors.ShapeTemplateStrategy;
using Orchard.Mvc.ViewEngines;

namespace OrchardHUN.Scripting.Php.PhpViewEngine
{
    public class PhpViewEngineProvider : IViewEngineProvider, IShapeTemplateViewEngine
    {
        public IViewEngine CreateThemeViewEngine(CreateThemeViewEngineParams parameters)
        {
            throw new NotImplementedException();
        }

        public IViewEngine CreateModulesViewEngine(CreateModulesViewEngineParams parameters)
        {
            throw new NotImplementedException();
        }

        public IViewEngine CreateBareViewEngine()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> DetectTemplateFileNames(IEnumerable<string> fileNames)
        {
            throw new NotImplementedException();
        }
    }
}