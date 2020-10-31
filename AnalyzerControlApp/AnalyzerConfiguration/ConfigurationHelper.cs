using System;
using System.IO;

namespace AnalyzerConfiguration
{
    public static class ConfigurationHelper
    {
        const string configurationDir = "Configuration";
        const string backupDir = "BackupConfiguration";

        const string backupDateTimeFormat = "dd_MM_yyyy_#_HH_mm_ss";

        public static void SaveBackup(string filename)
        {
            string backupPath = $"{backupDir}/{DateTime.Now.ToString(backupDateTimeFormat)}/";

            string fullFilename = GetConfigurationPath(filename);

            if (File.Exists(fullFilename))
            {
                Directory.CreateDirectory(backupPath);

                string fileNameForBackup = Path.Combine(backupPath, filename);
                File.Move(fullFilename, fileNameForBackup);
            }
        }

        public static string GetConfigurationPath(string filename)
        {
            if(!Directory.Exists(configurationDir))
            {
                Directory.CreateDirectory(configurationDir);
            }

            return Path.Combine(configurationDir, filename);
        }
    }
}
