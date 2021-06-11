using Autodesk.Revit.UI;
using CommonLibrary.Properties;
using CommonLibrary.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CommonLibrary.Helpers
{
    /// <summary>
    /// Клас помогает создать панель для дополнения AutodeskRevit
    /// </summary>
    public static class PanelHelper
    {
        /// <summary>
        /// Метод создания панели
        /// </summary>
        /// <param name="application">Пользовательский интерфейс Autodesk Revit</param>
        /// <param name="tabName">Название вкладки</param>
        /// <param name="panelName">Название панели</param>
        /// <returns>Созданая панель</returns>
        public static RibbonPanel CreateRibbonPanel(UIControlledApplication application, String tabName, String panelName)
        {
            ArgumentHelper.Null(application, Resources.Msg_Exception_UIControlledApplication_Is_Null);
            ArgumentHelper.NotSupported(() => String.IsNullOrEmpty(tabName), Resources.Msg_Exception_TabName_Is_Null);
            ArgumentHelper.NotSupported(() => String.IsNullOrEmpty(panelName), Resources.Msg_Exception_TabPanel_Is_Null);

            RibbonPanel ribbonPanel = null;
            try
            {
                application.CreateRibbonTab(tabName);
            }
            catch (Exception ex)
            {
                DebugUtil.HandleError(ex);
            }

            try
            {
                RibbonPanel panel = application.CreateRibbonPanel(tabName, panelName);
            }
            catch (Exception ex)
            {
                DebugUtil.HandleError(ex);
            }

            List<RibbonPanel> panels = application.GetRibbonPanels(tabName);
            foreach (RibbonPanel p in panels.Where(p => p.Name == panelName))
                ribbonPanel = p;

            return ribbonPanel;
        }
    }
}
