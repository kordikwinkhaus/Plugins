using System;

namespace Ctor.Models
{
    public class ModelException : Exception
    {
        public ModelException(string message) 
            : base(message)
        {
        }
    }
}
