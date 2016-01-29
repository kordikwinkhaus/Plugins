using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ShapeOffset.ViewModels;

namespace ShapeOffset.Views
{
    public partial class CanvasView : UserControl
    {
        private CanvasViewModel _viewmodel;

        public CanvasView()
        {
            InitializeComponent();
        }

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _viewmodel = e.NewValue as CanvasViewModel;
        }

        private void DrawingCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var position = Mouse.GetPosition(canvas);
            if (_viewmodel != null)
            {
                _viewmodel.AddPoint((int)position.X, (int)position.Y);
            }
        }
    }
}
