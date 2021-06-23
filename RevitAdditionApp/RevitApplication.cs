using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Windows;
using CommonLibrary.Commands;
using CommonLibrary.Events;
using CommonLibrary.Helpers;
using CommonLibrary.Interfaces;
using RevitAdditionApp.Properties;
using RevitAdditionApp.View;
using RevitAdditionApp.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace RevitAdditionApp
{
    [Transaction(TransactionMode.Manual)]
    public class RevitApplication : IExternalApplication, IExternalCommand
    {
        #region Fields
        /// <summary>
        /// Статическое поле приложения (кеш приложения)
        /// </summary>
        private static UIControlledApplication cachedUiCtrApp;

        /// <summary>
        /// Событие отвечающее за опопвещение сообщением
        /// </summary>
        private static StringEventHandler StringEvent;
        #endregion

        #region Methods
        /// <summary>
        /// Метод обработки при обращении Autodesk Revit к данному приложению
        /// </summary>
        /// <param name="application">Пользовательский интерфейс Autodesk Revit</param>
        /// <returns>Результат выполнения</returns>
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        /// <summary>
        /// Метод пост обработки при закрытии Autodesk Revit
        /// </summary>
        /// <param name="application">Пользовательский интерфейс Autodesk Revit</param>
        /// <returns>Результат выполнения</returns>
        public Result OnStartup(UIControlledApplication application)
        {
            try
            {
                cachedUiCtrApp = application;
                cachedUiCtrApp.Idling += CachedUiCtrApp_Idling;
                StringEvent = new StringEventHandler();
                return Result.Succeeded;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return Result.Failed;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CachedUiCtrApp_Idling(object sender, Autodesk.Revit.UI.Events.IdlingEventArgs e)
        {
            cachedUiCtrApp.Idling -= CachedUiCtrApp_Idling;

            Autodesk.Revit.UI.RibbonPanel panel = PanelHelper.CreateRibbonPanel(cachedUiCtrApp, Resources.Title_Tab, "Settings");
            String thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

            if (panel.AddItem(new PushButtonData("Settings", "Settings", thisAssemblyPath, "RevitAdditionApp.RevitApplication")) is PushButton settingsButton)
            {
                settingsButton.ToolTip = "Settings";
                settingsButton.LargeImage = new BitmapImage(new Uri("pack://application:,,,/RevitAdditionApp;component/Images/Settings.png"));
            }

            RegenerationInfo(sender as UIApplication, thisAssemblyPath);
        }

        /// <summary>
        /// Регенерация информации
        /// </summary>
        /// <param name="uiapp">Приложение</param>
        /// <param name="assemblyPath">Путь к сборке</param>
        private void RegenerationInfo(UIApplication uiapp, String assemblyPath)
        {
            CompositionContainer container = new CompositionContainer(LoadPluginsToMemory(Path.GetDirectoryName(assemblyPath)));
            IEnumerable<Lazy<IRevitPlugin>> plugins = container.GetExports<IRevitPlugin>();

            RibbonControl ribbon = ComponentManager.Ribbon;
            RibbonTab tab = ribbon.Tabs.FirstOrDefault(item => item.Id.Contains(Resources.Title_Tab));
            if (tab == null)
                return;

            foreach(Autodesk.Windows.RibbonPanel aPanel in tab.Panels)
            {
                if (aPanel.Source.Title.Contains("Settings"))
                    continue;

                aPanel.Source.Items.Clear();
            }

            foreach (Lazy<IRevitPlugin> plugin in plugins)
            {
                Autodesk.Windows.RibbonPanel aPanel = tab.Panels.FirstOrDefault(item => item.Source.Title.Contains(plugin.Value.PanelName));
                if (aPanel == null)
                {
                    PanelHelper.CreateRibbonPanel(cachedUiCtrApp, Resources.Title_Tab, plugin.Value.PanelName);
                    aPanel = tab.Panels.FirstOrDefault(item => item.Source.Title.Contains(plugin.Value.PanelName));
                }

                if (aPanel == null)
                    break;

                aPanel.Source.Items.Add(new SecondRibbonButton(plugin.Value, uiapp, aPanel) { CommandHandler = new RelayCommand(DoExecute) });
                if (aPanel.Source.Items.Count == aPanel.Source.Items.Where(item => item.IsVisible == false).Count())
                    aPanel.IsVisible = false;
            }
        }

        /// <summary>
        /// Выполнение команды кнопкой
        /// </summary>
        /// <param name="obj"></param>
        private void DoExecute(object obj)
        {
            SecondRibbonButton button = obj as SecondRibbonButton;
            if (button == null)
                return;

            String message = String.Empty;
            Result result = button.ExecuteCommand(ref message);
            if (!String.IsNullOrEmpty(message))
                StringEvent.Execute(button.Application, message);
        }

        /// <summary>
        /// Этот метод реализует внешнюю команду внутри  Revit.
        /// </summary>
        /// <param name="commandData">
        /// Объект который содержит ссылку на приложение и представление,
        /// необходимые для внешней команды.
        /// </param>
        /// <param name="message">
        /// Сообщение об ошибке может быть возвращено внешней командой. Это будет
        /// отображаться только в том случае, если статус команды был «Неудачный».
        /// Это сообщение может содержать не более 1023 символов; строки длиннее этого будут обрезаны.
        /// </param>
        /// <param name="elements">
        /// Набор элементов, указывающий на элементы проблемы,
        /// отображаемые в диалоговом окне сбоя. Это будет использоваться только
        /// в том случае, если статус команды был «Неудачный».
        /// </param>
        /// <returns>
        /// Результат указывает, было ли выполнение неудачным, успешным или отмененным
        /// пользователем. Если это не удастся, Revit отменит все изменения, внесенные
        /// внешней командой.
        /// </returns>	  
        public Result Execute(ExternalCommandData commandData, ref String message, ElementSet elements)
        {
            SettingsViewModel viewModel = new SettingsViewModel(commandData.Application);
            viewModel.UpdatePluginsFromCatalogEvent += ViewModel_UpdatePluginsFromCatalogEvent;
            SettingsView view = new SettingsView() { DataContext = viewModel };
            view.ShowDialog();
            viewModel.UpdatePluginsFromCatalogEvent -= ViewModel_UpdatePluginsFromCatalogEvent;

            return Result.Succeeded;
        }

        /// <summary>
        /// Метод оповещающий об обновлении списка плагинов из каталога
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewModel_UpdatePluginsFromCatalogEvent(object sender, EventArgs e)
        {
            UIApplication application = sender as UIApplication;
            if (application == null)
                return;

            RegenerationInfo(application, Assembly.GetExecutingAssembly().Location);
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
        #endregion
    }
}
