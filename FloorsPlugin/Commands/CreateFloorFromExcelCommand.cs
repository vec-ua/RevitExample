using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CommonLibrary.Models;
using Microsoft.Win32;
using System;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace FloorsPlugin.Commands
{
    /// <summary>
    /// Класс команды для выполнения загрузки этажей из Excel файла
    /// </summary>
    public sealed class CreateFloorFromExcelCommand : AbstractExecuteCommand
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
            Transaction tr = new Transaction(commandData.Application.ActiveUIDocument.Document);
            tr.Start("Добавление уровней из Excel");
            Result result = Execute(commandData.Application, ref message);
            tr.Commit();
            tr.Dispose();
            return result;
        }

        /// <summary>
        /// Этот метод реализует внешнюю команду внутри  Revit
        /// </summary>
        /// <param name="uiapp">Приложение</param>
        /// <returns>Результат выполнения</returns>
        public override Result Execute(UIApplication uiapp, ref String message)
        {
            Excel.Application xlApp = null;
            Excel.Workbook xlWorkBook = null;
            Excel.Worksheet xlWorkSheet = null;

            try
            {
                OpenFileDialog file = new OpenFileDialog();
                if (file.ShowDialog() == true)
                {
                    xlApp = new Excel.Application();
                    xlWorkBook = xlApp.Workbooks.Open(file.FileName, 0, true, 5, "", "", true, Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                    xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                    Excel.Range range = xlWorkSheet.UsedRange;

                    for (Int32 rIdx = 1; rIdx <= range.Rows.Count; rIdx++)
                    {
                        Int32 elevation = 0;
                        if (Int32.TryParse((range.Cells[rIdx, 2] as Excel.Range).Value2.ToString(), out elevation))
                        {
                            Level lvl = Level.Create(uiapp.ActiveUIDocument.Document, elevation);
                            lvl.Name = (range.Cells[rIdx, 1] as Excel.Range).Value2.ToString();
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }
            finally
            {
                if (xlWorkSheet != null)
                    Marshal.ReleaseComObject(xlWorkSheet);

                if (xlWorkBook != null)
                {
                    xlWorkBook.Close(true, null, null);
                    Marshal.ReleaseComObject(xlWorkBook);
                }

                if (xlApp != null)
                {
                    xlApp.Quit();
                    Marshal.ReleaseComObject(xlApp);
                }
            }

            return Result.Succeeded;
        }
    }
}
