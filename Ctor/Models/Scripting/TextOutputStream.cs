using System;
using System.IO;
using System.Text;

namespace Ctor.Models.Scripting
{
    internal class TextOutputStream : Stream
    {
        private MemoryStream _text = new MemoryStream();

        internal TextOutputStream()
        {
        }

        public override bool CanRead
        {
            get { return false; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override long Length
        {
            get { return _text.Length; }
        }

        public override long Position
        {
            get { return _text.Length; }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override void Flush()
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _text.Write(buffer, offset, count);
            RaiseTextChangedEvent();
        }

        private void RaiseTextChangedEvent()
        {
            if (TextChanged != null)
            {
                TextChanged(this, new TextChangedEventArgs(GetText()));
            }
        }

        internal event EventHandler<TextChangedEventArgs> TextChanged;

        private string GetText()
        {
            StreamReader sr = new StreamReader(_text);
            long oldPosition = _text.Position;
            _text.Position = 0;
            string result = sr.ReadToEnd();
            _text.Position = oldPosition;
            return result;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (_text != null)
            {
                _text.Dispose();
                _text = null;
            }
        }

        internal void WriteLine(string errorMessage)
        {
            byte[] data = Encoding.UTF8.GetBytes(errorMessage);
            this.Write(data, 0, data.Length);

            data = Encoding.UTF8.GetBytes("\r\n");
            this.Write(data, 0, data.Length);
        }

        internal void Clear()
        {
            var old = _text;
            _text = new MemoryStream();
            RaiseTextChangedEvent();
            try
            {
                old.Dispose();
            }
            catch { }
        }
    }

    public class TextChangedEventArgs : EventArgs
    {
        internal TextChangedEventArgs(string newText)
        {
            this.Text = newText;
        }

        public string Text { get; private set; }
    }
}
