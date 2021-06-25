using CommonLibrary.Interfaces;
using CommonLibrary.Models;
using OffsetPlugin.Commands;
using OffsetPlugin.Properties;
using OffsetPlugin.Settings;
using System;
using System.Configuration;

namespace OffsetPlugin
{
    /// <summary>
    /// Плагин виполняет смещение данных
    /// </summary>
    public class OffsetPlugin : IRevitPlugin
    {
		#region Fields
		/// <summary>
		/// Общие настройки кнопки
		/// </summary>
		public GeneralSettings Settings;
		#endregion

		#region Properties
		/// <summary>
		/// Серийный номер
		/// </summary>
		public Guid SerialNumber
		{ get { return new Guid("0e05eb25-6d13-4708-893c-9fca2216f91a"); } }

		/// <summary>
		/// Название плагина
		/// </summary>
		public String Name
		{ get { return Resources.Title_Plugin; } }

		/// <summary>
		/// Название панели
		/// </summary>
		public String PanelName
		{ get { return Resources.Title_Panel; } }

		/// <summary>
		/// Подсказка для кнопки
		/// </summary>
		public String ToolTipButton
		{ get { return Resources.ToolTip_Button; } }

		/// <summary>
		/// Команда
		/// </summary>
		public AbstractExecuteCommand Command
		{ get { return new OffsetCommand(); } }

		/// <summary>
		/// Флаг видимости плагина
		/// </summary>
		public Boolean Visible
		{
			get { return Settings.Visible; }
			set
			{
				Settings.Visible = value;
				Settings.Save();
			}
		}

		/// <summary>
		/// Ресурс рисунка
		/// </summary>
		public Uri BitmapUri
		{ get { return new Uri("pack://application:,,,/OffsetPlugin;component/Images/Button.png"); } }
		#endregion

		#region Constructor
		/// <summary>
		/// Конструктор
		/// </summary>
		public OffsetPlugin()
		{
			Settings = (GeneralSettings)SettingsBase.Synchronized(new GeneralSettings());
		}
		#endregion

	}
}
