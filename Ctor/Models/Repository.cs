using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Okna.Data;

namespace Ctor.Models
{
    public class Repository : IRepository
    {
        private ISqlConnectionWrapper _conn;

        internal Repository(ISqlConnectionWrapper conn)
        {
            _conn = conn;
        }


    }
}
