using Autodesk.Revit.UI;
using System;

namespace CommonLibrary.Events
{
    /// <summary>
    /// Класс для создания аргументов внешних событий 
    /// </summary>
    /// <typeparam name="TAgrument">Тип данных</typeparam>
    public abstract class AbstractEventHandler<TAgrument> : IExternalEventHandler where TAgrument : class
    {
        #region Fields
        /// <summary>
        /// Объект для блокировки доступа к данным
        /// </summary>
        private readonly Object LockObj;

        /// <summary>
        /// Аргументы
        /// </summary>
        private TAgrument Args;

        /// <summary>
        /// Внешнее событие
        /// </summary>
        private readonly ExternalEvent RevitEvent;
        #endregion

        #region Constructor
        /// <summary>
        /// конструктор
        /// </summary>
        public AbstractEventHandler()
        {
            RevitEvent = ExternalEvent.Create(this);
            LockObj = new object();
        }
        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        public void Execute(UIApplication app)
        {
            TAgrument args;

            lock (LockObj)
            {
                args = Args;
                Args = default;
            }

            Execute(app, args);
        }

        /// <summary>
        /// Получить название операции
        /// </summary>
        /// <returns>Название операции</returns>
        public string GetName()
        {
            return GetType().Name;
        }

        /// <summary>
        /// Выполните обернутое внешнее событие в допустимом контексте Revit API
        /// </summary>
        /// <param name="args">Аргументы</param>
        public void Raise(TAgrument args)
        {
            lock (LockObj)
                Args = args;

            RevitEvent.Raise();
        }

        /// <summary>
        /// Переопределенный метод, который оборачивает метод Execution в допустимый контекст Revit API
        /// </summary>
        /// <param name="app">Приложение Revit UI для использования в качестве «оболочки» контекста API</param>
        /// <param name="args">Аргументы</param>
        public abstract void Execute(UIApplication app, TAgrument args);
        #endregion
    }
}
