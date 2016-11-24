using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using Okna.Plugins;
using Okna.Plugins.ViewModels;
using WHOkna;

namespace Imager.ViewModels
{
    public class ImagerViewModel : ViewModelBase
    {
        public ImagerViewModel()
        {
            this.SaveImageCommand = new RelayCommand(SaveImage);

            this.Width = 120;
            this.Height = 120;
            this.DirectoryPath = GetDirectoryPath();
        }

        private static string GetDirectoryPath()
        {
            string filePath = new Uri(Assembly.GetAssembly(typeof(ImagerViewModel)).CodeBase).LocalPath;
            return System.IO.Path.Combine(System.IO.Path.GetDirectoryName(filePath), "Scripts");
        }

        private uint _width;
        public uint Width
        {
            get { return _width; }
            set
            {
                if (_width != value)
                {
                    _width = value;
                    OnPropertyChanged(nameof(Width));
                }
            }
        }

        private uint _height;
        public uint Height
        {
            get { return _height; }
            set
            {
                if (_height != value)
                {
                    _height = value;
                    OnPropertyChanged(nameof(Height));
                }
            }
        }

        private string _directoryPath;
        public string DirectoryPath
        {
            get { return _directoryPath; }
            set
            {
                if (_directoryPath != value)
                {
                    _directoryPath = value;
                    OnPropertyChanged(nameof(DirectoryPath));
                }
            }
        }

        private string _filename;
        public string Filename
        {
            get { return _filename; }
            set
            {
                if (_filename != value)
                {
                    _filename = value;
                    OnPropertyChanged(nameof(Filename));
                }
            }
        }

        public IPart Part { get; internal set; }

        public ICommand SaveImageCommand { get; private set; }

        private void SaveImage(object param)
        {
            if (this.Part != null)
            {
                try
                {
                    CreateAndSaveImage(this.Part);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Imager", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void CreateAndSaveImage(IPart part)
        {
            part.Update(true); // PG: Constant height handles were not moved to its positions.
            IDrawing drawing = part.GetDrawing();

            drawing.Emf = false;
            drawing.Colors = part.Position.Document.Application.ColorsOnScreen;
            //drawing.DimsFlags = 1; // main dimensions
            drawing.DimsFlags = 0; // no dimensions
            drawing.PenStyle = 0;
            drawing.FontName = "Arial";
            drawing.FontSize = 10;
            drawing.Height = this.Height;
            drawing.Labels = false;
            drawing.Links = true;
            drawing.PenWidth = 1;
            drawing.Width = this.Width;
            drawing.Proportions = true;
            if (drawing.Update())
            {
                var ms = drawing.Picture as MemoryStream;
                ms.Position = 0;

                File.WriteAllBytes(System.IO.Path.Combine(this.DirectoryPath, this.Filename + ".png"), ms.GetBuffer());
            }
        }
    }
}
