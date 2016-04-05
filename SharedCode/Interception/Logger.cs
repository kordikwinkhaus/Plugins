using System;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using Okna.Plugins.ViewModels;

namespace Okna.Plugins.Interception
{
    public class Logger : ViewModelBase
    {
        public Logger()
        {
            this.Events = new ObservableCollection<LogEvent>();
        }

        private XElement _data;
        public XElement Data
        {
            get { return _data; }
            set
            {
                if (_data != value)
                {
                    _data = value;
                    OnPropertyChanged(nameof(Data));
                }
            }
        }

        public LogEvent Log(string className, string memberName, params object[] args)
        {
            LogEvent newEvent = null;
            try
            {
                newEvent = new LogEvent { ClassName = className, MemberName = memberName, Args = args };
            }
            catch (Exception)
            {
                newEvent = new LogEvent();
            }
            this.Events.Add(newEvent);
            return newEvent;
        }

        public ObservableCollection<LogEvent> Events { get; private set; }

        private LogEvent _event;
        public LogEvent CurrentEvent
        {
            get { return _event; }
            set
            {
                _event = value;
                OnPropertyChanged("CurrentEvent");
            }
        }

        private LogArg _arg;
        public LogArg CurrentArg
        {
            get { return _arg; }
            set
            {
                _arg = value;
                OnPropertyChanged("CurrentArg");
            }
        }
    }
}
