using CommonLibrary.Helpers;
using System;
using System.Windows.Input;

namespace CommonLibrary.Commands
{
    /// <summary>
    /// Класс поддержки команд
    /// </summary>
    public sealed class RelayCommand : ICommand
    {
        private Action<object> executeGeneric;
        private Func<object, bool> canExecuteGeneric;

        private Action execute;
        private Func<bool> canExecute;

        /// <summary>
        /// Название команды
        /// </summary>
        public String Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="name"> Название команды </param>
        /// <param name="execute"> Параметризированный метод, вызываемый при выполнении команды </param>
        /// <param name="canExecute"> Параметризированный метод, вызываемый для определения возможности выполнения команды </param>
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null, String name = null)
        {
            ArgumentHelper.Null(execute, "execute");

            this.executeGeneric = execute;
            this.canExecuteGeneric = canExecute;
            Name = name ?? String.Empty;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="name"> Название команды </param>
        /// <param name="execute"> Метод, вызываемый при выполнении команды </param>
        /// <param name="canExecute"> Метод, вызываемый для определения возможности выполнения команды </param>
        public RelayCommand(Action execute, Func<bool> canExecute = null, String name = null)
        {
            ArgumentHelper.Null(execute, "execute");

            this.execute = execute;
            this.canExecute = canExecute;
            Name = name ?? String.Empty;
        }

        /// <summary>
        /// Проверить допустимость выполнения команды
        /// </summary>
        /// <param name="parameter"> Входной параметр команды </param>
        /// <returns> Результат проверки </returns>
        public bool CanExecute(object parameter)
        {
            if (canExecute != null)
                return canExecute();
            else
            {
                if (canExecuteGeneric != null)
                    return canExecuteGeneric(parameter);
            }

            return true;
        }

        /// <summary>
        /// Привести входной параметр команды к указанному типу
        /// </summary>
        /// <typeparam name="TType"> Выходной тип </typeparam>
        /// <param name="parameter"> Входной параметр команды </param>
        /// <param name="value"> Значение выходного типа </param>
        /// <returns> Признак успешности выполнения операции </returns>
        public static bool CastParameterToType<TType>(object parameter, out TType value)
        {
            try
            { value = (TType)parameter; }
            catch
            {
                value = default(TType);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Выполнить команду
        /// </summary>
        /// <param name="parameter"> Входной параметр команды </param>
        public void Execute(object parameter)
        {
            if (execute != null)
                execute();
            else
                executeGeneric(parameter);
        }

        /// <summary>
        /// Событие, генерируемое при возникновении событий, могущих повлиять на доступность выполнения команды
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            { CommandManager.RequerySuggested += value; }
            remove
            { CommandManager.RequerySuggested -= value; }
        }
    }
}
