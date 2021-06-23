using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CommonLibrary.Models;
using System;
using System.Windows;

namespace MessageBoxPlugin.Commands
{
    /// <summary>
    /// Класс команды для выполнения отображения сообщения
    /// </summary>
    public sealed class MessageBoxCommand : AbstractExecuteCommand
    {
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
        public override Result Execute(ExternalCommandData commandData, ref String message, ElementSet elements)
        {
            return Execute(commandData.Application, ref message);
        }

        /// <summary>
        /// Этот метод реализует внешнюю команду внутри  Revit
        /// </summary>
        /// <param name="uiapp">Приложение</param>
        /// <returns>Результат выполнения</returns>
        public override Result Execute(UIApplication uiapp, ref String message)
        {
            MessageBox.Show("Отображение сообщения для проверки плагина");
            return Result.Succeeded;

        }
    }
}
