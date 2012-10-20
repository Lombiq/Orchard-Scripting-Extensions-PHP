using Orchard.Events;
using OrchardHUN.Scripting.Models;
using PHP.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrchardHUN.Scripting.Php
{
    public interface IPhpRuntimeEventHandler : IEventHandler
    {
        void BeforeExecution(BeforePhpExecutionContext context);
        void AfterExecution(AfterPhpExecutionContext context);
    }

    public abstract class PhpScriptingEventContext
    {
        public ScriptScope Scope { get; set; }
        public ScriptContext Context { get; private set; }

        protected PhpScriptingEventContext(ScriptScope scope, ScriptContext context)
        {
            Scope = scope;
            Context = context;
        }
    }

    public class BeforePhpExecutionContext : PhpScriptingEventContext
    {
        public BeforePhpExecutionContext(ScriptScope scope, ScriptContext context)
            : base(scope, context)
        {
        }
    }

    public class AfterPhpExecutionContext : PhpScriptingEventContext
    {
        public AfterPhpExecutionContext(ScriptScope scope, ScriptContext context)
            : base(scope, context)
        {
        }
    }
}
