using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using CommonLibrary.Helpers;
using System;
using System.Collections.Generic;

namespace CommonLibrary.Utils
{
    /// <summary>
    /// Класс утилита для работы с классом типа Conduit
    /// </summary>
    public static class ConduitUtils
    {
        /// <summary>
        /// Получите минимальное расстояние между Conduit-ами 
        /// </summary>
        /// <param name="first">Первый Conduit</param>
        /// <param name="second">Второй Conduit</param>
        /// <returns>Расстояние между Conduit-ами</returns>
        public static double GetMinDistanceBtwConduits(Conduit first, Conduit second)
        {
            LocationCurve firstLoc = first.Location as LocationCurve;
            LocationCurve secondLoc = second.Location as LocationCurve;

            ArgumentHelper.NotSupported(() => (firstLoc == null) || (secondLoc == null), "LocationCurve is NULL");

            Double distance = 0.0;
            try
            {
                IList<ClosestPointsPairBetweenTwoCurves> collection;
                firstLoc.Curve.ComputeClosestPoints(secondLoc.Curve, false, false, false, out collection);
                distance = collection[0].Distance;
            }
            catch
            {
                firstLoc.Curve.MakeUnbound();
                Double dist0 = firstLoc.Curve.Distance(secondLoc.Curve.GetEndPoint(0));
                Double dist1 = firstLoc.Curve.Distance(secondLoc.Curve.GetEndPoint(1));
                distance = dist0 < dist1 ? dist0 : dist1;
            }

            return distance;
        }

        /// <summary>
        /// Получение минимального расстояния между двумя Conduit-ами
        /// </summary>
        /// <param name="first">Первый Conduit</param>
        /// <param name="second">Второй Conduit</param>
        /// <returns>Расстояние Conduit-ами</returns>
        public static Double GetMinimumDistanceBetweenConduitEdges(Conduit first, Conduit second)
        {
            double minDist = Double.MaxValue;
            foreach (Connector firstConnector in first.ConnectorManager.Connectors)
            {
                foreach (Connector seconfConnector in second.ConnectorManager.Connectors)
                {
                    if (firstConnector.Origin.DistanceTo(seconfConnector.Origin) < minDist)
                        minDist = firstConnector.Origin.DistanceTo(seconfConnector.Origin);
                }
            }

            return minDist;
        }

        /// <summary>
        /// Метод изменения размера Conduit-а
        /// </summary>
        /// <param name="conduit">Conduit</param>
        /// <param name="delta">Дельта</param>
        /// <param name="connectionPair">Пара коннекторов</param>
        public static void ResizeConduit(Conduit conduit, Double delta, Tuple<Connector, Connector, Double> connectionPair)
        {
            LocationCurve location = conduit.Location as LocationCurve;
            ArgumentHelper.NotSupported(() => location == null, "Variable conduit in method ResizeConduit isnt Conduit type");

            Double condStartPoint = location.Curve.GetEndParameter(0);
            Double condEndPoint = location.Curve.GetEndParameter(1);

            if (location.Curve.GetEndPoint(0).DistanceTo(connectionPair.Item1.Origin) < location.Curve.GetEndPoint(1).DistanceTo(connectionPair.Item1.Origin))
                condStartPoint -= delta / 2;
            else
                condEndPoint += delta / 2;

            ArgumentHelper.NotSupported(() => condStartPoint > condEndPoint, "Can't set MakeBound in LocationCurve because start point bigger than end point, please change angle");

            location.Curve.MakeUnbound();
            location.Curve.MakeBound(condStartPoint, condEndPoint);
        }
    }
}
