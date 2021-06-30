using CommonLibrary.Commands;
using CommonLibrary.Models;
using System;
using System.Windows;
using System.Windows.Input;

namespace OffsetPlugin.ViewModels
{
    /// <summary>
    /// Представление для установки смещения выбранным градусом
    /// </summary>
    public sealed class OffsetViewModel : AbstractDialogViewModel
    {
        #region Commands
        /// <summary>
        /// Выбран угол с помоью кнопки
        /// </summary>
        public ICommand ButtonAngleCommand
        { get; private set; }

        /// <summary>
        /// Выбран угол с пользователем
        /// </summary>
        public ICommand UserAngleCommand
        { get; private set; }
        #endregion

        #region Properties
        private Double _Angle;
        /// <summary>
        /// Подпись названия плагина
        /// </summary>
        public Double Angle
        {
            get { return _Angle; }
            set
            {
                if ((value >= 0) && (value <= 90))
                    _Angle = value;

                if (value > 90)
                    _Angle = 90;

                if (value < 0)
                    _Angle = 0;

                RaisePropertyChanged("Angle");
            }
        }

        /// <summary>
        /// Флаг отмены обработки данных
        /// </summary>
        public Boolean Canceled
        { get; private set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Конструктор
        /// </summary>
        public OffsetViewModel(Window managedDialog) : base(managedDialog)
        {
            Canceled = true;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Инициализация команд
        /// </summary>
        protected override void Initialize()
        {
            ButtonAngleCommand = new RelayCommand(DoButtonAngle, CanDoButtonAngle);
            UserAngleCommand = new RelayCommand(DoUserAngle, () => (Angle > 0) && (Angle <= 90));
        }

        /// <summary>
        /// Выполнение команды по получению угла выбранного пользователем
        /// </summary>
        private void DoUserAngle()
        {
            Canceled = false;
            ManagedDialogWindow.Close();
        }

        /// <summary>
        /// Выполнение команды по получению угла выбранного с помощью кнопки
        /// </summary>
        /// <param name="obj">Выбраный угол</param>
        private void DoButtonAngle(object obj)
        {
            Canceled = false;
            Angle = Double.Parse(obj.ToString());
            ManagedDialogWindow.Close();
        }

        /// <summary>
        /// Метод проверки выбранного угла с помощью кнопки
        /// </summary>
        /// <param name="obj">Выбраный угол</param>
        /// <returns>Флаг возвращающий разрешение вполнения команды</returns>
        private bool CanDoButtonAngle(object obj)
        {
            Double num = 0;
            return Double.TryParse(obj.ToString(), out num);
        }
        #endregion
    }
}
