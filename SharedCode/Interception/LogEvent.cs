using System;
using System.Collections;
using System.Collections.Generic;

namespace Okna.Plugins.Interception
{
    public class LogEvent
    {
        internal LogEvent()
        {
            this.Cas = DateTime.Now;
        }

        public DateTime Cas { get; private set; }

        private List<LogArg> _arguments;
        public List<LogArg> Arguments
        {
            get
            {
                if (_arguments == null)
                {
                    _arguments = new List<LogArg>();
                    if (this.Args != null)
                    {
                        foreach (var arg in Args)
                        {
                            _arguments.Add(new LogArg(arg, false));

                            if (arg is IEnumerable)
                            {
                                var enumArg = (IEnumerable)arg;
                                foreach (var item in enumArg)
                                {
                                    _arguments.Add(new LogArg(item, false, true));
                                }
                            }
                        }
                    }
                    if (ReturnValue != null)
                    {
                        _arguments.Add(new LogArg(ReturnValue, true));

                        if (ReturnValue is IEnumerable)
                        {
                            var enumArg = (IEnumerable)ReturnValue;
                            foreach (var item in enumArg)
                            {
                                _arguments.Add(new LogArg(item, true, true));
                            }
                        }
                    }
                }
                return _arguments;
            }
        }

        public string ClassName { get; set; }
        public string MemberName { get; set; }
        public object[] Args { get; set; }
        public object ReturnValue { get; set; }
    }
}
