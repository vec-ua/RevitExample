using System;

namespace CommonLibrary.Helpers
{
    public static class ArgumentHelper
    {
        /// <summary>
        /// Проверка входного параметра на допустимость
        /// </summary>
        /// <param name="predicate"> Метод проверки </param>
        /// <param name="message"> Сообщение при недопустимом параметре </param>
        public static void NotSupported(Func<bool> predicate, String message)
        {
            Null((object)predicate, "predicate");
            if (predicate())
                throw new NotSupportedException(message);
        }

        /// <summary>
        /// Проверка входного параметра на NULL
        /// </summary>
        /// <param name="value"> Значение параметра </param>
        /// <param name="paramName"> Название параметра </param>
        /// <param name="message"> Сообщение </param>
        public static void Null(object value, String paramName, String message = null)
        {
            if (value == null)
                throw new ArgumentNullException(paramName, message);
        }

        /// <summary>
        /// Проверка входного параметра на NULL
        /// </summary>
        /// <param name="predicate"> Метод проверки </param>
        /// <param name="paramName"> Название параметра </param>
        public static void Null(Func<bool> predicate, String paramName)
        {
            Null((object)predicate, "predicate");
            if (predicate())
                throw new ArgumentNullException(paramName);
        }
    }
}
