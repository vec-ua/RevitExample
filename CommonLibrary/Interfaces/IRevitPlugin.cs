using CommonLibrary.Models;
using System;

namespace CommonLibrary.Interfaces
{
    /// <summary>
    /// Интерфейс плагина для подключения к программе
    /// </summary>
    public interface IRevitPlugin
    {
		/// <summary>
		/// Глобальный уникальный идентификатор
		/// </summary>
		Guid PluginGuid
		{ get; }

		/// <summary>
		/// Тип плагина в виде строки
		/// </summary>
		String PluginType
		{ get; }

		/// <summary>
		/// Контекст представления модели для плагина
		/// </summary>
		AbstractViewModel Context
		{ get; }
	}
}
