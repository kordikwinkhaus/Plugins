using UserExtensions;
using WHOkna;

namespace Okna.Plugins.Interception
{
    public class InterceptionView3 : InterceptionView2, IUserForm3
    {
        public InterceptionView3(IUserForm3 userform3, bool documentView = false)
            : base(userform3, documentView)
        {
            this.UserForm3 = userform3;
        }

        public IUserForm3 UserForm3 { get; private set; }

        public IOknaDocument OknaDoc
        {
            get
            {
                var ev = _logger.Log(nameof(InterceptionView3), "get_" + nameof(OknaDoc));
                IOknaDocument returnValue = this.UserForm3.OknaDoc;
                ev.ReturnValue = returnValue;
                return returnValue;
            }
            set
            {
                _logger.Log(nameof(InterceptionView3), "set_" + nameof(OknaDoc), value);
                this.UserForm3.OknaDoc = value;
            }
        }

        public IPart Part
        {
            get
            {
                var ev = _logger.Log(nameof(InterceptionView3), "get_" + nameof(Part));
                IPart returnValue = this.UserForm3.Part;
                ev.ReturnValue = returnValue;
                return returnValue;
            }
            set
            {
                _logger.Log(nameof(InterceptionView3), "set_" + nameof(Part), value);
                this.UserForm3.Part = value;
            }
        }
    }
}
