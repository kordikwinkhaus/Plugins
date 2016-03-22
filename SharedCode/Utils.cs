using System.Data.OleDb;
using System.Data.SqlClient;

namespace Okna.Plugins
{
    public static class Utils
    {
        public static string ModifyConnString(string connString)
        {
            OleDbConnectionStringBuilder oleConnBuilder = new OleDbConnectionStringBuilder(connString);
            SqlConnectionStringBuilder sqlConnBuilder = new SqlConnectionStringBuilder();

            object tokenValue;
            if (oleConnBuilder.TryGetValue("Application Name", out tokenValue))
            {
                sqlConnBuilder.ApplicationName = tokenValue.ToString();
            }
            
            object val;
            oleConnBuilder.TryGetValue ("Server", out val);
            if (val != null) sqlConnBuilder.Add("Server", val.ToString());

            oleConnBuilder.TryGetValue("Database", out val);
            if (val != null) sqlConnBuilder.Add("Database", val.ToString());
            if (oleConnBuilder.TryGetValue("Trusted_Connection", out tokenValue))
            {
                sqlConnBuilder.IntegratedSecurity = (string.Compare(tokenValue.ToString(), "yes", System.StringComparison.OrdinalIgnoreCase) == 0);
            }
            if (!sqlConnBuilder.IntegratedSecurity)
            {
                sqlConnBuilder.UserID = oleConnBuilder["User Id"].ToString();
                sqlConnBuilder.Password = oleConnBuilder["Password"].ToString();
            }
            if (oleConnBuilder.TryGetValue("MARS Connection", out tokenValue))
            {
                sqlConnBuilder.MultipleActiveResultSets = (string.Compare(tokenValue.ToString(), "true", System.StringComparison.OrdinalIgnoreCase) == 0);
            }

            return sqlConnBuilder.ToString();
        }
    }
}
