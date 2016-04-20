using System;
using System.Windows;
using System.Windows.Interop;

namespace Ctor
{
    public class CustomDialogFactory : IDialogFactory
    {
        private readonly IDialogFactory _inner;

        internal CustomDialogFactory(IDialogFactory inner)
        {
            _inner = inner;
        }

        internal IntPtr ParentHwnd { get; set; }

        public Window GetDialogFor(Type viewModelType)
        {
            var window = _inner.GetDialogFor(viewModelType);
            this.TrySetOwner(window);
            return window;
        }

        public Window GetDialogFor<T>() where T : class
        {
            var window = _inner.GetDialogFor<T>();
            this.TrySetOwner(window);
            return window;
        }

        private void TrySetOwner(Window currentWindow)
        {
            if (this.ParentHwnd == IntPtr.Zero) return;

            var helper = new WindowInteropHelper(currentWindow);
            helper.Owner = this.ParentHwnd;
        }
    }
}
