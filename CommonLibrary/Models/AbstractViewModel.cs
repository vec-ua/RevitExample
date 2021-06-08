
namespace CommonLibrary.Models
{
    /// <summary>
    /// Абстрактный класс представления модели
    /// </summary>
    public abstract class AbstractViewModel : PropertyChangedNotify
    {
        #region Constructor
        /// <summary>
        /// Конструктор
        /// </summary>
        protected AbstractViewModel()
        {
            Initialize();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Инициализация команд
        /// </summary>
        protected abstract void Initialize();
        #endregion
    }
}
