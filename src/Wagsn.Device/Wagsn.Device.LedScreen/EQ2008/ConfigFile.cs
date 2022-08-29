using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace TCZNGB.Service.ZNGB_PlayLedMesg
{
    /// <summary>
    /// Represents a configuration file.
    /// </summary>
    public class ConfigFile
    {
        #region Fields and Properties

        Dictionary<string, SettingsGroup> _groups;


        /// <summary>
        /// Gets the groups found in the configuration file.
        /// </summary>
        public Dictionary<string, SettingsGroup> SettingGroups
        {
            get { return _groups; }
        }

        #endregion

        #region Public Constructors

        /// <summary>
        /// Creates a blank configuration file.
        /// </summary>
        public ConfigFile()
        {
            _groups = new Dictionary<string, SettingsGroup>();
        }

        /// <summary>
        /// Loads a configuration file.
        /// </summary>
        /// <param name="file">The filename where the configuration file can be found.</param>
        public ConfigFile(string file)
        {
            LoadFromFile(file);
        }

        #endregion

        #region Loading/Saving

        /// <summary>
        /// Loads the configuration from a file.
        /// </summary>
        /// <param name="file">The file from which to load the configuration.</param>
        public void LoadFromFile(string file)
        {
            //track line numbers for exceptions
            int lineNumber = 0;

            //groups found
            List<SettingsGroup> groups = new List<SettingsGroup>();

            //current group information
            string currentGroupName = null;
            List<Setting> settings = null;
            string comment = string.Empty;
            using (StreamReader reader = new StreamReader(file, Encoding.Default))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine().Trim();
                    lineNumber++;

                    //comments
                    if (line.Length == 0)
                        continue;
                    else if (line[0] == '#')
                    {
                        //if comment has many rows
                        if (string.IsNullOrEmpty(comment))
                            comment = line.Remove(0, 1);
                        else
                            comment = string.Concat(comment, "\r\n", line);
                        continue;
                    }
                    int comentIndex = line.IndexOf("#");
                    if (comentIndex > -1)
                    {
                        //delete comment and record it
                        comment = line.Substring(comentIndex + 1, line.Length - 1 - comentIndex);
                        line = line.Substring(0, comentIndex);
                    }

                    //try to match a group name
                    Match match = Regex.Match(line, "\\[[\u4e00-\u9fa5]+\\：\\s*\\d]");

                    //found group name
                    if (match.Success)
                    {
                        //if we have a current group we're on, we save it
                        if (settings != null && currentGroupName != null)
                            groups.Add(new SettingsGroup(currentGroupName, settings));

                        //make sure the name exists
                        if (match.Value.Length == 2)
                            throw new Exception(string.Format("Group must have name (line {0})", lineNumber));

                        //set our current group information
                        currentGroupName = match.Value.Substring(1, match.Length - 2);
                        settings = new List<Setting>();
                    }

                    //no group name, check for setting with equals sign
                    else if (line.Contains("="))
                    {
                        //split the line
                        string[] parts = line.Split('=');

                        //if we have any more than 2 parts, we have a problem
                        if (parts.Length != 2)
                            throw new Exception(string.Format("Settings must be in the format 'name = value' (line {0})", lineNumber));

                        //trim off whitespace
                        parts[0] = parts[0].Trim();
                        parts[1] = parts[1].Trim();

                        //figure out if we have an array or not
                        bool isArray = false;
                        bool inString = false;

                        //go through the characters
                        foreach (char c in parts[1])
                        {
                            //any comma not in a string makes us creating an array
                            if (c == ',' && !inString)
                                isArray = true;

                            //flip the inString value each time we hit a quote
                            else if (c == '"')
                                inString = !inString;
                        }

                        //if we have an array, we have to trim off whitespace for each item and
                        //do some checking for boolean values.
                        if (isArray)
                        {
                            //split our value array
                            string[] pieces = parts[1].Split(',');

                            //need to build a new string
                            StringBuilder builder = new StringBuilder();

                            for (int i = 0; i < pieces.Length; i++)
                            {
                                //trim off whitespace
                                string s = pieces[i].Trim();

                                //convert to lower case
                                string t = s.ToLower();

                                //check for any of the true values
                                if (t == "on" || t == "yes" || t == "true")
                                    s = "true";

                                //check for any of the false values
                                else if (t == "off" || t == "no" || t == "false")
                                    s = "false";

                                //append the value
                                builder.Append(s);

                                //if we are not on the last value, add a comma
                                if (i < pieces.Length - 1)
                                    builder.Append(",");
                            }

                            //save the built string as the value
                            parts[1] = builder.ToString();
                        }

                        //if not an array
                        else
                        {
                            //make sure we are not working with a string value
                            if (!parts[1].StartsWith("\""))
                            {
                                //convert to lower
                                string t = parts[1].ToLower();

                                //check for any of the true values
                                if (t == "on" || t == "yes" || t == "true")
                                    parts[1] = "true";

                                //check for any of the false values
                                else if (t == "off" || t == "no" || t == "false")
                                    parts[1] = "false";
                            }
                        }

                        //add the setting to our list making sure, once again, we have stripped
                        //off the whitespace

                        settings.Add(new Setting(parts[0].Trim(), parts[1].Trim(), isArray, comment));

                        //add a setting , then clear comment
                        comment = string.Empty;
                    }
                }
            }

            //make sure we save off the last group
            if (settings != null && currentGroupName != null)
                groups.Add(new SettingsGroup(currentGroupName, settings));

            //create our new group dictionary
            _groups = new Dictionary<string, SettingsGroup>();

            //add each group to the dictionary
            foreach (SettingsGroup group in groups)
                _groups.Add(group.Name, group);
        }

        /// <summary>
        /// Saves the configuration to a file
        /// </summary>
        /// <param name="filename">The filename for the saved configuration file.</param>
        public void Save(string filename)
        {
            using (StreamWriter writer = new StreamWriter(filename, false, Encoding.UTF8))
            {
                foreach (KeyValuePair<string, SettingsGroup> groupValue in _groups)
                {
                    writer.WriteLine("[{0}]", groupValue.Key);

                    foreach (KeyValuePair<string, Setting> settingValue in groupValue.Value.Settings)
                    {
                        if (!string.IsNullOrEmpty(settingValue.Value.Desp))
                            writer.WriteLine("#" + settingValue.Value.Desp);
                        writer.WriteLine("{0} = {1}", settingValue.Key, settingValue.Value.RawValue);
                    }

                    writer.WriteLine();
                }
            }
        }

        #endregion

        #region Indexer
        /// <summary>
        /// get a group,if not exists, add this group
        /// </summary>
        /// <param name="name">group name</param>
        /// <returns></returns>
        public SettingsGroup this[string name]
        {
            get
            {
                SettingsGroup group;
                if (SettingGroups.TryGetValue(name, out group))
                    return group;
                group = new SettingsGroup(name);
                SettingGroups.Add(name, group);
                return group;
            }
        }
        #endregion
    }
}
