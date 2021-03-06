﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Okna.Plugins.ViewModels;

namespace ShapeOffset.ViewModels
{
    public class ItemViewModel : ViewModelBase
    {
        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }
    }
}
