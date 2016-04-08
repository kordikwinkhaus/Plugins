namespace Ctor.Views
{
    internal class PythonLineHighlightParser : ILineHighlightParser
    {
        #region ILineHighlightParser Members

        public LineHighlightPositions GetHighlightPositions(string line)
        {
            string startTrimedText = line.TrimStart(' ', '\t');
            int startOffset = line.Length - startTrimedText.Length;
            string endTrimedText = startTrimedText.TrimComment().TrimEnd(' ', '\t');
            int endOffset = startTrimedText.Length - endTrimedText.Length;

            return new LineHighlightPositions(startOffset, endOffset);
        }

        #endregion
    }

    public static class PythonLineParser
    {
        public static string TrimComment(this string line)
        {
            int sharp = -1;
            bool inApos = false;
            bool inQuotes = false;
            int lastEscape = -999;

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                if (c == '#')
                {
                    if (inApos || inQuotes) continue;

                    sharp = i;
                    break;
                }
                else if (c == '\'')
                {
                    if (inApos && lastEscape == (i - 1)) continue;

                    inApos = !inApos;
                }
                else if (c == '\"')
                {
                    if (inQuotes && lastEscape == (i - 1)) continue;

                    inQuotes = !inQuotes;
                }
                else if (c == '\\')
                {
                    if (inApos || inQuotes)
                    {
                        lastEscape = i;
                    }
                }
            }

            if (sharp != -1)
            {
                return line.Substring(0, sharp);
            }
            else
            {
                return line;
            }
        }
    }
}
