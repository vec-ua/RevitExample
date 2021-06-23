using CommonLibrary.Interfaces;
using CommonLibrary.Models;
using MessageBoxPlugin.Commands;
using MessageBoxPlugin.Properties;
using MessageBoxPlugin.Settings;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBoxPlugin
{
    public sealed class MessageBoxPlugin : IRevitPlugin
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
		{ get { return new Guid("a63ed2ad-825a-45b8-ae72-c82cf8dcb860"); } }

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
		{ get { return new MessageBoxCommand(); } }

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
		{ get { return new Uri("pack://application:,,,/MessageBoxPlugin;component/Images/Button.png"); } }
		#endregion

		#region Constructor
		/// <summary>
		/// Конструктор
		/// </summary>
		public MessageBoxPlugin()
		{
			Settings = (GeneralSettings)SettingsBase.Synchronized(new GeneralSettings());
		}
		#endregion
	}
}
