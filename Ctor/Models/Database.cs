using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Ctor.Resources;
using Okna.Data;
using WHOkna;

namespace Ctor.Models
{
    public class Database : IDatabase
    {
        private readonly IOknaApplication _app;
        private readonly ISqlConnectionWrapper _conn;
        private readonly Dictionary<int, WindowType> _windowTypes = new Dictionary<int, WindowType>();
        private readonly Dictionary<int, FittingsGroup> _fittingsGroups = new Dictionary<int, FittingsGroup>();
        private readonly Dictionary<string, List<string>> _virtualProfiles = new Dictionary<string, List<string>>();

        internal Database(ISqlConnectionWrapper conn, IOknaApplication app)
        {
            _conn = conn;
            _app = app;
        }

        internal IOknaDocument CurrentDocument { get; set; }

        public WindowType GetWindowType(int id)
        {
            WindowType result;
            if (!_windowTypes.TryGetValue(id, out result))
            {
                result = GetWindowTypeCore(id);
                _windowTypes.Add(id, result);
            }
            return result;
        }

        private WindowType GetWindowTypeCore(int id)
        {
            DatabaseCommand cmd = new DatabaseCommand
            {
                CommandText = "SELECT * FROM dbo.typyp WHERE indeks=@id"
            };
            cmd.AddParameter("@id", id);

            var types = _conn.ExecuteQuery(cmd);
            if (types.Count != 1)
            {
                throw new ModelException(string.Format(Strings.NoWindowTypeWithID, id));
            }
            return new WindowType(types[0], this);
        }

        public FittingsGroup GetFittingsGroup(int id)
        {
            FittingsGroup result;
            if (!_fittingsGroups.TryGetValue(id, out result))
            {
                result = GetFittingsGroupCore(id);
                _fittingsGroups.Add(id, result);
            }
            result.Init(this.CurrentDocument);
            return result;
        }

        private FittingsGroup GetFittingsGroupCore(int id)
        {
            DatabaseCommand cmd = new DatabaseCommand
            {
                CommandText = "SELECT * FROM dbo.grupy WHERE indeks=@id"
            };
            cmd.AddParameter("@id", id);

            var groups = _conn.ExecuteQuery(cmd);
            if (groups.Count != 1)
            {
                throw new ModelException(string.Format(Strings.NoFittingsGroupWithID, id));
            }
            return new FittingsGroup(groups[0], this);
        }

        public bool IsVirtual(string nr_art, string table)
        {
            List<string> virtualsAtTable;
            if (!_virtualProfiles.TryGetValue(table, out virtualsAtTable))
            {
                virtualsAtTable = GetVirtuals(table);
                _virtualProfiles.Add(table, virtualsAtTable);
            }
            return virtualsAtTable.Contains(nr_art);
        }

        private List<string> GetVirtuals(string table)
        {
            List<string> result = new List<string>();
            using (SqlCommand cmd = _conn.GetCmd("SELECT nr_art FROM " + table + " WHERE virtual=1"))
            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    result.Add(dr.GetString(0));
                }
            }
            return result;
        }

        public List<int> GetCommonColors(IList<string> articles)
        {
            List<int> result = new List<int>();

            using (SqlCommand cmd = _conn.GetCmd("dbo.CommonColors"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@articles", string.Join("|", articles));
                cmd.Parameters.AddWithValue("@kolor_db", "koloryp");
                cmd.Parameters.AddWithValue("@dealer", _app.UserProfile == EProfile.eDealer);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        result.Add(dr.GetInt32(0));
                    }
                }
            }

            return result;
        }

        public IList<DynamicDictionary> ExecuteQuery(DatabaseCommand cmd)
        {
            return _conn.ExecuteQuery(cmd);
        }

        public int FindFittingType(int fittingsGroupID, FittingsFindArgs args)
        {
            var fittingsGroup = GetFittingsGroup(fittingsGroupID);
            return fittingsGroup.FindFittingType(args);
        }
    }
}
