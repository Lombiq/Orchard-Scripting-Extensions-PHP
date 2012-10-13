﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Orchard.DisplayManagement.Descriptors.ShapeTemplateStrategy;
using Orchard.Logging;
using Orchard.Mvc.ViewEngines;
using Orchard.Mvc.ViewEngines.WebForms;

namespace OrchardHUN.Scripting.Php.PhpViewEngine
{
    public class PhpViewEngineProvider : IViewEngineProvider, IShapeTemplateViewEngine
    {
        public ILogger Logger { get; set; }
        static readonly string[] DisabledFormats = new[] { "~/Disabled" };

        public PhpViewEngineProvider()
        {
            Logger = NullLogger.Instance;
        }

        public IViewEngine CreateThemeViewEngine(CreateThemeViewEngineParams parameters)
        {
            var partialViewLocationFormats = new[] {
                parameters.VirtualPath + "/Views/{0}.php",
            };

            var areaPartialViewLocationFormats = new[] {
                parameters.VirtualPath + "/Views/{2}/{1}/{0}.php",
            };

            var viewEngine = new PhpViewEngine()
            {
                MasterLocationFormats = DisabledFormats,
                ViewLocationFormats = DisabledFormats,
                PartialViewLocationFormats = partialViewLocationFormats,
                AreaMasterLocationFormats = DisabledFormats,
                AreaViewLocationFormats = DisabledFormats,
                AreaPartialViewLocationFormats = areaPartialViewLocationFormats,
                ViewLocationCache = new ThemeViewLocationCache(parameters.VirtualPath),
            };

            return viewEngine;
        }

        public IViewEngine CreateModulesViewEngine(CreateModulesViewEngineParams parameters)
        {
            var areaFormats = new[] {
                                        "~/Core/{2}/Views/{1}/{0}.php",
                                        "~/Modules/{2}/Views/{1}/{0}.php",
                                        "~/Themes/{2}/Views/{1}/{0}.php",
                                    };

            var universalFormats = parameters.VirtualPaths
                .SelectMany(x => new[] {
                                           x + "/Views/{0}.php",
                                       })
                .ToArray();

            var viewEngine = new PhpViewEngine()
            {
                MasterLocationFormats = DisabledFormats,
                ViewLocationFormats = universalFormats,
                PartialViewLocationFormats = universalFormats,
                AreaMasterLocationFormats = DisabledFormats,
                AreaViewLocationFormats = areaFormats,
                AreaPartialViewLocationFormats = areaFormats,
            };

            return viewEngine;
        }

        public IViewEngine CreateBareViewEngine()
        {
            return new PhpViewEngine()
            {
                MasterLocationFormats = DisabledFormats,
                ViewLocationFormats = DisabledFormats,
                PartialViewLocationFormats = DisabledFormats,
                AreaMasterLocationFormats = DisabledFormats,
                AreaViewLocationFormats = DisabledFormats,
                AreaPartialViewLocationFormats = DisabledFormats,
            };
        }

        public IEnumerable<string> DetectTemplateFileNames(IEnumerable<string> fileNames)
        {
            return fileNames.Where(fileName => fileName.EndsWith(".php", StringComparison.OrdinalIgnoreCase));
        }
    }
}