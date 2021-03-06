﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.IO.IsolatedStorage;
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Microsoft.Phone.Scheduler;
using System.Threading;
using Microsoft.Phone.Shell;
using System.Collections.Generic;
using System.Windows.Shapes;





// Namespace
namespace MyScheduledTaskAgent
{




    // ScheduledAgent
    public class ScheduledAgent : ScheduledTaskAgent
    {





        // Klasse erstellen
        // ---------------------------------------------------------------------------------------------------
        static ScheduledAgent()
        {
        }
        // ---------------------------------------------------------------------------------------------------





        // Variablen
        // ---------------------------------------------------------------------------------------------------
        // Iso Store Variablen erstellen
        IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication();
        IsolatedStorageFileStream filestream;
        StreamReader streamReader;
        StreamWriter streamWriter;


        // Random erstellen
        Random rand = new Random();


        // Ob Lock Screen App
        bool isSetLockScreenApp = false;


        // Variablen zur Version
        bool fullVersion = false;


        // Variable ob Lock Screen erstellt wird
        bool engineRun = false;


        // Zeitstempel des ersten Starts
        DateTime dtFirstStart = DateTime.Now;


        // Zeitstempel aktuelle Zeit erstellen
        DateTime dtNow = DateTime.Now;


        // Testszeitraum // Stunden
        int trialTime = 48;


        // Display Auflösung
        double resolutionWidth = -1;
        double resolutionHeight = -1;
        double resolutionScaleFactor = -1;
        string resolution = "480X800";


        // Allgemeine Einstellungen
        string setLockScreen = "";
        string setLockScreenStart = "";
        int setLockScreenCount = 0;


        // Einstellungen Variablen
        bool boolUsePicturesTransparency = false;
        int intPicturesTransparencyMin = 10;
        int intPicturesTransparencyMax = 10;
        string stringBackgroundType = "color";
        string stringBackgroundColor = "bc";
        string stringUserInformationBackgroundColor = "ac";
        string stringFrameColor = "ac";
        int intFrameSize = 5;
        bool boolUseUserInformation = false;
        int intUserInformationVerticalAlignment = 20;
        string stringUserInformation1 = "";
        string stringUserInformation2 = "";
        string stringUserInformation3 = "";
        string stringUserInformation4 = "";
        string stringUserInformationFontColor = "#FFFFFFFF";
        bool boolUseInformationBackground = false;
        string stringInformationBackgroundColor = "ac";
        int intInformationBackgroundSize = 40;
        // Arrays der Bilder
        string[] arSquare = null;
        string[] arLandscape = null;
        string[] arPortrait = null;
        string[] arBackground = null;
        string[] arUser = null;
        // Listen der Bilder
        List<string> listUser = new List<string>();
        List<string> listSquare = new List<string>();
        List<string> listLandscape = new List<string>();
        List<string> listPortrait = new List<string>();
        List<string> listBackground = new List<string>();
        List<string> listSquareSelection = new List<string>();
        List<string> listLandscapeSelection = new List<string>();
        List<string> listPortraitSelection = new List<string>();
        List<string> listBackgroundSelection = new List<string>();


        // Pfad zum Demo Bild
        string pathLockScreenDemo = "/Images/LockScreenDemo.480X800.jpg";
        // ---------------------------------------------------------------------------------------------------





        // Tasks die ausgeführt werden
        // ---------------------------------------------------------------------------------------------------
        protected override void OnInvoke(ScheduledTask task)
        {
            // Periodischer Task
            if (task is PeriodicTask)
            {
                // Bild erstellen
                // ----------------------------------------------------
                Deployment.Current.Dispatcher.BeginInvoke(delegate
                {





                    // Prüfen ob momentan Lockscreen App
                    if (Windows.Phone.System.UserProfile.LockScreenManager.IsProvidedByCurrentApplication)
                    {
                        // IsLockscreenApp auf true stellen
                        isSetLockScreenApp = true;
                    }


                    // Speichergröße auslesen
                    int memoryLimit = Convert.ToInt32(Microsoft.Phone.Info.DeviceStatus.DeviceTotalMemory / 1024 / 1024);


                    // Display Auflösung ermitteln
                    double scaleWidth = Application.Current.Host.Content.ActualWidth;
                    double scaleHeight = Application.Current.Host.Content.ActualHeight;
                    double scaleFactor = Application.Current.Host.Content.ScaleFactor;
                    resolutionWidth = Math.Round(scaleWidth / Convert.ToDouble(100) * scaleFactor, MidpointRounding.ToEven);
                    resolutionHeight = Math.Round(scaleHeight / Convert.ToDouble(100) * scaleFactor, MidpointRounding.ToEven);


                    // Wenn Format 480X800 // 15:9
                    if (resolutionWidth == 480 & resolutionHeight == 800)
                    {
                        // Auflösung festlegen
                        resolution = "480X800";
                        // Demo Bild Pfad erstellen
                        pathLockScreenDemo = "/Images/LockScreenDemo.480X800.jpg";
                    }
                    // Wenn Format 768X1280 // 15:9
                    else if (resolutionWidth == 768 & resolutionHeight == 1280)
                    {
                        // Bei 512 MB RAM
                        if (memoryLimit < 1000)
                        {
                            // Auflösung festlegen
                            resolution = "480X800";
                            // Demo Bild Pfad erstellen
                            pathLockScreenDemo = "/Images/LockScreenDemo.480X800.jpg";
                        }
                        // Bei mehr als 512 MB RAM
                        else
                        {
                            // Auflösung festlegen
                            resolution = "768X1280";
                            // Demo Bild Pfad erstellen
                            pathLockScreenDemo = "/Images/LockScreenDemo.768X1280.jpg";
                        }
                    }
                    // Wenn Format 720X1280 und 1080X1920 und 2560X1440 // 16:9
                    else
                    {
                        // Bei 1024 MB RAM
                        if (memoryLimit < 2500)
                        {
                            // Auflösung festlegen
                            resolution = "720X1280";
                            // Demo Bild Pfad erstellen
                            pathLockScreenDemo = "/Images/LockScreenDemo.720X1280.jpg";
                        }
                        // Bei mehr als 1024 MB RAM
                        else
                        {
                            // Auflösung festlegen
                            resolution = "1080X1920";
                            // Demo Bild Pfad erstellen
                            pathLockScreenDemo = "/Images/LockScreenDemo.1080X1920.jpg";
                        }
                    }





                    // Grundeinstellungen vornehmen
                    // Anzahl Bilder
                    int lsVertical = 10;
                    int lsHorizontal = 6;


                    // Bild Größe
                    int lsWidth = 480;
                    int lsHeigth = 800;


                    // Bild Rander und Abstände
                    int lsMarginTop = 20;
                    int lsMarginLeft = 14;
                    int lsMarginMiddle = 11;


                    // Tile Größe
                    int lsTileSize = 66;





                    // Wenn Format 480X800 // 15:9
                    if (resolution == "480X800")
                    {
                        // Bildgröße festlegen
                        lsVertical = 10;
                        lsWidth = 480;
                        lsHeigth = 800;
                        lsMarginTop = 20;
                        lsMarginLeft = 14;
                        lsMarginMiddle = 11;
                        lsTileSize = 66;
                    }
                    // Wenn Format 768X1280 // 15:9
                    if (resolution == "768X1280")
                    {
                        // Bildgröße festlegen
                        lsVertical = 10;
                        lsWidth = 768;
                        lsHeigth = 1280;
                        lsMarginTop = 33;
                        lsMarginLeft = 23;
                        lsMarginMiddle = 17;
                        lsTileSize = 106;
                    }
                    // Wenn Format 720X1280 // 16:9
                    if (resolution == "720X1280")
                    {
                        // Bildgröße festlegen
                        lsVertical = 11;
                        lsWidth = 720;
                        lsHeigth = 1280;
                        lsMarginTop = 15;
                        lsMarginLeft = 23;
                        lsMarginMiddle = 16;
                        lsTileSize = 99;
                    }
                    // Wenn Format 1080X1920 // 16:9
                    if (resolution == "1080X1920")
                    {
                        // Bildgröße festlegen
                        lsVertical = 11;
                        lsWidth = 1080;
                        lsHeigth = 1920;
                        lsMarginTop = 21;
                        lsMarginLeft = 33;
                        lsMarginMiddle = 25;
                        lsTileSize = 148;
                    }





                    // Einstellungen laden
                    // Allgemeine Einstellungen laden
                    filestream = file.OpenFile("Settings/FullVersion.txt", FileMode.Open);
                    streamReader = new StreamReader(filestream);
                    int intFullVersion = Convert.ToInt32(streamReader.ReadToEnd().Trim());
                    // Wenn Vollversion
                    if (intFullVersion == 1)
                    {
                        fullVersion = true;
                        engineRun = true;
                    }
                    else
                    {
                        // Zeitstempel erster Start laden
                        filestream = file.OpenFile("Settings/FirstTime.txt", FileMode.Open);
                        streamReader = new StreamReader(filestream);
                        string stDateTime = streamReader.ReadToEnd().Trim();
                        string[] arDateTime = Regex.Split(stDateTime, "~");
                        dtFirstStart = new DateTime(Convert.ToInt32(arDateTime[0]), Convert.ToInt32(arDateTime[1]), Convert.ToInt32(arDateTime[2]), Convert.ToInt32(arDateTime[3]), Convert.ToInt32(arDateTime[4]), 0);

                        // Wenn noch zeit vorhanden
                        if (dtFirstStart.AddHours(trialTime) > dtNow)
                        {
                            engineRun = true;
                        }
                    }
                    filestream.Close();


                    // Allgemeine Einstellungen laden
                    filestream = file.OpenFile("Settings/Settings.txt", FileMode.Open);
                    streamReader = new StreamReader(filestream);
                    string stSettings = streamReader.ReadToEnd();
                    filestream.Close();
                    stSettings = stSettings.Trim();


                    // Einstellungen durchlaufen
                    string[] arSettings = Regex.Split(stSettings, ";");
                    for (int i = 0; i < arSettings.Count(); i++)
                    {
                        // Einzelne Einstellung Splitten
                        string[] arSetting = Regex.Split(arSettings[i], "=");

                        // LockScreen
                        if (arSetting[0] == "LockScreen")
                        {
                            setLockScreen = arSetting[1];
                            setLockScreenStart = arSetting[1];
                        }

                        // Swap Image
                        if (arSetting[0] == "LockScreenCount")
                        {
                            setLockScreenCount = Convert.ToInt32(arSetting[1]);
                        }
                    }





                    // Hauptbauteile erstellen
                    // Bild ausgabe erstellen
                    WriteableBitmap wbLockScreen = new WriteableBitmap(0, 0);


                    // Bild erstellen
                    Grid grLockScreen = new Grid
                    {
                        Width = lsWidth,
                        Height = lsHeigth,
                    };





                    // Wenn Sperrbildschirm vorhanden und Sperrbildschirm erstellt wird
                    if (setLockScreen != "" & (file.DirectoryExists("LockScreens/" + setLockScreen) | setLockScreen == "/random/" | setLockScreen == "/randomAll/" ) & engineRun)
                    {
                        // Bei Random // Bei ausgewähltem Lock Screen
                        if (setLockScreen != "/randomAll/")
                        {
                            // Bei Random
                            if (setLockScreen == "/random/")
                            {
                                string[] tempScreens = file.GetDirectoryNames("/LockScreens/");
                                List<string> listTempScreens = new List<string>();
                                for (int i = 0; i < 10; i++)
                                {
                                    for (int i2 = 0; i2 < tempScreens.Count(); i2++)
                                    {
                                        listTempScreens.Add(tempScreens[i2]);
                                    }
                                }
                                setLockScreen = listTempScreens[rand.Next(0, listTempScreens.Count())];
                            }

                            // Bilderlisten laden
                            // stringSquare
                            filestream = file.OpenFile("LockScreens/" + setLockScreen + "/Square/ImagesList.txt", FileMode.Open);
                            streamReader = new StreamReader(filestream);
                            string stringSquare = streamReader.ReadToEnd().Trim();
                            filestream.Close();

                            // stringLandscape
                            filestream = file.OpenFile("/LockScreens/" + setLockScreen + "/Landscape/ImagesList.txt", FileMode.Open);
                            streamReader = new StreamReader(filestream);
                            string stringLandscape = streamReader.ReadToEnd().Trim();
                            filestream.Close();

                            // stringPortrait
                            filestream = file.OpenFile("/LockScreens/" + setLockScreen + "/Portrait/ImagesList.txt", FileMode.Open);
                            streamReader = new StreamReader(filestream);
                            string stringPortrait = streamReader.ReadToEnd().Trim();
                            filestream.Close();

                            // stringBackground
                            filestream = file.OpenFile("/LockScreens/" + setLockScreen + "/Background/ImagesList.txt", FileMode.Open);
                            streamReader = new StreamReader(filestream);
                            string stringBackground = streamReader.ReadToEnd().Trim();
                            filestream.Close();

                            // stringUser
                            filestream = file.OpenFile("/LockScreens/" + setLockScreen + "/User/ImagesList.txt", FileMode.Open);
                            streamReader = new StreamReader(filestream);
                            string stringUser = streamReader.ReadToEnd().Trim();
                            filestream.Close();

                            // Bilderlisten erstellen
                            arSquare = Regex.Split(stringSquare, "~");
                            arLandscape = Regex.Split(stringLandscape, "~");
                            arPortrait = Regex.Split(stringPortrait, "~");
                            arBackground = Regex.Split(stringBackground, "~");
                            arUser = Regex.Split(stringUser, "~");
                            for (int i = 1; i < (arSquare.Count() - 1); i++)
                            {
                                listSquare.Add(setLockScreen + "/" + arSquare[i]);
                                listSquareSelection.Add(setLockScreen + "/" + arSquare[i]);
                            }
                            for (int i = 1; i < (arLandscape.Count() - 1); i++)
                            {
                                listLandscape.Add(setLockScreen + "/" + arLandscape[i]);
                                listLandscapeSelection.Add(setLockScreen + "/" + arLandscape[i]);
                            }
                            for (int i = 1; i < (arPortrait.Count() - 1); i++)
                            {
                                listPortrait.Add(setLockScreen + "/" + arPortrait[i]);
                                listPortraitSelection.Add(setLockScreen + "/" + arPortrait[i]);
                            }
                            for (int i = 1; i < (arBackground.Count() - 1); i++)
                            {
                                listBackground.Add(setLockScreen + "/" + arBackground[i]);
                                listBackgroundSelection.Add(setLockScreen + "/" + arBackground[i]);
                            }
                            for (int i = 1; i < (arUser.Count() - 1); i++)
                            {
                                listUser.Add(setLockScreen + "/" + arUser[i]);
                            }

                            // Einstellungen laden
                            filestream = file.OpenFile("/LockScreens/" + setLockScreen + "/Settings.txt", FileMode.Open);
                            streamReader = new StreamReader(filestream);
                            string strSettings = streamReader.ReadToEnd().Trim();
                            filestream.Close();

                            // Einstellungen splitten
                            arSettings = Regex.Split(strSettings, "&");

                            // Einstellungen durchlaufen
                            for (int i = 0; i < arSettings.Count(); i++)
                            {
                                // Einzelne Einstellungen splitten
                                string[] arSetting = Regex.Split(arSettings[i].Trim(), "=");
                                // Einstellungen anwenden
                                if (arSetting[0] == "backgroundType")
                                {
                                    stringBackgroundType = arSetting[1];
                                }
                                else if (arSetting[0] == "backgroundColor")
                                {
                                    stringBackgroundColor = arSetting[1];
                                }
                                else if (arSetting[0] == "userInformationBackgroundColor")
                                {
                                    stringUserInformationBackgroundColor = arSetting[1];
                                }
                                else if (arSetting[0] == "frameColor")
                                {
                                    stringFrameColor = arSetting[1];
                                }
                                else if (arSetting[0] == "frameSize")
                                {
                                    intFrameSize = Convert.ToInt32(arSetting[1]);
                                }
                                else if (arSetting[0] == "picturesTransparencyMin")
                                {
                                    intPicturesTransparencyMin = Convert.ToInt32(arSetting[1]);
                                }
                                else if (arSetting[0] == "picturesTransparencyMax")
                                {
                                    intPicturesTransparencyMax = Convert.ToInt32(arSetting[1]);
                                }
                                else if (arSetting[0] == "useUserInformation")
                                {
                                    boolUseUserInformation = Convert.ToBoolean(arSetting[1]);
                                }
                                else if (arSetting[0] == "userInformationVerticalAlignment")
                                {
                                    intUserInformationVerticalAlignment = Convert.ToInt32(arSetting[1]);
                                }
                                else if (arSetting[0] == "userInformation1")
                                {
                                    stringUserInformation1 = arSetting[1];
                                }
                                else if (arSetting[0] == "userInformation2")
                                {
                                    stringUserInformation2 = arSetting[1];
                                }
                                else if (arSetting[0] == "userInformation3")
                                {
                                    stringUserInformation3 = arSetting[1];
                                }
                                else if (arSetting[0] == "userInformation4")
                                {
                                    stringUserInformation4 = arSetting[1];
                                }
                                else if (arSetting[0] == "userInformationFontColor")
                                {
                                    stringUserInformationFontColor = arSetting[1];
                                }
                                else if (arSetting[0] == "useInformationBackground")
                                {
                                    boolUseInformationBackground = Convert.ToBoolean(arSetting[1]);
                                }
                                else if (arSetting[0] == "informationBackgroundColor")
                                {
                                    stringInformationBackgroundColor = arSetting[1];
                                }
                                else if (arSetting[0] == "informationBackgroundSize")
                                {
                                    intInformationBackgroundSize = Convert.ToInt32(arSetting[1]);
                                }
                                else if (arSetting[0] == "usePicturesTransparency")
                                {
                                    boolUsePicturesTransparency = Convert.ToBoolean(arSetting[1]);
                                }
                            }
                        }

                        // Bei Random aus allen
                        else
                        {
                            // Lock Screens auslesen
                            string[] tempScreens = file.GetDirectoryNames("/LockScreens/");

                            // Lock Screens durchlaufen
                            for (int i3 = 0; i3 < tempScreens.Count(); i3++)
                            {
                                // Bilderlisten laden
                                // stringSquare
                                filestream = file.OpenFile("LockScreens/" + tempScreens[i3] + "/Square/ImagesList.txt", FileMode.Open);
                                streamReader = new StreamReader(filestream);
                                string stringSquare = streamReader.ReadToEnd().Trim();
                                filestream.Close();

                                // stringLandscape
                                filestream = file.OpenFile("/LockScreens/" + tempScreens[i3] + "/Landscape/ImagesList.txt", FileMode.Open);
                                streamReader = new StreamReader(filestream);
                                string stringLandscape = streamReader.ReadToEnd().Trim();
                                filestream.Close();

                                // stringPortrait
                                filestream = file.OpenFile("/LockScreens/" + tempScreens[i3] + "/Portrait/ImagesList.txt", FileMode.Open);
                                streamReader = new StreamReader(filestream);
                                string stringPortrait = streamReader.ReadToEnd().Trim();
                                filestream.Close();

                                // stringBackground
                                filestream = file.OpenFile("/LockScreens/" + tempScreens[i3] + "/Background/ImagesList.txt", FileMode.Open);
                                streamReader = new StreamReader(filestream);
                                string stringBackground = streamReader.ReadToEnd().Trim();
                                filestream.Close();

                                // stringUser
                                filestream = file.OpenFile("/LockScreens/" + tempScreens[i3] + "/User/ImagesList.txt", FileMode.Open);
                                streamReader = new StreamReader(filestream);
                                string stringUser = streamReader.ReadToEnd().Trim();
                                filestream.Close();

                                // Bilderlisten erstellen
                                arSquare = Regex.Split(stringSquare, "~");
                                arLandscape = Regex.Split(stringLandscape, "~");
                                arPortrait = Regex.Split(stringPortrait, "~");
                                arBackground = Regex.Split(stringBackground, "~");
                                arUser = Regex.Split(stringUser, "~");
                                for (int i = 1; i < (arSquare.Count() - 1); i++)
                                {
                                    listSquare.Add(tempScreens[i3] + "/" + arSquare[i]);
                                    listSquareSelection.Add(tempScreens[i3] + "/" + arSquare[i]);
                                }
                                for (int i = 1; i < (arLandscape.Count() - 1); i++)
                                {
                                    listLandscape.Add(tempScreens[i3] + "/" + arLandscape[i]);
                                    listLandscapeSelection.Add(tempScreens[i3] + "/" + arLandscape[i]);
                                }
                                for (int i = 1; i < (arPortrait.Count() - 1); i++)
                                {
                                    listPortrait.Add(tempScreens[i3] + "/" + arPortrait[i]);
                                    listPortraitSelection.Add(tempScreens[i3] + "/" + arPortrait[i]);
                                }
                                for (int i = 1; i < (arBackground.Count() - 1); i++)
                                {
                                    listBackground.Add(tempScreens[i3] + "/" + arBackground[i]);
                                    listBackgroundSelection.Add(tempScreens[i3] + "/" + arBackground[i]);
                                }
                                for (int i = 1; i < (arUser.Count() - 1); i++)
                                {
                                    listUser.Add(tempScreens[i3] + "/" + arUser[i]);
                                }
                            }

                            // Eine Einstellung auswählen
                            List<string> listTempScreens = new List<string>();
                            for (int i = 0; i < 10; i++)
                            {
                                for (int i2 = 0; i2 < tempScreens.Count(); i2++)
                                {
                                    listTempScreens.Add(tempScreens[i2]);
                                }
                            }
                            setLockScreen = listTempScreens[rand.Next(0, listTempScreens.Count())];

                            // Einstellungen laden
                            filestream = file.OpenFile("/LockScreens/" + setLockScreen + "/Settings.txt", FileMode.Open);
                            streamReader = new StreamReader(filestream);
                            string strSettings = streamReader.ReadToEnd().Trim();
                            filestream.Close();

                            // Einstellungen splitten
                            arSettings = Regex.Split(strSettings, "&");

                            // Einstellungen durchlaufen
                            for (int i = 0; i < arSettings.Count(); i++)
                            {
                                // Einzelne Einstellungen splitten
                                string[] arSetting = Regex.Split(arSettings[i].Trim(), "=");
                                // Einstellungen anwenden
                                if (arSetting[0] == "backgroundType")
                                {
                                    stringBackgroundType = arSetting[1];
                                }
                                else if (arSetting[0] == "backgroundColor")
                                {
                                    stringBackgroundColor = arSetting[1];
                                }
                                else if (arSetting[0] == "userInformationBackgroundColor")
                                {
                                    stringUserInformationBackgroundColor = arSetting[1];
                                }
                                else if (arSetting[0] == "frameColor")
                                {
                                    stringFrameColor = arSetting[1];
                                }
                                else if (arSetting[0] == "frameSize")
                                {
                                    intFrameSize = Convert.ToInt32(arSetting[1]);
                                }
                                else if (arSetting[0] == "picturesTransparencyMin")
                                {
                                    intPicturesTransparencyMin = Convert.ToInt32(arSetting[1]);
                                }
                                else if (arSetting[0] == "picturesTransparencyMax")
                                {
                                    intPicturesTransparencyMax = Convert.ToInt32(arSetting[1]);
                                }
                                else if (arSetting[0] == "useUserInformation")
                                {
                                    boolUseUserInformation = Convert.ToBoolean(arSetting[1]);
                                }
                                else if (arSetting[0] == "userInformationVerticalAlignment")
                                {
                                    intUserInformationVerticalAlignment = Convert.ToInt32(arSetting[1]);
                                }
                                else if (arSetting[0] == "userInformation1")
                                {
                                    stringUserInformation1 = arSetting[1];
                                }
                                else if (arSetting[0] == "userInformation2")
                                {
                                    stringUserInformation2 = arSetting[1];
                                }
                                else if (arSetting[0] == "userInformation3")
                                {
                                    stringUserInformation3 = arSetting[1];
                                }
                                else if (arSetting[0] == "userInformation4")
                                {
                                    stringUserInformation4 = arSetting[1];
                                }
                                else if (arSetting[0] == "userInformationFontColor")
                                {
                                    stringUserInformationFontColor = arSetting[1];
                                }
                                else if (arSetting[0] == "useInformationBackground")
                                {
                                    boolUseInformationBackground = Convert.ToBoolean(arSetting[1]);
                                }
                                else if (arSetting[0] == "informationBackgroundColor")
                                {
                                    stringInformationBackgroundColor = arSetting[1];
                                }
                                else if (arSetting[0] == "informationBackgroundSize")
                                {
                                    intInformationBackgroundSize = Convert.ToInt32(arSetting[1]);
                                }
                                else if (arSetting[0] == "usePicturesTransparency")
                                {
                                    boolUsePicturesTransparency = Convert.ToBoolean(arSetting[1]);
                                }
                            }
                        }





                        // Farbeinstellungen verarbeiten // Hintergrundfarbe
                        string code = stringBackgroundColor;
                        string colorNew = "";
                        if (code == "bc")
                        {
                            Color cBackgroundColor = (Color)Application.Current.Resources["PhoneBackgroundColor"];
                            colorNew = Convert.ToString(cBackgroundColor);
                        }
                        else if (code == "ac")
                        {
                            Color cAccentColor = (Color)Application.Current.Resources["PhoneAccentColor"];
                            colorNew = Convert.ToString(cAccentColor);
                        }
                        else if (code == "fc")
                        {
                            Color cForegroundColor = (Color)Application.Current.Resources["PhoneForegroundColor"];
                            colorNew = Convert.ToString(cForegroundColor);
                        }
                        else
                        {
                            colorNew = code;
                        }
                        byte A = Convert.ToByte(colorNew.Substring(1, 2), 16);
                        byte R = Convert.ToByte(colorNew.Substring(3, 2), 16);
                        byte G = Convert.ToByte(colorNew.Substring(5, 2), 16);
                        byte B = Convert.ToByte(colorNew.Substring(7, 2), 16);
                        SolidColorBrush backgroundColor = new SolidColorBrush(Color.FromArgb(A, R, G, B));



                        // Farbeinstellungen verarbeiten // Rahmenfarbe
                        code = stringFrameColor;
                        colorNew = "";
                        if (code == "bc")
                        {
                            Color cBackgroundColor = (Color)Application.Current.Resources["PhoneBackgroundColor"];
                            colorNew = Convert.ToString(cBackgroundColor);
                        }
                        else if (code == "ac")
                        {
                            Color cAccentColor = (Color)Application.Current.Resources["PhoneAccentColor"];
                            colorNew = Convert.ToString(cAccentColor);
                        }
                        else if (code == "fc")
                        {
                            Color cForegroundColor = (Color)Application.Current.Resources["PhoneForegroundColor"];
                            colorNew = Convert.ToString(cForegroundColor);
                        }
                        else
                        {
                            colorNew = code;
                        }
                        A = Convert.ToByte(colorNew.Substring(1, 2), 16);
                        R = Convert.ToByte(colorNew.Substring(3, 2), 16);
                        G = Convert.ToByte(colorNew.Substring(5, 2), 16);
                        B = Convert.ToByte(colorNew.Substring(7, 2), 16);
                        SolidColorBrush frameColor = new SolidColorBrush(Color.FromArgb(A, R, G, B));


                        // Farbeinstellungen verarbeiten // Benutzerinformationen Hintergrundfarbe
                        code = stringUserInformationBackgroundColor;
                        colorNew = "";
                        if (code == "bc")
                        {
                            Color cBackgroundColor = (Color)Application.Current.Resources["PhoneBackgroundColor"];
                            colorNew = Convert.ToString(cBackgroundColor);
                        }
                        else if (code == "ac")
                        {
                            Color cAccentColor = (Color)Application.Current.Resources["PhoneAccentColor"];
                            colorNew = Convert.ToString(cAccentColor);
                        }
                        else if (code == "fc")
                        {
                            Color cForegroundColor = (Color)Application.Current.Resources["PhoneForegroundColor"];
                            colorNew = Convert.ToString(cForegroundColor);
                        }
                        else
                        {
                            colorNew = code;
                        }
                        A = Convert.ToByte(colorNew.Substring(1, 2), 16);
                        R = Convert.ToByte(colorNew.Substring(3, 2), 16);
                        G = Convert.ToByte(colorNew.Substring(5, 2), 16);
                        B = Convert.ToByte(colorNew.Substring(7, 2), 16);
                        SolidColorBrush userInformationBackgroundColor = new SolidColorBrush(Color.FromArgb(A, R, G, B));


                        // Farbeinstellungen verarbeiten // Benutzerinformationen Schriftfarbe
                        code = stringUserInformationFontColor;
                        colorNew = "";
                        if (code == "bc")
                        {
                            Color cBackgroundColor = (Color)Application.Current.Resources["PhoneBackgroundColor"];
                            colorNew = Convert.ToString(cBackgroundColor);
                        }
                        else if (code == "ac")
                        {
                            Color cAccentColor = (Color)Application.Current.Resources["PhoneAccentColor"];
                            colorNew = Convert.ToString(cAccentColor);
                        }
                        else if (code == "fc")
                        {
                            Color cForegroundColor = (Color)Application.Current.Resources["PhoneForegroundColor"];
                            colorNew = Convert.ToString(cForegroundColor);
                        }
                        else
                        {
                            colorNew = code;
                        }
                        A = Convert.ToByte(colorNew.Substring(1, 2), 16);
                        R = Convert.ToByte(colorNew.Substring(3, 2), 16);
                        G = Convert.ToByte(colorNew.Substring(5, 2), 16);
                        B = Convert.ToByte(colorNew.Substring(7, 2), 16);
                        SolidColorBrush userInformationFontColor = new SolidColorBrush(Color.FromArgb(A, R, G, B));


                        // Information Hintergrund // Hintergrundfarbe
                        code = stringInformationBackgroundColor;
                        colorNew = "";
                        if (code == "bc")
                        {
                            Color cBackgroundColor = (Color)Application.Current.Resources["PhoneBackgroundColor"];
                            colorNew = Convert.ToString(cBackgroundColor);
                        }
                        else if (code == "ac")
                        {
                            Color cAccentColor = (Color)Application.Current.Resources["PhoneAccentColor"];
                            colorNew = Convert.ToString(cAccentColor);
                        }
                        else if (code == "fc")
                        {
                            Color cForegroundColor = (Color)Application.Current.Resources["PhoneForegroundColor"];
                            colorNew = Convert.ToString(cForegroundColor);
                        }
                        else
                        {
                            colorNew = code;
                        }
                        A = Convert.ToByte(colorNew.Substring(1, 2), 16);
                        R = Convert.ToByte(colorNew.Substring(3, 2), 16);
                        G = Convert.ToByte(colorNew.Substring(5, 2), 16);
                        B = Convert.ToByte(colorNew.Substring(7, 2), 16);
                        SolidColorBrush informationBackgroundColor = new SolidColorBrush(Color.FromArgb(A, R, G, B));





                        // Quadrat in der Hintergrundfarbe erstellen
                        Rectangle recLockScreen = new Rectangle
                        {
                            Width = lsWidth,
                            Height = lsHeigth,
                            Fill = backgroundColor,
                        };

                        // Hintergrund in Grid einfügen
                        grLockScreen.Children.Add(recLockScreen);

                        // Hintergundbild erstellen
                        if (stringBackgroundType == "pictures" & listBackground.Count() > 0)
                        {
                            // Temponäre Liste der Hintergundbilder erstellen
                            List<string> tempBackgroundPictures = new List<string>();
                            for (int i = 0; i < 10; i++)
                            {
                                for (int i2 = 0; i2 < listBackground.Count(); i2++)
                                {
                                    tempBackgroundPictures.Add(listBackground[i2]);
                                }
                            }

                            // Zufälliges Hintergundbild erstellen
                            string backgroundPictureName = tempBackgroundPictures[rand.Next(0, tempBackgroundPictures.Count())];

                            // Bild Daten auslesen
                            string[] pictureData = Regex.Split(backgroundPictureName, "/");

                            // Hintergrundbild erstellen
                            Image imgBackground = new Image();

                            // Bild laden
                            byte[] data1;
                            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
                            {
                                using (IsolatedStorageFileStream isfs = isf.OpenFile("/LockScreens/" + pictureData[0] + "/Background/Pictures/" + pictureData[1], FileMode.Open, FileAccess.Read))
                                {
                                    data1 = new byte[isfs.Length];
                                    isfs.Read(data1, 0, data1.Length);
                                    isfs.Close();
                                }
                            }
                            MemoryStream ms = new MemoryStream(data1);
                            BitmapImage bi = new BitmapImage();
                            bi.SetSource(ms);
                            imgBackground.Source = bi;

                            // Hintergrundbild in Grid einfügen
                            grLockScreen.Children.Add(imgBackground);
                        }





                        // Liste der Möglichkeiten erstellen
                        List<bool> listFormat = new List<bool>();
                        List<bool> listFormatSelection = new List<bool>();
                        for (int i = 0; i < 7; i++)
                        {
                            listFormat.Add(false);
                            listFormatSelection.Add(false);
                        }
                        // Wenn Square Formate möglich sind
                        if (listSquare.Count() > 0)
                        {
                            listFormat[0] = true; // ID 0 // Square Small
                            listFormat[1] = true; // ID 1 // Square Medium
                            listFormat[2] = true; // ID 2 // Square Large
                            listFormatSelection[0] = true; // ID 0 // Square Small
                            listFormatSelection[1] = true; // ID 1 // Square Medium
                            listFormatSelection[2] = true; // ID 2 // Square Large
                        }
                        // Wenn Landscape Formate möglich sind
                        if (listLandscape.Count() > 0)
                        {
                            listFormat[3] = true; // ID 3 // Landscape Small
                            listFormat[4] = true; // ID 4 // Landscape Large
                            listFormatSelection[3] = true; // ID 3 // Landscape Small
                            listFormatSelection[4] = true; // ID 4 // Landscape Large
                        }
                        // Wenn Portrait Formate möglich sind
                        if (listPortrait.Count() > 0)
                        {
                            listFormat[5] = true; // ID 5 // Portrait Small
                            listFormat[6] = true; // ID 6 // Portrait Large
                            listFormatSelection[5] = true; // ID 5 // Portrait Small
                            listFormatSelection[6] = true; // ID 6 // Portrait Large
                        }





                        // Wenn Bilder möglich sind
                        if (listSquare.Count() > 0 | listLandscape.Count() > 0 | listPortrait.Count() > 0)
                        {


                            // Liste der Transparenzen erstellen
                            List<double> listTransparency = new List<double>();
                            List<double> listTransparencySelection = new List<double>();
                            // Wenn Transparenzen verwendet werden
                            if (boolUsePicturesTransparency)
                            {
                                // Transparenzen in Liste schreiben
                                for (int i = 0; i <= 10; i++)
                                {
                                    if (i >= intPicturesTransparencyMin & i <= intPicturesTransparencyMax)
                                    {
                                        double dbTempTransparency = 0.0;
                                        for (int i2 = 0; i2 < i; i2++)
                                        {
                                            dbTempTransparency += 0.1;
                                        }
                                        listTransparency.Add(dbTempTransparency);
                                        listTransparencySelection.Add(dbTempTransparency);
                                    }
                                }
                            }
                            // Wenn keine Transparenzen verwendet werden
                            else
                            {
                                listTransparency.Add(1.0);
                                listTransparencySelection.Add(1.0);
                            }


                            // Liste Raster erstellen
                            List<ClassRaster> listRaster = new List<ClassRaster>();
                            // Raster erstellen erstellen
                            for (int i = 0; i < lsVertical; i++)
                            {
                                listRaster.Add(new ClassRaster(i, lsHorizontal));
                            }


                            // Raster durchlaufen // Vertikal
                            for (int iRV = 0; iRV < lsVertical; iRV++)
                            {
                                // Raster durchlaufen // Horizontal
                                for (int iRH = 0; iRH < lsHorizontal; iRH++)
                                {
                                    // Wenn Tile noch frei
                                    if (listRaster[iRV].listHorizontal[iRH])
                                    {
                                        // Liste Verbrauchter Transparenzen überprüfen
                                        if (listTransparencySelection.Count() == 0)
                                        {
                                            // Liste Verbrauchter Transparenzen neu erstellen
                                            for (int i = 0; i < listTransparency.Count(); i++)
                                            {
                                                listTransparencySelection.Add(listTransparency[i]);
                                            }
                                        }
                                        // Temponäre Liste mit IDs der Transparenzen erstellen
                                        List<int> tempListTransparency = new List<int>();
                                        for (int i = 0; i < 10; i++)
                                        {
                                            for (int i2 = 0; i2 < listTransparencySelection.Count(); i2++)
                                            {
                                                tempListTransparency.Add(i2);
                                            }
                                        }
                                        // Transparenz auswählen
                                        int tempId = tempListTransparency[rand.Next(0, tempListTransparency.Count())];
                                        double dbOpacity = listTransparencySelection[tempId];
                                        // Transparenz aus liste löschen
                                        listTransparencySelection.RemoveAt(tempId);


                                        // Liste Verbrauchter Formate überprüfen
                                        bool reCreate = true;
                                        for (int i = 0; i < listFormatSelection.Count(); i++)
                                        {
                                            if (listFormatSelection[i] == true)
                                            {
                                                reCreate = false;
                                                break;
                                            }
                                        }
                                        // Liste neu erstellen, wenn keine verbrauten Formate übrig
                                        if (reCreate)
                                        {
                                            for (int i = 0; i < listFormatSelection.Count(); i++)
                                            {
                                                listFormatSelection[i] = listFormat[i];
                                            }
                                        }


                                        // Temponäre Liste mit den möglich Tiles erstellen
                                        List<int> listPossibleTilesTemp = new List<int>();


                                        // Endlos schleife, bis Tile festgelegt ist
                                        for (int i = 0; i <= 1; i++)
                                        {
                                            // Prüfen ob möglich // Square Small // ID = 0;
                                            if ((i == 0 & listFormatSelection[0]) | (i == 1 & listFormat[0]))
                                            {
                                                listPossibleTilesTemp.Add(0);
                                            }


                                            // Prüfen ob möglich // Square Medium // ID = 1;
                                            if ((i == 0 & listFormatSelection[1]) | (i == 1 & listFormat[1]))
                                            {
                                                if ((iRH + 1) < lsHorizontal)
                                                {
                                                    if ((iRV + 1) < lsVertical)
                                                    {
                                                        if (listRaster[iRV].listHorizontal[(iRH + 1)] & listRaster[(iRV + 1)].listHorizontal[iRH] & listRaster[(iRV + 1)].listHorizontal[(iRH + 1)])
                                                        {
                                                            listPossibleTilesTemp.Add(1);
                                                        }
                                                    }
                                                }
                                            }


                                            // Prüfen ob möglich // Square Big // ID = 2
                                            if ((i == 0 & listFormatSelection[2]) | (i == 1 & listFormat[2]))
                                            {
                                                if ((iRH + 3) < lsHorizontal)
                                                {
                                                    if ((iRV + 3) < lsVertical)
                                                    {
                                                        if (listRaster[iRV].listHorizontal[(iRH + 1)] & listRaster[iRV].listHorizontal[(iRH + 2)] & listRaster[iRV].listHorizontal[(iRH + 3)] & listRaster[(iRV + 1)].listHorizontal[iRH] & listRaster[(iRV + 1)].listHorizontal[(iRH + 1)] & listRaster[(iRV + 1)].listHorizontal[(iRH + 2)] & listRaster[(iRV + 1)].listHorizontal[(iRH + 3)] & listRaster[(iRV + 2)].listHorizontal[iRH] & listRaster[(iRV + 2)].listHorizontal[(iRH + 1)] & listRaster[(iRV + 2)].listHorizontal[(iRH + 2)] & listRaster[(iRV + 2)].listHorizontal[(iRH + 3)] & listRaster[(iRV + 3)].listHorizontal[iRH] & listRaster[(iRV + 3)].listHorizontal[(iRH + 1)] & listRaster[(iRV + 3)].listHorizontal[(iRH + 2)] & listRaster[(iRV + 3)].listHorizontal[(iRH + 3)])
                                                        {
                                                            listPossibleTilesTemp.Add(2);
                                                        }
                                                    }
                                                }
                                            }


                                            // Prüfen ob möglich // Landscape Small // ID = 3;
                                            if ((i == 0 & listFormatSelection[3]) | (i == 1 & listFormat[3]))
                                            {
                                                if ((iRH + 1) < lsHorizontal)
                                                {
                                                    if (listRaster[iRV].listHorizontal[(iRH + 1)])
                                                    {
                                                        listPossibleTilesTemp.Add(3);
                                                    }
                                                }
                                            }


                                            // Prüfen ob möglich // Landscape Big // ID = 4;
                                            if ((i == 0 & listFormatSelection[4]) | (i == 1 & listFormat[4]))
                                            {
                                                if ((iRH + 3) < lsHorizontal)
                                                {
                                                    if ((iRV + 1) < lsVertical)
                                                    {
                                                        if (listRaster[iRV].listHorizontal[(iRH + 1)] & listRaster[iRV].listHorizontal[(iRH + 2)] & listRaster[iRV].listHorizontal[(iRH + 3)] & listRaster[(iRV + 1)].listHorizontal[iRH] & listRaster[(iRV + 1)].listHorizontal[(iRH + 1)] & listRaster[(iRV + 1)].listHorizontal[(iRH + 2)] & listRaster[(iRV + 1)].listHorizontal[(iRH + 3)])
                                                        {
                                                            listPossibleTilesTemp.Add(4);
                                                        }
                                                    }
                                                }
                                            }


                                            // Prüfen ob möglich // Portrait Medium // ID = 5;
                                            if ((i == 0 & listFormatSelection[5]) | (i == 1 & listFormat[5]))
                                            {
                                                if ((iRV + 1) < lsVertical)
                                                {
                                                    if (listRaster[iRV + 1].listHorizontal[iRH])
                                                    {
                                                        listPossibleTilesTemp.Add(5);
                                                    }
                                                }
                                            }


                                            // Prüfen ob möglich // Portrait Big // ID = 6;
                                            if ((i == 0 & listFormatSelection[6]) | (i == 1 & listFormat[6]))
                                            {
                                                if ((iRH + 1) < lsHorizontal)
                                                {
                                                    if ((iRV + 3) < lsVertical)
                                                    {
                                                        if (listRaster[iRV].listHorizontal[(iRH + 1)] & listRaster[(iRV + 1)].listHorizontal[iRH] & listRaster[(iRV + 1)].listHorizontal[(iRH + 1)] & listRaster[(iRV + 2)].listHorizontal[iRH] & listRaster[(iRV + 2)].listHorizontal[(iRH + 1)] & listRaster[(iRV + 3)].listHorizontal[iRH] & listRaster[(iRV + 3)].listHorizontal[(iRH + 1)])
                                                        {
                                                            listPossibleTilesTemp.Add(6);
                                                        }
                                                    }
                                                }
                                            }


                                            // Wenn Formate in der Liste vorhanden
                                            if (listPossibleTilesTemp.Count() > 0)
                                            {
                                                break;
                                            }
                                        }


                                        // Liste möglicher Tiles erstellen und aus temponäre Tiles verhundertfachen
                                        List<int> listPossibleTiles = new List<int>();
                                        for (int i = 0; i < 100; i++)
                                        {
                                            for (int i2 = 0; i2 < listPossibleTilesTemp.Count(); i2++)
                                            {
                                                listPossibleTiles.Add(listPossibleTilesTemp[i2]);
                                            }
                                        }


                                        // Tile größe ermitteln
                                        int tileSizeId = listPossibleTiles[rand.Next(0, listPossibleTiles.Count())];


                                        // Tile Grid erstellen
                                        Grid grTile = new Grid
                                        {
                                            VerticalAlignment = System.Windows.VerticalAlignment.Top,
                                            HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                                            Margin = new Thickness(lsMarginLeft + (iRH * (lsTileSize + lsMarginMiddle)), lsMarginTop + (iRV * (lsTileSize + lsMarginMiddle)), 0, 0),
                                            Opacity = dbOpacity,
                                        };


                                        // Tile Image erstellen
                                        Image tile = new Image();


                                        // Tile erstellen // Square Small // ID = 0
                                        if (tileSizeId == 0)
                                        {
                                            // Wenn Bild vorhanden
                                            if (listSquare.Count() > 0)
                                            {
                                                // Wenn keine Bilder mehr in Auswahlliste
                                                if (listSquareSelection.Count() == 0)
                                                {
                                                    // Auswahlliste neu erstellen
                                                    for (int i = 0; i < listSquare.Count(); i++)
                                                    {
                                                        listSquareSelection.Add(listSquare[i]);
                                                    }
                                                }

                                                // Zufällig bild auswählen, und aus Liste löschen
                                                int listID = rand.Next(0, (listSquareSelection.Count() * 10));
                                                for (int i = 0; i == 0; i = 0)
                                                {
                                                    if ((listID - listSquareSelection.Count()) >= 0)
                                                    {
                                                        listID -= listSquareSelection.Count();
                                                    }
                                                    else
                                                    {
                                                        break;
                                                    }
                                                }

                                                // Bild ermitteln
                                                string pictureName = listSquareSelection[listID];

                                                // Bild aus Auswahlliste löschen
                                                listSquareSelection.RemoveAt(listID);

                                                // Bild Daten auslesen
                                                string[] pictureData = Regex.Split(pictureName, "/");

                                                // Versuchen Bild zu laden
                                                try
                                                {
                                                    //Bilder laden
                                                    byte[] data1;
                                                    using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
                                                    {
                                                        using (IsolatedStorageFileStream isfs = isf.OpenFile("/LockScreens/" + pictureData[0] + "/Square/Small/" + pictureData[1], FileMode.Open, FileAccess.Read))
                                                        {
                                                            data1 = new byte[isfs.Length];
                                                            isfs.Read(data1, 0, data1.Length);
                                                            isfs.Close();
                                                        }
                                                    }
                                                    MemoryStream ms = new MemoryStream(data1);
                                                    BitmapImage bi = new BitmapImage();
                                                    bi.SetSource(ms);
                                                    tile.Source = bi;
                                                }
                                                catch
                                                {
                                                }
                                            }

                                            // Bild größe einstellen
                                            tile.Width = lsTileSize - (intFrameSize * 2);

                                            // Bild Margin erstellen
                                            tile.Margin = new Thickness(intFrameSize, intFrameSize, intFrameSize, intFrameSize);

                                            // Hintergrundquadrat, Größe erstellen
                                            grTile.Width = lsTileSize;
                                            grTile.Height = lsTileSize;

                                            // Tiles aus Raster löschen
                                            listRaster[iRV].listHorizontal[iRH] = false;
                                        }


                                        // Tile erstellen // Square Medium // ID = 1
                                        else if (tileSizeId == 1)
                                        {
                                            // Wenn Bild vorhanden
                                            if (listSquare.Count() > 0)
                                            {
                                                // Wenn keine Bilder mehr in Auswahlliste
                                                if (listSquareSelection.Count() == 0)
                                                {
                                                    // Auswahlliste neu erstellen
                                                    for (int i = 0; i < listSquare.Count(); i++)
                                                    {
                                                        listSquareSelection.Add(listSquare[i]);
                                                    }
                                                }

                                                // Zufällig bild auswählen, und aus Liste löschen
                                                int listID = rand.Next(0, (listSquareSelection.Count() * 10));
                                                for (int i = 0; i == 0; i = 0)
                                                {
                                                    if ((listID - listSquareSelection.Count()) >= 0)
                                                    {
                                                        listID -= listSquareSelection.Count();
                                                    }
                                                    else
                                                    {
                                                        break;
                                                    }
                                                }

                                                // Bild ermitteln
                                                string pictureName = listSquareSelection[listID];

                                                // Bild aus Auswahlliste löschen
                                                listSquareSelection.RemoveAt(listID);

                                                // Bild Daten auslesen
                                                string[] pictureData = Regex.Split(pictureName, "/");

                                                // Versuchen Bild zu laden
                                                try
                                                {
                                                    //Bilder laden
                                                    byte[] data1;
                                                    using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
                                                    {
                                                        using (IsolatedStorageFileStream isfs = isf.OpenFile("/LockScreens/" + pictureData[0] + "/Square/Medium/" + pictureData[1], FileMode.Open, FileAccess.Read))
                                                        {
                                                            data1 = new byte[isfs.Length];
                                                            isfs.Read(data1, 0, data1.Length);
                                                            isfs.Close();
                                                        }
                                                    }
                                                    MemoryStream ms = new MemoryStream(data1);
                                                    BitmapImage bi = new BitmapImage();
                                                    bi.SetSource(ms);
                                                    tile.Source = bi;
                                                }
                                                catch
                                                {
                                                }
                                            }

                                            // Bild größe einstellen
                                            tile.Width = lsTileSize + lsMarginMiddle + lsTileSize - (intFrameSize * 2);

                                            // Bild Margin erstellen
                                            tile.Margin = new Thickness(intFrameSize, intFrameSize, intFrameSize, intFrameSize);

                                            // Hintergrundquadrat, Größe erstellen
                                            grTile.Width = lsTileSize + lsMarginMiddle + lsTileSize;
                                            grTile.Height = lsTileSize + lsMarginMiddle + lsTileSize;

                                            // Tiles aus Raster löschen
                                            listRaster[iRV].listHorizontal[iRH] = false;
                                            listRaster[iRV].listHorizontal[(iRH + 1)] = false;
                                            listRaster[(iRV + 1)].listHorizontal[iRH] = false;
                                            listRaster[(iRV + 1)].listHorizontal[(iRH + 1)] = false;
                                        }


                                        // Tile erstellen // Square Big // ID = 2
                                        else if (tileSizeId == 2)
                                        {
                                            // Wenn Bild vorhanden
                                            if (listSquare.Count() > 0)
                                            {
                                                // Wenn keine Bilder mehr in Auswahlliste
                                                if (listSquareSelection.Count() == 0)
                                                {
                                                    // Auswahlliste neu erstellen
                                                    for (int i = 0; i < listSquare.Count(); i++)
                                                    {
                                                        listSquareSelection.Add(listSquare[i]);
                                                    }
                                                }

                                                // Zufällig bild auswählen, und aus Liste löschen
                                                int listID = rand.Next(0, (listSquareSelection.Count() * 10));
                                                for (int i = 0; i == 0; i = 0)
                                                {
                                                    if ((listID - listSquareSelection.Count()) >= 0)
                                                    {
                                                        listID -= listSquareSelection.Count();
                                                    }
                                                    else
                                                    {
                                                        break;
                                                    }
                                                }

                                                // Bild ermitteln
                                                string pictureName = listSquareSelection[listID];

                                                // Bild aus Auswahlliste löschen
                                                listSquareSelection.RemoveAt(listID);

                                                // Bild Daten auslesen
                                                string[] pictureData = Regex.Split(pictureName, "/");

                                                // Versuchen Bild zu laden
                                                try
                                                {
                                                    //Bilder laden
                                                    byte[] data1;
                                                    using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
                                                    {
                                                        using (IsolatedStorageFileStream isfs = isf.OpenFile("/LockScreens/" + pictureData[0] + "/Square/Large/" + pictureData[1], FileMode.Open, FileAccess.Read))
                                                        {
                                                            data1 = new byte[isfs.Length];
                                                            isfs.Read(data1, 0, data1.Length);
                                                            isfs.Close();
                                                        }
                                                    }
                                                    MemoryStream ms = new MemoryStream(data1);
                                                    BitmapImage bi = new BitmapImage();
                                                    bi.SetSource(ms);
                                                    tile.Source = bi;
                                                }
                                                catch
                                                {
                                                }
                                            }

                                            // Bild größe einstellen
                                            tile.Width = (lsTileSize * 4) + (lsMarginMiddle * 3) - (intFrameSize * 2);

                                            // Bild Margin erstellen
                                            tile.Margin = new Thickness(intFrameSize, intFrameSize, intFrameSize, intFrameSize);

                                            // Hintergrundquadrat, Größe erstellen
                                            grTile.Width = (lsTileSize * 4) + (lsMarginMiddle * 3);
                                            grTile.Height = (lsTileSize * 4) + (lsMarginMiddle * 3);

                                            // Tiles aus Raster löschen
                                            listRaster[iRV].listHorizontal[iRH] = false;
                                            listRaster[iRV].listHorizontal[(iRH + 1)] = false;
                                            listRaster[iRV].listHorizontal[(iRH + 2)] = false;
                                            listRaster[iRV].listHorizontal[(iRH + 3)] = false;
                                            listRaster[(iRV + 1)].listHorizontal[iRH] = false;
                                            listRaster[(iRV + 1)].listHorizontal[(iRH + 1)] = false;
                                            listRaster[(iRV + 1)].listHorizontal[(iRH + 2)] = false;
                                            listRaster[(iRV + 1)].listHorizontal[(iRH + 3)] = false;
                                            listRaster[(iRV + 2)].listHorizontal[iRH] = false;
                                            listRaster[(iRV + 2)].listHorizontal[(iRH + 1)] = false;
                                            listRaster[(iRV + 2)].listHorizontal[(iRH + 2)] = false;
                                            listRaster[(iRV + 2)].listHorizontal[(iRH + 3)] = false;
                                            listRaster[(iRV + 3)].listHorizontal[iRH] = false;
                                            listRaster[(iRV + 3)].listHorizontal[(iRH + 1)] = false;
                                            listRaster[(iRV + 3)].listHorizontal[(iRH + 2)] = false;
                                            listRaster[(iRV + 3)].listHorizontal[(iRH + 3)] = false;
                                        }


                                        // Tile erstellen // Landscape Small // ID = 3
                                        else if (tileSizeId == 3)
                                        {
                                            // Wenn Bild vorhanden
                                            if (listLandscape.Count() > 0)
                                            {
                                                // Wenn keine Bilder mehr in Auswahlliste
                                                if (listLandscapeSelection.Count() == 0)
                                                {
                                                    // Auswahlliste neu erstellen
                                                    for (int i = 0; i < listLandscape.Count(); i++)
                                                    {
                                                        listLandscapeSelection.Add(listLandscape[i]);
                                                    }
                                                }

                                                // Zufällig bild auswählen, und aus Liste löschen
                                                int listID = rand.Next(0, (listLandscapeSelection.Count() * 10));
                                                for (int i = 0; i == 0; i = 0)
                                                {
                                                    if ((listID - listLandscapeSelection.Count()) >= 0)
                                                    {
                                                        listID -= listLandscapeSelection.Count();
                                                    }
                                                    else
                                                    {
                                                        break;
                                                    }
                                                }

                                                // Bild ermitteln
                                                string pictureName = listLandscapeSelection[listID];

                                                // Bild aus Auswahlliste löschen
                                                listLandscapeSelection.RemoveAt(listID);

                                                // Bild Daten auslesen
                                                string[] pictureData = Regex.Split(pictureName, "/");

                                                // Versuchen Bild zu laden
                                                try
                                                {
                                                    //Bilder laden
                                                    byte[] data1;
                                                    using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
                                                    {
                                                        using (IsolatedStorageFileStream isfs = isf.OpenFile("/LockScreens/" + pictureData[0] + "/Landscape/Small/" + pictureData[1], FileMode.Open, FileAccess.Read))
                                                        {
                                                            data1 = new byte[isfs.Length];
                                                            isfs.Read(data1, 0, data1.Length);
                                                            isfs.Close();
                                                        }
                                                    }
                                                    MemoryStream ms = new MemoryStream(data1);
                                                    BitmapImage bi = new BitmapImage();
                                                    bi.SetSource(ms);
                                                    tile.Source = bi;
                                                }
                                                catch
                                                {
                                                }
                                            }

                                            // Bild größe einstellen
                                            tile.Width = (lsTileSize * 2) + lsMarginMiddle - (intFrameSize * 2);

                                            // Bild Margin erstellen
                                            double percent = Convert.ToDouble(100) / Convert.ToDouble((lsTileSize * 2) + lsMarginMiddle) * Convert.ToDouble((lsTileSize * 2) + lsMarginMiddle - (intFrameSize * 2));
                                            double tempHeight = Convert.ToDouble(lsTileSize) / Convert.ToDouble(100) * percent;
                                            double margin = (Convert.ToDouble(lsTileSize) - tempHeight) / 2;
                                            tile.Margin = new Thickness(intFrameSize, margin, intFrameSize, margin);

                                            // Hintergrundquadrat, Größe erstellen
                                            grTile.Width = (lsTileSize * 2) + lsMarginMiddle;
                                            grTile.Height = lsTileSize;

                                            // Tiles aus Raster löschen
                                            listRaster[iRV].listHorizontal[iRH] = false;
                                            listRaster[iRV].listHorizontal[(iRH + 1)] = false;
                                        }


                                        // Tile erstellen // Landscape Big // ID = 4
                                        else if (tileSizeId == 4)
                                        {
                                            // Wenn Bild vorhanden
                                            if (listLandscape.Count() > 0)
                                            {
                                                // Wenn keine Bilder mehr in Auswahlliste
                                                if (listLandscapeSelection.Count() == 0)
                                                {
                                                    // Auswahlliste neu erstellen
                                                    for (int i = 0; i < listLandscape.Count(); i++)
                                                    {
                                                        listLandscapeSelection.Add(listLandscape[i]);
                                                    }
                                                }

                                                // Zufällig bild auswählen, und aus Liste löschen
                                                int listID = rand.Next(0, (listLandscapeSelection.Count() * 10));
                                                for (int i = 0; i == 0; i = 0)
                                                {
                                                    if ((listID - listLandscapeSelection.Count()) >= 0)
                                                    {
                                                        listID -= listLandscapeSelection.Count();
                                                    }
                                                    else
                                                    {
                                                        break;
                                                    }
                                                }

                                                // Bild ermitteln
                                                string pictureName = listLandscapeSelection[listID];

                                                // Bild aus Auswahlliste löschen
                                                listLandscapeSelection.RemoveAt(listID);

                                                // Bild Daten auslesen
                                                string[] pictureData = Regex.Split(pictureName, "/");

                                                // Versuchen Bild zu laden
                                                try
                                                {
                                                    //Bilder laden
                                                    byte[] data1;
                                                    using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
                                                    {
                                                        using (IsolatedStorageFileStream isfs = isf.OpenFile("/LockScreens/" + pictureData[0] + "/Landscape/Large/" + pictureData[1], FileMode.Open, FileAccess.Read))
                                                        {
                                                            data1 = new byte[isfs.Length];
                                                            isfs.Read(data1, 0, data1.Length);
                                                            isfs.Close();
                                                        }
                                                    }
                                                    MemoryStream ms = new MemoryStream(data1);
                                                    BitmapImage bi = new BitmapImage();
                                                    bi.SetSource(ms);
                                                    tile.Source = bi;
                                                }
                                                catch
                                                {
                                                }
                                            }

                                            // Bild größe einstellen
                                            tile.Width = (lsTileSize * 4) + (lsMarginMiddle * 3) - (intFrameSize * 2);

                                            // Bild Margin erstellen
                                            double percent = Convert.ToDouble(100) / Convert.ToDouble((lsTileSize * 4) + (lsMarginMiddle * 3)) * Convert.ToDouble((lsTileSize * 4) + (lsMarginMiddle * 3) - (intFrameSize * 2));
                                            double tempHeight = Convert.ToDouble((lsTileSize * 2) + lsMarginMiddle) / Convert.ToDouble(100) * percent;
                                            double margin = (Convert.ToDouble((lsTileSize * 2) + lsMarginMiddle) - tempHeight) / 2;
                                            tile.Margin = new Thickness(intFrameSize, margin, intFrameSize, margin);

                                            // Hintergrundquadrat, Größe erstellen
                                            grTile.Width = (lsTileSize * 4) + (lsMarginMiddle * 3);
                                            grTile.Height = (lsTileSize * 2) + lsMarginMiddle;

                                            // Tiles aus Raster löschen
                                            listRaster[iRV].listHorizontal[iRH] = false;
                                            listRaster[iRV].listHorizontal[(iRH + 1)] = false;
                                            listRaster[iRV].listHorizontal[(iRH + 2)] = false;
                                            listRaster[iRV].listHorizontal[(iRH + 3)] = false;
                                            listRaster[(iRV + 1)].listHorizontal[iRH] = false;
                                            listRaster[(iRV + 1)].listHorizontal[(iRH + 1)] = false;
                                            listRaster[(iRV + 1)].listHorizontal[(iRH + 2)] = false;
                                            listRaster[(iRV + 1)].listHorizontal[(iRH + 3)] = false;
                                        }


                                        // Tile erstellen // Portrait Small // ID = 5
                                        else if (tileSizeId == 5)
                                        {
                                            // Wenn Bild vorhanden
                                            if (listPortrait.Count() > 0)
                                            {
                                                // Wenn keine Bilder mehr in Auswahlliste
                                                if (listPortraitSelection.Count() == 0)
                                                {
                                                    // Auswahlliste neu erstellen
                                                    for (int i = 0; i < listPortrait.Count(); i++)
                                                    {
                                                        listPortraitSelection.Add(listPortrait[i]);
                                                    }
                                                }

                                                // Zufällig bild auswählen, und aus Liste löschen
                                                int listID = rand.Next(0, (listPortraitSelection.Count() * 10));
                                                for (int i = 0; i == 0; i = 0)
                                                {
                                                    if ((listID - listPortraitSelection.Count()) >= 0)
                                                    {
                                                        listID -= listPortraitSelection.Count();
                                                    }
                                                    else
                                                    {
                                                        break;
                                                    }
                                                }

                                                // Bild ermitteln
                                                string pictureName = listPortraitSelection[listID];

                                                // Bild aus Auswahlliste löschen
                                                listPortraitSelection.RemoveAt(listID);

                                                // Bild Daten auslesen
                                                string[] pictureData = Regex.Split(pictureName, "/");

                                                // Versuchen Bild zu laden
                                                try
                                                {
                                                    //Bilder laden
                                                    byte[] data1;
                                                    using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
                                                    {
                                                        using (IsolatedStorageFileStream isfs = isf.OpenFile("/LockScreens/" + pictureData[0] + "/Portrait/Small/" + pictureData[1], FileMode.Open, FileAccess.Read))
                                                        {
                                                            data1 = new byte[isfs.Length];
                                                            isfs.Read(data1, 0, data1.Length);
                                                            isfs.Close();
                                                        }
                                                    }
                                                    MemoryStream ms = new MemoryStream(data1);
                                                    BitmapImage bi = new BitmapImage();
                                                    bi.SetSource(ms);
                                                    tile.Source = bi;
                                                }
                                                catch
                                                {
                                                }
                                            }

                                            // Bild größe einstellen
                                            tile.Height = (lsTileSize * 2) + lsMarginMiddle - (intFrameSize * 2);

                                            // Bild Margin erstellen
                                            double percent = Convert.ToDouble(100) / Convert.ToDouble((lsTileSize * 2) + lsMarginMiddle) * Convert.ToDouble((lsTileSize * 2) + lsMarginMiddle - (intFrameSize * 2));
                                            double tempWidth = Convert.ToDouble(lsTileSize) / Convert.ToDouble(100) * percent;
                                            double margin = (Convert.ToDouble(lsTileSize) - tempWidth) / 2;
                                            tile.Margin = new Thickness(margin, intFrameSize, margin, intFrameSize);

                                            // Hintergrundquadrat, Größe erstellen
                                            grTile.Width = lsTileSize;
                                            grTile.Height = (lsTileSize * 2) + lsMarginMiddle;

                                            // Tiles aus Raster löschen
                                            listRaster[iRV].listHorizontal[iRH] = false;
                                            listRaster[(iRV + 1)].listHorizontal[iRH] = false;
                                        }


                                        // Tile erstellen // Portrait Big // ID = 6
                                        else if (tileSizeId == 6)
                                        {
                                            // Wenn Bild vorhanden
                                            if (listPortrait.Count() > 0)
                                            {
                                                // Wenn keine Bilder mehr in Auswahlliste
                                                if (listPortraitSelection.Count() == 0)
                                                {
                                                    // Auswahlliste neu erstellen
                                                    for (int i = 0; i < listPortrait.Count(); i++)
                                                    {
                                                        listPortraitSelection.Add(listPortrait[i]);
                                                    }
                                                }

                                                // Zufällig bild auswählen, und aus Liste löschen
                                                int listID = rand.Next(0, (listPortraitSelection.Count() * 10));
                                                for (int i = 0; i == 0; i = 0)
                                                {
                                                    if ((listID - listPortraitSelection.Count()) >= 0)
                                                    {
                                                        listID -= listPortraitSelection.Count();
                                                    }
                                                    else
                                                    {
                                                        break;
                                                    }
                                                }

                                                // Bild ermitteln
                                                string pictureName = listPortraitSelection[listID];

                                                // Bild aus Auswahlliste löschen
                                                listPortraitSelection.RemoveAt(listID);

                                                // Bild Daten auslesen
                                                string[] pictureData = Regex.Split(pictureName, "/");

                                                // Versuchen Bild zu laden
                                                try
                                                {
                                                    //Bilder laden
                                                    byte[] data1;
                                                    using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
                                                    {
                                                        using (IsolatedStorageFileStream isfs = isf.OpenFile("/LockScreens/" + pictureData[0] + "/Portrait/Large/" + pictureData[1], FileMode.Open, FileAccess.Read))
                                                        {
                                                            data1 = new byte[isfs.Length];
                                                            isfs.Read(data1, 0, data1.Length);
                                                            isfs.Close();
                                                        }
                                                    }
                                                    MemoryStream ms = new MemoryStream(data1);
                                                    BitmapImage bi = new BitmapImage();
                                                    bi.SetSource(ms);
                                                    tile.Source = bi;
                                                }
                                                catch
                                                {
                                                }
                                            }

                                            // Bild größe einstellen
                                            tile.Height = (lsTileSize * 4) + (lsMarginMiddle * 3) - (intFrameSize * 2);

                                            // Bild Margin erstellen
                                            double percent = Convert.ToDouble(100) / Convert.ToDouble((lsTileSize * 4) + (lsMarginMiddle * 3)) * Convert.ToDouble((lsTileSize * 4) + (lsMarginMiddle * 3) - (intFrameSize * 2));
                                            double tempWidth = Convert.ToDouble((lsTileSize * 2) + lsMarginMiddle) / Convert.ToDouble(100) * percent;
                                            double margin = (Convert.ToDouble((lsTileSize * 2) + lsMarginMiddle) - tempWidth) / 2;
                                            tile.Margin = new Thickness(margin, intFrameSize, margin, intFrameSize);

                                            // Hintergrundquadrat, Größe erstellen
                                            grTile.Width = (lsTileSize * 2) + lsMarginMiddle;
                                            grTile.Height = (lsTileSize * 4) + (lsMarginMiddle * 3);

                                            // Tiles aus Raster löschen
                                            listRaster[iRV].listHorizontal[iRH] = false;
                                            listRaster[iRV].listHorizontal[(iRH + 1)] = false;
                                            listRaster[(iRV + 1)].listHorizontal[iRH] = false;
                                            listRaster[(iRV + 1)].listHorizontal[(iRH + 1)] = false;
                                            listRaster[(iRV + 2)].listHorizontal[iRH] = false;
                                            listRaster[(iRV + 2)].listHorizontal[(iRH + 1)] = false;
                                            listRaster[(iRV + 3)].listHorizontal[iRH] = false;
                                            listRaster[(iRV + 3)].listHorizontal[(iRH + 1)] = false;
                                        }


                                        // Bild in Grid einfügen
                                        grTile.Children.Add(tile);


                                        // Rahmen erstellen, wenn nötig
                                        if (intFrameSize > 0)
                                        {
                                            // Rahmen in Hintergundfarbe
                                            Rectangle frameLeftBg = new Rectangle
                                            {
                                                Width = intFrameSize,
                                                Height = grTile.Height - (2 * intFrameSize),
                                                Fill = new SolidColorBrush((Color)Application.Current.Resources["PhoneBackgroundColor"]),
                                                Margin = new Thickness(0, intFrameSize, 0, 0),
                                            };
                                            Rectangle frameTopBg = new Rectangle
                                            {
                                                Width = grTile.Width,
                                                Height = intFrameSize,
                                                Fill = new SolidColorBrush((Color)Application.Current.Resources["PhoneBackgroundColor"]),
                                            };
                                            Rectangle frameRightBg = new Rectangle
                                            {
                                                Width = intFrameSize,
                                                Height = grTile.Height - (2 * intFrameSize),
                                                Fill = new SolidColorBrush((Color)Application.Current.Resources["PhoneBackgroundColor"]),
                                                Margin = new Thickness((grTile.Width - intFrameSize), intFrameSize, 0, 0),
                                            };
                                            Rectangle frameBottomBg = new Rectangle
                                            {
                                                Width = grTile.Width,
                                                Height = intFrameSize,
                                                Fill = new SolidColorBrush((Color)Application.Current.Resources["PhoneBackgroundColor"]),
                                                Margin = new Thickness(0, (grTile.Height - intFrameSize), 0, 0),
                                            };

                                            // Rahmen in Grid einfügen
                                            grTile.Children.Add(frameLeftBg);
                                            grTile.Children.Add(frameTopBg);
                                            grTile.Children.Add(frameRightBg);
                                            grTile.Children.Add(frameBottomBg);


                                            // Rahmen in eingestellter Farbe
                                            Rectangle frameLeft = new Rectangle
                                            {
                                                Width = intFrameSize,
                                                Height = grTile.Height - (2 * intFrameSize),
                                                Fill = frameColor,
                                                Margin = new Thickness(0, intFrameSize, 0, 0),
                                            };
                                            Rectangle frameTop = new Rectangle
                                            {
                                                Width = grTile.Width,
                                                Height = intFrameSize,
                                                Fill = frameColor,
                                            };
                                            Rectangle frameRight = new Rectangle
                                            {
                                                Width = intFrameSize,
                                                Height = grTile.Height - (2 * intFrameSize),
                                                Fill = frameColor,
                                                Margin = new Thickness((grTile.Width - intFrameSize), intFrameSize, 0, 0),
                                            };
                                            Rectangle frameBottom = new Rectangle
                                            {
                                                Width = grTile.Width,
                                                Height = intFrameSize,
                                                Fill = frameColor,
                                                Margin = new Thickness(0, (grTile.Height - intFrameSize), 0, 0),
                                            };

                                            // Rahmen in Grid einfügen
                                            grTile.Children.Add(frameLeft);
                                            grTile.Children.Add(frameTop);
                                            grTile.Children.Add(frameRight);
                                            grTile.Children.Add(frameBottom);
                                        }


                                        // Tile in LockScreen einfügen
                                        grLockScreen.Children.Add(grTile);


                                        // Fromat aus Liste verbrauchter Formate löschen
                                        listFormatSelection[tileSizeId] = false;
                                    }
                                }
                            }
                        }





                        // Informationen Hintergrund hinzufügen
                        if (boolUseInformationBackground & intInformationBackgroundSize > 0)
                        {
                            // Größe erstellen
                            int intSize = Convert.ToInt32(Convert.ToDouble(lsHeigth) / Convert.ToDouble(100) * Convert.ToDouble(intInformationBackgroundSize));

                            // Quadrat erstellen
                            Rectangle recInformationBackground = new Rectangle
                            {
                                Fill = informationBackgroundColor,
                                Margin = new Thickness(0, lsHeigth - intSize, 0, 0),
                                Height = intSize,
                                Width = lsWidth,
                            };

                            // Quadrat einfügen
                            grLockScreen.Children.Add(recInformationBackground);
                        }





                        // Benutzerinformationen hinzufügen
                        if (boolUseUserInformation)
                        {
                            // Benutzerbild erstellen
                            BitmapImage bi = new BitmapImage();

                            // Bild erstellen
                            Image imgUserInformation = new Image
                            {
                                Margin = new Thickness(intFrameSize),
                            };

                            // Bild größe Festlegen
                            int intImageSize = 0;

                            bool imgError = false;

                            // Wenn Bild vorhanden
                            if (listUser.Count > 0)
                            {
                                // Benutzerbild ermitteln
                                string strUser = listUser[rand.Next(0, listUser.Count())];

                                // Bild Daten auslesen
                                string[] pictureData = Regex.Split(strUser, "/");

                                // Versuchen Bild zu laden
                                try
                                {
                                    //Bilder laden
                                    byte[] data1;
                                    using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
                                    {
                                        using (IsolatedStorageFileStream isfs = isf.OpenFile("/LockScreens/" + pictureData[0] + "/User/Pictures/" + pictureData[1], FileMode.Open, FileAccess.Read))
                                        {
                                            data1 = new byte[isfs.Length];
                                            isfs.Read(data1, 0, data1.Length);
                                            isfs.Close();
                                        }
                                    }
                                    MemoryStream ms = new MemoryStream(data1);
                                    bi.SetSource(ms);

                                    // Bild größe Festlegen
                                    intImageSize = bi.PixelHeight;

                                    // Image einstellen
                                    imgUserInformation.Source = bi;
                                }
                                // Bei Fehlern
                                catch
                                {
                                    // Angeben das Standard Bild geladen wir
                                    imgError = true;
                                }
                            }
                            // Wenn kein Bild vorhanden
                            else
                            {
                                // Angeben das Standard Bild geladen wir
                                imgError = true;
                            }

                            // Wenn Standardbild geladen wird
                            if (imgError)
                            {
                                // Pfad des Benutzerbildes erstellen // 480X800 // 15:9
                                string pathUserPictures = "/Images/user150.jpg";

                                // Wenn Format 768X1280 // 15:9
                                if (resolution == "768X1280")
                                {
                                    // Benutzerbild einstellen
                                    pathUserPictures = "/Images/user240.jpg";
                                }
                                // Wenn Format 720X1280 // 16:9
                                if (resolution == "720X1280")
                                {
                                    // Benutzerbild einstellen
                                    pathUserPictures = "/Images/user225.jpg";
                                }
                                // Wenn Format 1080X1920 // 16:9
                                if (resolution == "1080X1920")
                                {
                                    // Benutzerbild einstellen
                                    pathUserPictures = "/Images/user338.jpg";
                                }

                                // Versuchen Bild zu laden
                                try
                                {
                                    //Bilder laden
                                    byte[] data1;
                                    using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
                                    {
                                        using (IsolatedStorageFileStream isfs = isf.OpenFile(pathUserPictures, FileMode.Open, FileAccess.Read))
                                        {
                                            data1 = new byte[isfs.Length];
                                            isfs.Read(data1, 0, data1.Length);
                                            isfs.Close();
                                        }
                                    }
                                    MemoryStream ms = new MemoryStream(data1);
                                    bi.SetSource(ms);

                                    // Bild größe Festlegen
                                    intImageSize = bi.PixelHeight;

                                    // Image einstellen
                                    imgUserInformation.Source = bi;
                                }
                                // Bei Fehlern
                                catch
                                {
                                }
                            }

                            // Bild größe einstellen
                            imgUserInformation.Height = intImageSize - (intFrameSize * 2);
                            imgUserInformation.Width = intImageSize - (intFrameSize * 2);

                            // Grid erstellen
                            Grid grUserInformation = new Grid();
                            Rectangle recUserInformation = new Rectangle
                            {
                                Fill = userInformationBackgroundColor,
                                Width = lsWidth,
                                Height = intImageSize + (2 * lsMarginLeft),
                            };

                            // Grid Margin errechnen
                            int tempRest = lsHeigth - (intImageSize + (2 * lsMarginLeft));
                            double dbGridMargin = Convert.ToDouble(tempRest) / Convert.ToDouble(100) * Convert.ToDouble(intUserInformationVerticalAlignment);
                            grUserInformation.Margin = new Thickness(0, dbGridMargin, 0, 0);

                            // Grid für das Bild erstellen
                            Grid grUserInformationImage = new Grid
                            {
                                Margin = new Thickness(lsMarginLeft * 2, lsMarginLeft, 0, lsMarginLeft),
                                Height = intImageSize,
                                Width = intImageSize,
                            };

                            // Bild in Grid hinzufügen
                            grUserInformationImage.Children.Add(imgUserInformation);

                            // Rahmen erstellen, wenn nötig
                            if (intFrameSize > 0)
                            {
                                // Rahmen in Hintergrundfarbe
                                Rectangle frameLeftBg = new Rectangle
                                {
                                    Width = intFrameSize,
                                    Height = intImageSize - (2 * intFrameSize),
                                    Fill = new SolidColorBrush((Color)Application.Current.Resources["PhoneBackgroundColor"]),
                                    Margin = new Thickness(0, intFrameSize, 0, 0),
                                };
                                Rectangle frameTopBg = new Rectangle
                                {
                                    Width = intImageSize,
                                    Height = intFrameSize,
                                    Fill = new SolidColorBrush((Color)Application.Current.Resources["PhoneBackgroundColor"]),
                                };
                                Rectangle frameRightBg = new Rectangle
                                {
                                    Width = intFrameSize,
                                    Height = intImageSize - (2 * intFrameSize),
                                    Fill = new SolidColorBrush((Color)Application.Current.Resources["PhoneBackgroundColor"]),
                                    Margin = new Thickness((intImageSize - intFrameSize), intFrameSize, 0, 0),
                                };
                                Rectangle frameBottomBg = new Rectangle
                                {
                                    Width = intImageSize,
                                    Height = intFrameSize,
                                    Fill = new SolidColorBrush((Color)Application.Current.Resources["PhoneBackgroundColor"]),
                                    Margin = new Thickness(0, (intImageSize - intFrameSize), 0, 0),
                                };

                                // Rahmen in Grid einfügen
                                grUserInformationImage.Children.Add(frameLeftBg);
                                grUserInformationImage.Children.Add(frameTopBg);
                                grUserInformationImage.Children.Add(frameRightBg);
                                grUserInformationImage.Children.Add(frameBottomBg);

                                // Rahmen in eingestellter Farbe
                                Rectangle frameLeft = new Rectangle
                                {
                                    Width = intFrameSize,
                                    Height = intImageSize - (2 * intFrameSize),
                                    Fill = frameColor,
                                    Margin = new Thickness(0, intFrameSize, 0, 0),
                                };
                                Rectangle frameTop = new Rectangle
                                {
                                    Width = intImageSize,
                                    Height = intFrameSize,
                                    Fill = frameColor,
                                };
                                Rectangle frameRight = new Rectangle
                                {
                                    Width = intFrameSize,
                                    Height = intImageSize - (2 * intFrameSize),
                                    Fill = frameColor,
                                    Margin = new Thickness((intImageSize - intFrameSize), intFrameSize, 0, 0),
                                };
                                Rectangle frameBottom = new Rectangle
                                {
                                    Width = intImageSize,
                                    Height = intFrameSize,
                                    Fill = frameColor,
                                    Margin = new Thickness(0, (intImageSize - intFrameSize), 0, 0),
                                };

                                // Rahmen in Grid einfügen
                                grUserInformationImage.Children.Add(frameLeft);
                                grUserInformationImage.Children.Add(frameTop);
                                grUserInformationImage.Children.Add(frameRight);
                                grUserInformationImage.Children.Add(frameBottom);
                            }

                            // Schrift Größe ermitteln
                            int intFontSize = Convert.ToInt32((Convert.ToDouble(intImageSize) / Convert.ToDouble(4)) / Convert.ToDouble(4) * Convert.ToDouble(2));
                            int intImgQuater = Convert.ToInt32(Convert.ToDouble(intImageSize) / Convert.ToDouble(4));
                            int intFontRest = Convert.ToInt32(((Convert.ToDouble(intImageSize) / Convert.ToDouble(4)) - Convert.ToDouble(intFontSize)) / Convert.ToDouble(2));

                            // Textblöcke erstellen
                            TextBlock tbUserInformation1 = new TextBlock
                            {
                                Text = stringUserInformation1,
                                Foreground = userInformationFontColor,
                                FontSize = intFontSize,
                                Margin = new Thickness(intImageSize + (3 * lsMarginLeft), lsMarginLeft + intFontRest, lsMarginLeft, 0),
                            };
                            TextBlock tbUserInformation2 = new TextBlock
                            {
                                Text = stringUserInformation2,
                                Foreground = userInformationFontColor,
                                FontSize = intFontSize,
                                Margin = new Thickness(intImageSize + (3 * lsMarginLeft), lsMarginLeft + intImgQuater + intFontRest, lsMarginLeft, 0),
                            };
                            TextBlock tbUserInformation3 = new TextBlock
                            {
                                Text = stringUserInformation3,
                                Foreground = userInformationFontColor,
                                FontSize = intFontSize,
                                Margin = new Thickness(intImageSize + (3 * lsMarginLeft), lsMarginLeft + (intImgQuater * 2) + intFontRest, lsMarginLeft, 0),
                            };
                            TextBlock tbUserInformation4 = new TextBlock
                            {
                                Text = stringUserInformation4,
                                Foreground = userInformationFontColor,
                                FontSize = intFontSize,
                                Margin = new Thickness(intImageSize + (3 * lsMarginLeft), lsMarginLeft + (intImgQuater * 3) + intFontRest, lsMarginLeft, 0),
                            };

                            // Bauteile zusammensetzen
                            grUserInformation.Children.Add(recUserInformation);
                            grUserInformation.Children.Add(grUserInformationImage);
                            grUserInformation.Children.Add(tbUserInformation1);
                            grUserInformation.Children.Add(tbUserInformation2);
                            grUserInformation.Children.Add(tbUserInformation3);
                            grUserInformation.Children.Add(tbUserInformation4);
                            grLockScreen.Children.Add(grUserInformation);
                        }
                    }





                    // Wenn kein Sperrbildschirm eingestellt, oder Sperrbildschirm nicht vorhanden
                    else
                    {
                        // Demo Bild zu laden
                        try
                        {
                            BitmapImage bi = new BitmapImage();
                            //Bilder laden
                            byte[] data1;
                            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
                            {
                                using (IsolatedStorageFileStream isfs = isf.OpenFile(pathLockScreenDemo, FileMode.Open, FileAccess.Read))
                                {
                                    data1 = new byte[isfs.Length];
                                    isfs.Read(data1, 0, data1.Length);
                                    isfs.Close();
                                }
                            }
                            MemoryStream ms = new MemoryStream(data1);
                            bi.SetSource(ms);


                            //Bilder laden
                            Image imgDemo = new Image();
                            imgDemo.Source = bi;
                            grLockScreen.Children.Add(imgDemo);
                        }
                        catch
                        {
                        }
                    }





                    // Writeable Bitmap aus Grid erstellen
                    wbLockScreen = new WriteableBitmap(grLockScreen, null);
                    // ----------------------------------------------------












                    // Bild speichern und als Sperrbildschirm setzen
                    // ----------------------------------------------------
                    // Prüfen ob Bild vorhanden
                    string savePath;


                    // Herausfinden ob fortlaufende Zahl gerade oder ungerade ist
                    if (setLockScreenCount % 2 == 0)
                    {
                        // Speicher Pfad neu erstellen
                        savePath = "LockScreenPictures/1.jpg";
                    }
                    else
                    {
                        // Speicher Pfad neu erstellen
                        savePath = "LockScreenPictures/2.jpg";
                    }


                    //Prüfen ob Bild vorhanden
                    if (file.FileExists("LockScreenPictures/1.jpg"))
                    {
                        file.DeleteFile("LockScreenPictures/1.jpg");
                    }
                    if (file.FileExists("LockScreenPictures/2.jpg"))
                    {
                        file.DeleteFile("LockScreenPictures/2.jpg");
                    }


                    // Count der erhöten Sperrbildschirme erhöhen
                    setLockScreenCount++;


                    // Einstellungen speichern
                    stSettings = "LockScreen=" + setLockScreenStart + ";" + "LockScreenCount=" + setLockScreenCount;
                    filestream = file.CreateFile("Settings/Settings.txt");
                    streamWriter = new StreamWriter(filestream);
                    streamWriter.WriteLine(stSettings);
                    streamWriter.Flush();
                    filestream.Close();


                    //Datei in Isolated Storage schreiben
                    filestream = file.CreateFile(savePath);
                    wbLockScreen.SaveJpeg(filestream, wbLockScreen.PixelWidth, wbLockScreen.PixelHeight, 0, 100);
                    filestream.Close();


                    // LockScreen setzen
                    var schema = "ms-appdata:///Local/";
                    var uri = new Uri(schema + savePath, UriKind.RelativeOrAbsolute);
                    // Wenn LockscreenApp
                    if (isSetLockScreenApp)
                    {
                        Windows.Phone.System.UserProfile.LockScreen.SetImageUri(uri);
                    }
                    // ----------------------------------------------------





                    // Cycle Tiles updaten
                    // ----------------------------------------------------
                    // Liste der zu erstellenden Tiles laden
                    filestream = file.OpenFile("AllTiles.txt", FileMode.Open);
                    streamReader = new StreamReader(filestream);
                    string stAllTiles = streamReader.ReadToEnd().Trim();
                    string[] arAllTiles = Regex.Split(stAllTiles, "~");


                    // Tiles durchlaufen
                    if (stAllTiles != "")
                    {
                        for (int i10 = 0; i10 < arAllTiles.Count(); i10++)
                        {
                            // Index Datei laden
                            filestream = file.OpenFile("LockScreens/" + arAllTiles[i10] + "/Id.dat", FileMode.Open);
                            streamReader = new StreamReader(filestream);
                            int id = Convert.ToInt32(streamReader.ReadToEnd().Trim());


                            // Prüfen ob Tile existiert
                            ShellTile shellTile = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("Id=" + id));

                            // Wenn Tile existiert
                            if (shellTile != null)
                            {

                                // Ordner des Tiles laden
                                string tileFolder = arAllTiles[i10];


                                // Liste aller Bilder erstellen
                                List<string> tileImages = new List<string>();


                                // Bilderlisten laden
                                filestream = file.OpenFile("/LockScreens/" + tileFolder + "/Square/ImagesList.txt", FileMode.Open);
                                streamReader = new StreamReader(filestream);
                                string stImagesTemp = streamReader.ReadToEnd();
                                string[] arImagesTemp = Regex.Split(stImagesTemp, "~");
                                for (int i = 1; i < arImagesTemp.Count() - 1; i++)
                                {
                                    tileImages.Add("/LockScreens/" + tileFolder + "/Square/Large/" + arImagesTemp[i]);
                                }
                                filestream = file.OpenFile("/LockScreens/" + tileFolder + "/Landscape/ImagesList.txt", FileMode.Open);
                                streamReader = new StreamReader(filestream);
                                stImagesTemp = streamReader.ReadToEnd();
                                arImagesTemp = Regex.Split(stImagesTemp, "~");
                                for (int i = 1; i < arImagesTemp.Count() - 1; i++)
                                {
                                    tileImages.Add("/LockScreens/" + tileFolder + "/Landscape/Large/" + arImagesTemp[i]);
                                }
                                filestream = file.OpenFile("/LockScreens/" + tileFolder + "/Portrait/ImagesList.txt", FileMode.Open);
                                streamReader = new StreamReader(filestream);
                                stImagesTemp = streamReader.ReadToEnd();
                                arImagesTemp = Regex.Split(stImagesTemp, "~");
                                for (int i = 1; i < arImagesTemp.Count() - 1; i++)
                                {
                                    tileImages.Add("/LockScreens/" + tileFolder + "/Portrait/Large/" + arImagesTemp[i]);
                                }


                                // Wenn keine Bilder vorhanden
                                if (tileImages.Count() == 0)
                                {
                                    // Versuchen Hintergrundbilder zu laden
                                    filestream = file.OpenFile("/LockScreens/" + tileFolder + "/Background/ImagesList.txt", FileMode.Open);
                                    streamReader = new StreamReader(filestream);
                                    stImagesTemp = streamReader.ReadToEnd();
                                    arImagesTemp = Regex.Split(stImagesTemp, "~");
                                    for (int i = 1; i < arImagesTemp.Count() - 1; i++)
                                    {
                                        tileImages.Add("/LockScreens/" + tileFolder + "/Background/Pictures/" + arImagesTemp[i]);
                                    }
                                }


                                // Liste der Uri´s erstellen
                                List<Uri> listUri = new List<Uri>();


                                // Wenn Bilder bestehen
                                if (tileImages.Count() > 0)
                                {
                                    // Random erstellen
                                    Random rand = new Random();

                                    // Bilder in Tile laden
                                    for (int i = 0; i < 9; i++)
                                    {
                                        // Wenn noch Bilder vorhanden
                                        if (tileImages.Count() > 0)
                                        {
                                            // Bild durch Random auswählen
                                            int imageIndex = rand.Next(0, tileImages.Count());
                                            // Bild in Shell laden
                                            if (file.FileExists("Shared/ShellContent/" + id + "-" + i + ".jpg"))
                                            {
                                                file.DeleteFile("Shared/ShellContent/" + id + "-" + i + ".jpg");
                                            }
                                            file.CopyFile(tileImages[imageIndex], "Shared/ShellContent/" + id + "-" + i + ".jpg");
                                            listUri.Add(new Uri("isostore:/Shared/ShellContent/" + id + "-" + i + ".jpg", UriKind.Absolute));
                                            // Bild aus Liste löschen
                                            tileImages.RemoveAt(imageIndex);
                                        }
                                        // Wenn keine Bilder mehr vorhanden
                                        else
                                        {
                                            // Restliche Bilder aus Shell löschen
                                            if (file.FileExists("Shared/ShellContent/" + id + "-" + i + ".jpg"))
                                            {
                                                file.DeleteFile("Shared/ShellContent/" + id + "-" + i + ".jpg");
                                            }
                                        }
                                    }
                                }
                                // Wenn keine Bilder bestehen
                                else
                                {
                                    // Standard Bild in Tile laden
                                    listUri.Add(new Uri("Assets/Tiles/FlipCycleTileLarge.png", UriKind.Relative));
                                }


                                // Cycle Tyle erstellen
                                CycleTileData cycleTileData = new CycleTileData()
                                {
                                    Title = "",
                                    Count = null,
                                    SmallBackgroundImage = new Uri("Assets/Tiles/FlipCycleTileSmall.png", UriKind.Relative),
                                    CycleImages = listUri,
                                };

                                // Tile erstellen
                                shellTile.Update(cycleTileData);
                            }
                        }
                    }

                    // ----------------------------------------------------





                });
                // ----------------------------------------------------






            }


            // Task erneut ausführen nach //Nur Debug Modus
#if DEBUG
            ScheduledActionService.LaunchForTest("PeriodicAgent", TimeSpan.FromSeconds(30));
#endif


            // Angeben das Task abgeschlossen
            NotifyComplete();
        }
        // ---------------------------------------------------------------------------------------------------





    }











    // Klasse zum erzeugen des Prüfrasters beim erstellen de Sperrbildschirmes
    // ***************************************************************************************************
    class ClassRaster
    {





        // Variablen
        // ---------------------------------------------------------------------------------------------------
        public int id { get; set; }
        public List<bool> listHorizontal { get; set; }
        // ---------------------------------------------------------------------------------------------------





        // Klasse erzeugen
        // ---------------------------------------------------------------------------------------------------
        public ClassRaster(int id, int tilesHorizontal)
        {
            // id übernehmen
            this.id = id;

            // Horizontale Bools erstellen
            listHorizontal = new List<bool>();
            for (int i = 0; i < tilesHorizontal; i++)
            {
                listHorizontal.Add(true);
            }
        }
        // ---------------------------------------------------------------------------------------------------
    }
}