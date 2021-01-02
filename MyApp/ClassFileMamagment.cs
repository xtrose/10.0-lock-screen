using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;





// Namespace
namespace MyApp
{





    // Klasse die die Dateien verwaltet
    class ClassFileMamagment
    {





        // Variablen
        // ---------------------------------------------------------------------------------------------------
        // Iso Store Variablen
        public static IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication();
        public static IsolatedStorageFileStream filestream;
        public static StreamReader streamReader;
        public static StreamWriter streamWriter;
        // ---------------------------------------------------------------------------------------------------





        // Datei laden, erstellen, überschreiben
        // ---------------------------------------------------------------------------------------------------
        public static async Task<string> loadCreateOverwrite(string path, string data, bool write)
        {
            // Ausgabe erstellen
            string output = "";


            // Wenn Datei bereits besteht
            if (file.FileExists(path))
            {
                // Wenn Datei geladen wird
                if(!write)
                {
                    filestream = file.OpenFile(path, FileMode.Open);
                    streamReader = new StreamReader(filestream);
                    output = streamReader.ReadToEnd();
                    filestream.Close();
                    output = output.Trim();
                }
                // Wenn Datei überschrieben wird
                else
                {
                    filestream = file.CreateFile(path);
                    streamWriter = new StreamWriter(filestream);
                    streamWriter.WriteLine(data);
                    streamWriter.Flush();
                    filestream.Close();
                    output = data;
                }
            }
            // Wenn Datei noch nicht besteht
            else if (write)
            {
                filestream = file.CreateFile(path);
                streamWriter = new StreamWriter(filestream);
                streamWriter.WriteLine(data);
                streamWriter.Flush();
                filestream.Close();
                output = data;
            }


            // Ausgabe
            return output;
        }
        // ---------------------------------------------------------------------------------------------------





        // Ordner allen Dateien und Unterorden löschen
        // ---------------------------------------------------------------------------------------------------
        public static async Task deleteFolder(string path)
        {
            // Alle Dateien und Ordner laden
            string[] files = file.GetFileNames(path + "/");
            string[] directorys = file.GetDirectoryNames(path + "/");


            // Alle Dateien durchlaufen und löschen
            for (int i = 0; i < files.Count(); i++)
            {
                file.DeleteFile(path + "/" + files[i]);
            }


            // Alle Ordner durchlaufen und löschen
            for (int i = 0; i < directorys.Count(); i++)
            {
                await deleteFolder(path + "/" + directorys[i]);
            }


            // Ordner löschen
            file.DeleteDirectory(path);
        }
        // ---------------------------------------------------------------------------------------------------





        // Sperrbildschirm Einstellungen speichern
        // ---------------------------------------------------------------------------------------------------
        public static async Task saveSettings()
        {
            // Einstellungen zusammen stellen
            string strSettings = "backgroundType=" + MainPage.backgroundType;
            strSettings += "&backgroundColor=" + MainPage.backgroundColor;
            strSettings += "&userInformationBackgroundColor=" + MainPage.userInformationBackgroundColor;
            strSettings += "&frameColor=" + MainPage.frameColor;
            strSettings += "&frameSize=" + MainPage.frameSize.ToString();
            strSettings += "&usePicturesTransparency=" + MainPage.usePicturesTransparency.ToString();
            strSettings += "&picturesTransparencyMin=" + MainPage.picturesTransparencyMin;
            strSettings += "&picturesTransparencyMax=" + MainPage.picturesTransparencyMax;
            strSettings += "&useUserInformation=" + MainPage.useUserInformation.ToString();
            strSettings += "&userInformationVerticalAlignment=" + MainPage.userInformationVerticalAlignment.ToString();
            strSettings += "&userInformation1=" + MainPage.userInformation1;
            strSettings += "&userInformation2=" + MainPage.userInformation2;
            strSettings += "&userInformation3=" + MainPage.userInformation3;
            strSettings += "&userInformation4=" + MainPage.userInformation4;
            strSettings += "&userInformationFontColor=" + MainPage.userInformationFontColor;
            strSettings += "&useInformationBackground=" + MainPage.useInformationBackground.ToString();
            strSettings += "&informationBackgroundColor=" + MainPage.informationBackgroundColor;
            strSettings += "&informationBackgroundSize=" + MainPage.informationBackgroundSize.ToString();

            // Einstellungen speichern
            await ClassFileMamagment.loadCreateOverwrite("/LockScreens/" + MainPage.folderName + "/Settings.txt", strSettings, true);
        }
        // ---------------------------------------------------------------------------------------------------





        // Sperrbildschirm Einstellungen laden
        // ---------------------------------------------------------------------------------------------------
        public static async Task loadSettings()
        {
            // Einstellungen laden
            string strSettings = await ClassFileMamagment.loadCreateOverwrite("/LockScreens/" + MainPage.folderName + "/Settings.txt", "", false);

            // Einstellungen splitten
            string[] arSettings = Regex.Split(strSettings, "&");

            // Einstellungen durchlaufen
            for (int i = 0; i < arSettings.Count(); i++)
            {
                // Einzelne Einstellungen splitten
                string[] arSetting = Regex.Split(arSettings[i].Trim(), "=");
                // Einstellungen anwenden
                if (arSetting[0] == "backgroundType")
                {
                    MainPage.backgroundType = arSetting[1];
                }
                else if (arSetting[0] == "backgroundColor")
                {
                    MainPage.backgroundColor = arSetting[1];
                }
                else if (arSetting[0] == "userInformationBackgroundColor")
                {
                    MainPage.userInformationBackgroundColor = arSetting[1];
                }
                else if (arSetting[0] == "frameColor")
                {
                    MainPage.frameColor = arSetting[1];
                }
                else if (arSetting[0] == "frameSize")
                {
                    MainPage.frameSize = Convert.ToInt32(arSetting[1]);
                }
                else if (arSetting[0] == "usePicturesTransparency")
                {
                    MainPage.usePicturesTransparency = Convert.ToBoolean(arSetting[1]);
                }
                else if (arSetting[0] == "picturesTransparencyMin")
                {
                    MainPage.picturesTransparencyMin = Convert.ToInt32(arSetting[1]);
                }
                else if (arSetting[0] == "picturesTransparencyMax")
                {
                    MainPage.picturesTransparencyMax = Convert.ToInt32(arSetting[1]);
                }
                else if (arSetting[0] == "useUserInformation")
                {
                    MainPage.useUserInformation = Convert.ToBoolean(arSetting[1]);
                }
                else if (arSetting[0] == "userInformationVerticalAlignment")
                {
                    MainPage.userInformationVerticalAlignment = Convert.ToInt32(arSetting[1]);
                }
                else if (arSetting[0] == "userInformation1")
                {
                    MainPage.userInformation1 = arSetting[1];
                }
                else if (arSetting[0] == "userInformation2")
                {
                    MainPage.userInformation2 = arSetting[1];
                }
                else if (arSetting[0] == "userInformation3")
                {
                    MainPage.userInformation3 = arSetting[1];
                }
                else if (arSetting[0] == "userInformation4")
                {
                    MainPage.userInformation4 = arSetting[1];
                }
                else if (arSetting[0] == "userInformationFontColor")
                {
                    MainPage.userInformationFontColor = arSetting[1];
                }
                else if (arSetting[0] == "useInformationBackground")
                {
                    MainPage.useInformationBackground = Convert.ToBoolean(arSetting[1]);
                }
                else if (arSetting[0] == "informationBackgroundColor")
                {
                    MainPage.informationBackgroundColor = arSetting[1];
                }
                else if (arSetting[0] == "informationBackgroundSize")
                {
                    MainPage.informationBackgroundSize = Convert.ToInt32(arSetting[1]);
                }
            }
        }
        // --------------------------------------------------------------------------------------------------- 





        // Haupteinstellungen speichern
        // ---------------------------------------------------------------------------------------------------
        public static async Task saveMainSettings()
        {
            // Einstellungen zusammenstellen
            string stSettings = "LockScreen=" + MainPage.setLockScreen + ";" + "LockScreenCount=" + MainPage.setLockScreenCount;


            // Einstellungen speichern
            await ClassFileMamagment.loadCreateOverwrite("Settings/Settings.txt", stSettings, true);
        }
        // ---------------------------------------------------------------------------------------------------





        // Haupteinstellungen laden
        // ---------------------------------------------------------------------------------------------------
        public static async Task loadMainSettings()
        {
            // Einstellungen laden
            string stSettings = await ClassFileMamagment.loadCreateOverwrite("Settings/Settings.txt", "", false);


            // Einstellungen durchlaufen
            string[] arSettings = Regex.Split(stSettings, ";");
            for (int i = 0; i < arSettings.Count(); i++)
            {
                // Einzelne Einstellung Splitten
                string[] arSetting = Regex.Split(arSettings[i], "=");

                // LockScreen
                if (arSetting[0] == "LockScreen")
                {
                    MainPage.setLockScreen = arSetting[1];
                }

                // Swap Image
                if (arSetting[0] == "LockScreenCount")
                {
                    MainPage.setLockScreenCount = Convert.ToInt32(arSetting[1]);
                }
            }
        }
        // ---------------------------------------------------------------------------------------------------





    }
}
