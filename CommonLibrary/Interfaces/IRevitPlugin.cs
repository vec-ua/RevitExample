using System;

namespace CommonLibrary.Interfaces
{
    public interface IRevitPlugin
    {
		/// <summary>
		/// Название плагина
		/// </summary>
		String PlaginName
		{ get; }

		/// <summary>
		/// Полный путь к файлу
		/// </summary>
		String FillPathName
		{ get; }

		/// <summary>
		/// Подсказка для кнопки
		/// </summary>
		String ToolTipButton
		{ get; }

		/// <summary>
		/// Путь к команде
		/// </summary>
		String CommandPath
		{ get; }

		/// <summary>
		/// Ресурс рисунка
		/// </summary>
		Uri BitmapUri
		{ get; }
	}
}
