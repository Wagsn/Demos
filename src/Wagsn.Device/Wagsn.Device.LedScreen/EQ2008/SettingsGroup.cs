using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCZNGB.Service.ZNGB_PlayLedMesg
{
    /// <summary>
    /// A group of settings from a configuration file.
    /// </summary>
    public class SettingsGroup
    {
        #region Fields and Properties

        string _name;
        Dictionary<string, Setting> _settings;

        /// <summary>
        /// Gets the name of the group.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Gets the settings found in the group.
        /// </summary>
        public Dictionary<string, Setting> Settings
        {
            get { return _settings; }
        }

        #endregion

        #region Internal Constructors

        internal SettingsGroup(string name)
        {
            _name = name;
            _settings = new Dictionary<string, Setting>();
        }

        internal SettingsGroup(string name, List<Setting> settings)
        {
            _name = name;
            _settings = new Dictionary<string, Setting>();

            foreach (Setting setting in settings)
                _settings.Add(setting.Name, setting);
        }

        #endregion

        #region Write Settings

        /// <summary>
        /// Write a setting to the group.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="isArray"></param>
        public void WriteSetting(string name, object value, bool isArray = false)
        {
            string settingValue = value.ToString();

            if (!_settings.ContainsKey(name))
                _settings.Add(name, new Setting(name, settingValue, isArray, string.Empty));
            else
            {
                _settings[name].RawValue = settingValue;
                _settings[name].IsArray = isArray;
            }
        }


        /// <summary>
        /// Write a array setting to the group.
        /// </summary>
        /// <typeparam name="T">The type of array</typeparam>
        /// <param name="name">The name of the setting.</param>
        /// <param name="values">The values of the setting.</param>
        public void WriteArraySetting<T>(string name, params T[] values)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < values.Length; i++)
            {
                builder.Append(values[i]);
                if (i < values.Length - 1)
                    builder.Append(",");
            }

            WriteSetting(name, builder.ToString(), true);
        }

        #endregion

        #region Deleting Settings

        /// <summary>
        /// Deletes a setting from the group.
        /// </summary>
        /// <param name="name">The name of the setting to delete.</param>
        public void DeleteSetting(string name)
        {
            if (_settings.ContainsKey(name))
                _settings.Remove(name);
        }

        #endregion

        #region Indexer
        /// <summary>
        /// get setting ,if not exists,add it
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Setting this[string name]
        {
            get
            {
                Setting setting;
                if (Settings.TryGetValue(name, out setting))
                    return setting;
                setting = new Setting(name, string.Empty, false, string.Empty);
                Settings.Add(name, setting);
                return setting;
            }
        }
        #endregion
    }
}
