using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Threading;
using Ctor.Models;
using Ctor.ViewModels;
using WHOkna;

namespace Ctor.Views
{
    public partial class AreaSelectorDialog : Window
    {
        private readonly DispatcherTimer _resizeTimer;
        private IArea _selectedArea;
        private AreaSelectorViewModel _viewmodel;

        public AreaSelectorDialog()
        {
            InitializeComponent();

            _resizeTimer = new DispatcherTimer();
            _resizeTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            _resizeTimer.IsEnabled = false;
            _resizeTimer.Tick += resizeTimer_Tick;
        }

        private IAreaProvider _areaProvider;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadImage();
        }

        private void Window_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _viewmodel = (AreaSelectorViewModel)e.NewValue;
            _areaProvider = _viewmodel.AreaProvider;

            areas.Width = _areaProvider.PositionWidth;
            areas.Height = _areaProvider.PositionHeight;

            var rectStyle = (Style)areas.FindResource("Empty");
            foreach (var area in _areaProvider.GetEmptyAreas())
            {
                var shape = new Rectangle();
                shape.Tag = area;
                var rect = area.Rectangle;
                Canvas.SetLeft(shape, rect.Left);
                Canvas.SetTop(shape, rect.Top);
                shape.Width = rect.Width;
                shape.Height = rect.Height;
                shape.Style = rectStyle;
                shape.MouseDown += Shape_MouseDown;
                areas.Children.Add(shape);
            }
        }

        private void Shape_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                Rectangle shape = (Rectangle)sender;
                _selectedArea = (IArea)shape.Tag;
                SetSelectedAreaAndExit();
            }
            else if (e.ClickCount == 1)
            {
                foreach (UIElement item in areas.Children)
                {
                    SetIsSelected(item, false);
                }
                _selectedArea = null;

                Rectangle shape = (Rectangle)sender;
                SetIsSelected(shape, true);
                _selectedArea = (IArea)shape.Tag;
            }
        }

        private void LoadImage()
        {
            double scale = viewbox.GetScaleFactor();
            if (double.IsNaN(scale)) scale = 0.5;
            posImage.Source = _areaProvider.GetPositionImage(scale).Image;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _resizeTimer.IsEnabled = true;
            _resizeTimer.Stop();
            _resizeTimer.Start();
        }

        void resizeTimer_Tick(object sender, EventArgs e)
        {
            _resizeTimer.IsEnabled = false;

            LoadImage();
        }

        public static bool GetIsSelected(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsSelectedProperty);
        }

        public static void SetIsSelected(DependencyObject obj, bool value)
        {
            obj.SetValue(IsSelectedProperty, value);
        }

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.RegisterAttached("IsSelected", 
                typeof(bool), 
                typeof(AreaSelectorDialog), 
                new PropertyMetadata(false));

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedArea != null)
            {
                SetSelectedAreaAndExit();
            }
        }

        private void SetSelectedAreaAndExit()
        {
            _viewmodel.SelectedArea = _selectedArea;
            this.DialogResult = true;
        }
    }
}
