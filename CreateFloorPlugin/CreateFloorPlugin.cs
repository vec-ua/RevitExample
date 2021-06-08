using CommonLibrary.Interfaces;
using CommonLibrary.Models;
using CreateFloorPlugin.Properties;
using CreateFloorPlugin.ViewModel;
using System;
using System.ComponentModel.Composition;

namespace CreateFloorPlugin
{
	[Export(typeof(IRevitPlugin))]
	[PartCreationPolicy(CreationPolicy.Shared)]
	public sealed class CreateFloorPlugin : IRevitPlugin
    {
		#region Properties
		/// <summary>
		/// Глобальный уникальный идентификатор
		/// </summary>
		public Guid PluginGuid
		{ get { return new Guid(Resources.STR_GUID); } }

		/// <summary>
		/// Тип плагина в виде строки
		/// </summary>
		public String PluginType
		{ get { return Resources.STR_TYPE_PLUGIN; } }

		/// <summary>
		/// Контекст представления модели для плагина
		/// </summary>
		public AbstractViewModel Context
		{ get; private set; }
		#endregion

		#region Constructor
		/// <summary>
		/// Конструктор
		/// </summary>
		public CreateFloorPlugin()
		{
			Context = new CreateFloorViewModel();
		}
		#endregion
	}
}
