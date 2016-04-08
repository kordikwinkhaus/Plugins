using System.Collections.Generic;
using System.Text.RegularExpressions;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Folding;

namespace Ctor.Views
{
    internal class PythonFoldingStrategy //: AbstractFoldingStrategy
    {
        class BlockDef
        {
            public int indent;
            public DocumentLine startLine;
            public DocumentLine endLine;
            public string blockType;

            public BlockDef(DocumentLine startLine, int indent, string blockType)
            {
                this.startLine = startLine;
                this.endLine = null;
                this.indent = indent;
                this.blockType = blockType;
            }
        };

        internal IEnumerable<NewFolding> CreateNewFoldings(TextDocument doc)
        {
            List<NewFolding> foldings = new List<NewFolding>();

            List<BlockDef> blocks = new List<BlockDef>();         // Actual list of blocks
            Stack<BlockDef> blockStack = new Stack<BlockDef>();     // For tracking open blocks

            string pattern = "(?<indent>[ \t]*)?(?<body>((?<blockDef>(def|class|if|else|elif|for|while|with|try|except|finally))[^:]*:)?(?<code>[^\r\n]*)?)";
            Regex ex = new Regex(pattern, RegexOptions.ExplicitCapture);

            foreach (DocumentLine line in doc.Lines)
            {
                string str = doc.GetText(line.Offset, line.Length).TrimComment().TrimEnd();

                Match m = ex.Match(str);

                int indent = m.Groups["indent"].Value.Length;

                if (str.Trim().Length > 0)
                {
                    // Close previous blocks as necessary
                    while (blockStack.Count > 0 && indent < blockStack.Peek().indent)
                    {
                        blockStack.Pop().endLine = line.PreviousLine;
                    }

                    // Check for new block
                    if (m.Groups["blockDef"].Value.Length > 0 && m.Groups["code"].Value.Length == 0)
                    {
                        BlockDef def = new BlockDef(line, indent + 1, m.Groups["blockDef"].Value);
                        blocks.Add(def);
                        blockStack.Push(def);
                    }
                }
            }

            // Close any remaining open blocks
            foreach (BlockDef block in blockStack)
            {
                block.endLine = doc.Lines[doc.LineCount - 1];
            }

            // Tidy up -> remove empty lines from the end of the block.
            foreach (var block in blocks)
            {
                while (doc.GetText(block.endLine).TrimComment().Trim().Length == 0)
                {
                    block.endLine = block.endLine.PreviousLine;
                }
            }

            // Build foldings list
            for (int i = 0; i < blocks.Count; i++)
            {
                NewFolding f = GetFolding(doc, i, blocks);
                if (f != null)
                {
                    foldings.Add(f);
                }
            }

            foldings.Sort((a, b) => a.StartOffset.CompareTo(b.StartOffset));

            return foldings;
        }

        // list of "continuation" blocks
        private Dictionary<string, string[]> _subBlocks = new Dictionary<string, string[]>
        {
            { "if", new string[] { "elif", "else" } },
            { "for", new string[] { "else" } },
            { "while", new string[] { "else" } },
            { "try", new string[] { "except", "else", "finally" } }
        };

        private NewFolding GetFolding(TextDocument doc, int i, List<BlockDef> blocks)
        {
            BlockDef block = blocks[i];

            if (block.startLine != null && block.endLine != null)
            {
                int endOffset = block.endLine.EndOffset;

                string[] subBlock;
                if (_subBlocks.TryGetValue(block.blockType, out subBlock))
                {
                    int nextI = i;
                    while (true)
                    {
                        nextI++;
                        if (nextI == blocks.Count) break;
                        var nextBlock = blocks[nextI];
                        if (nextBlock.indent == block.indent)
                        {
                            bool found = false;
                            for (int s = 0; s < subBlock.Length; s++)
                            {
                                if (subBlock[s] == nextBlock.blockType)
                                {
                                    found = true;
                                    endOffset = nextBlock.endLine.EndOffset;
                                    if (s == (subBlock.Length - 1))
                                    {
                                        goto LoopExit;
                                    }
                                }
                            }
                            if (!found)
                            {
                                goto LoopExit;
                            }
                        }
                        else if (nextBlock.indent < block.indent)
                        {
                            goto LoopExit;
                        }
                    }
                }

                LoopExit:
                NewFolding f = new NewFolding(block.startLine.Offset, endOffset);
                f.Name = doc.GetText(block.startLine);
                return f;
            }
            return null;
        }
    }
}