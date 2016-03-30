using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Okna.Data;

namespace Ctor.Models
{
    public class Database : IDatabase
    {
        private ISqlConnectionWrapper _conn;

        internal Database(ISqlConnectionWrapper conn)
        {
            _conn = conn;
        }


    }
}
