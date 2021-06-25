using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

namespace CommonLibrary.Helpers
{
    /// <summary>
    /// Вспомогательный класс для быстрой настройки колонок и строк класса Grid
    /// </summary>
    public sealed class GridHelper
    {
        static GridHelper()
        {
            ColumnCountProperty = DependencyProperty.RegisterAttached(
                "ColumnCount", typeof(byte), typeof(GridHelper), new UIPropertyMetadata(OnColumnCountChanged));
            ColumnsMaxWidthProperty = DependencyProperty.RegisterAttached(
                "ColumnsMaxWidth", typeof(String), typeof(GridHelper), new UIPropertyMetadata(OnColumnsMaxWidthChanged));
            ColumnsMinWidthProperty = DependencyProperty.RegisterAttached(
                "ColumnsMinWidth", typeof(String), typeof(GridHelper), new UIPropertyMetadata(OnColumnsMinWidthChanged));
            ColumnsWidthProperty = DependencyProperty.RegisterAttached(
                "ColumnsWidth", typeof(String), typeof(GridHelper), new UIPropertyMetadata(OnColumnsWidthChanged));
            RowCountProperty = DependencyProperty.RegisterAttached(
                "RowCount", typeof(byte), typeof(GridHelper), new UIPropertyMetadata(OnRowCountChanged));
            RowsHeightProperty = DependencyProperty.RegisterAttached(
                "RowsHeight", typeof(String), typeof(GridHelper), new UIPropertyMetadata(OnRowsHeightChanged));
            RowsMaxHeightProperty = DependencyProperty.RegisterAttached(
                "RowsMaxHeight", typeof(String), typeof(GridHelper), new UIPropertyMetadata(OnRowsMaxHeightChanged));
            RowsMinHeightProperty = DependencyProperty.RegisterAttached(
                "RowsMinHeight", typeof(String), typeof(GridHelper), new UIPropertyMetadata(OnRowsMinHeightChanged));
            StarColumnsProperty = DependencyProperty.RegisterAttached(
                "StarColumns", typeof(String), typeof(GridHelper), new UIPropertyMetadata(OnStarColumnsChanged));
            StarRowsProperty = DependencyProperty.RegisterAttached(
                "StarRows", typeof(String), typeof(GridHelper), new UIPropertyMetadata(OnStarRowsChanged));
        }

        /// <summary>
        /// Количество колонок
        /// </summary>
        public static readonly DependencyProperty ColumnCountProperty;
        /// <summary>
        /// Максимальная ширина колонок в единицах измерения
        /// </summary>
        public static readonly DependencyProperty ColumnsMaxWidthProperty;
        /// <summary>
        /// Минимальная ширина колонок в единицах измерения
        /// </summary>
        public static readonly DependencyProperty ColumnsMinWidthProperty;
        /// <summary>
        /// Ширина колонок в единицах измерения
        /// </summary>
        public static readonly DependencyProperty ColumnsWidthProperty;
        /// <summary>
        /// Количество строк
        /// </summary>
        public static readonly DependencyProperty RowCountProperty;
        /// <summary>
        /// Высота строк в единицах измерения
        /// </summary>
        public static readonly DependencyProperty RowsHeightProperty;
        /// <summary>
        /// Максимальная высота строк в единицах изменения
        /// </summary>
        public static readonly DependencyProperty RowsMaxHeightProperty;
        /// <summary>
        /// Минимальная высота строк в единицах изменения
        /// </summary>
        public static readonly DependencyProperty RowsMinHeightProperty;
        /// <summary>
        /// Индексы колонок в режиме *
        /// </summary>
        public static readonly DependencyProperty StarColumnsProperty;
        /// <summary>
        /// Индексы строк в режиме *
        /// </summary>
        public static readonly DependencyProperty StarRowsProperty;


        #region Get\Set methods of attached properties

        public static byte GetColumnCount(DependencyObject element)
        {
            return (byte)element.GetValue(ColumnCountProperty);
        }

        public static void SetColumnCount(DependencyObject element, byte value)
        {
            element.SetValue(ColumnCountProperty, value);
        }

        public static String GetColumnsMaxWidth(DependencyObject element)
        {
            return (String)element.GetValue(ColumnsMaxWidthProperty);
        }

        public static void SetColumnsMaxWidth(DependencyObject element, String value)
        {
            element.SetValue(ColumnsMaxWidthProperty, value);
        }

        public static String GetColumnsMinWidth(DependencyObject element)
        {
            return (String)element.GetValue(ColumnsMinWidthProperty);
        }

        public static void SetColumnsMinWidth(DependencyObject element, String value)
        {
            element.SetValue(ColumnsMinWidthProperty, value);
        }

        public static String GetColumnsWidth(DependencyObject element)
        {
            return (String)element.GetValue(ColumnsWidthProperty);
        }

        public static void SetColumnsWidth(DependencyObject element, String value)
        {
            element.SetValue(ColumnsWidthProperty, value);
        }

        public static byte GetRowCount(DependencyObject element)
        {
            return (byte)element.GetValue(RowCountProperty);
        }

        public static void SetRowCount(DependencyObject element, byte value)
        {
            element.SetValue(RowCountProperty, value);
        }

        public static String GetRowsHeight(DependencyObject element)
        {
            return (String)element.GetValue(RowsHeightProperty);
        }

        public static void SetRowsHeight(DependencyObject element, String value)
        {
            element.SetValue(RowsHeightProperty, value);
        }

        public static String GetRowsMaxHeight(DependencyObject element)
        {
            return (String)element.GetValue(RowsMaxHeightProperty);
        }

        public static void SetRowsMaxHeight(DependencyObject element, String value)
        {
            element.SetValue(RowsMaxHeightProperty, value);
        }

        public static String GetRowsMinHeight(DependencyObject element)
        {
            return (String)element.GetValue(RowsMinHeightProperty);
        }

        public static void SetRowsMinHeight(DependencyObject element, String value)
        {
            element.SetValue(RowsMinHeightProperty, value);
        }

        public static String GetStarColumns(DependencyObject element)
        {
            return (String)element.GetValue(StarColumnsProperty);
        }

        public static void SetStarColumns(DependencyObject element, String value)
        {
            element.SetValue(StarColumnsProperty, value);
        }

        public static String GetStarRows(DependencyObject element)
        {
            return (String)element.GetValue(StarRowsProperty);
        }

        public static void SetStarRows(DependencyObject element, String value)
        {
            element.SetValue(StarRowsProperty, value);
        }

        #endregion

        #region OnChanged methods of attached properties

        private static void OnColumnCountChanged(DependencyObject element, DependencyPropertyChangedEventArgs args)
        {
            Grid grid = element as Grid;
            if (grid != null)
            {
                grid.ColumnDefinitions.Clear();
                for (int index = 0; index < (byte)args.NewValue; index++)
                    grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            }
        }

        private static void OnColumnsMaxWidthChanged(DependencyObject element, DependencyPropertyChangedEventArgs args)
        {
            Grid grid = element as Grid;
            if (grid == null)
                return;

            double[] widthValues = GetValues(args.NewValue as String);
            for (int index = 0; index < grid.ColumnDefinitions.Count; index++)
            {
                if (index >= widthValues.Length)
                    break;

                if (widthValues[index] != 0)
                    grid.ColumnDefinitions[index].MaxWidth = widthValues[index];
            }
        }

        private static void OnColumnsMinWidthChanged(DependencyObject element, DependencyPropertyChangedEventArgs args)
        {
            Grid grid = element as Grid;
            if (grid == null)
                return;

            double[] widthValues = GetValues(args.NewValue as String);
            for (int index = 0; index < grid.ColumnDefinitions.Count; index++)
            {
                if (index >= widthValues.Length)
                    break;

                if (widthValues[index] != 0)
                    grid.ColumnDefinitions[index].MinWidth = widthValues[index];
            }
        }

        private static void OnColumnsWidthChanged(DependencyObject element, DependencyPropertyChangedEventArgs args)
        {
            Grid grid = element as Grid;
            if (grid == null)
                return;

            double[] widthValues = GetValues(args.NewValue as String);
            for (int index = 0; index < grid.ColumnDefinitions.Count; index++)
            {
                if (index >= widthValues.Length)
                    break;

                if (widthValues[index] != 0)
                    grid.ColumnDefinitions[index].Width = new GridLength(widthValues[index]);
            }
        }

        private static void OnRowCountChanged(DependencyObject element, DependencyPropertyChangedEventArgs args)
        {
            Grid grid = element as Grid;
            if (grid != null)
            {
                grid.RowDefinitions.Clear();
                for (int index = 0; index < (byte)args.NewValue; index++)
                    grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            }
        }

        private static void OnRowsHeightChanged(DependencyObject element, DependencyPropertyChangedEventArgs args)
        {
            Grid grid = element as Grid;
            if (grid == null)
                return;

            double[] heightValues = GetValues(args.NewValue as String);
            for (int index = 0; index < grid.RowDefinitions.Count; index++)
            {
                if (index >= heightValues.Length)
                    break;

                if (heightValues[index] != 0)
                    grid.RowDefinitions[index].Height = new GridLength(heightValues[index]);
            }
        }

        private static void OnRowsMaxHeightChanged(DependencyObject element, DependencyPropertyChangedEventArgs args)
        {
            Grid grid = element as Grid;
            if (grid == null)
                return;

            double[] heightValues = GetValues(args.NewValue as String);
            for (int index = 0; index < grid.RowDefinitions.Count; index++)
            {
                if (index >= heightValues.Length)
                    break;

                if (heightValues[index] != 0)
                    grid.RowDefinitions[index].MaxHeight = heightValues[index];
            }
        }

        private static void OnRowsMinHeightChanged(DependencyObject element, DependencyPropertyChangedEventArgs args)
        {
            Grid grid = element as Grid;
            if (grid == null)
                return;

            double[] heightValues = GetValues(args.NewValue as String);
            for (int index = 0; index < grid.RowDefinitions.Count; index++)
            {
                if (index >= heightValues.Length)
                    break;

                if (heightValues[index] != 0)
                    grid.RowDefinitions[index].MinHeight = heightValues[index];
            }               
        }

        private static void OnStarColumnsChanged(DependencyObject element, DependencyPropertyChangedEventArgs args)
        {
            Grid grid = element as Grid;
            if (grid == null)
                return;

            foreach (int index in GetValues(args.NewValue as String).Select((d) => (int)d))
            {
                if ((index >= 0) && (index < grid.ColumnDefinitions.Count))
                    grid.ColumnDefinitions[index].Width = new GridLength(1, GridUnitType.Star);
            }
        }

        private static void OnStarRowsChanged(DependencyObject element, DependencyPropertyChangedEventArgs args)
        {
            Grid grid = element as Grid;
            if (grid == null)
                return;

            foreach (int index in GetValues(args.NewValue as String).Select((d) => (int)d))
            {
                if ((index >= 0) && (index < grid.RowDefinitions.Count))
                    grid.RowDefinitions[index].Height = new GridLength(1, GridUnitType.Star);
            }
        }

        #endregion

        private static double[] GetValues(String values)
        {
            if (String.IsNullOrEmpty(values))
                return new double[] { };

            List<double> result = new List<double>();
            foreach (String sValue in values.Split(';'))
            {
                if (String.IsNullOrEmpty(sValue))
                    result.Add(0.0);
                else
                {
                    double value;
                    if (Double.TryParse(sValue, out value))
                        result.Add(value);
                }
            }

            return result.ToArray();
        }
    }
}