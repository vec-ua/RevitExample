using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;

namespace CommonLibrary.Behaviors
{
    /// <summary>
    /// Класс поддержки стилизации компонентов окон
    /// </summary>
    public sealed class WindowStyleBehavior
    {
        #region WinSDK import

        private const int GWL_STYLE = -16;
        private const int WS_MAXIMIZEBOX = 0x10000;
        private const int WS_MINIMIZEBOX = 0x20000;

        [DllImport("user32.dll")]
        extern private static int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        extern private static int SetWindowLong(IntPtr hwnd, int index, int value);

        #endregion

        static WindowStyleBehavior()
        {
            DisableMaximizeButtonProperty = DependencyProperty.RegisterAttached(
                "DisableMaximizeButton", typeof(bool), typeof(WindowStyleBehavior), new UIPropertyMetadata(false, OnDisableWindowButton));
            DisableMinimizeButtonProperty = DependencyProperty.RegisterAttached(
                "DisableMinimizeButton", typeof(bool), typeof(WindowStyleBehavior), new UIPropertyMetadata(false, OnDisableWindowButton));
        }

        /// <summary>
        /// Признак отключения кнопки максимизации окна
        /// </summary>
        public static readonly DependencyProperty DisableMaximizeButtonProperty;
        /// <summary>
        /// Признак отключения кнопки минимизации окна
        /// </summary>
        public static readonly DependencyProperty DisableMinimizeButtonProperty;

        #region Get\Set methods of attached properties

        public static bool GetDisableMaximizeButton(DependencyObject obj)
        {
            return (bool)obj.GetValue(DisableMaximizeButtonProperty);
        }

        public static void SetDisableMaximizeButton(DependencyObject obj, bool value)
        {
            obj.SetValue(DisableMaximizeButtonProperty, value);
        }

        public static bool GetDisableMinimizeButton(DependencyObject obj)
        {
            return (bool)obj.GetValue(DisableMinimizeButtonProperty);
        }

        public static void SetDisableMinimizeButton(DependencyObject obj, bool value)
        {
            obj.SetValue(DisableMinimizeButtonProperty, value);
        }

        #endregion

        private static void ChangeButtonState(Button button, bool active)
        {
            if (active)
            {
                button.IsHitTestVisible = true;
                button.Opacity = 100;
            }
            else
            {
                button.IsHitTestVisible = false;
                button.Opacity = 0;
            }
        }

        private static void OnDisableWindowButton(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            Window window = d as Window;
            if (window != null)
            {
                window.Loaded -= OnWindowLoaded;
                window.Loaded += OnWindowLoaded;
            }
        }

        private static void OnWindowLoaded(object sender, EventArgs args)
        {
            Window window = sender as Window;

            IntPtr windowHandle = new WindowInteropHelper(window).Handle;
            int windowStyle = GetWindowLong(windowHandle, GWL_STYLE);

            if (GetDisableMaximizeButton(window))
                windowStyle &= ~WS_MAXIMIZEBOX;
            else
                windowStyle |= WS_MAXIMIZEBOX;
            if (GetDisableMinimizeButton(window))
                windowStyle &= ~WS_MINIMIZEBOX;
            else
                windowStyle |= WS_MINIMIZEBOX;

            SetWindowLong(windowHandle, GWL_STYLE, windowStyle);
        }
    }
}
