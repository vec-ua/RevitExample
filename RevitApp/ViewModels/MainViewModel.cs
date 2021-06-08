using CommonLibrary.Helpers;
using CommonLibrary.Interfaces;
using CommonLibrary.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitApp.ViewModels
{
    public sealed class MainViewModel : AbstractViewModel
    {
        #region Fields
        /// <summary>
        /// Внутренний список плагинов
        /// </summary>
        private ObservableCollection<IRevitPlugin> InternalPlugin;
        #endregion

        #region Properties
        /// <summary>
        /// Список плагинов (только для чтения)
        /// </summary>
        public ReadOnlyObservableCollection<IRevitPlugin> Plugins
        { get; private set; }

        private IRevitPlugin _SelectedPlugin;
        /// <summary>
        /// Выбраный плагин
        /// </summary>
        public IRevitPlugin SelectedPlugin
        {
            get { return _SelectedPlugin; }
            set
            {
                _SelectedPlugin = value;
                RaisePropertyChanged("SelectedPlugin");
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Конструктор
        /// </summary>
        public MainViewModel()
        { }
        #endregion

        #region Methods
        /// <summary>
        /// Инициализация команд
        /// </summary>
        protected override void Initialize()
        {
            InternalPlugin = new ObservableCollection<IRevitPlugin>();
            Plugins = new ReadOnlyObservableCollection<IRevitPlugin>(InternalPlugin);

            IEnumerable<IRevitPlugin> plugins = LoadLibrariesHelper<IRevitPlugin>.Instance.GetAllLibraries();
            foreach(IRevitPlugin plugin in plugins)
            {
                IRevitPlugin obj = InternalPlugin.FirstOrDefault(plg => plg.PluginGuid.Equals(plugin.PluginGuid));
                if (obj == null)
                    InternalPlugin.Add(plugin);
            }
        }
        #endregion
    }
}
