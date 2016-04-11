using System;
using Ctor.Models;
using Ctor.Models.Scripting;
using Microsoft.Scripting.Hosting;

namespace Ctor.ViewModels
{
    internal class ContextScriptScopeExtender : IScriptScopeExtender
    {
        private readonly FastInsertViewModel _parent;

        internal ContextScriptScopeExtender(FastInsertViewModel parent)
        {
            if (parent == null) throw new ArgumentNullException(nameof(parent));

            _parent = parent;
        }

        public void ExtendScope(ScriptScope scope)
        {
            scope.SetVariable("pos", new Position(_parent.Document.ActivePos));
            scope.SetVariable("ctx", new Context(_parent.Context));
            scope.SetVariable("msg", Msg.Instance);
            scope.SetVariable("db", _parent.Database);
        }
    }
}
