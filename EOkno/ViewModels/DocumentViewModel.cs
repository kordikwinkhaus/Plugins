using System;
using EOkno.Models;

namespace EOkno.ViewModels
{
    public class DocumentViewModel : ColorsAndComponentsViewModel
    {
        private DocumentData _model;

        internal DocumentViewModel()
        {
        }

        internal void SetModel(DocumentData model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            if (_model != null)
            {
                _model.DataChanged -= model_DataChanged;
            }

            base.SetModel(model);
            _model = model;
            _model.DataChanged += model_DataChanged;

            OnPropertyChanged(nameof(Sleva));
        }

        private void model_DataChanged(object sender, EventArgs e)
        {
            if (this.Broker != null)
            {
                this.Broker.DocumentUpdated(this);
            }
        }

        internal MessageBroker Broker { get; set; }

        public decimal Sleva
        {
            get { return _model.Sleva; }
            set
            {
                if (_model.Sleva != value)
                {
                    _model.Sleva = value;
                    OnPropertyChanged(nameof(Sleva));
                }
            }
        }

        internal override void SetDefaults()
        {
            base.SetDefaults();
            this.Sleva = this.VychoziSleva;
        }
    }
}
