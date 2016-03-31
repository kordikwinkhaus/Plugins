using Ctor.Models;
using Ctor.Resources;

namespace Ctor.ViewModels
{
    public class Context : IContext
    {
        private ContextViewModel _parent;

        internal Context(ContextViewModel parent)
        {
            _parent = parent;
        }

        /// <summary>
        /// ID vybraného typu okna.
        /// Pokud není vybráno, vyhodí výjimku <see cref="ModelException"/>.
        /// </summary>
        public int WindowsType
        {
            get
            {
                int id = _parent.WindowTypeID;
                if (id == 0)
                {
                    throw new ModelException(Strings.SelectWindowTypeFirst);
                }
                return id;
            }
        }

        /// <summary>
        /// ID vybrané barvy.
        /// Pokud není vybráno, vyhodí výjimku <see cref="ModelException"/>.
        /// </summary>
        public int WindowsColor
        {
            get
            {
                int id = _parent.WindowColorID;
                if (id == 0)
                {
                    throw new ModelException(Strings.SelectWindowColorFirst);
                }
                return id;
            }
        }

        /// <summary>
        /// Číslo paketu (buďto výchozího z typu nebo vybraného).
        /// Pokud není vybráno, popř. zadána platná volba, 
        /// vyhodí výjimku <see cref="ModelException"/>.
        /// </summary>
        public string Glasspacket
        {
            get
            {
                string nr_art = null;
                if (_parent.UseDefaultGlasspacket)
                {
                    nr_art = _parent.WindowType?.DefaultGlasspacket;
                }
                else
                {
                    nr_art = _parent.GlasspacketNrArt;
                }

                if (string.IsNullOrEmpty(nr_art))
                {
                    throw new ModelException(Strings.SelectGlasspacketFirst);
                }

                return nr_art;
            }
        }
    }
}