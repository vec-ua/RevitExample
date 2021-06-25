using CommonLibrary.Commands;
using CommonLibrary.Models;
using OffsetPlugin.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OffsetPlugin.ViewModels
{
    public sealed class OffsetViewModel : AbstractViewModel
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
                if ((value >= 0) && (value <= 359))
                    _Angle = value;

                if (value > 359)
                    _Angle = 359;

                if (value < 0)
                    _Angle = 0;

                RaisePropertyChanged("Angle");
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Конструктор
        /// </summary>
        public OffsetViewModel()
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Инициализация команд
        /// </summary>
        protected override void Initialize()
        {
            ButtonAngleCommand = new RelayCommand(DoButtonAngle, CanDoButtonAngle);
            UserAngleCommand = new RelayCommand(DoUserAngle, () => (Angle > 0) && (Angle < 359));
        }

        /// <summary>
        /// Выполнение команды по получению угла выбранного пользователем
        /// </summary>
        private void DoUserAngle()
        {

        }

        /// <summary>
        /// Выполнение команды по получению угла выбранного с помощью кнопки
        /// </summary>
        /// <param name="obj">Выбраный угол</param>
        private void DoButtonAngle(object obj)
        {
            Angle = Double.Parse(obj.ToString());
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
