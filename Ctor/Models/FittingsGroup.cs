using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ctor.Resources;
using Okna.Data;
using WHOkna;

namespace Ctor.Models
{
    /// <summary>
    /// Skupina kování.
    /// </summary>
    public class FittingsGroup
    {
        private readonly Database _database;
        private readonly DynamicDictionary _dict;
        private string _dealer;
        private IOknaApplication _app;

        internal FittingsGroup(DynamicDictionary dynamicDictionary, Database database)
        {
            _dict = dynamicDictionary;
            _database = database;
        }

        /// <summary>
        /// ID skupiny kování.
        /// </summary>
        public int ID
        {
            get { return (int)_dict.GetValue("indeks"); }
        }

        /// <summary>
        /// Název skupiny kování.
        /// </summary>
        public string Name
        {
            get { return (string)_dict.GetValue("nazwa"); }
        }

        internal void Init(IOknaDocument currentDocument)
        {
            _dealer = currentDocument.Dealer;
            _app = currentDocument.Application;
        }

        /// <summary>
        /// Vyhledá kování v této skupině a vrátí první nalezený typ.
        /// </summary>
        /// <param name="args">Parametry pro vyhledání kování.</param>
        public int FindFittingType(FittingsFindArgs args)
        {
            var cmd = GetCmd(args, true);
            var types = _database.ExecuteQuery(cmd);
            if (types.Count == 1)
            {
                return (int)types[0].GetValue("indeks");
            }

            throw new ModelException(Strings.CannotFindFittingsType);
        }

        /// <summary>
        /// Vyhledá kování v této skupině a vrátí všechny nalezené typy.
        /// </summary>
        /// <param name="args">Parametry pro vyhledání kování.</param>
        public IList<DynamicDictionary> FindFittingsTypes(FittingsFindArgs args)
        {
            var cmd = GetCmd(args, false);
            return _database.ExecuteQuery(cmd);
        }

        private DatabaseCommand GetCmd(FittingsFindArgs args, bool onlyFirst)
        {
            StringBuilder sql = new StringBuilder();
            DatabaseCommand cmd = new DatabaseCommand();

            if (onlyFirst)
            {
                sql.Append("SELECT TOP 1 indeks ");
            }
            else
            {
                sql.Append("SELECT indeks, nazwa, skrot, przymi, przymii ");
                // obchodní názvy
                // iif(o.nazwa <> N'', o.nazwa, t.nazwa) nazwa,iif(o.skrot <> N'', o.skrot, t.skrot) skrot
            }
            sql.Append("FROM dbo.typy WHERE idx_grupy=@idx_grupy AND luk=0 AND skos=0");
            cmd.AddParameter("@idx_grupy", this.ID);

            if (args.FalseMullion1)
            {
                sql.Append(" AND przymi=1");
            }
            else if (args.FalseMullion2)
            {
                sql.Append(" AND przymii=1 AND (przymtyp=0 OR przymtyp=(SELECT TOP 1 wrab FROM dbo.przymyki WHERE nr_art=@nr_art))");
                cmd.AddParameter("@nr_art", args.FalseMullionNrArt);
            }
            else
            {
                sql.Append(" AND prostok=1");
                if (args.TiltOnly)
                {
                    sql.Append(" AND SUBSTRING(widok, 13, 1)=0x02");
                }
            }

            // atributy
            sql.Append(" AND (NOT EXISTS(SELECT * FROM dbo.typypattr WHERE idx_typu=@idx_typu) OR ")
               .Append("EXISTS(SELECT * FROM dbo.typypattr tpa LEFT JOIN dbo.typyattr ta ON tpa.atrybut=ta.atrybut WHERE tpa.idx_typu=@idx_typu AND ta.idx_typu=indeks))");
            cmd.AddParameter("@idx_typu", args.WindowTypeID);

            // skryté typy
            sql.Append(" AND ").Append(_app.HideAtDealerFilter("typy", _dealer, string.Empty));

            sql.Append(" ORDER BY longlp, nazwa");

            cmd.CommandText = sql.ToString();
            return cmd;
        }
    }
}
