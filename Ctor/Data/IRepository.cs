using System.Collections.Generic;
using Ctor.Models;

namespace Ctor.Data
{
    internal interface IRepository
    {
        void InitDatabase();

        IList<ScriptCommand> GetCommands();

        void DeleteCommand(ScriptCommand command);

        void SaveCommand(ScriptCommand command);
    }
}
