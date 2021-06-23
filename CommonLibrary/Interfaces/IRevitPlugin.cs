using CommonLibrary.Models;
using System;
using System.ComponentModel.Composition;

namespace CommonLibrary.Interfaces
{
	/// <summary>
	/// Интерфейс плагина
	/// </summary>
	[InheritedExport(typeof(IRevitPlugin))]
	public interface IRevitPlugin
    {
		/// <summary>
		/// Серийный номер
		/// </summary>
		Guid SerialNumber
		{ get; }

		/// <summary>
		/// Название плагина
		/// </summary>
		String Name
		{ get; }

		/// <summary>
		/// Название панели
		/// </summary>
		String PanelName
		{ get; }

		/// <summary>
		/// Подсказка для кнопки
		/// </summary>
		String ToolTipButton
		{ get; }

		/// <summary>
		/// Команда
		/// </summary>
		AbstractExecuteCommand Command
		{ get; }

		/// <summary>
		/// Флаг видимости плагина
		/// </summary>
		Boolean Visible
		{ get; set; }

		/// <summary>
		/// Ресурс рисунка
		/// </summary>
		Uri BitmapUri
		{ get; }
	}
}
