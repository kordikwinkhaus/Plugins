using System.Windows.Media;

namespace Ctor.Views
{
    interface IScriptEditor
    {
        void BeginScriptExecMode();
        void EndScriptExecMode();
        void HighlightLine(int? line, SolidColorBrush background);
        bool IsBreakPointOnLine(int line);
        string Text { get; }
    }
}
