﻿using System;
using System.Configuration;

namespace FloorsPlugin.Settings
{
    /// <summary>
    /// Класс представления секции настроек кнопки
    /// </summary>
    [SettingsGroupName("FloorsPluginGeneral")]
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
