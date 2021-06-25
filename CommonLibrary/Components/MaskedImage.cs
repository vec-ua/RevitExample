using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CommonLibrary.Components
{
    /// <summary>
    /// Компонент отображения рисунков с возможностью наложения маски при
    /// изменении состояния доступности компонента (enabled\disabled)
    /// </summary>
    public class MaskedImage : Image
    {
        static MaskedImage()
        {
            MaskPixelFormatProperty = DependencyProperty.Register(
                "MaskPixelFormat", typeof(PixelFormat), typeof(MaskedImage), new FrameworkPropertyMetadata(PixelFormats.Gray8), IsPixelFormatValid);

            IsEnabledProperty.OverrideMetadata(typeof(MaskedImage), new FrameworkPropertyMetadata(IsEnabledChange));
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MaskedImage), new FrameworkPropertyMetadata(typeof(MaskedImage)));
        }

        public static readonly DependencyProperty MaskPixelFormatProperty;

        /// <summary>
        /// Формат пикселей, используемый при формировании маски рисунка
        /// </summary>
        public PixelFormat MaskPixelFormat
        {
            get
            { return (PixelFormat)GetValue(MaskPixelFormatProperty); }
            set
            { SetValue(MaskPixelFormatProperty, value); }
        }

        /// <summary>
        /// Изменение маски рисунка при изменении состояния доступности компонента
        /// </summary>
        private static void IsEnabledChange(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            MaskedImage image = obj as MaskedImage;
            if ((image == null) || (image.Source == null))
                return;

            if ((bool)args.NewValue)
            {
                if ((image.Source as FormatConvertedBitmap) != null)
                    image.Source = ((FormatConvertedBitmap)image.Source).Source;
                image.OpacityMask = null;
            }
            else
            {
                BitmapImage bitmap = new BitmapImage(new Uri(image.Source.ToString()));
                image.Source = new FormatConvertedBitmap(bitmap, image.MaskPixelFormat, null, 0);
                image.OpacityMask = new ImageBrush(bitmap);
            }
        }

        /// <summary>
        /// Проверка корректности устанавливаемого значения PixelFormat
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsPixelFormatValid(object value)
        {
            return (value != null);
        }
    }
}