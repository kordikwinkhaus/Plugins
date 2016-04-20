using System;
using System.IO;
using System.Windows.Media.Imaging;
using WHOkna;

namespace Ctor.Models
{
    public static class IPartExtensions
    {
        public static BitmapFrameResult GetImageOnly(this IPart part, double scale)
        {
            if (part == null) throw new ArgumentNullException(nameof(part));

            if (part != null)
            {
                ITopObject topObj = null;
                while (topObj == null && part.Parent != null)
                {
                    topObj = part as ITopObject;
                    part = part.Parent;
                }

                if (topObj != null)
                {
                    //part.Update(true); // PG: Constant height handles were not moved to its positions.
                    IDrawing drawing = topObj.GetDrawing();

                    drawing.Emf = false;
                    drawing.Colors = part.Document.Application.ColorsOnScreen;
                    drawing.DimsFlags = 0; // no dims
                    drawing.PenStyle = 0;
                    drawing.FontName = "Arial";
                    drawing.FontSize = 10;
                    drawing.Height = (uint)(topObj.Rectangle.Height * scale);
                    drawing.Width = (uint)(topObj.Rectangle.Width * scale);
                    drawing.Labels = false;
                    drawing.Links = true;
                    drawing.PenWidth = 1;
                    drawing.Proportions = true;
                    if (drawing.Update())
                    {
                        var source = (MemoryStream)drawing.Picture;
                        return new BitmapFrameResult(source);
                    }
                }
            }

            return null;
        }
    }

    public class BitmapFrameResult
    {
        private MemoryStream _ms = new MemoryStream();

        internal BitmapFrameResult(MemoryStream source)
        {
            source.Position = 0;
            source.CopyTo(_ms);

            _ms.Position = 0;
            BitmapDecoder decoder = PngBitmapDecoder.Create(_ms, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
            decoder.DownloadCompleted += decoder_DownloadCompleted;
            this.Image = decoder.Frames[0];
        }

        public BitmapFrame Image { get; private set; }

        private void decoder_DownloadCompleted(object sender, EventArgs e)
        {
            BitmapDecoder decoder = sender as BitmapDecoder;
            if (decoder != null)
            {
                decoder.DownloadCompleted -= decoder_DownloadCompleted;
            }
            if (_ms != null)
            {
                _ms.Dispose();
                _ms = null;
            }
        }
    }
}
