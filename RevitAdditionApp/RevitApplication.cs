using Autodesk.Revit.UI;
using CommonLibrary.Helpers;
using CommonLibrary.Interfaces;
using RevitAdditionApp.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace RevitAdditionApp
{
    public class RevitApplication : IExternalApplication
    {
        #region Fields
        /// <summary>
        /// Import plugins
        /// </summary>
        [ImportMany]
        private IEnumerable<IRevitPlugin> Libraries;
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
            RibbonPanel panel = PanelHelper.CreateRibbonPanel(application, Resources.Title_Tab, Resources.Title);
            String thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

            DirectoryCatalog catalog = new DirectoryCatalog(Path.GetDirectoryName(thisAssemblyPath), "*Plugin.dll");
            CompositionContainer container = new CompositionContainer(catalog);
            container.ComposeParts(this);

            foreach (IRevitPlugin plugin in Libraries)
            {
                if (panel.AddItem(new PushButtonData(plugin.PlaginName, plugin.PlaginName, plugin.FillPathName, plugin.CommandPath)) is PushButton button)
                {
                    button.ToolTip = plugin.ToolTipButton;
                    button.LargeImage = new BitmapImage(plugin.BitmapUri);
                }
            }

            return Result.Succeeded;
        }
        #endregion
    }
}
