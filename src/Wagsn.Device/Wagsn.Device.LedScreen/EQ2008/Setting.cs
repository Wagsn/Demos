using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCZNGB.Service.ZNGB_PlayLedMesg
{
    /// <summary>
    /// A single setting from a configuration file
    /// </summary>
    public class Setting
    {
        #region Fields and Properties

        string _name;
        string _rawValue;
        bool _isArray = false;
        string _desp;

        /// <summary>
        /// Gets the name of the setting.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Gets the raw value of the setting.
        /// </summary>
        public string RawValue
        {
            get { return _rawValue; }
            set { _rawValue = value; }
        }

        /// <summary>
        /// Gets whether or not the setting is an array.
        /// </summary>
        public bool IsArray
        {
            get { return _isArray; }
            set { _isArray = value; }
        }

        /// <summary>
        /// Gets the Description of the setting
        /// </summary>
        public string Desp
        {
            get { return _desp; }
        }

        #endregion

        #region Internal Constructors

        internal Setting(string name, string value, bool isArray, string desp)
        {
            _name = name;
            _rawValue = value;
            _isArray = isArray;
            _desp = desp;
        }

        #endregion

        #region Getting Value

        /// <summary>
        /// Attempts to return the setting's value as an integer.
        /// </summary>
        /// <param name="value">default value, missins value is -1</param>
        /// <returns>An integer representation of the value</returns>
        public int AsInt(int value = -1)
        {
            int retValue;
            if (int.TryParse(RawValue, out retValue))
                return retValue;
            return value;
        }

        /// <summary>
        /// Attempts to return the setting's value as a float.
        /// </summary>
        /// <param name="value">default value, missins value is -1f</param>
        /// <returns>A float representation of the value</returns>
        public float AsFloat(float value = -1f)
        {
            float retValue;
            if (float.TryParse(RawValue, out retValue))
                return retValue;
            return value;
        }

        /// <summary>
        /// Attempts to return the setting's value as a bool.
        /// </summary>
        /// <param name="value">default value, missins value is false</param>
        /// <returns>A bool representation of the value</returns>
        public bool AsBool(bool value = false)
        {
            bool retValue;
            if (bool.TryParse(RawValue, out retValue))
                return retValue;
            return value;
        }

        /// <summary>
        /// Attempts to return the setting's value as a string.
        /// </summary>
        /// <returns>A string representation of the value</returns>
        public string AsString()
        {
            return RawValue;
        }

        /// <summary>
        /// Get Value as T
        /// </summary>
        /// <typeparam name="T">value type</typeparam>
        /// <param name="value">default value</param>
        /// <returns></returns>
        public T As<T>(T value)
        {
            try
            {
                return (T)Convert.ChangeType(RawValue, typeof(T));
            }
            catch (Exception)
            {
                return value;
            }
        }

        /// <summary>
        /// Get array Value as T[]
        /// </summary>
        /// <typeparam name="T">array value type</typeparam>
        /// <returns></returns>
        public T[] AsArray<T>()
        {
            string[] parts = RawValue.Split(',');

            T[] valueParts = new T[parts.Length];

            for (int i = 0; i < parts.Length; i++)
                valueParts[i] = (T)Convert.ChangeType(parts[i], typeof(T));

            return valueParts;
        }

        #endregion

        #region Setting Value

        /// <summary>
        /// Sets the value of the setting.
        /// </summary>
        /// <param name="value">The new value to store.</param>
        public void SetValue(object value)
        {
            _rawValue = value.ToString();
        }

        /// <summary>
        /// Sets the array Value of the setting
        /// </summary>
        /// <typeparam name="T">array value type</typeparam>
        /// <param name="values">array</param>
        public void SetValue(params object[] values)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < values.Length; i++)
            {
                builder.Append(values[i].ToString());
                if (i < values.Length - 1)
                    builder.Append(",");
            }

            _rawValue = builder.ToString();
        }

        #endregion
    }
}
