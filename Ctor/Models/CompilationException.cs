using System;

namespace Ctor.Models
{
    public class CompilationException : Exception
    {
        public CompilationException(Exception innerException)
            : base("Compilation error.", innerException)
        {
        }
    }
}
