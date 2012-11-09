﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OrchardHUN.Scripting.EventHandlers;

namespace OrchardHUN.Scripting.Php.Services
{
    public class PhpAssemblyLoader : IPhpRuntimeEventHandler
    {
        public void BeforeExecution(BeforePhpExecutionContext context)
        {
            context.Context.ApplicationContext.AssemblyLoader.Load(typeof(PhpShape).Assembly, null);
        }

        public void AfterExecution(AfterPhpExecutionContext context)
        {
        }
    }
}