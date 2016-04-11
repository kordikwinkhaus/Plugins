using Microsoft.Scripting.Hosting;

namespace Ctor.Models.Scripting
{
    public interface IScriptScopeExtender
    {
        void ExtendScope(ScriptScope scope);
    }
}
