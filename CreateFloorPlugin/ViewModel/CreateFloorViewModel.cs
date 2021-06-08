using CommonLibrary.Commands;
using CommonLibrary.Models;
using CreateFloorPlugin.Properties;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CreateFloorPlugin.ViewModel
{
    /// <summary>
    /// Представление модели для плагина "Создания этажей"
    /// </summary>
    public sealed class CreateFloorViewModel : AbstractPluginViewModel
    {
        #region Command
        /// <summary>
        /// Добавить фильтр
        /// </summary>
        public ICommand OpenExcelFileCommand
        { get; private set; }
        #endregion

        #region Properties
        /// <summary>
        /// Единый идентификатор ресурса на шаблон данных
        /// </summary>
        public override Uri ResourceDictionary
        { get { return new Uri("pack://application:,,,/CreateFloorPlugin;component/ResourceDictionary/CreateFloorResourceDictionary.xaml"); } }
        #endregion

        #region Constructor
        /// <summary>
        /// Конструктор
        /// </summary>
        internal CreateFloorViewModel()
        { }
        #endregion

        #region Methods
        /// <summary>
        /// Инициализация команд
        /// </summary>
        protected override void Initialize()
        {
            OpenExcelFileCommand = new RelayCommand(DoOpenExcelFile);
        }

        /// <summary>
        /// Метод открытия Excel файла 
        /// </summary>
        private void DoOpenExcelFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                RestoreDirectory = true,
                Title = Resources.STR_TITLE_OPEN_FILE_EXCEL,
                DefaultExt = Resources.STR_EXCEL_EXT,
                Filter = String.Format(Resources.FRMT_EXCEL_FILTER, Resources.STR_EXCEL_EXT, Resources.STR_EXCEL_EXT)
            };

            if (openFileDialog.ShowDialog() == true)
            {

            }
        }
        #endregion
    }
}
