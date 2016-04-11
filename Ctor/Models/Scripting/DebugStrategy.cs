using System;
using IronPython.Runtime;
using IronPython.Runtime.Exceptions;

namespace Ctor.Models.Scripting
{
    internal abstract class DebugStrategy
    {
        internal const int NO_ACTION = 0;
        internal const int CONTINUE = 1;
        internal const int TB_CALL = 2;
        internal const int TB_RETURN = 3;
        internal const int TB_LINE = 4;

        internal virtual bool BreakTrace
        {
            get { return true; }
        }

        internal abstract int Call(TraceBackFrame frame, FunctionCode code);
        internal abstract int Line(TraceBackFrame frame, FunctionCode code);
        internal abstract int Return(TraceBackFrame frame, FunctionCode code);
    }

    internal class StepIntoStrategy : DebugStrategy
    {
        internal static StepIntoStrategy Instance = new StepIntoStrategy();

        internal override int Call(TraceBackFrame frame, FunctionCode code)
        {
            return TB_CALL;
        }

        internal override int Line(TraceBackFrame frame, FunctionCode code)
        {
            return TB_LINE;
        }

        internal override int Return(TraceBackFrame frame, FunctionCode code)
        {
            return TB_RETURN;
        }
    }

    internal class StepOutStrategy : DebugStrategy
    {
        internal static StepOutStrategy Instance = new StepOutStrategy();

        private int _returnLevel = 0;

        internal override int Call(TraceBackFrame frame, FunctionCode code)
        {
            _returnLevel++;
            return CONTINUE;
        }

        internal override int Line(TraceBackFrame frame, FunctionCode code)
        {
            return CONTINUE;
        }

        internal override int Return(TraceBackFrame frame, FunctionCode code)
        {
            if (_returnLevel == 0)
            {
                return TB_RETURN;
            }
            else
            {
                _returnLevel--;
                return CONTINUE;
            }
        }

        internal void ResetReturnLevel()
        {
            _returnLevel = 0;
        }
    }

    internal class StepOverStrategy : DebugStrategy
    {
        internal static StepOverStrategy Instance = new StepOverStrategy();

        private int _level = 0;
        private bool _shouldBreak = false;

        internal override int Call(TraceBackFrame frame, FunctionCode code)
        {
            _level++;
            _shouldBreak = false;
            return CONTINUE;
        }

        internal override int Line(TraceBackFrame frame, FunctionCode code)
        {
            return (_level <= 0 && _shouldBreak) ? TB_LINE : CONTINUE;
        }

        internal override int Return(TraceBackFrame frame, FunctionCode code)
        {
            if (_level <= 0 && _shouldBreak) return TB_RETURN;

            _level--;
            if (_level == 0)
            {
                _shouldBreak = true;
            }
            return CONTINUE;
        }

        internal void ResetLevel()
        {
            _level = 0;
            _shouldBreak = true;
        }
    }

    internal class RunToEndStrategy : DebugStrategy
    {
        internal static RunToEndStrategy Instance = new RunToEndStrategy();

        internal override bool BreakTrace
        {
            get { return false; }
        }

        internal override int Call(TraceBackFrame frame, FunctionCode code)
        {
            throw new InvalidOperationException();
        }

        internal override int Line(TraceBackFrame frame, FunctionCode code)
        {
            throw new InvalidOperationException();
        }

        internal override int Return(TraceBackFrame frame, FunctionCode code)
        {
            throw new InvalidOperationException();
        }
    }

    internal class RunToBreakPointStrategy : DebugStrategy
    {
        private readonly IScriptEditor _editor;

        internal RunToBreakPointStrategy(IScriptEditor editor)
        {
            _editor = editor;
        }

        internal override int Call(TraceBackFrame frame, FunctionCode code)
        {
            return (_editor.IsBreakPointOnLine((int)frame.f_lineno)) ? TB_CALL : CONTINUE;
        }

        internal override int Line(TraceBackFrame frame, FunctionCode code)
        {
            return (_editor.IsBreakPointOnLine((int)frame.f_lineno)) ? TB_LINE : CONTINUE;
        }

        internal override int Return(TraceBackFrame frame, FunctionCode code)
        {
            return CONTINUE;
        }
    }
}
