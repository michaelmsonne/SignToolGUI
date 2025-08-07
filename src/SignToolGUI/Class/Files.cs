using System;

namespace SignToolGUI.Class
{
    class Files
    {
        public static string ConfigIniPath
        {
            get
            {
                // Path to the configuration file
                var configIniPathvar = ProgramDataFilePath + @"\Config.ini";
                return configIniPathvar;
            }
        }
        public static string ProgramDataFilePath
        {
            get
            {
                // Path to the program data folder
                var programDataFilePathvar = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\SignToolGUI";
                return programDataFilePathvar;
            }
        }

        public static string LogFilePath
        {
            get
            {
                // Root folder for log files
                var logfilePathvar = ProgramDataFilePath + @"\Log";
                return logfilePathvar;
            }
        }
    }
}