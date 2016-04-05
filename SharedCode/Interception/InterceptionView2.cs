using UserExtensions;

namespace Okna.Plugins.Interception
{
    public class InterceptionView2 : InterceptionView, IUserForm2
    {
        public InterceptionView2(IUserForm2 userform2, bool documentView = false)
            : base(userform2, documentView)
        {
            this.UserForm2 = userform2;
        }

        public IUserForm2 UserForm2 { get; private set; }

        public IDataRequest DataCallback
        {
            get
            {
                var ev = _logger.Log(nameof(InterceptionView2), "get_" + nameof(DataCallback));
                IDataRequest returnValue = this.UserForm2.DataCallback;
                ev.ReturnValue = returnValue;
                return returnValue;
            }
            set
            {
                _logger.Log(nameof(InterceptionView2), "set_" + nameof(DataCallback), value);
                this.UserForm2.DataCallback = value;
            }
        }
    }
}
