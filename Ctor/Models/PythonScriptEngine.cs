using System;
using System.IO;
using System.Text;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;

namespace Ctor.Models
{
    internal class PythonScriptEngine
    {
        private readonly ScriptEngine _engine;
        private readonly ScriptRuntime _runtime;
        private readonly ScriptScope _scope;

        internal PythonScriptEngine()
        {
            _engine = IronPython.Hosting.Python.CreateEngine();
            _runtime = _engine.Runtime;
            _scope = _engine.CreateScope();
            _scope.SetVariable("__name__", "__main__");
        }

        internal ScriptScope Variables
        {
            get { return _scope; }
        }

        internal string ErrorMessage { get; private set; }

        internal void SetOutput(Stream output)
        {
            _engine.Runtime.IO.SetOutput(output, Encoding.UTF8);
            _engine.Runtime.IO.SetOutput(output, Encoding.UTF8);
        }

        //internal void SetTrace(TracebackDelegate traceback)
        //{
        //    _engine.SetTrace(traceback);
        //}

        internal bool Execute(string source)
        {
            ScriptSource script = _engine.CreateScriptSourceFromString(source, SourceCodeKind.Statements);
            CompiledCode code = null;
            try
            {
                code = script.Compile();
            }
            catch (Exception ex)
            {
                throw new CompilationException(ex);
            }

            try
            {
                code.Execute(_scope);
                return true;
            }
            catch (Exception e)
            {
                ExceptionOperations eo = _engine.GetService<ExceptionOperations>();
                this.ErrorMessage = eo.FormatException(e);
                return false;
            }
        }
    }
}
