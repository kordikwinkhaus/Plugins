using System;
using System.IO;
using System.Text;
using IronPython.Hosting;
using IronPython.Runtime.Exceptions;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;

namespace Ctor.Models.Scripting
{
    internal class PythonScriptEngine
    {
        private readonly ScriptEngine _engine;
        private readonly ScriptRuntime _runtime;
        private ScriptScope _scope;

        internal PythonScriptEngine()
        {
            _engine = Python.CreateEngine();
            _runtime = _engine.Runtime;
        }

        internal void InitVariablesScope()
        {
            _scope = _engine.CreateScope();
        }

        internal ScriptScope Variables
        {
            get { return _scope; }
        }

        internal string ErrorMessage { get; private set; }

        internal void SetOutput(Stream output)
        {
            _engine.Runtime.IO.SetOutput(output, Encoding.UTF8);
            _engine.Runtime.IO.SetErrorOutput(output, Encoding.UTF8);
        }

        internal void SetTrace(TracebackDelegate traceback)
        {
            _engine.SetTrace(traceback);
        }

        internal CompiledCode Compile(string source)
        {
            ScriptSource script = _engine.CreateScriptSourceFromString(source, SourceCodeKind.Statements);
            try
            {
                return script.Compile();
            }
            catch (Exception ex)
            {
                throw new CompilationException(ex);
            }
        }

        internal bool Execute(string source)
        {
            CompiledCode code = Compile(source);

            try
            {
                code.Execute(_scope);
                return true;
            }
            catch (SystemExitException)
            {
                return true;
            }
            catch (ModelException mex)
            {
                this.ErrorMessage = mex.Message;
                return false;
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
