namespace Okna.Plugins.Interception
{
    public class LogArg
    {
        internal LogArg(object val, bool isRetVal, bool isListMember = false)
        {
            if (val == null)
            {
                this.Val = new object();
                this.TypeName = "NULL";
            }
            else
            {
                this.Val = val;
                this.TypeName = val.GetType().FullName;
                if (val.GetType().IsValueType)
                {
                    this.TypeName += " => " + val.ToString();
                }
            }
            this.IsRetVal = isRetVal;
            this.IsListMember = isListMember;
        }

        public bool IsRetVal { get; private set; }
        public bool IsListMember { get; private set; }
        public string TypeName { get; private set; }
        public object Val { get; private set; }
    }
}
