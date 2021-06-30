using Autodesk.Revit.DB;
using System;

namespace CommonLibrary.Utils
{
    /// <summary>
    /// Класс утилита для работы с классом типа Connector
    /// </summary>
    public static class ConnectoUtils
    {
        /// <summary>
        /// Возвращает коннектор который находится ближе всех к координате
        /// </summary>
        public static Tuple<Double, Connector> GetConnectorClosestTo(ConnectorSet connectors, XYZ point)
        {
            Connector targetConnector = null;
            Double minDist = Double.MaxValue;

            foreach (Connector connector in connectors)
            {
                Double distance = connector.Origin.DistanceTo(point);
                if (distance < minDist)
                {
                    targetConnector = connector;
                    minDist = distance;
                }
            }

            return new Tuple<Double, Connector>(minDist, targetConnector);
        }

        /// <summary>
        /// Метод получения пары коннекторов
        /// </summary>
        /// <param name="firstCollection">Первый список коннекторов</param>
        /// <param name="secondCollection">Второй список коннекторов</param>
        /// <returns>Выбраная пара коннекторов</returns>
        public static Tuple<Connector, Connector, Double> GetConnectorsPair(ConnectorSet firstCollection, ConnectorSet secondCollection)
        {
            Double savedDistance = Double.MaxValue;
            Tuple<Connector, Connector, Double> resultConnectors = null;

            foreach (Connector firstConnector in firstCollection)
            {
                Tuple<Double, Connector> result = GetConnectorClosestTo(secondCollection, firstConnector.Origin);
                if (result.Item1 < savedDistance)
                {
                    savedDistance = result.Item1;
                    resultConnectors = new Tuple<Connector, Connector, Double>(firstConnector, result.Item2, savedDistance);
                }
            }

            return resultConnectors;
        }
    }
}
