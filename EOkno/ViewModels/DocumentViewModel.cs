using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Okna.Plugins.ViewModels;

namespace EOkno.ViewModels
{
    public class DocumentViewModel : ViewModelBase
    {
        public DocumentViewModel()
        {

        }

        public List<KomponentaViewModel> Komponenty { get; private set; } = new List<KomponentaViewModel>();

        public List<PovrchovaUpravaViewModel> PovrchoveUpravy { get; set; } = new List<PovrchovaUpravaViewModel>();

        private PovrchovaUpravaViewModel _vybranaPU;
        public PovrchovaUpravaViewModel VybranaPU
        {
            get { return _vybranaPU; }
            set
            {
                if (_vybranaPU != value)
                {
                    _vybranaPU = value;
                    OnPropertyChanged(nameof(VybranaPU));
                }
            }
        }

    }
}
