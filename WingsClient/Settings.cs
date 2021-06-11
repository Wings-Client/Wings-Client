using System.Collections.Generic;
using System.IO;
using MelonLoader;

namespace WingsClient
{
    public class Settings
    {
        private const string SettingsPath = "WingsClient/Settings.ini";

        public Settings()
        {
            if (!File.Exists(SettingsPath))
            {
                File.Create(SettingsPath).Close();
                MelonLogger.Msg("Created empty settings file.");
            }
        }

        public void SetSetting(string key, string newValue)
        {
            foreach (string line in File.ReadAllLines(SettingsPath))
            {
                if (key.Contains(line.Split('=')[0].Trim().ToLower()))
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

        public string GetSetting(string key)
        {
            foreach (string line in File.ReadAllLines(SettingsPath))
            {
                if (key.Contains(line.Split('=')[0].Trim().ToLower()))
                {
                    return line.Split('=')[1];
                }
            }

            return null;
        }
    }
}