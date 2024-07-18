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
                var configIniPathvar = ProgramDataFilePath + @"\Data.ini";
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
    }
}