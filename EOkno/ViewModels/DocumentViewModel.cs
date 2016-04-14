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

            base.SetModel(model);
            _model = model;

            OnPropertyChanged(nameof(Sleva));
        }

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

        // TODO: broker

        internal MessageBroker Broker { get; set; }

        internal override void NotifyChange()
        {
            if (this.Broker != null)
            {
                this.Broker.DocumentUpdated(this);
            }
        }
    }
}
