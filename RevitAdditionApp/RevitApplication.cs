using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CommonLibrary.Interfaces;
using RevitAdditionApp.View;
using RevitAdditionApp.ViewModels;
using System;
using System.Windows;

namespace RevitAdditionApp
{
    [Transaction(TransactionMode.Manual)]
    public class RevitApplication : IExternalApplication, IExternalCommand
    {
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
                RevitWorker.Instance.ImportUIControlledApplication(application);
                RevitWorker.Instance.GenerateAllItems();
                application.ControlledApplication.ApplicationInitialized += ControlledApplication_ApplicationInitialized;

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
        private void ControlledApplication_ApplicationInitialized(object sender, Autodesk.Revit.DB.Events.ApplicationInitializedEventArgs e)
        {
            RevitWorker.Instance.ImportUIApplication(new UIApplication(sender as Autodesk.Revit.ApplicationServices.Application));
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
            if (RevitWorker.Instance.SelectedPlugin != null)
            {
                Result result = RevitWorker.Instance.SelectedPlugin.Command.Execute(commandData, ref message, elements);
                RevitWorker.Instance.SelectedPlugin = null;
                return result;
            }

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

            RevitWorker.Instance.GenerateAllItems();
        }
    }
}
