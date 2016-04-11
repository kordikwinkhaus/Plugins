using System.Windows.Media;

namespace Ctor.Models.Scripting
{
    public interface IScriptEditor
    {
        void BeginScriptExecMode();
        void EndScriptExecMode();
        void HighlightLine(int? line, SolidColorBrush background);// TODO: výčet místo brushe
        bool IsBreakPointOnLine(int line);
        string Text { get; set; }
    }
}
