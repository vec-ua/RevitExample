using System;
using System.ComponentModel;

namespace CommonLibrary
{
    /// <summary>
    /// Базовый класс оповещения о изменении значений свойств
    /// </summary>
    public abstract class PropertyChangedNotify : INotifyPropertyChanged
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        protected PropertyChangedNotify()
        { }

        /// <summary>
        /// Сгенерировать событие PropertyChanged для измененного свойства
        /// </summary>
        /// <param name="propertyName"> Имя свойства </param>
        protected void RaisePropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Сгенерировать событие PropertyChanged для измененных свойств
        /// </summary>
        /// <param name="propertyNames"> Имена свойств </param>
        protected void RaisePropertiesChanged(params String[] propertyNames)
        {
            if ((propertyNames != null) && (propertyNames.Length > 0))
            {
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null)
                {
                    foreach (String pname in propertyNames)
                        handler(this, new PropertyChangedEventArgs(pname));
                }
            }
        }

        /// <summary>
        /// Установить значение свойства с генерацией события PropertyChanged при наличии изменения
        /// </summary>
        /// <typeparam name="TProperty"> Тип свойства </typeparam>
        /// <param name="propertyName"> Имя свойства </param>
        /// <param name="currentValue"> Текущее значение </param>
        /// <param name="newValue"> Новое значение </param>
        /// <returns> Признак наличия изменения значения свойства </returns>
        protected bool SetProperty<TProperty>(String propertyName, ref TProperty currentValue, TProperty newValue)
        {
            if (!Object.Equals(currentValue, newValue))
            {
                currentValue = newValue;
                RaisePropertyChanged(propertyName);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Установить значение структурного свойства (с поддержкой интерфейса IEqutable) с генерацией события PropertyChanged при наличии изменения
        /// </summary>
        /// <typeparam name="TProperty"> Тип свойства </typeparam>
        /// <param name="propertyName"> Имя свойства </param>
        /// <param name="currentValue"> Текущее значение </param>
        /// <param name="newValue"> Новое значение </param>
        /// <returns> Признак наличия изменения значения свойства </returns>
        protected bool SetStructEquatableProperty<TProperty>(String propertyName, ref TProperty currentValue, TProperty newValue) where TProperty : struct, IEquatable<TProperty>
        {
            if (!currentValue.Equals(newValue))
            {
                currentValue = newValue;
                RaisePropertyChanged(propertyName);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Событие, генерируемое при изменении значений свойств
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
