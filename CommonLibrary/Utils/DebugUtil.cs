using CommonLibrary.Properties;
using System;
using System.Diagnostics;
using System.Threading;

namespace CommonLibrary.Utils
{
    /// <summary>
    /// Класс позволяющий вывести отладочную диагностику
    /// </summary>
    internal static class DebugUtil
    {
        /// <summary>
        /// Протоколирование задачи
        /// </summary>
        /// <param name="name">Имя задачи</param>
        public static void LogThreadInfo(string name)
        {
            Thread th = Thread.CurrentThread;
            Debug.WriteLine(String.Format(Resources.Frmt_Exception_Srting, th.ManagedThreadId, th.Name, name));
        }

        /// <summary>
        /// Протоколирование исключения
        /// </summary>
        /// <param name="ex">Исключение</param>
        public static void HandleError(Exception ex)
        {
            Debug.WriteLine(ex.Message);
            Debug.WriteLine(ex.Source);
            Debug.WriteLine(ex.StackTrace);
        }
    }
}
