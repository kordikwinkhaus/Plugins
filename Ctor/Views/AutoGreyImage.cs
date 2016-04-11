using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Ctor.Views
{
    public class AutoGreyImage : Image
    {
        static AutoGreyImage()
        {
            IsEnabledProperty.OverrideMetadata(typeof(AutoGreyImage), new FrameworkPropertyMetadata(true, new PropertyChangedCallback(OnAutoGreyScaleImageIsEnabledPropertyChanged)));
        }

        private static void OnAutoGreyScaleImageIsEnabledPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs args)
        {
            var autoGreyScaleImg = source as AutoGreyImage;
            var isEnable = Convert.ToBoolean(args.NewValue);
            if (autoGreyScaleImg != null)
            {
                if (!isEnable)
                {
                    // Get the source bitmap
                    var bitmapImage = new BitmapImage(new Uri(autoGreyScaleImg.Source.ToString()));

                    // Convert it to Gray
                    autoGreyScaleImg.Source = new FormatConvertedBitmap(bitmapImage, PixelFormats.Gray32Float, null, 0);

                    // Create Opacity Mask for greyscale image as FormatConvertedBitmap does not keep transparency info
                    autoGreyScaleImg.OpacityMask = new ImageBrush(bitmapImage);
                }
                else
                {
                    // Set the Source property to the original value.
                    autoGreyScaleImg.Source = ((FormatConvertedBitmap)autoGreyScaleImg.Source).Source;

                    // Reset the Opacity Mask
                    autoGreyScaleImg.OpacityMask = null;
                }
            }
        }
    }
}
