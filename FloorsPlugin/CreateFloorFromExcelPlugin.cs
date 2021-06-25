using CommonLibrary.Interfaces;
using CommonLibrary.Models;
using FloorsPlugin.Commands;
using FloorsPlugin.Properties;
using FloorsPlugin.Settings;
using System;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace FloorsPlugin
{
	/// <summary>
	/// Плагин
	/// </summary>
	public sealed class CreateFloorFromExcelPlugin : IRevitPlugin
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
		{ get { return new Guid("8305a045-c373-41c5-90b3-90cfe27a4a91"); } }

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
		{ get { return new CreateFloorFromExcelCommand(); } }

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
		{ get { return new Uri("pack://application:,,,/FloorsPlugin;component/Images/Button.png"); } }
		#endregion

		#region Constructor
		/// <summary>
		/// Конструктор
		/// </summary>
		public CreateFloorFromExcelPlugin()
        {
			Settings = (GeneralSettings)SettingsBase.Synchronized(new GeneralSettings());
		}
		#endregion
	}
}
