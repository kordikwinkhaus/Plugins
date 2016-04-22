using System.Collections.Generic;
using Okna.Data;

namespace Ctor.Models
{
    /// <summary>
    /// Typ okna.
    /// </summary>
    public class WindowType
    {
        private static string[] s_frames = new[] { "osciez1", "osciez2", "osciez3", "osciez4", "osciez5", "osciez6" };
        private static string[] s_sashes = new[] { "skrzydl1", "skrzydl2", "skrzydl3", "skrzydl4" };

        private readonly IDatabase _database;
        private readonly DynamicDictionary _dict;
        private readonly dynamic _data;
        private ProfileColors _colors;

        internal WindowType(DynamicDictionary data, IDatabase database)
        {
            _data = _dict = data;
            _database = database;
        }

        /// <summary>
        /// Index typu okna.
        /// </summary>
        public int ID
        {
            get { return (int)_data.indeks; }
        }

        /// <summary>
        /// Název typu okna.
        /// </summary>
        public string Name
        {
            get { return (string)_data.nazwa; }
        }

        public override string ToString()
        {
            return "{ID = " + this.ID + ", Name = \"" + this.Name + "\"}";
        }

        /// <summary>
        /// Číslo výrobku výchozího paketu.
        /// </summary>
        public string DefaultGlasspacket
        {
            get { return _data.szybadef as string; }
        }

        /// <summary>
        /// Index výchozí skupiny kování.
        /// </summary>
        public int DefaultFittingsGroup
        {
            get { return (int)_data.grupadef; }
        }

        /// <summary>
        /// Číslo výrobku prvního zadaného štulpu.
        /// </summary>
        public string DefaultFalseMullion
        {
            get { return (string)_data.przymyk1; }
        }

        /// <summary>
        /// Vrací pole všech zadaných štulpů (nevrací nezadané prvky).
        /// </summary>
        public IList<string> FalseMullions
        {
            get
            {
                List<string> mullions = new List<string>();

                for (int i = 1; i < 6; i++)
                {
                    string nrArt = (string)_dict.GetValue("przymyk" + i);
                    if (!string.IsNullOrWhiteSpace(nrArt))
                    {
                        mullions.Add(nrArt);
                    }
                }

                return mullions;
            }
        }

        /// <summary>
        /// Vrací objekt pro zadání parametrů sloupku(ů).
        /// </summary>
        public MullionFindArgs Mullions
        {
            get { return new MullionFindArgs(_dict); }
        }

        /// <summary>
        /// Vrací hodnotu předaného pole.
        /// </summary>
        /// <param name="fieldname">Název pole (tabulka dbo.typyp). Umožňuje načíst i doplňková pole.</param>
        public object GetField(string fieldname)
        {
            return _dict.GetValue(fieldname);
        }

        internal ProfileColors GetProfileColors()
        {
            if (_colors == null)
            {
                _colors = new ProfileColors(_database);
                foreach (var fieldname in s_frames)
                {
                    string nr_art = (string)_dict.GetValue(fieldname);
                    _colors.AddFrame(nr_art);
                }
                foreach (var fieldname in s_sashes)
                {
                    string nr_art = (string)_dict.GetValue(fieldname);
                    _colors.AddSash(nr_art);
                }
                _colors.ReadColors();
            }
            return _colors;
        }

        #region Nested classes

        internal class ProfileColors
        {
            private readonly IDatabase _database;
            private readonly List<int> _colors = new List<int>();
            private readonly List<string> _profiles = new List<string>();

            public ProfileColors(IDatabase database)
            {
                _database = database;
            }

            internal void AddFrame(string nr_art)
            {
                if (!string.IsNullOrWhiteSpace(nr_art) && !_profiles.Contains(nr_art))
                {
                    if (!_database.IsVirtualProfile(nr_art, "dbo.osciezp"))
                    {
                        _profiles.Add(nr_art);
                    }
                }
            }

            internal void AddSash(string nr_art)
            {
                if (!string.IsNullOrWhiteSpace(nr_art) && !_profiles.Contains(nr_art))
                {
                    if (!_database.IsVirtualProfile(nr_art, "dbo.skrzyd"))
                    {
                        _profiles.Add(nr_art);
                    }
                }
            }

            internal void ReadColors()
            {
                _colors.Clear();
                _colors.AddRange(_database.GetCommonColors(_profiles));
            }

            internal bool ContainsColor(int colorID)
            {
                return _colors.Contains(colorID);
            }

            internal IEnumerable<int> GetColors()
            {
                return _colors;
            }
        }

        #endregion
    }
}
