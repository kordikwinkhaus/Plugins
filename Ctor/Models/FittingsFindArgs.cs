using Ctor.Resources;

namespace Ctor.Models
{
    /// <summary>
    /// Parametry pro vyhledání kování pro křídlo.
    /// </summary>
    public class FittingsFindArgs
    {
        internal FittingsFindArgs()
        {
        }

        /// <summary>
        /// ID typu okna.
        /// </summary>
        public int WindowTypeID { get; set; }

        /// <summary>
        /// Standardní výběr (jednokřídlové).
        /// </summary>
        public bool Standard { get; set; }

        /// <summary>
        /// Štulpové křídlo, 1. segment (horní).
        /// </summary>
        public bool FalseMullion1 { get; set; }

        /// <summary>
        /// Štulpové křídlo, 2. segment (dolní).
        /// </summary>
        public bool FalseMullion2 { get; set; }

        /// <summary>
        /// Číslo výrobku štulpu (pokud se jedná o 2. segment).
        /// </summary>
        public string FalseMullionNrArt { get; set; }

        /// <summary>
        /// Nastaví křídlo jako sklopné.
        /// </summary>
        public bool TiltOnly
        {
            get { return _tiltOnly; }
            set
            {
                if (this.Standard)
                {
                    _tiltOnly = true;
                }
                else
                {
                    throw new ModelException(Strings.NotStandardSash);
                }
            }
        }
        private bool _tiltOnly;
    }
}
