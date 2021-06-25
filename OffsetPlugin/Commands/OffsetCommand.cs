using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CommonLibrary.Models;
using OffsetPlugin.ViewModels;
using OffsetPlugin.Views;
using System;

namespace OffsetPlugin.Commands
{
    /// <summary>
    /// Класс команды для выполнения смещения данных
    /// </summary>
    public sealed class OffsetCommand : AbstractExecuteCommand
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
            OffsetViewModel viewModel = new OffsetViewModel();
            OffsetView view = new OffsetView() { DataContext = viewModel };
            view.Show();

            return Result.Succeeded;
        }
    }
}
