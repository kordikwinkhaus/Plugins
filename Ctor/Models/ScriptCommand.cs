namespace Ctor.Models
{
    internal class ScriptCommand
    {
        internal string CmdName { get; set; }

        internal int DisplayOrder { get; set; }

        internal string CmdDesc { get; set; }

        internal string CmdCode { get; set; }

        internal byte[] CmdImage { get; set; }
    }
}
