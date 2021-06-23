using Autodesk.Revit.UI;
using CommonLibrary.Commands;
using CommonLibrary.Models;
using RevitAdditionApp.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RevitAdditionApp.ViewModels
{
    public sealed class SettingsViewModel : AbstractViewModel
    {
        #region Commands
        /// <summary>
        /// Обновить список плагинов из каталога
        /// </summary>
        public ICommand RefreshPluginsFromCatalogCommand
        { get; private set; }
        #endregion

        #region Fields
        /// <summary>
        /// Приложение
        /// </summary>
        private UIApplication Application;
        #endregion

        #region Properties
        /// <summary>
        /// Список панелей
        /// </summary>
        public ObservableCollection<Autodesk.Windows.RibbonPanel> Panels
        { get; private set; }
        #endregion

        #region Events
        /// <summary>
        /// Событие оповещающее обновление плагинов из каталога
        /// </summary>
        public event EventHandler UpdatePluginsFromCatalogEvent;
        #endregion

        #region Constructor
        /// <summary>
        /// Конструктор
        /// </summary>
        public SettingsViewModel(UIApplication application)
        {
            Application = application;
            Panels = new ObservableCollection<Autodesk.Windows.RibbonPanel>();
            CheckPanels();
        }
        #endregion

        #region Methods
        /// <summary>
        /// AbstractViewModel
        /// </summary>
        protected override void Initialize()
        {
            RefreshPluginsFromCatalogCommand = new RelayCommand(DoRefreshPluginsFromCatalog);
        }

        /// <summary>
        /// Метод обновления плагинов из каталога
        /// </summary>
        private void DoRefreshPluginsFromCatalog()
        {
            if (UpdatePluginsFromCatalogEvent != null)
                UpdatePluginsFromCatalogEvent(Application, new EventArgs());
            
            CheckPanels();
        }

        /// <summary>
        /// Обрабатываем отображение панелей
        /// </summary>
        private void CheckPanels()
        {
            Autodesk.Windows.RibbonControl ribbon = Autodesk.Windows.ComponentManager.Ribbon;
            Autodesk.Windows.RibbonTab tab = ribbon.Tabs.FirstOrDefault(item => item.Id.Contains(Resources.Title_Tab));
            
            Panels.Clear();
            foreach(Autodesk.Windows.RibbonPanel panel in tab.Panels)
            {
                if (panel.Source.Title.Contains("Settings") || (panel.Source.Items.Count <= 0))
                    continue;

                Panels.Add(panel);
            }
        }
        #endregion
    }
}
