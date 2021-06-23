using System;
using System.Configuration;

namespace MessageBoxPlugin.Settings
{
    /// <summary>
    /// Класс представления секции настроек кнопки
    /// </summary>
    [SettingsGroupName("MessageBoxPluginGeneral")]
    public sealed class GeneralSettings : ApplicationSettingsBase
    {
        /// <summary>
        /// Флаг видимости кнопки
        /// </summary>
        [UserScopedSetting]
        [DefaultSettingValue("True")]
        public Boolean Visible
        {
            get
            { return (Boolean)this["Visible"]; }
            set
            { this["Visible"] = value; }
        }
    }
}
