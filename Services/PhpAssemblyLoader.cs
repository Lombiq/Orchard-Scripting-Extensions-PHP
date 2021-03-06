﻿using OrchardHUN.Scripting.Services;

namespace OrchardHUN.Scripting.Php.Services
{
    public class PhpAssemblyLoader : IPhpRuntimeEventHandler
    {
        public void BeforeExecution(BeforePhpExecutionContext context)
        {
            context.Context.ApplicationContext.AssemblyLoader.Load(typeof(StaticShape).Assembly, null);
        }

        public void AfterExecution(AfterPhpExecutionContext context)
        {
        }
    }
}