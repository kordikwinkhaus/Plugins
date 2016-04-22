using System;
using IronPython.Runtime;

namespace Ctor.Models.Scripting
{
    internal class TracebackStepEventArgs : EventArgs
    {
        internal TracebackStepEventArgs(PythonDictionary globals, PythonDictionary locals, 
            TracebackStepType tracebackStepType, object payload)
        {
            this.Globals = globals;
            this.Locals = locals;
            this.StepType = tracebackStepType;
            this.Payload = payload;
        }

        internal PythonDictionary Globals { get; private set; }

        internal PythonDictionary Locals { get; private set; }

        internal TracebackStepType StepType { get; private set; }

        /// <summary>
        /// For call and line, payload is null.
        /// For return, payload is the value being returned from the function. 
        /// For exception, the payload is information about the exception and where it was thrown.
        /// </summary>
        internal object Payload { get; private set; }
    }

    internal enum TracebackStepType
    {
        NotSet,
        Call,
        Line,
        Return,
        Exception
    }
}
