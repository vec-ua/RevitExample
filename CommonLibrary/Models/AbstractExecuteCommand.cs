using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;

namespace CommonLibrary.Models
{
    /// <summary>
    /// Абстрактный класс для выполнения команды
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    public abstract class AbstractExecuteCommand : IExternalCommand
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
        public abstract Result Execute(ExternalCommandData commandData, ref String message, ElementSet elements);

        /// <summary>
        /// Этот метод реализует внешнюю команду внутри  Revit
        /// </summary>
        /// <param name="uiapp">Приложение</param>
        /// <returns>Результат выполнения</returns>
        public abstract Result Execute(UIApplication uiapp, ref String message);
    }
}
