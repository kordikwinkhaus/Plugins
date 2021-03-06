﻿using System;
using System.Windows.Input;
using Okna.Plugins;
using Okna.Plugins.ViewModels;
using WindowOffset.Models;
using WindowOffset.Properties;

namespace WindowOffset.ViewModels
{
    public class SideOffsetViewModel : ViewModelBase, IScaleable
    {
        private readonly SideOffset _model;
        private readonly string _name;

        internal SideOffsetViewModel(SideOffset model)
        {
            _model = model;
            _name = GetName(model.Side);
            this.ResetValueCommand = new RelayCommand(ResetValue, CanResetValue);
        }

        private static string GetName(int side)
        {
            switch (side)
            {
                case -1:
                    return Resources.Position;

                case 0:
                    return Resources.Left;

                case 1:
                    return Resources.TopLeft;

                case 2:
                    return Resources.Top;

                case 3:
                    return Resources.TopRight;

                case 4:
                    return Resources.Right;

                case 5:
                    return Resources.BottomRight;

                case 6:
                    return Resources.Bottom;

                case 7:
                    return Resources.BottomLeft;

                default:
                    throw new ArgumentException($"Invalid side value ({side}).");
            }
        }

        internal SideOffset Model
        {
            get { return _model; }
        }

        // strana v konstrukci 0=levý, 1=lh, 2=horní...
        internal int Side { get; set; }

        public string Name
        {
            get { return _name; }
        }

        public ICommand ResetValueCommand { get; private set; }

        protected virtual bool CanResetValue(object param)
        {
            return this.HasOwnValue;
        }

        protected virtual void ResetValue(object param)
        {
            if (this.CanResetValue(param))
            {
                _model.ResetOffset();

                OnPropertyChanged(nameof(Offset));
                OnPropertyChanged(nameof(HasOwnValue));
            }
        }

        public virtual int Offset
        {
            get { return this.Model.Offset; }
            set
            {
                if (_model.Offset != value)
                {
                    _model.Offset = value;
                    OnPropertyChanged(nameof(Offset));
                    OnPropertyChanged(nameof(HasOwnValue));
                }
            }
        }

        public bool HasOwnValue
        {
            get { return _model.HasOwnValue; }
        }

        internal void TrySetParentOffset(int offset)
        {
            _model.TrySetParentOffset(offset);
            OnPropertyChanged(nameof(Offset));
            OnPropertyChanged(nameof(HasOwnValue));
        }

        public virtual void Recalculate(double scale, double left, double top, double dimLayerHeight)
        {
            double x = (_model.Start.X + _model.End.X) / 2.0;
            double y = (_model.Start.Y + _model.End.Y) / 2.0;

            this.X = x / scale + left;
            this.Y = y / scale + top;
        }

        private double _x;
        public double X
        {
            get { return _x; }
            set
            {
                if (_x != value)
                {
                    _x = value;
                    OnPropertyChanged(nameof(X));
                }
            }
        }

        private double _y;
        public double Y
        {
            get { return _y; }
            set
            {
                if (_y != value)
                {
                    _y = value;
                    OnPropertyChanged(nameof(Y));
                }
            }
        }
    }
}
