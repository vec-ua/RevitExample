using System;
using System.Windows;
using System.Windows.Media;

namespace CommonLibrary.Components
{
    /// <summary>
    /// Кнопка с поддержкой отображения иконки и текста
    /// </summary>
    public class MixedButton : System.Windows.Controls.Primitives.ToggleButton
    {
        static MixedButton()
        {
            IconProperty = DependencyProperty.Register("Icon", typeof(ImageSource), typeof(MixedButton));
            IconHeightProperty = DependencyProperty.Register("IconHeight", typeof(double), typeof(MixedButton), new FrameworkPropertyMetadata(double.NaN));
            IconWidthProperty = DependencyProperty.Register("IconWidth", typeof(double), typeof(MixedButton), new FrameworkPropertyMetadata(double.NaN));
            TextProperty = DependencyProperty.Register("Text", typeof(String), typeof(MixedButton));
            CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(MixedButton));
        }

        public static readonly DependencyProperty IconProperty;
        public static readonly DependencyProperty IconHeightProperty;
        public static readonly DependencyProperty IconWidthProperty;
        public static readonly DependencyProperty TextProperty;
        public static readonly DependencyProperty CornerRadiusProperty;

        /// <summary>
        /// Иконка
        /// </summary>
        public ImageSource Icon
        {
            get
            { return (ImageSource)GetValue(IconProperty); }
            set
            { SetValue(IconProperty, value); }
        }

        /// <summary>
        /// Иконка
        /// </summary>
        public double IconHeight
        {
            get
            { return (double)GetValue(IconHeightProperty); }
            set
            { SetValue(IconHeightProperty, value); }
        }

        /// <summary>
        /// Иконка
        /// </summary>
        public double IconWidth
        {
            get
            { return (double)GetValue(IconWidthProperty); }
            set
            { SetValue(IconWidthProperty, value); }
        }

        /// <summary>
        /// Текст
        /// </summary>
        public String Text
        {
            get
            { return (String)GetValue(TextProperty); }
            set
            { SetValue(TextProperty, value); }
        }

        /// <summary>
        /// Закругление
        /// </summary>
        public CornerRadius CornerRadius
        {
            get
            { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set
            { SetValue(CornerRadiusProperty, value); }
        }
    }
}