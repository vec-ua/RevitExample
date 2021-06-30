using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CommonLibrary.Commands;
using CommonLibrary.Events;
using CommonLibrary.Helpers;
using CommonLibrary.Interfaces;
using RevitAdditionApp.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using UIFramework;

namespace RevitAdditionApp
{
    public sealed class RevitWorker
    {
        #region Fields
        /// <summary>
        /// Экземпляр пользовательского интерфейса приложения
        /// </summary>
        private UIApplication AppUI;

        /// <summary>
        /// Класс оповещающий об событии в программе Autodesk Revit
        /// </summary>
        private StringEventHandler StringEvent;

        /// <summary>
        /// Главная вкладка плагина
        /// </summary>
        private Autodesk.Windows.RibbonTab Tab;

        /// <summary>
        /// Стаический экземпляр класса
        /// </summary>
        private static RevitWorker _instance = null;
        #endregion

        #region Properties
        /// <summary>
        /// Экземпляр класса
        /// </summary>
        public static RevitWorker Instance
        { get { return _instance ?? (_instance = new RevitWorker()); } }

        /// <summary>
        /// Выбраный плагин
        /// </summary>
        public IRevitPlugin SelectedPlugin
        { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Конструктор
        /// </summary>
        private RevitWorker()
        { }
        #endregion

        #region Methods
        /// <summary>
        /// Метод импорта класса для дальнейшей обработки данных по кастомизации
        /// </summary>
        /// <param name="appControlled">Экземпляр пользовательского интерфейса для кастомизации</param>
        public void ImportUIControlledApplication(UIControlledApplication appControlled)
        {
            RibbonPanel panel = PanelHelper.CreateRibbonPanel(appControlled, Resources.Title_Tab, "Settings");
            String thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

            if (panel.AddItem(new PushButtonData("Settings", "Settings", thisAssemblyPath, "RevitAdditionApp.RevitApplication")) is PushButton settingsButton)
            {
                settingsButton.ToolTip = "Settings";
                settingsButton.LargeImage = new BitmapImage(new Uri("pack://application:,,,/RevitAdditionApp;component/Images/Settings.png"));
            }

            Autodesk.Windows.RibbonControl Control = RevitRibbonControl.RibbonControl;
            Tab = Control.Tabs.FirstOrDefault(item => item.Id.Contains(Resources.Title_Tab));

            GenerateAllItems();
        }

        /// <summary>
        /// Метод импорта класса для дальнейшей обработки команд от кнопок
        /// </summary>
        /// <param name="appUI"></param>
        public void ImportUIApplication(UIApplication appUI)
        {
            if (appUI != null)
                AppUI = appUI;
        }

        /// <summary>
        /// Сгенерировать все данные: панели и кнопки
        /// </summary>
        public void GenerateAllItems()
        {
            if (StringEvent == null)
                StringEvent = new StringEventHandler();

            String thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

            foreach (Autodesk.Windows.RibbonPanel aPanel in Tab.Panels)
            {
                if (aPanel.Source.Title.Contains("Settings"))
                    continue;

                aPanel.Source.Items.Clear();
                aPanel.IsVisible = false;
            }

            try
            {
                CompositionContainer container = new CompositionContainer(LoadPluginsToMemory(Path.GetDirectoryName(thisAssemblyPath)));
                IEnumerable<Lazy<IRevitPlugin>> plugins = container.GetExports<IRevitPlugin>();

                foreach (Lazy<IRevitPlugin> plugin in plugins)
                {
                    Autodesk.Windows.RibbonPanel aPanel = Tab.Panels.FirstOrDefault(item => item.Source.Title.Contains(plugin.Value.PanelName));
                    if (aPanel == null)
                    {
                        aPanel = new Autodesk.Windows.RibbonPanel() { Source = new Autodesk.Windows.RibbonPanelSource() { Title = plugin.Value.PanelName } };
                        Tab.Panels.Add(aPanel);
                    }

                    aPanel.Source.Items.Add(new SecondRibbonButton(plugin.Value, aPanel)
                    {
                        CommandHandler = new RelayCommand(DoExecute),
                        Tag = new ControlHelperExtension()
                        {
                            CommandId = Resources.Title_Tab + "%" + plugin.Value.PanelName + "%" + plugin.Value.Name,
                            IfControlNotPresent = IfControlNotPresentOption.Warn,
                            CreateBreadcrumb = true
                        }
                    });

                    aPanel.IsVisible = !(aPanel.Source.Items.Count == aPanel.Source.Items.Where(item => item.IsVisible == false).Count());
                }
            }
            catch (Exception ex)
            {
                StringEvent.Execute(AppUI, ex.Message);
            }
        }

        /// <summary>
        /// Выполнение команды кнопкой
        /// </summary>
        /// <param name="obj"></param>
        private void DoExecute(object obj)
        {
            SecondRibbonButton button = obj as SecondRibbonButton;
            if ((button == null) || (AppUI == null))
                return;

            // Вариант №1 вызываем рикрепленную команду к кнопке настройки, название в кнопке можно поменять
            SelectedPlugin = button.Plugin;
            RevitCommandId id = RevitCommandId.LookupCommandId("CustomCtrl_%CustomCtrl_%Application-test%Settings%Settings");
            AppUI.PostCommand(id);


            // Вариант №2 просто выполняем действия
            /*
            String message = String.Empty;
            Result result = button.Plugin.Command.Execute(AppUI, ref message);

            if (!String.IsNullOrEmpty(message))
                StringEvent.Execute(AppUI, message);
            */
        }

        /// <summary>
        /// Загрузка плагинов в программу
        /// </summary>
        /// <param name="pluginPath">Путь к плагинам</param>
        /// <returns>Каталог, который объединяет элементы</returns>
        private AggregateCatalog LoadPluginsToMemory(string pluginPath)
        {
            if (!Directory.Exists(pluginPath))
                return null;

            String[] files = Directory.GetFiles(pluginPath, "*Plugin.dll");
            AggregateCatalog aggregateCatalog = new AggregateCatalog();

            foreach (String file in files)
            {
                try
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                        {
                            Byte[] bytes = new Byte[fs.Length];
                            fs.Read(bytes, 0, (int)fs.Length);
                            ms.Write(bytes, 0, (int)fs.Length);
                            fs.Close();
                            ms.Close();

                            Assembly assembly = Assembly.Load(ms.ToArray());
                            AssemblyCatalog assemblyCatalog = new AssemblyCatalog(Assembly.Load(ms.ToArray()));
                            aggregateCatalog.Catalogs.Add(assemblyCatalog);
                        }
                    }
                }
                catch
                { }
            }

            return aggregateCatalog;
        }

        /// <summary>
        /// Показать пользовательское сообщение
        /// </summary>
        /// <param name="message">Сообщение</param>
        public void ShowMessage(String message)
        {
            if (StringEvent == null)
                StringEvent = new StringEventHandler();

            StringEvent.Execute(AppUI, message);
        }
        #endregion
    }
}
