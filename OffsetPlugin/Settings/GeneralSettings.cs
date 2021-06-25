using System;
using System.Configuration;

namespace OffsetPlugin.Settings
{
    /// <summary>
    /// Класс представления секции настроек кнопки
    /// </summary>
    [SettingsGroupName("OffsetGeneral")]
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
