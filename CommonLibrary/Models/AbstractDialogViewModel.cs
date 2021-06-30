using CommonLibrary.Helpers;
using CommonLibrary.Properties;
using System;
using System.Windows;

namespace CommonLibrary.Models
{
    /// <summary>
    /// Абстрактный класс для отображения диалогов в программе
    /// </summary>
    public abstract class AbstractDialogViewModel : AbstractViewModel
    {
        #region Properties
        /// <summary>
        /// Управляемое окно диалога
        /// </summary>
        protected Window ManagedDialogWindow
        { get; private set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="managedDialog">Управляемое окно</param>
        public AbstractDialogViewModel(Window managedDialog)
        {
            ArgumentHelper.Null(managedDialog, Resources.Caption_CantCreateDialog);

            ManagedDialogWindow = managedDialog;
            ManagedDialogWindow.DataContext = this;
        }
        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        public void ShowDialogWindos()
        {
            ManagedDialogWindow.ShowDialog();
        }
        #endregion
    }
}
