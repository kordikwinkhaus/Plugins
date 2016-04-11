using Ctor.Models.Scripting;

namespace Ctor.ViewModels
{
    interface ICodeEditorView
    {
        IScriptEditor Editor { get; }
    }
}
