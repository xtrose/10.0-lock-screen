using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MyApp.Resources;
using System.Threading;
using System.Windows.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO.IsolatedStorage;
using System.IO;
using System.Collections.ObjectModel;
using Microsoft.Phone.Scheduler;
using System.Text.RegularExpressions;
using System.ComponentModel;
using Microsoft.Phone.Tasks;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Media.PhoneExtensions;
using Windows.Phone.System.UserProfile;
using System.Globalization;
using Microsoft.Phone.Marketplace;
using Windows.Graphics.Display;
using MyApp.Resources;





// Namespace
namespace MyApp
{





    // Main Page
    public partial class MainPage : PhoneApplicationPage
    {





        // Variablen und Bauteile
        // ---------------------------------------------------------------------------------------------------
        // Display Auflösung
        public static double resolutionWidth = -1;
        public static double resolutionHeight = -1;
        public static double resolutionScaleFactor = -1;
        public static string resolution = "480X800";


        // Allgemeine Variablen
        public static int imagesCount = -1;
        public static string imagesListSquare = "";
        public static string imagesListLandscape = "";
        public static string imagesListPortrait = "";
        public static string imagesListBackground = "";
        public static string imagesListUser = "";
        public static string[] arImagesSquare;
        public static string[] arImagesLandscape;
        public static string[] arImagesPortrait;
        public static string[] arImagesBackground;
        public static string[] arImagesUser;
        public static int cImagesSquare;
        public static int cImagesLandscape;
        public static int cImagesPortrait;
        public static int cImagesBackground;
        public static int cImagesUser;
        public static string background = "#FF000000";


        // Lock Screen Variablen
        public static string folderName = "";
        public static string format = "";
        public static string backgroundType = "color";
        public static string backgroundColor = "bc";
        public static string frameColor = "ac";
        public static string userInformationBackgroundColor = "ac";
        public static int frameSize = 0;
        public static bool usePicturesTransparency = false;
        public static int picturesTransparencyMin = 10;
        public static int picturesTransparencyMax = 10;
        public static bool useUserInformation = false;
        public static int userInformationVerticalAlignment = 20;
        public static string userInformation1 = "";
        public static string userInformation2 = "";
        public static string userInformation3 = "";
        public static string userInformation4 = "";
        public static string userInformationFontColor = "#FFFFFFFF";
        public static bool useInformationBackground = false;
        public static string informationBackgroundColor = "ac";
        public static int informationBackgroundSize = 40;


        // Variablen zum steuern der Seite
        string pagePart = "start";


        // Ordner die erstellt werden
        string[] foldersToCreate = { "/LockScreens", "/Settings", "/LockScreenPictures", "/Images" };


        // Array mit allen Ordner Namen
        string[] arFolders;


        // Liste der Sperrbildschirme
        ObservableCollection<ClassLockScreens> listLockScreens = new ObservableCollection<ClassLockScreens>();
        ObservableCollection<ClassLockScreens> listLockScreensSettings = new ObservableCollection<ClassLockScreens>();


        // Iso Store Variablen
        IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication();
        IsolatedStorageFileStream filestream;
        StreamReader streamReader;
        StreamWriter streamWriter;


        // Sprachdateien
        public static string cul = "";


        // Ob Lock Screen App
        bool isSetLockScreenApp = false;


        // ScheduledTaskAgent Variabeln
        bool isSetBackgroundTask = false;
        PeriodicTask periodicTask;
        string periodicTaskName = "PeriodicAgent";


        // Lizenzinformationen
        private static LicenseInformation licenseInformation = new LicenseInformation();


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


        // Einstellungen
        public static string setLockScreen = "";
        public static int setLockScreenCount = 0;


        // Pfad zum Demo Bild
        string pathLockScreenDemo = "/Images/LockScreenDemo.480X800.jpg";


        // Gibt an ob die App gerade gestartet wurde
        bool firstLoad = true;


        // Gibt an ob die Gepinnt wurde oder Lock Screen geöffnet wurde
        bool pinUnpin = false;
        int lbSelectedIndex = -1;


        // Count Datei der Sperrbildschirme
        public static int countLockScreens = 0;


        // Liste aller Lock Screens von denen Tiles bestehen
        public static List<string> createdTiles = new List<string>();


        // Gibt die aktuelle Version an
        public static string version = "";


        // Gibt an wie Bilder geladen werden
        public static bool addMultipleImage = true;
        public static bool photoChooserTaskIsRunning = false;
        // ---------------------------------------------------------------------------------------------------





        // Klasse erzeugen
        // ---------------------------------------------------------------------------------------------------
        public MainPage()
        {
            // Update auf Version 1.2.0.0
            if (!file.FileExists("/Version.dat"))
            {
                // Versions Datei erstellen
                filestream = file.CreateFile("/Version.dat");
                streamWriter = new StreamWriter(filestream);
                streamWriter.WriteLine("1.2.0.0");
                streamWriter.Flush();
                filestream.Close();

                // Count Variable erstellen
                if (file.DirectoryExists("/LockScreens"))
                {
                    // Lock Screens laden
                    arFolders = file.GetDirectoryNames("/LockScreens/");
                    for (int i = 0; i < arFolders.Count(); i++)
                    {
                        // Id Datei erstellen
                        if (!file.FileExists("/LockScreens/" + arFolders[i] + "/Id.dat"))
                        {
                            // Count Datei der Sperrbildschirme erstellen
                            filestream = file.CreateFile("/LockScreens/" + arFolders[i] + "/Id.dat");
                            streamWriter = new StreamWriter(filestream);
                            streamWriter.WriteLine(countLockScreens.ToString());
                            streamWriter.Flush();
                            filestream.Close();

                            // Count erhöhen
                            countLockScreens++;
                        }
                    }
                }

                // Count Datei erstellen
                filestream = file.CreateFile("countLockScreens.dat");
                streamWriter = new StreamWriter(filestream);
                streamWriter.WriteLine(countLockScreens.ToString());
                streamWriter.Flush();
                filestream.Close();

                // Version erstellen
                version = "1.2.0.0";
            }


            // Wenn Version bereits besteht
            else
            {
                // Versionsdatei laden
                filestream = file.OpenFile("Version.dat", FileMode.Open);
                streamReader = new StreamReader(filestream);
                version = streamReader.ReadToEnd().Trim();
                filestream.Close();
            }



            // Update auf Version 1.3.0.0
            if (version == "1.2.0.0")
            {
                // Prüfen ob Ordner bereits vorhanden
                if (!file.DirectoryExists("Images"))
                {
                    file.CreateDirectory("Images");
                }
                // Bilder in Storage kopieren
                string[] imagesToCopy = { "LockScreenDemo.480X800.jpg", "LockScreenDemo.720X1280.jpg", "LockScreenDemo.768X1280.jpg", "LockScreenDemo.1080X1920.jpg" };
                for (int i = 0; i < imagesToCopy.Count(); i++)
                {
                    // Altes Bild löschen
                    if (file.FileExists("Images/" + imagesToCopy[i]))
                    {
                        file.DeleteFile("Images/" + imagesToCopy[i]);
                    }
                    // Quell Stream erstellen
                    using (Stream input = Application.GetResourceStream(new Uri("Images/" + imagesToCopy[i], UriKind.Relative)).Stream)
                    {
                        // Ziel Stream erstellen
                        using (filestream = file.CreateFile("Images/" + imagesToCopy[i]))
                        {
                            // Byte Puffer erstellen
                            byte[] readBuffer = new byte[4096];
                            int bytesRead = -1;

                            // Dateien kopieren
                            while ((bytesRead = input.Read(readBuffer, 0, readBuffer.Length)) > 0)
                            {
                                filestream.Write(readBuffer, 0, bytesRead);
                            }
                        }
                    }
                }

                // Version erstellen
                version = "1.3.0.0";
            }



            //Prüfen ob eine Sprachdatei besteht
            if (file.FileExists("Cul.dat"))
            {
                //Spachdatei laden
                filestream = file.OpenFile("Cul.dat", FileMode.Open);
                streamReader = new StreamReader(filestream);
                cul = streamReader.ReadToEnd();
                cul = cul.TrimEnd(new char[] { '\r', '\n' });
                filestream.Close();
                //Sprache einstellen
                CultureInfo newCulture = new CultureInfo(cul);
                Thread.CurrentThread.CurrentUICulture = newCulture;
            }
            // Wenn keine Sprachdatei existiert
            else
            {
                cul = CultureInfo.CurrentCulture.Name;
            }



            // XAML initialisieren
            InitializeComponent();


            // Weiter UI Komponenten anpassen
            btnLanguage.Content = "⚠ " + MyApp.Resources.AppResources.X002_changeLanguage;


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
                if (memoryLimit < 900)
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
            // Wenn Format 720X1280 und 1080X1920 und 1440X2560 // 16:9
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


            // Demo Bild laden
            imgLockScreen.Source = new BitmapImage(new Uri(pathLockScreenDemo, UriKind.Relative));
        }
        // ---------------------------------------------------------------------------------------------------





        // Wird bei jedem Aufruf der Klasse ausgeführt
        // ---------------------------------------------------------------------------------------------------
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            // Wenn Ordner und Dateien noch nicht vorhanden
            if (!file.DirectoryExists("/LockScreens"))
            {
                // Ordner und Dateien erstellen
                createFiles();
            }


            // Dateien laden
            loadFiles();


            // Einstellungen verarbeiten
            if (setLockScreen == "")
            {
                // Lock Screen Button einstellen
                btnLockScreen.Content = AppResources.X002_notSet;
            }
            else
            {
                // Lock Screen Button einstellen
                if (setLockScreen == "/random/")
                {
                    btnLockScreen.Content = AppResources.X003_random;
                }
                else if (setLockScreen == "/randomAll/")
                {
                    btnLockScreen.Content = AppResources.X003_randomAll;
                }
                else
                {
                    btnLockScreen.Content = setLockScreen;
                }
            }


            // Liste der Lock Screens erstellen
            listLockScreens.Clear();
            listLockScreensSettings.Clear();
            listLockScreensSettings.Add(new ClassLockScreens(-2, "♺ " + MyApp.Resources.AppResources.X003_random, false));
            listLockScreensSettings.Add(new ClassLockScreens(-2, "♺ " + MyApp.Resources.AppResources.X003_randomAll, false));
            createdTiles.Clear();
            arFolders = file.GetDirectoryNames("/LockScreens/");
            for (int i = 0; i < arFolders.Count(); i++)
            {
                listLockScreens.Add(new ClassLockScreens(i, arFolders[i], true));
                listLockScreensSettings.Add(new ClassLockScreens(i, arFolders[i], true));
            }


            // Liste der Tiles zusammenstellen und speichern
            string allTiles = "";
            for (int i = 0; i < createdTiles.Count(); i++)
            {
                // Liste der Tiles zusammenschreiben
                allTiles += createdTiles[i];
                if (createdTiles.Count() - 1 != i)
                {
                    allTiles += "~";
                }
            }
            // Datei speichern
            await ClassFileMamagment.loadCreateOverwrite("AllTiles.txt", allTiles, true);


            // Wenn Sperrbildschirme vorhanden
            if (listLockScreens.Count() > 0)
            {
                // Liste der Sperrbildschirme erstellen
                lbLockscreens.ItemsSource = listLockScreens;

                // Liste der Sperrbildschirme, in den Einstellungen erstellen
                lbLockScreensSettings.ItemsSource = listLockScreensSettings;

                // Liste der Sperrbildschirme sichtbar machen
                lbLockscreens.Visibility = System.Windows.Visibility.Visible;

                // Buttons in den Einstellungen Transparenz entfernen
                tbLockScreen.Opacity = 1.0;
                btnLockScreen.Opacity = 1.0;

                // Wenn Sperrbildschirm eingestellt
                if (setLockScreen != "")
                {
                    // Infos unsichtbar machen
                    grNoLockScreens.Visibility = System.Windows.Visibility.Collapsed;
                    grNoLockScreens2.Visibility = System.Windows.Visibility.Collapsed;
                    grNoLockScreens3.Visibility = System.Windows.Visibility.Collapsed;
                }
                // Wenn kein Sperrbildschirm eingestellt
                else
                {
                    // Infos sichtbar machen
                    grNoLockScreens.Visibility = System.Windows.Visibility.Collapsed;
                    grNoLockScreens2.Visibility = System.Windows.Visibility.Visible;
                    grNoLockScreens3.Visibility = System.Windows.Visibility.Visible;
                    tbNoLockScreens3Header.Text = MyApp.Resources.AppResources.X002_noLockScreens5;
                    tbNoLockScreens3Text.Text = MyApp.Resources.AppResources.X002_noLockScreens6 + " →";
                    tbNoLockScreens2Header.Text = MyApp.Resources.AppResources.X002_noLockScreens5;
                    tbNoLockScreens2Text.Text = MyApp.Resources.AppResources.X002_noLockScreens6;
                }
            }


            // Wenn keine Sperrbildschirme vorhanden
            else
            {
                // Infos sichtbar machen
                lbLockscreens.Visibility = System.Windows.Visibility.Collapsed;
                grNoLockScreens.Visibility = System.Windows.Visibility.Visible;
                grNoLockScreens2.Visibility = System.Windows.Visibility.Visible;
                grNoLockScreens3.Visibility = System.Windows.Visibility.Visible;
                tbNoLockScreensHeader.Text = MyApp.Resources.AppResources.X002_noLockScreens1;
                tbNoLockScreensText.Text = MyApp.Resources.AppResources.X002_noLockScreens2;
                tbNoLockScreens3Header.Text = MyApp.Resources.AppResources.X002_noLockScreens1;
                tbNoLockScreens3Text.Text = MyApp.Resources.AppResources.X002_noLockScreens3 + " →";
                tbNoLockScreens2Header.Text = MyApp.Resources.AppResources.X002_noLockScreens1;
                tbNoLockScreens2Text.Text = "← " + MyApp.Resources.AppResources.X002_noLockScreens3;

                // Buttons in den Einstellungen Transparenz hinzufügen
                tbLockScreen.Opacity = 0.5;
                btnLockScreen.Opacity = 0.5;
            }


            // Version überprüfen
            checkVersion();


            // Prüfen ob ScheduledTaskAgent läuft
            periodicTask = ScheduledActionService.Find(periodicTaskName) as PeriodicTask;
            if (periodicTask != null)
            {
                // agentsAreEnabled auf true
                isSetBackgroundTask = true;
            }


            // Prüfen ob momentan Lockscreen App
            if (Windows.Phone.System.UserProfile.LockScreenManager.IsProvidedByCurrentApplication)
            {
                // IsLockscreenApp auf true stellen
                isSetLockScreenApp = true;
            }


            // Wenn beim ersten Start die App keine LockScreen App oder Hintergrund Task nicht aktiv ist
            if (!isSetLockScreenApp & pagePart == "start" | !isSetBackgroundTask)
            {
                // Lockhelper aktivieren
                string filePathOfTheImage = pathLockScreenDemo;
                bool isAppResource = true;
                await lockHelper(filePathOfTheImage, isAppResource);
            }


            // Hintergrund Task einmalig neu aktivieren // Und Lockscreen einmalig neu erstellen
            if (pagePart == "start")
            {
                //Task neu Starten
                startPeriodicAgent();

                //Lockscreen Bild wechseln
                if (isSetLockScreenApp == true)
                {
                    // Lock Screen Bild wechseln
                    createLockScreen();
                }
            }


            // Wenn beide Tasks laufen
            if (periodicTask != null & isSetLockScreenApp == true)
            {
                // Button umstellen
                btnLockScreenChanger.Content = MyApp.Resources.AppResources.X002_yes;
            }


            // Wenn einer der Tasks nicht läuft
            else
            {
                // Button umstellen
                btnLockScreenChanger.Content = MyApp.Resources.AppResources.X002_no;
            }


            // Seitenteil auf Main Page stellen
            pagePart = "mainPage";


            // Wenn App gerade erste gestartet wurde 
            if (firstLoad)
            {
                // Angeben das App bereits gestartet wurde
                firstLoad = false;

                // Wenn ob Vollversion
                if (fullVersion)
                {

                    // Wenn Rate Datei besteht
                    if (file.FileExists("Rate.dat"))
                    {
                        // Rate Datei laden
                        int inRate = Convert.ToInt32(await ClassFileMamagment.loadCreateOverwrite("Rate.dat", "0", false));

                        // Wenn Rate < 5
                        if (inRate < 5)
                        {
                            inRate++;
                            await ClassFileMamagment.loadCreateOverwrite("Rate.dat", inRate.ToString(), true);
                        }

                        // Wenn Rate = 5
                        else if (inRate == 5)
                        {
                            inRate = 100;
                            await ClassFileMamagment.loadCreateOverwrite("Rate.dat", inRate.ToString(), true);
                            // Bewertung öffnen
                            NavigationService.Navigate(new Uri("/Pages/Rate.xaml", UriKind.Relative));
                        }
                    }

                    // Wenn Rate nicht besteht
                    else
                    {
                        // Rate Datei erstellen
                        await ClassFileMamagment.loadCreateOverwrite("Rate.dat", "1", true);
                    }
                }
            }
        }
        // ---------------------------------------------------------------------------------------------------





        // Dateien erstellen
        // ---------------------------------------------------------------------------------------------------
        private async void createFiles()
        {
            // Ordner erstellen
            for (int i = 0; i < foldersToCreate.Count(); i++)
            {
                file.CreateDirectory(foldersToCreate[i]);
            }


            // Voll Version erstellen
            await ClassFileMamagment.loadCreateOverwrite("Settings/FullVersion.txt", "0", true);


            // Zeit des ersten App Starts erstellen
            await ClassFileMamagment.loadCreateOverwrite("Settings/FirstTime.txt", dtNow.Year.ToString() + "~" + dtNow.Month.ToString() + "~" + dtNow.Day.ToString() + "~" + dtNow.Hour.ToString() + "~" + dtNow.Minute.ToString() + "~", true);


            // Bilder Count erstellen
            await ClassFileMamagment.loadCreateOverwrite("Settings/ImageCount.txt", "0", true);


            // Bilder in Storage kopieren
            string[] imagesToCopy = { "LockScreenDemo.480X800.jpg", "LockScreenDemo.720X1280.jpg", "LockScreenDemo.768X1280.jpg", "LockScreenDemo.1080X1920.jpg", "user150.jpg", "user225.jpg", "user240.jpg", "user338.jpg" };
            for (int i = 0; i < imagesToCopy.Count(); i++)
            {
                // Altes Bild löschen
                if (file.FileExists("Images/" + imagesToCopy[i]))
                {
                    file.DeleteFile("Images/" + imagesToCopy[i]);
                }
                // Quell Stream erstellen
                using (Stream input = Application.GetResourceStream(new Uri("Images/" + imagesToCopy[i], UriKind.Relative)).Stream)
                {
                    // Ziel Stream erstellen
                    using (filestream = file.CreateFile("Images/" + imagesToCopy[i]))
                    {
                        // Byte Puffer erstellen
                        byte[] readBuffer = new byte[4096];
                        int bytesRead = -1;

                        // Dateien kopieren
                        while ((bytesRead = input.Read(readBuffer, 0, readBuffer.Length)) > 0)
                        {
                            filestream.Write(readBuffer, 0, bytesRead);
                        }
                    }
                }
            }


            // Einstellungen speichern
            await ClassFileMamagment.saveMainSettings();
        }
        // ---------------------------------------------------------------------------------------------------





        // Dateien laden
        // ---------------------------------------------------------------------------------------------------
        private async void loadFiles()
        {
            // Voll Version laden
            if (await ClassFileMamagment.loadCreateOverwrite("Settings/FullVersion.txt", "", false) == "1")
            {
                fullVersion = true;
            }

            // Lock Screens Count laden
            countLockScreens = Convert.ToInt32(await ClassFileMamagment.loadCreateOverwrite("countLockScreens.dat", "", false));

            // Bilder Count laden
            imagesCount = Convert.ToInt32(await ClassFileMamagment.loadCreateOverwrite("Settings/ImageCount.txt", "", false));


            // Einstellungen laden
            await ClassFileMamagment.loadMainSettings();
        }
        // ---------------------------------------------------------------------------------------------------





        // Version prüfen und App anpassen
        // ---------------------------------------------------------------------------------------------------
        private async void checkVersion()
        {
            // Wenn App gerade gekauft wurde
            if (!licenseInformation.IsTrial() & !fullVersion)
            {
#if DEBUG

#else
                // Settings neu erstellen
                filestream = file.CreateFile("Settings/FullVersion.txt");
                streamWriter = new StreamWriter(filestream);
                streamWriter.WriteLine("1");
                streamWriter.Flush();
                filestream.Close();

                // FullVersion umstellen
                fullVersion = true;

                // Benachrichtigung ausgeben
                MessageBox.Show(MyApp.Resources.AppResources.X002_purchaseNote);
#endif
            }


            // Bei Vollversion // App anpassen
            if (fullVersion)
            {
                // Buttons anpassen
                spSave.Opacity = 1.0;
                spSaveAndShare.Opacity = 1.0;
                spAboutBuy.Visibility = System.Windows.Visibility.Collapsed;
                spOverviewBuy.Visibility = System.Windows.Visibility.Collapsed;
                spTrialVersion.Visibility = System.Windows.Visibility.Collapsed;

                // Angeben das Lock Screen erstellt wird
                engineRun = true;
            }


            // Bei Testversion // App anpassen
            else
            {
                // Zeitstempel erster Start laden
                string stDateTime = await ClassFileMamagment.loadCreateOverwrite("Settings/FirstTime.txt", "", false);
                string[] arDateTime = Regex.Split(stDateTime, "~");
                dtFirstStart = new DateTime(Convert.ToInt32(arDateTime[0]), Convert.ToInt32(arDateTime[1]), Convert.ToInt32(arDateTime[2]), Convert.ToInt32(arDateTime[3]), Convert.ToInt32(arDateTime[4]), 0);

                // Wenn noch zeit vorhanden
                if (dtFirstStart.AddHours(trialTime) > dtNow)
                {
                    // Zeit String errechen
                    string stRest = "";

                    // Zeit errechnen
                    TimeSpan tsRest = dtFirstStart.AddHours(trialTime) - dtNow;

                    // Tage errechenen
                    if (tsRest.Days == 1)
                    {
                        stRest = "1 " + MyApp.Resources.AppResources.X002_day + " | ";
                    }
                    else if (tsRest.Days > 1)
                    {
                        stRest = tsRest.Days + " " + MyApp.Resources.AppResources.X002_days + " | ";
                    }

                    // Stunden errechenen
                    if (tsRest.Hours == 1)
                    {
                        stRest += "1 " + MyApp.Resources.AppResources.X002_hour + " | ";
                    }
                    else if (tsRest.Hours > 1)
                    {
                        stRest += tsRest.Hours + " " + MyApp.Resources.AppResources.X002_hours + " | ";
                    }
                    else
                    {
                        if (stRest.Length > 0)
                        {
                            stRest += "0 " + MyApp.Resources.AppResources.X002_hours + " | ";
                        }
                    }

                    // Minuten errechnen
                    if (tsRest.Minutes == 1)
                    {
                        stRest += "1 " + MyApp.Resources.AppResources.X002_minute;
                    }
                    else if (tsRest.Minutes > 1)
                    {
                        stRest += tsRest.Minutes + " " + MyApp.Resources.AppResources.X002_minutes;
                    }
                    else
                    {
                        stRest += "0 " + MyApp.Resources.AppResources.X002_minute;
                    }

                    // In Textbox schreiben
                    tbTrial.Text = stRest;

                    // Angeben das Lock Scren erstellt wird
                    engineRun = true;
                }

                // Wenn keine Zeit mehr vorhanden
                else
                {
                    // Angeben das Testzeit abgelaufen
                    tbTrial.Text = MyApp.Resources.AppResources.X002_trialExpired;

                    // Angeben das kein Lock Screen erstellt wird
                    engineRun = false;
                }

                // Buttons anpassen
                spSave.Opacity = 0.4;
                spSaveAndShare.Opacity = 0.4;
                spAboutBuy.Visibility = System.Windows.Visibility.Visible;
                spOverviewBuy.Visibility = System.Windows.Visibility.Visible;
            }
        }
        // ---------------------------------------------------------------------------------------------------





        // Button // Neuen Lock Screen erstellen
        // ---------------------------------------------------------------------------------------------------
        private void create_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Ordner Name löschen
            folderName = "";


            // Editor öffnen und neuen Sperrbildschirm erstellen
            NavigationService.Navigate(new Uri("/Pages/LockScreen.xaml", UriKind.Relative));
        }
        // ---------------------------------------------------------------------------------------------------





        // Lock Screen aus Liste wählen
        // ---------------------------------------------------------------------------------------------------
        // Variablen
        bool lbLockscreensSelect = true;


        // Auswahl
        private void lbLockscreens_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Wenn Auswahl angewendet wird
            if (lbLockscreensSelect)
            {
                // Wenn nicht gepinnt wurde
                if (!pinUnpin)
                {
                    // Ordner Name aus Liste wählen
                    folderName = arFolders[lbLockscreens.SelectedIndex];


                    // Auswahl zurücksetzen
                    lbLockscreensSelect = false;
                    try
                    {
                        lbLockscreens.SelectedIndex = -1;
                    }
                    catch
                    {
                    }
                    lbLockscreensSelect = true;


                    // Editor öffnen und Sperrbildschirm laden
                    NavigationService.Navigate(new Uri("/Pages/LockScreen.xaml", UriKind.Relative));
                }
                // Wenn gepinnt wurde
                else
                {
                    // pinUnpin Variable zurücksetzen
                    pinUnpin = false;

                    // Auswahl zurücksetzen
                    lbLockscreensSelect = false;
                    try
                    {
                        lbLockscreens.SelectedIndex = -1;
                    }
                    catch
                    {
                    }
                    lbLockscreensSelect = true;
                }
            }
        }
        // ---------------------------------------------------------------------------------------------------





        // Buttons einstellungen
        // ---------------------------------------------------------------------------------------------------
        // Sperrbildschirm Wechsler // Aktivieren // Deaktivieren
        private async void btnLockScreenChanger_Click(object sender, RoutedEventArgs e)
        {
            // Wenn nicht Lock Screen app
            if (!isSetLockScreenApp)
            {
                // Lockhelper aktivieren
                string filePathOfTheImage = pathLockScreenDemo;
                bool isAppResource = true;
                await lockHelper(filePathOfTheImage, isAppResource);
            }


            // Wenn ScheduledAgent nicht aktiv
            else if (!isSetBackgroundTask)
            {
                // Hintergrund Task aktivieren
                startPeriodicAgent();
            }


            // Wenn ScheduledAgent aktiv
            else if (isSetBackgroundTask)
            {
                // Hintergrund Task deaktivieren
                removeAgent(periodicTaskName);

                // Benachrichtigung ausgeben
                MessageBox.Show("⚠ " + MyApp.Resources.AppResources.X002_lockScreenOff);
            }


            // Wenn beide Tasks laufen
            if (isSetBackgroundTask & isSetLockScreenApp)
            {
                // Button umstellen
                btnLockScreenChanger.Content = MyApp.Resources.AppResources.X002_yes;
            }


            // Wenn einer der Tasks nicht läuft
            else
            {
                // Button umstellen
                btnLockScreenChanger.Content = MyApp.Resources.AppResources.X002_no;
            }
        }



        // Sperrbildschirm auswählen
        private void btnLockScreen_Click(object sender, RoutedEventArgs e)
        {
            // Wenn Sperrbildschirme vorhanden
            if (listLockScreens.Count() > 0)
            {
                // Auswahl der Sperrbildschirme sichtbar machen
                grLockScreensSettings.Visibility = System.Windows.Visibility.Visible;

                // Variable zum Steuern der Seite ändern
                pagePart = "settingsSelectLockScreen";
            }
        }



        // Sperrbildschirm aus Liste auswählen
        private async void lbLockScreensSettings_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Wenn Auswahl nicht -1
            if (lbLockScreensSettings.SelectedIndex != -1)
            {
                // Versuchen Sperrbildschirm zu erstellen
                try
                {
                    // Sperrbildschirm erstellen
                    WriteableBitmap wbLockScreen = new WriteableBitmap(0, 0);
                    // Bei Zufällig
                    if (lbLockScreensSettings.SelectedIndex == 0)
                    {
                        wbLockScreen = await ClassCreateLockScreen.createLockScreen("/random/");
                    }
                    // Bei Zufällig aus allen
                    else if (lbLockScreensSettings.SelectedIndex == 1)
                    {
                        wbLockScreen = await ClassCreateLockScreen.createLockScreen("/randomAll/");
                    }
                    else
                    {
                        wbLockScreen = await ClassCreateLockScreen.createLockScreen(arFolders[lbLockScreensSettings.SelectedIndex - 2]);
                    }

                    // Bild als Sperrbildschirm sezten
                    saveAndSetLockScreen(wbLockScreen);

                    // Sperrbildschirm in Übersicht stellen
                    imgLockScreen.Source = wbLockScreen;

                    // Sperrbildschirm, Einstellungen setzen // Sperrbildschirm in Button schreiben
                    if (lbLockScreensSettings.SelectedIndex == 0)
                    {
                        setLockScreen = "/random/";
                        btnLockScreen.Content = AppResources.X003_random;
                    }
                    else if (lbLockScreensSettings.SelectedIndex == 1)
                    {
                        setLockScreen = "/randomAll/";
                        btnLockScreen.Content = AppResources.X003_randomAll;
                    }
                    else
                    {
                        setLockScreen = arFolders[lbLockScreensSettings.SelectedIndex - 2];
                        btnLockScreen.Content = setLockScreen;
                    }

                    // Einstellungen speichern
                    await ClassFileMamagment.saveMainSettings();

                    // Infos unsichtbar machen
                    grNoLockScreens.Visibility = System.Windows.Visibility.Collapsed;
                    grNoLockScreens2.Visibility = System.Windows.Visibility.Collapsed;
                    grNoLockScreens3.Visibility = System.Windows.Visibility.Collapsed;
                }
                catch
                {
                    MessageBox.Show("⚠ " + MyApp.Resources.AppResources.X002_noPicturesAvailable);
                }

                // Auswahl aufheben
                try
                {
                    lbLockScreensSettings.SelectedIndex = -1;
                }
                catch
                {
                }

                // Auswahl unsichtbar machen
                grLockScreensSettings.Visibility = System.Windows.Visibility.Collapsed;

                // Variable zum Steuern der Seite ändern
                pagePart = "start";
            }
        }



        // Sprache ändern
        private void btnLanguage_Click(object sender, RoutedEventArgs e)
        {
            // Spache wechseln auswählen
            NavigationService.Navigate(new Uri("/Pages/Language.xaml", UriKind.Relative));
        }
        // ---------------------------------------------------------------------------------------------------





        // Buttons Übersicht
        // ---------------------------------------------------------------------------------------------------
        // Sperrbildschirm aktuallisieren
        private void btnRefresh_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Versuchen Sperrbildchirm zu aktuallisieren
            createLockScreen();
        }
        


        // Speichern
        private void btnSave_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Wenn Vollversion
            if (fullVersion)
            {
                // Transparenz entfernen
                spSave.Opacity = 1.0;

                // Abfragen ob gespeichert werden soll
                if (MessageBox.Show("", MyApp.Resources.AppResources.X002_save, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    // Bild aus Demo Bild erstellen
                    var wbSaveAndShare = new WriteableBitmap(this.imgLockScreen, null);

                    // Bild speichern
                    using (var memoryStream = new MemoryStream(wbSaveAndShare.PixelWidth * wbSaveAndShare.PixelHeight * 4))
                    {
                        // Bild speichern
                        wbSaveAndShare.SaveJpeg(memoryStream, wbSaveAndShare.PixelWidth, wbSaveAndShare.PixelHeight, 0, 100);
                        memoryStream.Seek(0, SeekOrigin.Begin);
                        MediaLibrary mediaLibrary = new MediaLibrary();
                        Picture savePicture = mediaLibrary.SavePicture(string.Format("LockScreen10.jpg"), memoryStream);
                    }

                    // Benachrichtigung ausgeben
                    MessageBox.Show(MyApp.Resources.AppResources.X002_saved);
                }
            }


            // Bei Test Version
            else
            {
                // Transparenz entfernen
                spSave.Opacity = 0.6;

                // Benachrichtigung ausgeben
                MessageBox.Show("⚠ " + MyApp.Resources.AppResources.X002_onlyFullVersion);
            }
        }
        private void btnSave_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            // Button Transparenz erstellen
            spSave.Opacity = 0.5;
        }
        private void btnSave_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Button Transparenz erstellen
            spSave.Opacity = 0.5;
        }
        private void btnSave_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            // Button Transparenz entfernen
            if (fullVersion)
            {
                spSave.Opacity = 1.0;
            }
            else
            {
                // Transparenz entfernen
                spSave.Opacity = 0.6;
            }
        }





        // Speichern und Teilen
        private void btnSaveAndShare_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Wenn Vollversion
            if (fullVersion)
            {
                // Transparenz entfernen
                spSaveAndShare.Opacity = 1.0;

                // Abfragen ob gespeichert werden soll
                if (MessageBox.Show("", MyApp.Resources.AppResources.X002_saveAndShare, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    // Bild aus Demo Bild erstellen
                    var wbSaveAndShare = new WriteableBitmap(this.imgLockScreen, null);

                    // Bild speichern und teilen
                    using (var memoryStream = new MemoryStream(wbSaveAndShare.PixelWidth * wbSaveAndShare.PixelHeight * 4))
                    {
                        // Bild speichern
                        wbSaveAndShare.SaveJpeg(memoryStream, wbSaveAndShare.PixelWidth, wbSaveAndShare.PixelHeight, 0, 100);
                        memoryStream.Seek(0, SeekOrigin.Begin);
                        MediaLibrary mediaLibrary = new MediaLibrary();
                        Picture savePicture = mediaLibrary.SavePicture(string.Format("LockScreen10.jpg"), memoryStream);
                        // Bild teilen
                        ShareMediaTask shareMediaTask = new ShareMediaTask();
                        shareMediaTask.FilePath = savePicture.GetPath();
                        shareMediaTask.Show();
                    }
                }
            }


            // Bei Testversion
            else
            {
                // Transparenz entfernen
                spSaveAndShare.Opacity = 0.6;

                // Benachrichtigung ausgeben
                MessageBox.Show("⚠ " + MyApp.Resources.AppResources.X002_onlyFullVersion);
            }
        }
        private void btnSaveAndShare_Enter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            // Button Transparenz erstellen
            spSaveAndShare.Opacity = 0.5;
        }
        private void btnSaveAndShare_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Button Transparenz erstellen
            spSaveAndShare.Opacity = 0.5;
        }
        private void btnSaveAndShare_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            // Button Transparenz entfernen
            if (fullVersion)
            {
                spSaveAndShare.Opacity = 1.0;
            }
            else
            {
                // Transparenz entfernen
                spSaveAndShare.Opacity = 0.6;
            }
        }
        // ---------------------------------------------------------------------------------------------------





        // Sperrbildschirm erstellen
        // ---------------------------------------------------------------------------------------------------
        public async void createLockScreen()
        {
            // Ob Demo Bild geladen wird
            bool loadDemo = true;


            // Einstellung prüfen
            if (listLockScreens.Count() > 0 & setLockScreen != "")
            {
                try
                {
                    // Sperrbildschirm erstellen
                    WriteableBitmap wbLockScreen = new WriteableBitmap(0, 0);
                    wbLockScreen = await ClassCreateLockScreen.createLockScreen(setLockScreen);

                    // Wenn Vollversion oder Testzeit
                    if (engineRun)
                    {
                        // Bild als Sperrbildschirm sezten
                        saveAndSetLockScreen(wbLockScreen);
                    }

                    // Wenn Testzeit abgelaufen
                    else
                    {
                        // Erstellten Sperrbildschirm in Demobild laden
                        imgLockScreen.Source = wbLockScreen;

                        // Demo laden
                        string filePathOfTheImage = pathLockScreenDemo;
                        bool isAppResource = true;
                        await lockHelper(filePathOfTheImage, isAppResource);
                    }

                    // Angeben das kein Demo geladen wird
                    loadDemo = false;
                }
                catch
                {
                }
            }


            // Wenn Demo geladen wird
            if (loadDemo)
            {
                // Demo laden
                string filePathOfTheImage = pathLockScreenDemo;
                bool isAppResource = true;
                await lockHelper(filePathOfTheImage, isAppResource);

                // Bild in Übersicht laden
                imgLockScreen.Source = new BitmapImage(new Uri(pathLockScreenDemo, UriKind.Relative));
            }

        }
        // ---------------------------------------------------------------------------------------------------





        // Sperrbildschirm speichern und setzen
        // ---------------------------------------------------------------------------------------------------
        private async void saveAndSetLockScreen(WriteableBitmap wbLockScreen)
        {
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
            await ClassFileMamagment.saveMainSettings();


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


            // Lock Screen in Demo Bild
            byte[] data1;
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream isfs = isf.OpenFile(savePath, FileMode.Open, FileAccess.Read))
                {
                    data1 = new byte[isfs.Length];
                    isfs.Read(data1, 0, data1.Length);
                    isfs.Close();
                }
            }
            MemoryStream ms = new MemoryStream(data1);
            BitmapImage bi = new BitmapImage();
            bi.SetSource(ms);
            imgLockScreen.Source = bi;
        }
        // ---------------------------------------------------------------------------------------------------





        // Lock Screen // Cycle Tile Pinnen // Unpinnen
        //--------------------------------------------------------------------------------------------------------
        private void Image_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Index ermitteln
            Image imgTemp = (Image)sender;
            ClassLockScreens i = (sender as Image).DataContext as ClassLockScreens;
            lbSelectedIndex = i.index;

            // Angeben das Gepinnt wird
            pinUnpin = true;

            // Cycle Tyle erstellen
            createCycleTyle();
        }
        //--------------------------------------------------------------------------------------------------------





        // Cycle Tyle erstellen
        //--------------------------------------------------------------------------------------------------------
        private async void createCycleTyle()
        {
            // Id des Tile laden
            int id = listLockScreens[lbSelectedIndex].id;


            // Wenn Tile nicht existiert
            if (listLockScreens[lbSelectedIndex].shellTile == null)
            {

                // Ordner des Tiles laden
                string tileFolder = listLockScreens[lbSelectedIndex].name;


                // Liste aller Bilder erstellen
                List<string> tileImages = new List<string>();


                // Bilderlisten laden
                string stImagesTemp = await ClassFileMamagment.loadCreateOverwrite("/LockScreens/" + tileFolder + "/Square/ImagesList.txt", "", false);
                string[] arImagesTemp = Regex.Split(stImagesTemp, "~");
                for (int i = 1; i < arImagesTemp.Count() - 1; i++)
                {
                    tileImages.Add("/LockScreens/" + tileFolder + "/Square/Large/" + arImagesTemp[i]);
                }
                stImagesTemp = await ClassFileMamagment.loadCreateOverwrite("/LockScreens/" + tileFolder + "/Landscape/ImagesList.txt", "", false);
                arImagesTemp = Regex.Split(stImagesTemp, "~");
                for (int i = 1; i < arImagesTemp.Count() - 1; i++)
                {
                    tileImages.Add("/LockScreens/" + tileFolder + "/Landscape/Large/" + arImagesTemp[i]);
                }
                stImagesTemp = await ClassFileMamagment.loadCreateOverwrite("/LockScreens/" + tileFolder + "/Portrait/ImagesList.txt", "", false);
                arImagesTemp = Regex.Split(stImagesTemp, "~");
                for (int i = 1; i < arImagesTemp.Count() - 1; i++)
                {
                    tileImages.Add("/LockScreens/" + tileFolder + "/Portrait/Large/" + arImagesTemp[i]);
                }


                // Wenn keine Bilder vorhanden
                if (tileImages.Count() == 0)
                {
                    // Versuchen Hintergrundbilder zu laden
                    stImagesTemp = await ClassFileMamagment.loadCreateOverwrite("/LockScreens/" + tileFolder + "/Background/ImagesList.txt", "", false);
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


                // Tile zur Liste der Tiles hinzufügen
                createdTiles.Add(tileFolder);
                // Liste der Tiles zusammenstellen und speichern
                string allTiles = "";
                for (int i = 0; i < createdTiles.Count(); i++)
                {
                    // Liste der Tiles zusammenschreiben
                    allTiles += createdTiles[i];
                    if (createdTiles.Count() - 1 != i)
                    {
                        allTiles += "~";
                    }
                    // Datei speichern
                    await ClassFileMamagment.loadCreateOverwrite("AllTiles.txt", allTiles, true);
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
                ShellTile.Create(new Uri("/MainPage.xaml?Id=" + id.ToString(), UriKind.Relative), cycleTileData, true);
            }


            // Wenn Tile bereits existiert
            else
            {
                // Tile entfernen
                listLockScreens[lbSelectedIndex].shellTile.Delete();

                // Bilder aus Shell löschen
                for (int i = 0; i < 9; i++)
                {
                    // Bilder löschen
                    if (file.FileExists("Shared/ShellContent/" + id + "-" + i + ".jpg"))
                    {
                        file.DeleteFile("Shared/ShellContent/" + id + "-" + i + ".jpg");
                    }
                }

                // Liste der Lock Screens Leeren
                listLockScreens.Clear();
                createdTiles.Clear();

                // Liste der Lock Screens neu erstellen
                listLockScreens.Clear();
                arFolders = file.GetDirectoryNames("/LockScreens/");
                for (int i = 0; i < arFolders.Count(); i++)
                {
                    listLockScreens.Add(new ClassLockScreens(i, arFolders[i], true));
                }

                // Liste der Lock Screens neu hinzufügen
                lbLockscreens.ItemsSource = listLockScreens;

                // Liste der Tiles zusammenstellen und speichern
                string allTiles = "";
                for (int i = 0; i < createdTiles.Count(); i++)
                {
                    // Liste der Tiles zusammenschreiben
                    allTiles += createdTiles[i];
                    if (createdTiles.Count() - 1 != i)
                    {
                        allTiles += "~";
                    }
                }
                // Datei speichern
                await ClassFileMamagment.loadCreateOverwrite("AllTiles.txt", allTiles, true);
            }
        }
        //--------------------------------------------------------------------------------------------------------






        // Back Button
        //--------------------------------------------------------------------------------------------------------
        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            // Wenn Schnitt Panel offen
            if (pagePart == "settingsSelectLockScreen")
            {
                // Navigation zurück abbrechen
                e.Cancel = true;


                // Auswahl ausblenden
                grLockScreensSettings.Visibility = System.Windows.Visibility.Collapsed;


                // Seitenteil anzeigen
                pagePart = "mainPage";
            }
        }
        //--------------------------------------------------------------------------------------------------------





        // Buttons About
        //--------------------------------------------------------------------------------------------------------
        // Button Buy
        private void btnAboutBuy_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Kauf öffnen
            MarketplaceDetailTask marketPlaceDetailTask = new MarketplaceDetailTask();
            marketPlaceDetailTask.Show();
        }



        // Button Rate
        private void btnAboutRate_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Editor öffnen und neuen Sperrbildschirm erstellen
            NavigationService.Navigate(new Uri("/Pages/Rate.xaml", UriKind.Relative));
        }



        // Button Contact
        private void btnAboutContact_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Abfragen Support kontaktiert werden soll
            if (MessageBox.Show("", MyApp.Resources.AppResources.X002_contactMsg, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                // Kontakt E-Mail erstellen
                EmailComposeTask emailcomposer = new EmailComposeTask();
                emailcomposer.To = "xtrose@hotmail.com";
                emailcomposer.Subject = "10.0 Lock Screen Support";
                emailcomposer.Body = "";
                emailcomposer.Show();
            }
        }



        // Button xtrose
        private void btnAboutXtrose_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Link zu xtrose
            var wb = new WebBrowserTask();
            wb.URL = "http://www.xtrose.com";
            wb.Show();
        }



        // Button Facebook
        private void btnAboutFacebook_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Link zu Facebook
            var wb = new WebBrowserTask();
            wb.URL = "https://www.facebook.com/xtrose.xtrose";
            wb.Show();
        }



        // Button VK
        private void btnAboutVK_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Link zu VK
            var wb = new WebBrowserTask();
            wb.URL = "http://vk.com/public54083459";
            wb.Show();
        }



        // Button You Tube
        private void btnAboutYouTube_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Link zu You Tube
            var wb = new WebBrowserTask();
            wb.URL = "https://www.youtube.com/user/xtrose2overdose";
            wb.Show();
        }



        // Button Twitter
        private void btnAboutTwitter_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Link zu Twitter
            var wb = new WebBrowserTask();
            wb.URL = "https://twitter.com/xtrose";
            wb.Show();
        }
        //--------------------------------------------------------------------------------------------------------










        // ScheduledTaskAgent
        // ---------------------------------------------------------------------------------------------------
        // ScheduledTaskAgent Starten
        private void startPeriodicAgent()
        {
            // ScheduledTask suchen
            periodicTask = ScheduledActionService.Find(periodicTaskName) as PeriodicTask;


            // Wenn Task aktiv ist
            if (periodicTask != null)
            {
                // Task stoppen
                removeAgent(periodicTaskName);
            }


            // Neuen Task erstellen
            periodicTask = new PeriodicTask(periodicTaskName);


            // Beschreibung des Tasks, wird in der Hintergundaufgaben bei den Einstellungen angezeigt
            periodicTask.Description = MyApp.Resources.AppResources.X002_taskInfo;


            // Versuchen Task zu starten
            try
            {
                // Task hinzufügen
                ScheduledActionService.Add(periodicTask);

#if DEBUG
                // Task Zeit auf 30 Sekunden stellen
                ScheduledActionService.LaunchForTest(periodicTaskName, TimeSpan.FromSeconds(30));
#endif
                // Angeben das Task gestartet ist
                isSetBackgroundTask = true;
            }


            // Benachrichtigungen ausgeben, falls Task nicht gestartet werden kann
            catch (InvalidOperationException exception)
            {
                // Wenn Task nicht aktiv
                if (exception.Message.Contains("Error: The action is disabled"))
                {
                    // Benachrichtigung ausgeben
                    MessageBox.Show("⚠ " + MyApp.Resources.AppResources.X002_exError);
                }

                // Wenn maximum der Anwendungen erreicht
                if (exception.Message.Contains("BNS Error: The maximum number of ScheduledActions of this type have already been added."))
                {
                    // Keine Aktion erförderlich, da eine Systemnachricht ausgegeben wird
                }

                // Variable ob Task aktiv ist
                isSetBackgroundTask = false;
            }


            // Bei Fehler im Scheduler Service
            catch (SchedulerServiceException)
            {
                // Variable ob Task aktiv ist
                isSetBackgroundTask = false;
            }


            // Wenn beide Tasks laufen
            if (periodicTask != null & isSetLockScreenApp == true)
            {
                // Button umstellen
                btnLockScreenChanger.Content = MyApp.Resources.AppResources.X002_yes;
            }


            // Wenn einer der Tasks nicht läuft
            else
            {
                // Button umstellen
                btnLockScreenChanger.Content = MyApp.Resources.AppResources.X002_no;
            }
        }




        // ScheduledTaskAgent entfernen
        private void removeAgent(string name)
        {
            try
            {
                // ScheduledTaskAgent erntfernen
                ScheduledActionService.Remove(name);

                // Angeben das Task entfernt wurde
                isSetBackgroundTask = false;
            }
            catch (Exception)
            {
            }
        }
        // ---------------------------------------------------------------------------------------------------





        // Lockhelper ausführen
        // ---------------------------------------------------------------------------------------------------
        private async Task lockHelper(string filePathOfTheImage, bool isAppResource)
        {
            try
            {
                // Prüfen ob App aktuell Lock Screen App ist
                var isProvider = Windows.Phone.System.UserProfile.LockScreenManager.IsProvidedByCurrentApplication;


                // Wenn nicht Lock Screen App
                if (!isProvider)
                {
                    // Abfragen ob als Lock Screen App gesetzt werden soll.
                    var op = await Windows.Phone.System.UserProfile.LockScreenManager.RequestAccessAsync();

                    // Als Lock Screen App setzen
                    isProvider = op == Windows.Phone.System.UserProfile.LockScreenRequestResult.Granted;
                }


                // Wenn Lock Screen App // Oder gerade als Lock Screen App gesetzt wurde
                if (isProvider)
                {
                    // Demo Bild als Sperrbildschirm setzen
                    var schema = isAppResource ? "ms-appx:///" : "ms-appdata:///Local/";
                    var uri = new Uri(schema + filePathOfTheImage, UriKind.Absolute);
                    Windows.Phone.System.UserProfile.LockScreen.SetImageUri(uri);

                    // Angeben das nun Sperrbildschim App
                    isSetLockScreenApp = true;

                    // Background Task starten
                    startPeriodicAgent();
                }

                // Wenn nicht als Lock Screen App gesetzt wurde
                else
                {
                    // Benachrichtigung ausgeben, das Lock Screen nicht geändert werden kann
                    MessageBox.Show("⚠ " + MyApp.Resources.AppResources.X002_lockHelperInfo);
                }
            }


            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }
        // ---------------------------------------------------------------------------------------------------





    }
}