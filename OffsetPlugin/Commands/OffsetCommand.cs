using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.UI;
using CommonLibrary.Models;
using CommonLibrary.Utils;
using OffsetPlugin.ViewModels;
using OffsetPlugin.Views;
using System;
using System.Collections.Generic;
using System.Linq;

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
            Transaction transaction = new Transaction(commandData.Application.ActiveUIDocument.Document);
            transaction.Start("Offset");
            Result result = Execute(commandData.Application, ref message);

            if (result != Result.Succeeded)
                transaction.RollBack();
            else
                transaction.Commit();

            transaction.Dispose();

            return result;
        }

        /// <summary>
        /// Этот метод реализует внешнюю команду внутри  Revit
        /// </summary>
        /// <param name="uiapp">Приложение</param>
        /// <returns>Результат выполнения</returns>
        public override Result Execute(UIApplication uiapp, ref String message)
        {
            OffsetViewModel viewModel = new OffsetViewModel(new OffsetView());
            viewModel.ShowDialogWindos();
            if (viewModel.Canceled)
                return Result.Succeeded;

            Double angle = viewModel.Angle * Math.PI / 180;

            try
            {
                List<Conduit> conduits = new List<Conduit>(RevitUtils.GetSelectedConduits<Conduit>(uiapp.ActiveUIDocument.Document, uiapp.ActiveUIDocument.Selection));
                if (conduits.Count <= 1)
                    throw new Exception("You need to select two conduits");
                else if (conduits.Count == 2)
                {
                    if (conduits[0].GetTypeId() == conduits[1].GetTypeId())
                        OffsetPairConduits(conduits[0], conduits[1], angle, uiapp);
                    else
                        throw new Exception("Invalid conduit types!");
                }
                else
                    OffsetParallelConduits(conduits, angle, uiapp);
            }
            catch (Exception ex)
            {
                message = ex.ToString();
                return Result.Failed;
            }


            return Result.Succeeded;
        }

        /// <summary>
        /// Метод обработки двух каналов
        /// </summary>
        /// <param name="first">Первый канал</param>
        /// <param name="second">Второй канал</param>
        /// <param name="angle">Угол</param>
        /// <param name="uiapp">Проиложение</param>
        private void OffsetPairConduits(Conduit first, Conduit second, Double angle, UIApplication uiapp)
        {
            Tuple<Connector, Connector, Double> pair = ConnectoUtils.GetConnectorsPair(first.ConnectorManager.Connectors, second.ConnectorManager.Connectors);

            Double distance = ConduitUtils.GetMinDistanceBtwConduits(first, second);
            Double Cathetus = Math.Sqrt(Math.Pow(ConduitUtils.GetMinimumDistanceBetweenConduitEdges(first, second), 2) - Math.Pow(distance, 2));
            Double Delta = Cathetus - (distance / Math.Tan(angle));

            ConduitUtils.ResizeConduit(first, Delta, pair);
            ConduitUtils.ResizeConduit(second, Delta, pair);

            Conduit additionalCond = Conduit.Create(uiapp.ActiveUIDocument.Document, first.GetTypeId(), pair.Item1.Origin, pair.Item2.Origin, first.LevelId);
            additionalCond.get_Parameter(BuiltInParameter.RBS_CONDUIT_DIAMETER_PARAM).Set(first.get_Parameter(BuiltInParameter.RBS_CONDUIT_DIAMETER_PARAM).AsDouble());

            Tuple<Connector, Connector, Double> pairFirst = ConnectoUtils.GetConnectorsPair(first.ConnectorManager.Connectors, additionalCond.ConnectorManager.Connectors);
            Tuple<Connector, Connector, Double> pairSecond = ConnectoUtils.GetConnectorsPair(additionalCond.ConnectorManager.Connectors, second.ConnectorManager.Connectors);

            uiapp.ActiveUIDocument.Document.Create.NewElbowFitting(pairFirst.Item1, pairFirst.Item2);
            uiapp.ActiveUIDocument.Document.Create.NewElbowFitting(pairSecond.Item1, pairSecond.Item2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conduits"></param>
        /// <param name="angle"></param>
        /// <param name="uiapp"></param>
        private void OffsetParallelConduits(IList<Conduit> conduits, double angle, UIApplication uiapp)
        {
            Conduit first = null;
            Conduit last = null;
            Double maximunDistance = Double.MinValue;
            List<Conduit> copies = new List<Conduit>(conduits);

            foreach (Conduit conduit in copies)
            {
                foreach (Conduit copy in copies)
                {
                    Double distance = ConnectoUtils.GetConnectorsPair(conduit.ConnectorManager.Connectors, copy.ConnectorManager.Connectors).Item3;
                    if (distance > maximunDistance)
                    {
                        first = conduit;
                        last = copy;
                        maximunDistance = distance;
                    }
                }
            }

            if ((first == null) || (last == null))
                throw new Exception("Unknown error with variables Conduit");

            List<Conduit> used = new List<Conduit>();
            foreach (Conduit conduit in copies)
            {
                Double savedDistance = Double.MaxValue;
                Tuple<Conduit, Conduit> infoConduits = null;
                foreach (Conduit copy in copies)
                {
                    Int32 count = used.Where(item => item == copy).Count();

                    if ((count > 1) || ((count == 1) && ((copy == first) || (copy == last))))
                        continue;

                    if ((conduit.Id == copy.Id) || (conduit.GetTypeId() != copy.GetTypeId()))
                        continue;

                    Double distance = ConnectoUtils.GetConnectorsPair(conduit.ConnectorManager.Connectors, copy.ConnectorManager.Connectors).Item3;
                    if (distance < savedDistance)
                    {
                        savedDistance = distance;
                        infoConduits = new Tuple<Conduit, Conduit>(conduit, copy);
                    }
                }

                if (infoConduits != null)
                {
                    used.AddRange(new Conduit[] { infoConduits.Item1, infoConduits.Item2 });
                    OffsetPairConduits(infoConduits.Item1, infoConduits.Item2, angle, uiapp);
                }
            }
        }
    }
}
