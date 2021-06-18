using System.Collections.Generic;
using System.IO;
using MelonLoader;

namespace WingsClient
{
    public class Settings
    {
        public const string SettingsPath = "WingsClient/Settings.ini";

        public void SetSetting(string key, string newValue)
        {
            foreach (string line in File.ReadAllLines(SettingsPath))
            {
                if (key.Contains(line.Split('=')[0].Trim()))
                {
                    var originalLines = File.ReadAllLines(SettingsPath);
                    var updatedLines = new List<string>();
                    foreach (var replaceLine in originalLines)
                    {
                        string[] settings = line.Split('=');
                        if (settings[0] == key)
                        {
                            settings[1] = newValue;
                        }

                        updatedLines.Add(string.Join("\n", settings));
                    }

                    File.WriteAllLines(SettingsPath, updatedLines);
                }
            }

            StreamWriter stream = File.AppendText(SettingsPath);
            stream.WriteLineAsync(key + '=' + newValue);
            stream.Close();
        }

        public string GetSetting(string key, string defaultKey)
        {
            foreach (string line in File.ReadAllLines(SettingsPath))
            {
                if (key.Contains(line.Split('=')[0].Trim()))
                {
                    return line.Split('=')[1];
                }
            }

            StreamWriter stream = File.AppendText(SettingsPath);
            stream.WriteLineAsync(key + '=' + defaultKey);
            stream.Close();
            return defaultKey;
        }
    }
}