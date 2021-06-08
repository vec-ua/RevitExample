using System;

namespace CommonLibrary.Models
{
    /// <summary>
    /// Абсткракнтый класс представления модели для плагина
    /// </summary>
    public abstract class AbstractPluginViewModel : AbstractViewModel
    {
        #region Properties
        /// <summary>
        /// Единый идентификатор ресурса на шаблон данных
        /// </summary>
        public abstract Uri ResourceDictionary
        { get; }
        #endregion

        #region Constructor
        /// <summary>
        /// Конструктор
        /// </summary>
        public AbstractPluginViewModel()
        { }
        #endregion
    }
}
