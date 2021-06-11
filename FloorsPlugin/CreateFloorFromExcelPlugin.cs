using CommonLibrary.Interfaces;
using FloorsPlugin.Properties;
using System;
using System.ComponentModel.Composition;
using System.Reflection;

namespace FloorsPlugin
{
	[Export(typeof(IRevitPlugin))]
	[PartCreationPolicy(CreationPolicy.Shared)]
	public sealed class CreateFloorFromExcelPlugin : IRevitPlugin
    {
		/// <summary>
		/// Название плагина
		/// </summary>
		public String PlaginName
		{ get { return Resources.Title_Plugin; } }

		/// <summary>
		/// Полный путь к файлу
		/// </summary>
		public String FillPathName
		{ get { return Assembly.GetExecutingAssembly().Location; } }

		/// <summary>
		/// Подсказка для кнопки
		/// </summary>
		public String ToolTipButton
		{ get { return Resources.ToolTip_Button; } }

		/// <summary>
		/// Путь к команде
		/// </summary>
		public String CommandPath
		{ get { return "FloorsPlugin.Commands.CreateFloorFromExcelCommand"; } }

		/// <summary>
		/// Ресурс рисунка
		/// </summary>
		public Uri BitmapUri
		{ get { return new Uri("pack://application:,,,/FloorsPlugin;component/Images/Button.png"); } }
	}
}
