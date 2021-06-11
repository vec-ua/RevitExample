using Autodesk.Revit.UI;
using CommonLibrary.Properties;
using System;

namespace CommonLibrary.Events
{
    /// <summary>
    /// Класс для оповещения в программе Autodesk Revit в виде текстового сообщения
    /// </summary>
    public sealed class StringEventHandler : AbstractEventHandler<String>
    {
        /// <summary>
        /// Метод выполнения действий по выводу тестового сообщения
        /// </summary>
        /// <param name="app">Приложение Revit UI для использования в качестве «оболочки» контекста API</param>
        /// <param name="args">Аргументы</param>
        public override void Execute(UIApplication uiApp, string args)
        {
            TaskDialog.Show(Resources.Msg_ExternalMessage, args);
        }
    }
}
