using Autodesk.Windows;
using CommonLibrary.Interfaces;
using System;
using System.Linq;
using System.Windows.Media.Imaging;

namespace RevitAdditionApp
{
    /// <summary>
    /// Измененная кнопка для отображения на панели
    /// </summary>
    public sealed class SecondRibbonButton : RibbonButton
    {
        #region Fields
        /// <summary>
        /// Панель к которой относится кнопка
        /// </summary>
        private RibbonPanel Panel;
        #endregion

        #region Properties
        /// <summary>
        /// Прикрепленное приложения
        /// </summary>
        public Autodesk.Revit.UI.UIApplication Application
        { get; private set; }

        /// <summary>
        /// Плагин привязаный к кнопке
        /// </summary>
        public IRevitPlugin Plugin
        { get; private set; }

        /// <summary>
        /// Флаг видимости кнопки
        /// </summary>
        public Boolean Visible
        {
            get { return base.IsVisible; }
            set
            {
                base.IsVisible = value;
                base.IsEnabled = value;
                Plugin.Visible = value;
                NotifyPropertyChanged("Visible");

                if (Panel.Source.Items.Count == Panel.Source.Items.Where(item => item.IsVisible == false).Count())
                    Panel.IsVisible = false;
                else
                    Panel.IsVisible = true;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="plugin">Плагин привязаный к кнопке</param>
        /// <param name="application">Прикрепленное приложения</param>
        public SecondRibbonButton(IRevitPlugin plugin, Autodesk.Revit.UI.UIApplication application, RibbonPanel panel)
        {
            Plugin = plugin;
            Application = application;
            Panel = panel;

            Name = Plugin.Name;
            Id = plugin.SerialNumber.ToString();
            AllowInStatusBar = true;
            AllowInToolBar = true;
            GroupLocation = Autodesk.Private.Windows.RibbonItemGroupLocation.Middle;
            IsToolTipEnabled = true;
            base.IsVisible = Plugin.Visible;
            base.IsEnabled = Plugin.Visible;
            ShowImage = true;
            ShowText = true;
            Text = plugin.Name;
            ToolTip = plugin.Name;
            LargeImage = new BitmapImage(plugin.BitmapUri);
            MinHeight = 0;
            MinWidth = 0;
            Size = RibbonItemSize.Large;
            ResizeStyle = RibbonItemResizeStyles.HideText;
            IsCheckable = true;
            Orientation = System.Windows.Controls.Orientation.Vertical;
            KeyTip = "ID-" + plugin.SerialNumber.ToString();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Изменить информацию в кнопке
        /// </summary>
        /// <param name="plugin">Плагин</param>
        public void ChangeInfo(IRevitPlugin plugin)
        {
            Plugin = plugin;
            Text = plugin.Name;
            ToolTip = plugin.Name;
            LargeImage = new BitmapImage(plugin.BitmapUri);
            Visible = Plugin.Visible;
        }

        /// <summary>
        /// Выполнить команду
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <returns>Результат выполнения</returns>
        public Autodesk.Revit.UI.Result ExecuteCommand(ref String message)
        {
            return Plugin.Command.Execute(Application, ref message);
        }
        #endregion
    }
}
