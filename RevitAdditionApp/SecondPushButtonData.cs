using Autodesk.Windows;
using CommonLibrary.Interfaces;
using RevitAdditionApp.Properties;
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
        public SecondRibbonButton(IRevitPlugin plugin, RibbonPanel panel)
        {
            Plugin = plugin;
            Panel = panel;

            Id = "CustomCtrl_%CustomCtrl_%" + Resources.Title_Tab + "%" + Plugin.PanelName + "%";

            Name = plugin.Name;
            Description = plugin.Name;
            IsCheckable = false;

            AllowInStatusBar = true;
            AllowInToolBar = true;
            GroupLocation = Autodesk.Private.Windows.RibbonItemGroupLocation.Middle;
            IsToolTipEnabled = true;
            IsVisible = Plugin.Visible;
            IsEnabled = Plugin.Visible;
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
        }
        #endregion

        #region Methods
        /// <summary>
        /// Изменить информацию в кнопке
        /// </summary>
        /// <param name="plugin">Плагин</param>
        public void ChangeInfo(IRevitPlugin plugin)
        {
            Id = "CustomCtrl_%CustomCtrl_%" + Resources.Title_Tab + "%" + Plugin.PanelName + "%";

            Name = plugin.Name;
            Plugin = plugin;
            Text = plugin.Name;
            ToolTip = plugin.Name;
            LargeImage = new BitmapImage(plugin.BitmapUri);
            Visible = Plugin.Visible;
        }
        #endregion
    }
}
