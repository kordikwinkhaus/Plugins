using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ctor.Models;
using Okna.Data;

namespace Ctor.Data
{
    internal class Repository : IRepository
    {
        private readonly ISqlConnectionWrapper _conn;

        internal Repository(ISqlConnectionWrapper conn)
        {
            _conn = conn;
        }

        public void InitDatabase()
        {
            string sql = @"CREATE TABLE dbo.ScriptCommands
(
  CmdName NVARCHAR(25) PRIMARY KEY NOT NULL,
  DisplayOrder INT NOT NULL,
  CmdDesc NVARCHAR(100) NULL,
  CmdCode NVARCHAR(MAX) NOT NULL,
  CmdImage IMAGE NULL 
)";
        }

        public void DeleteCommand(ScriptCommand command)
        {
            throw new NotImplementedException();
        }

        public IList<ScriptCommand> GetCommands()
        {
            throw new NotImplementedException();
        }

        public void SaveCommand(ScriptCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
