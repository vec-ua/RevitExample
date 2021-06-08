using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace CommonLibrary.Helpers
{
    public sealed class LoadLibrariesHelper<T>
    {
        #region Fields
        /// <summary>
        /// Экземпляр интерфейса
        /// </summary>
        private static LoadLibrariesHelper<T> instance;

        /// <summary>
        /// Import plugins
        /// </summary>
        [ImportMany]
        private IEnumerable<T> Libraries;
        #endregion

        #region Properties
        /// <summary>
        /// Экземпляр класса (шаблон проектирования "Одиночка")
        /// </summary>
        public static LoadLibrariesHelper<T> Instance
        { get { return instance ?? (instance = new LoadLibrariesHelper<T>()); } }
        #endregion

        #region Constructor
        /// <summary>
        /// Конструктор
        /// </summary>
        private LoadLibrariesHelper()
        { }
        #endregion

        #region Methods
        /// <summary>
        /// Получить все библиотеки которые модно импортировать в проект
        /// </summary>
        /// <returns>Список библиотек</returns>
        public IEnumerable<T> GetAllLibraries()
        {
            Libraries = null;
            DirectoryCatalog catalog = new DirectoryCatalog(AppDomain.CurrentDomain.BaseDirectory, "*.dll");
            CompositionContainer container = new CompositionContainer(catalog);
            container.ComposeParts(this);
            return Libraries;
        }
        #endregion
    }
}
