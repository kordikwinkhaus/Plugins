using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Indentation;

namespace Ctor.Views
{
    internal class PythonIndentationStrategy : DefaultIndentationStrategy
    {
        private const string s_indentation = "    ";

        public override void IndentLine(TextDocument document, DocumentLine line)
        {
            var prevLine = line.PreviousLine;
            if (prevLine != null)
            {
                string prevLineText = document.GetText(prevLine.Offset, prevLine.Length).TrimComment().Trim();
                if (prevLineText.EndsWith(":"))
                {
                    ModifyIndentation(document, line, prevLine);
                    return;
                }
                else if (prevLineText == "pass")
                {
                    ModifyIndentation(document, line, prevLine, increase: false);
                    return;
                }
            }

            base.IndentLine(document, line);
        }

        private static void ModifyIndentation(TextDocument document, DocumentLine line,
            DocumentLine prevLine, bool increase = true)
        {
            ISegment indentationSegment = TextUtilities.GetWhitespaceAfter(document, prevLine.Offset);
            string indentation = document.GetText(indentationSegment);
            if (increase)
            {
                indentation += s_indentation;
            }
            else
            {
                int idx = indentation.IndexOf(s_indentation);
                if (idx != -1)
                {
                    indentation = indentation.Remove(idx, s_indentation.Length);
                }
            }

            indentationSegment = TextUtilities.GetWhitespaceAfter(document, line.Offset);
            document.Replace(indentationSegment, indentation);
        }
    }
}