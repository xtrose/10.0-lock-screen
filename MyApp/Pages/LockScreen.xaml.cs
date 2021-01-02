using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO.IsolatedStorage;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ImageTools;
using System.ComponentModel;





// Namespace
namespace MyApp.Pages
{





    // Editor // Lockscreen Bilder hochladen
    public partial class LockScreen : PhoneApplicationPage
    {





        // Variablen
        // ---------------------------------------------------------------------------------------------------
        // Iso Store Variablen
        IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication();
        IsolatedStorageFileStream filestream;
        StreamReader streamReader;
        StreamWriter streamWriter;


        // Infofarbe erstellen
        SolidColorBrush brushInfo = new SolidColorBrush(Color.FromArgb(255, 207, 40, 40));
        SolidColorBrush brushForeground;


        // Variablen zum steuern der Seite
        string pagePart = "start";
        // ---------------------------------------------------------------------------------------------------





        // Klasse erstellen
        // ---------------------------------------------------------------------------------------------------
        public LockScreen()
        {
            // XAML Initialisieren
            InitializeComponent();


            // Buttons beschriften
            btnDelete.Content = "⚠ " + MyApp.Resources.AppResources.X002_deleteLockScreen;


            // Vordergrundfarbe erstellen
           brushForeground = new SolidColorBrush(Color.FromArgb(255,255,255,255));


            // Slider der Rahmengröße nach Displaygröße anpassen
            if (MainPage.resolution == "1080X1920")
            {
                slFrameSize.Maximum = 45;
            }
            else if (MainPage.resolution == "768X1280")
            {
                slFrameSize.Maximum = 32;
            }
            else if (MainPage.resolution == "720X1280")
            {
                slFrameSize.Maximum = 30;
            }
            else if (MainPage.resolution == "480X800")
            {
                slFrameSize.Maximum = 20;
            }
        }
        // ---------------------------------------------------------------------------------------------------





        // Wird bei jedem Aufruf der Seite ausgeführt
        // ---------------------------------------------------------------------------------------------------
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Wenn keine Ordner ID vorhanden // Neuen Lock Screen erstellen
            if (MainPage.folderName == "")
            {
                // Endlosschleife bis neuer Lock Screen erstellt
                for (int i = 1; i > 0; i++)
                {
                    // Lock Screen Name erstellen
                    MainPage.folderName = MyApp.Resources.AppResources.X002_lockScreen + " " + i.ToString();
                    // Wenn Lock Screen noch nicht vorhanden
                    if (!file.DirectoryExists("/LockScreens/" + MainPage.folderName))
                    {
                        // Ordner erstellen
                        file.CreateDirectory("/LockScreens/" + MainPage.folderName);
                        file.CreateDirectory("/LockScreens/" + MainPage.folderName + "/Square");
                        file.CreateDirectory("/LockScreens/" + MainPage.folderName + "/Square/Small");
                        file.CreateDirectory("/LockScreens/" + MainPage.folderName + "/Square/Medium");
                        file.CreateDirectory("/LockScreens/" + MainPage.folderName + "/Square/Large");
                        file.CreateDirectory("/LockScreens/" + MainPage.folderName + "/Square/Thumbnails");
                        file.CreateDirectory("/LockScreens/" + MainPage.folderName + "/Landscape");
                        file.CreateDirectory("/LockScreens/" + MainPage.folderName + "/Landscape/Small");
                        file.CreateDirectory("/LockScreens/" + MainPage.folderName + "/Landscape/Large");
                        file.CreateDirectory("/LockScreens/" + MainPage.folderName + "/Landscape/Thumbnails");
                        file.CreateDirectory("/LockScreens/" + MainPage.folderName + "/Portrait");
                        file.CreateDirectory("/LockScreens/" + MainPage.folderName + "/Portrait/Small");
                        file.CreateDirectory("/LockScreens/" + MainPage.folderName + "/Portrait/Large");
                        file.CreateDirectory("/LockScreens/" + MainPage.folderName + "/Portrait/Thumbnails");
                        file.CreateDirectory("/LockScreens/" + MainPage.folderName + "/Background");
                        file.CreateDirectory("/LockScreens/" + MainPage.folderName + "/Background/Pictures");
                        file.CreateDirectory("/LockScreens/" + MainPage.folderName + "/Background/Thumbnails");
                        file.CreateDirectory("/LockScreens/" + MainPage.folderName + "/User");
                        file.CreateDirectory("/LockScreens/" + MainPage.folderName + "/User/Pictures");
                        file.CreateDirectory("/LockScreens/" + MainPage.folderName + "/User/Thumbnails");

                        // Bilder Listen erstellen
                        MainPage.imagesListSquare = await ClassFileMamagment.loadCreateOverwrite("/LockScreens/" + MainPage.folderName + "/Square/ImagesList.txt", "~", true);
                        MainPage.imagesListLandscape = await ClassFileMamagment.loadCreateOverwrite("/LockScreens/" + MainPage.folderName + "/Landscape/ImagesList.txt", "~", true);
                        MainPage.imagesListPortrait = await ClassFileMamagment.loadCreateOverwrite("/LockScreens/" + MainPage.folderName + "/Portrait/ImagesList.txt", "~", true);
                        MainPage.imagesListBackground = await ClassFileMamagment.loadCreateOverwrite("/LockScreens/" + MainPage.folderName + "/Background/ImagesList.txt", "~", true);
                        MainPage.imagesListUser = await ClassFileMamagment.loadCreateOverwrite("/LockScreens/" + MainPage.folderName + "/User/ImagesList.txt", "~", true);

                        // Count erstellen
                        MainPage.imagesListUser = await ClassFileMamagment.loadCreateOverwrite("/LockScreens/" + MainPage.folderName + "/Id.dat", MainPage.countLockScreens.ToString(), true);
                        MainPage.countLockScreens++;
                        MainPage.imagesListUser = await ClassFileMamagment.loadCreateOverwrite("countLockScreens.dat", MainPage.countLockScreens.ToString(), true);

                        // Einstellungen erstellen
                        MainPage.backgroundType = "color";
                        MainPage.backgroundColor = "#FF444444";
                        MainPage.userInformationBackgroundColor = "#88000000";
                        MainPage.frameColor = "#FFDDDDDD";
                        MainPage.frameSize = 2;
                        MainPage.usePicturesTransparency = false;
                        MainPage.picturesTransparencyMin = 10;
                        MainPage.picturesTransparencyMax = 10;
                        MainPage.useUserInformation = false;
                        MainPage.userInformationVerticalAlignment = 20;
                        MainPage.userInformation1 = "";
                        MainPage.userInformation2 = "";
                        MainPage.userInformation3 = "";
                        MainPage.userInformation4 = "";
                        MainPage.userInformationFontColor = "#FFFFFFFF";
                        MainPage.useInformationBackground = false;
                        MainPage.informationBackgroundColor = "#88000000";
                        MainPage.informationBackgroundSize = 40;

                        // Einstellungen speichern
                        await ClassFileMamagment.saveSettings();

                        // Abbrechen
                        break;
                    }
                }
            }
            // Einstellungen laden
            await ClassFileMamagment.loadSettings();


            // Bilderlisten laden
            MainPage.imagesListSquare = await ClassFileMamagment.loadCreateOverwrite("/LockScreens/" + MainPage.folderName + "/Square/ImagesList.txt", "", false);
            MainPage.imagesListLandscape = await ClassFileMamagment.loadCreateOverwrite("/LockScreens/" + MainPage.folderName + "/Landscape/ImagesList.txt", "", false);
            MainPage.imagesListPortrait = await ClassFileMamagment.loadCreateOverwrite("/LockScreens/" + MainPage.folderName + "/Portrait/ImagesList.txt", "", false);
            MainPage.imagesListBackground = await ClassFileMamagment.loadCreateOverwrite("/LockScreens/" + MainPage.folderName + "/Background/ImagesList.txt", "", false);
            MainPage.imagesListUser = await ClassFileMamagment.loadCreateOverwrite("/LockScreens/" + MainPage.folderName + "/User/ImagesList.txt", "", false);


            // Transparenz UI Elemente ändern
            tbPicturesTransparencyMin.Text = MainPage.picturesTransparencyMin.ToString();
            tbPicturesTransparencyMax.Text = MainPage.picturesTransparencyMax.ToString();
            slPicturesTransparencyMin.Value = MainPage.picturesTransparencyMin;
            slPicturesTransparencyMax.Value = MainPage.picturesTransparencyMax;
            picturesTranspacencyChange = true;


            // Bilder Count erstellen
            MainPage.arImagesSquare = Regex.Split(MainPage.imagesListSquare, "~");
            MainPage.arImagesLandscape = Regex.Split(MainPage.imagesListLandscape, "~");
            MainPage.arImagesPortrait = Regex.Split(MainPage.imagesListPortrait, "~");
            MainPage.arImagesBackground = Regex.Split(MainPage.imagesListBackground, "~");
            MainPage.arImagesUser = Regex.Split(MainPage.imagesListUser, "~");
            MainPage.cImagesSquare = MainPage.arImagesSquare.Count() - 2;
            MainPage.cImagesLandscape = MainPage.arImagesLandscape.Count() - 2;
            MainPage.cImagesPortrait = MainPage.arImagesPortrait.Count() - 2;
            MainPage.cImagesBackground = MainPage.arImagesBackground.Count() - 2;
            MainPage.cImagesUser = MainPage.arImagesUser.Count() - 2;


            // Hintergundeinstellungen verarbeiten
            if (MainPage.backgroundType == "color")
            {
                spBackgroundColor.Visibility = System.Windows.Visibility.Visible;
                spBackgroundPictures.Visibility = System.Windows.Visibility.Collapsed;
                lpBackgroundChange = false;
                lpBackground.SelectedIndex = 0;
                lpBackgroundChange = true;
            }
            else
            {
                spBackgroundColor.Visibility = System.Windows.Visibility.Collapsed;
                spBackgroundPictures.Visibility = System.Windows.Visibility.Visible;
                lpBackgroundChange = false;
                lpBackground.SelectedIndex = 1;
                lpBackgroundChange = true;
            }


            // Farbeinstellungen verarbeiten // Hintergrundfarbe
            recBackgroundColor.Fill = getSolidColorBrush(MainPage.backgroundColor);

            // Farbeinstellungen verarbeiten // Rahmenfarbe
            recFrameColor.Fill = getSolidColorBrush(MainPage.frameColor);

            // Farbeinstellungen verarbeiten // Benutzerinformation Hintergrundfarbe
            recUserInformationBackgroundColor.Fill = getSolidColorBrush(MainPage.userInformationBackgroundColor);

            // Farbeinstellungen verarbeiten // Benutzerinformation Schrift
            recUserInformationFontColor.Fill = getSolidColorBrush(MainPage.userInformationFontColor);

            // Farbeinstellungen verarbeiten // Information Hintergrund
            recInformationBackground.Fill = getSolidColorBrush(MainPage.informationBackgroundColor);


            // Rahmengröße, Slider einstellen
            slFrameSize.Value = MainPage.frameSize;


            // Größe in Textbox schreiben
            tbFrameSizeValue.Text = MainPage.frameSize.ToString();


            // Textfelder zurücksetzen
            tbSquareImages.Foreground = brushForeground;
            tbSquareImagesCount.Foreground = brushForeground;
            tbSquareImages.Opacity = 1.0;
            tbSquareImagesCount.Opacity = 1.0;
            tbLandscapeImages.Foreground = brushForeground;
            tbLandscapeImagesCount.Foreground = brushForeground;
            tbLandscapeImages.Opacity = 1.0;
            tbLandscapeImagesCount.Opacity = 1.0;
            tbPortraitImages.Foreground = brushForeground;
            tbPortraitImagesCount.Foreground = brushForeground;
            tbPortraitImages.Opacity = 1.0;
            tbPortraitImagesCount.Opacity = 1.0;


            // Textfelder bearbeiten
            tbName.Text = MainPage.folderName;
            teName.Text = MainPage.folderName;
            if (MainPage.cImagesSquare == 0)
            {
                tbSquareImagesCount.Text = MainPage.cImagesSquare + " " + MyApp.Resources.AppResources.X002_pictures;
                tbSquareImages.Opacity = 0.6;
                tbSquareImagesCount.Opacity = 0.6;
            }
            else if (MainPage.cImagesSquare == 1)
            {
                tbSquareImagesCount.Text = MainPage.cImagesSquare + " " + MyApp.Resources.AppResources.X002_picture;
                tbSquareImages.Foreground = brushForeground;
                tbSquareImagesCount.Foreground = brushForeground;
            }
            else
            {
                tbSquareImagesCount.Text = MainPage.cImagesSquare + " " + MyApp.Resources.AppResources.X002_pictures;
                tbSquareImages.Foreground = brushForeground;
                tbSquareImagesCount.Foreground = brushForeground;
            }

            if (MainPage.cImagesLandscape == 0)
            {
                tbLandscapeImagesCount.Text = MainPage.cImagesLandscape + " " + MyApp.Resources.AppResources.X002_pictures;
                tbLandscapeImages.Opacity = 0.6;
                tbLandscapeImagesCount.Opacity = 0.6;
            }
            else if (MainPage.cImagesLandscape == 1)
            {
                tbLandscapeImagesCount.Text = MainPage.cImagesLandscape + " " + MyApp.Resources.AppResources.X002_picture;
                tbLandscapeImages.Foreground = brushForeground;
                tbLandscapeImagesCount.Foreground = brushForeground;
            }
            else
            {
                tbLandscapeImagesCount.Text = MainPage.cImagesLandscape + " " + MyApp.Resources.AppResources.X002_pictures;
                tbLandscapeImages.Foreground = brushForeground;
                tbLandscapeImagesCount.Foreground = brushForeground;
            }

            if (MainPage.cImagesPortrait == 0)
            {
                tbPortraitImagesCount.Text = MainPage.cImagesPortrait + " " + MyApp.Resources.AppResources.X002_pictures;
                tbPortraitImages.Opacity = 0.6;
                tbPortraitImagesCount.Opacity = 0.6;
            }
            else if (MainPage.cImagesPortrait == 1)
            {
                tbPortraitImagesCount.Text = MainPage.cImagesPortrait + " " + MyApp.Resources.AppResources.X002_picture;
                tbPortraitImages.Foreground = brushForeground;
                tbPortraitImagesCount.Foreground = brushForeground;
            }
            else
            {
                tbPortraitImagesCount.Text = MainPage.cImagesPortrait + " " + MyApp.Resources.AppResources.X002_pictures;
                tbPortraitImages.Foreground = brushForeground;
                tbPortraitImagesCount.Foreground = brushForeground;
            }

            if (MainPage.cImagesBackground == 0)
            {
                tbBackgroundImagesCount.Text = MainPage.cImagesBackground + " " + MyApp.Resources.AppResources.X002_pictures;
                tbBackgroundImages.Opacity = 0.6;
                tbBackgroundImagesCount.Opacity = 0.6;
            }
            else if (MainPage.cImagesBackground == 1)
            {
                tbBackgroundImagesCount.Text = MainPage.cImagesBackground + " " + MyApp.Resources.AppResources.X002_picture;
                tbBackgroundImages.Foreground = brushForeground;
                tbBackgroundImagesCount.Foreground = brushForeground;
            }
            else
            {
                tbBackgroundImagesCount.Text = MainPage.cImagesBackground + " " + MyApp.Resources.AppResources.X002_pictures;
                tbBackgroundImages.Foreground = brushForeground;
                tbBackgroundImagesCount.Foreground = brushForeground;
            }

            if (MainPage.cImagesUser == 0)
            {
                tbUserImagesCount.Text = MainPage.cImagesUser + " " + MyApp.Resources.AppResources.X002_pictures;
            }
            else if (MainPage.cImagesUser == 1)
            {
                tbUserImagesCount.Text = MainPage.cImagesUser + " " + MyApp.Resources.AppResources.X002_picture;
            }
            else
            {
                tbUserImagesCount.Text = MainPage.cImagesUser + " " + MyApp.Resources.AppResources.X002_pictures;
            }



            // Wenn kein Bild vorhanden // Alle drei Bildformate Rot färben
            if (MainPage.cImagesSquare == 0 & MainPage.cImagesLandscape == 0 & MainPage.cImagesPortrait == 0 & MainPage.cImagesBackground == 0)
            {
                tbSquareImages.Foreground = brushInfo;
                tbSquareImagesCount.Foreground = brushInfo;
                tbSquareImages.Opacity = 1.0;
                tbSquareImagesCount.Opacity = 1.0;
                tbLandscapeImages.Foreground = brushInfo;
                tbLandscapeImagesCount.Foreground = brushInfo;
                tbLandscapeImages.Opacity = 1.0;
                tbLandscapeImagesCount.Opacity = 1.0;
                tbPortraitImages.Foreground = brushInfo;
                tbPortraitImagesCount.Foreground = brushInfo;
                tbPortraitImages.Opacity = 1.0;
                tbPortraitImagesCount.Opacity = 1.0;
                tbBackgroundImages.Foreground = brushInfo;
                tbBackgroundImagesCount.Foreground = brushInfo;
                tbBackgroundImages.Opacity = 1.0;
                tbBackgroundImagesCount.Opacity = 1.0;
            }



            // Bilder Transparenz // Einblenden // Ausblenden
            if (MainPage.usePicturesTransparency)
            {
                spPicturesTransparency.Visibility = System.Windows.Visibility.Visible;
                lpPicturesTransparency.SelectedIndex = 1;
            }
            else
            {
                spPicturesTransparency.Visibility = System.Windows.Visibility.Collapsed;
                lpPicturesTransparency.SelectedIndex = 0;
            }
            lpPicturesTransparencyChange = true;



            // Benutzerinformationen // Einblenden // Ausblenden
            if (MainPage.useUserInformation)
            {
                spUserInformations.Visibility = System.Windows.Visibility.Visible;
                lpUserInformations.SelectedIndex = 1;
            }
            else
            {
                spUserInformations.Visibility = System.Windows.Visibility.Collapsed;
                lpUserInformations.SelectedIndex = 0;
            }
            lpUserInformationChange = true;



            // Benutzerinformationen // Slider einstellen
            slUserInformationVerticalAlignment.Value = MainPage.userInformationVerticalAlignment;
            tbUserInformationVerticalAlignment.Text = MainPage.userInformationVerticalAlignment + "%";



            // Benutzerinformationen // Textboxen einstellen
            tbUserInformation1.Text = MainPage.userInformation1;
            tbUserInformation2.Text = MainPage.userInformation2;
            tbUserInformation3.Text = MainPage.userInformation3;
            tbUserInformation4.Text = MainPage.userInformation4;



            // Information Hintergrund // Einblenden // Ausblenden
            if (MainPage.useInformationBackground)
            {
                spInformationBackground.Visibility = System.Windows.Visibility.Visible;
                lpInformationBackground.SelectedIndex = 1;
            }
            else
            {
                spInformationBackground.Visibility = System.Windows.Visibility.Collapsed;
                lpInformationBackground.SelectedIndex = 0;
            }
            lpInformationBackgroundChange = true;



            // Information Hintergrund // Slider einstellen
            slInformationBackgroundSize.Value = MainPage.informationBackgroundSize;
            tbInformationBackgroundSize.Text = MainPage.informationBackgroundSize + "%";



            // Wenn Bilder vorhanden // Sperrbildschirm Demo erstellen
            if (MainPage.cImagesSquare > 0 | MainPage.cImagesLandscape > 0 | MainPage.cImagesPortrait > 0 | (MainPage.cImagesBackground > 0 & MainPage.backgroundType == "pictures"))
            {
                // Info Unsichtbar machen
                grNoPictures.Visibility = System.Windows.Visibility.Collapsed;
                grNoPictures2.Visibility = System.Windows.Visibility.Collapsed;
            }


            // Wenn keine Bilder vorhanden // Info Sichtbar machen
            else
            {
                // Obere Info sichtbar machen
                grNoPictures.Visibility = System.Windows.Visibility.Visible;
                // Untere Info nur sichtbar machen wenn Hintergrund auf Bilder gestellt ist
                if (MainPage.backgroundType == "pictrues")
                {
                    grNoPictures2.Visibility = System.Windows.Visibility.Visible;
                }
            }


            // Sperrbildschirm erstellen
            createLockScreen();
        }
        // ---------------------------------------------------------------------------------------------------





        // Button // Name des Lock Screens ändern
        // ---------------------------------------------------------------------------------------------------
        // Wenn Edit Button gedrückt wird
        private void btnEditName_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // TextBox umwandeln
            tbName.Visibility = System.Windows.Visibility.Collapsed;
            teName.Visibility = System.Windows.Visibility.Visible;
            grTeName.Visibility = System.Windows.Visibility.Visible;
            // teName.LostFocus += teName_LostFocus;

            // TextBox anwählen
            teName.Focus();
        }



        // TextBox Buttons auswerten
        private void btnEditName_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            // Prüfen ob Return gedrückt wurde
            string tempkey = Convert.ToString(e.Key);
            if (tempkey == "Enter")
            {
                // Eingabe abschließen
                teName_Finished();
            }
        }



        // Wenn TextBox abgewählt wird
        void teName_LostFocus(object sender, RoutedEventArgs e)
        {
            // Eingabe abschließen
            teName_Finished();
        }



        // Eingabe abschließen
        async void teName_Finished()
        {
            // ";" Zeichen herauslöschen
            teName.Text = Regex.Replace(teName.Text, ";", "");

            // Prüfen ob noch Zeichen vorhanden
            if (teName.Text.Length > 0)
            {
                // Wenn leere Eingabe
                bool temp = Regex.IsMatch(teName.Text, @"^[a-zA-Z0-9 ]+$");
                temp = true;
                if (temp == false)
                {
                    MessageBox.Show(MyApp.Resources.AppResources.X002_errorName);
                    teName.Text = MainPage.folderName;
                    teName.Focus();
                }

                // Wenn keine leere Eingabe
                else
                {
                    // Wenn Eingabe gleich Name
                    if (teName.Text == MainPage.folderName)
                    {
                        tbName.Visibility = System.Windows.Visibility.Visible;
                        teName.Visibility = System.Windows.Visibility.Collapsed;
                        grTeName.Visibility = System.Windows.Visibility.Collapsed;
                    }

                    // Wenn Eingabe anders als Name
                    else
                    {
                        // Wenn Ordner noch nicht besteht
                        if (!file.DirectoryExists("/LockScreens/" + teName.Text))
                        {
                            // Versuchen Ordner umzubenennen
                            try
                            {
                                // Ordner umbenennen
                                file.MoveDirectory("/LockScreens/" + MainPage.folderName, "/LockScreens/" + teName.Text);

                                // Wenn Sperrbildschirm, eingestellter Sperrbildschirm
                                if (MainPage.setLockScreen == tbName.Text)
                                {
                                    // Einstellung ändern
                                    MainPage.setLockScreen = teName.Text;

                                    // Einstellungen speichern
                                    await ClassFileMamagment.saveMainSettings();
                                }

                                // Tile Liste durchlaufen und prüfen ob Name vorhanden
                                for (int i = 0; i < MainPage.createdTiles.Count(); i++)
                                {
                                    // Wenn Tile erstellt wird
                                    if (MainPage.createdTiles[i] == MainPage.folderName)
                                    {
                                        // Eintrag ändern
                                        MainPage.createdTiles[i] = teName.Text;
                                        // Liste der Tiles zusammenstellen und speichern
                                        string allTiles = "";
                                        for (int i2 = 0; i2 < MainPage.createdTiles.Count(); i2++)
                                        {
                                            // Liste der Tiles zusammenschreiben
                                            allTiles += MainPage.createdTiles[i2];
                                            if (MainPage.createdTiles.Count() - 1 != i2)
                                            {
                                                allTiles += "~";
                                            }
                                        }
                                        // Datei speichern
                                        await ClassFileMamagment.loadCreateOverwrite("AllTiles.txt", allTiles, true);
                                        break;
                                    }
                                }

                                // Name der Anzeige ändern
                                MainPage.folderName = teName.Text;
                                tbName.Text = teName.Text;
                                tbName.Visibility = System.Windows.Visibility.Visible;
                                teName.Visibility = System.Windows.Visibility.Collapsed;
                                grTeName.Visibility = System.Windows.Visibility.Collapsed;
                            }
                            catch
                            {
                                // Benachrichtigung das Umbennen nich möglich
                                MessageBox.Show(MyApp.Resources.AppResources.X002_errorName);
                                teName.Text = MainPage.folderName;
                                teName.Focus();
                            }
                        }

                        // Wenn Ordner bereits besteht
                        else
                        {
                            // Benachrichtigung das Umbennen nich möglich
                            MessageBox.Show(MyApp.Resources.AppResources.X002_errorName);
                            teName.Text = MainPage.folderName;
                            teName.Focus();
                        }
                    }
                }
            }
        }



        // Alle Dateien und Unterordner verschieben
        private async Task moveFiles(string source, string destination)
        {
            // Alle Dateien verschieben
            string[] arFiles = file.GetFileNames(source + "/");
            foreach (string arFiles2 in arFiles)
            {
                file.MoveFile(source + "/" + arFiles2, destination + "/" + arFiles2);
            }

            // Alle Ordner laden
            string[] arFolders = file.GetDirectoryNames(source + "/");
            for (int i = 0; i < arFolders.Count(); i++)
            {
                file.CreateDirectory(destination + "/" + arFolders[i]);
                await moveFiles(source + "/" + arFolders[i], destination + "/" + arFolders[i]);
            }

            // Ordner löschen
            file.DeleteDirectory(source + "/");
        }
        // ---------------------------------------------------------------------------------------------------





        // Buttons Bider bearbeiten
        // ---------------------------------------------------------------------------------------------------
        // Hochformat
        private void btnPortrait_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Benutzerinformationen Texte speichern
            saveUserInformationText();

            // Bild Editor öffnen
            MainPage.format = "Portrait";
            NavigationService.Navigate(new Uri("/Pages/Thumbnails.xaml", UriKind.Relative));
        }



        // Querformat
        private void btnLandscape_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Benutzerinformationen Texte speichern
            saveUserInformationText();

            // Bild Editor öffnen
            MainPage.format = "Landscape";
            NavigationService.Navigate(new Uri("/Pages/Thumbnails.xaml", UriKind.Relative));
        }



        // Quadrat
        private void btnSquare_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Benutzerinformationen Texte speichern
            saveUserInformationText();

            // Bild Editor öffnen
            MainPage.format = "Square";
            NavigationService.Navigate(new Uri("/Pages/Thumbnails.xaml", UriKind.Relative));
        }
        // ---------------------------------------------------------------------------------------------------





        // Buttons im Menü
        // ---------------------------------------------------------------------------------------------------
        // Auswahl // Information Hintergrund // Anwenden // Nicht anwenden
        bool lpPicturesTransparencyChange = false;
        private async void lpPicturesTransparency_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Wenn angewendet wird
            if (lpPicturesTransparencyChange)
            {
                // Anwenden
                if (lpPicturesTransparency.SelectedIndex == 1)
                {
                    MainPage.usePicturesTransparency = true;
                }
                else
                {
                    MainPage.usePicturesTransparency = false;
                }

                // Einstellungen speichern
                await ClassFileMamagment.saveSettings();

                // Einstellungen anzeigen ausblenden 
                if (MainPage.usePicturesTransparency)
                {
                    spPicturesTransparency.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    spPicturesTransparency.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
        }

        // Slider // Bilder Transparenz Min // ValueChanged
        bool picturesTranspacencyChange = false;
        private void slPicturesTransparencyMin_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Wenn angewendet wird
            if (picturesTranspacencyChange)
            {
                // Wert ändern
                MainPage.picturesTransparencyMin = Convert.ToInt32(slPicturesTransparencyMin.Value);

                // Wenn Maximal kleiner als Minimal
                if (MainPage.picturesTransparencyMax < MainPage.picturesTransparencyMin)
                {
                    MainPage.picturesTransparencyMax = MainPage.picturesTransparencyMin;
                    picturesTranspacencyChange = false;
                    slPicturesTransparencyMax.Value = Convert.ToInt32(MainPage.picturesTransparencyMax);
                    picturesTranspacencyChange = true;
                }
                // Werte in Textbox schreiben
                tbPicturesTransparencyMin.Text = MainPage.picturesTransparencyMin.ToString();
                tbPicturesTransparencyMax.Text = MainPage.picturesTransparencyMax.ToString();
            }
        }

        // Slider // Bilder Transparenz // MouseLeftButtonUp
        private void slPicturesTransparencyMin_MoseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Transparenz speichern
            savePicturesTransparency();
        }

        // Slider // Bilder Transparenz Max // ValueChanged
        private void slPicturesTransparencyMax_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Wenn angewendet wird
            if (picturesTranspacencyChange)
            {
                // Wert ändern
                MainPage.picturesTransparencyMax = Convert.ToInt32(slPicturesTransparencyMax.Value);

                // Wenn Maximal größer als Minimal
                if (MainPage.picturesTransparencyMin > MainPage.picturesTransparencyMax)
                {
                    MainPage.picturesTransparencyMin = MainPage.picturesTransparencyMax;
                    picturesTranspacencyChange = false;
                    slPicturesTransparencyMin.Value = Convert.ToInt32(MainPage.picturesTransparencyMin);
                    picturesTranspacencyChange = true;
                }
                // Werte in Textbox schreiben
                tbPicturesTransparencyMin.Text = MainPage.picturesTransparencyMin.ToString();
                tbPicturesTransparencyMax.Text = MainPage.picturesTransparencyMax.ToString();
            }
        }
        // Slider // Bilder Transparenz // MouseLeftButtonUp
        private void slPicturesTransparencyMax_MoseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Transparenz speichern
            savePicturesTransparency();
        }

        // Slider // Bilder Transparenz // Größe in Einstellungen speichern
        private async void savePicturesTransparency()
        {
            // Bilder Transparenz speichern
            await ClassFileMamagment.saveSettings();

            // Wenn Bilder vorhanden // Sperrbildschirm Demo erstellen
            if (MainPage.cImagesSquare > 0 | MainPage.cImagesLandscape > 0 | MainPage.cImagesPortrait > 0)
            {
                // Info Unsichtbar machen
                grNoPictures.Visibility = System.Windows.Visibility.Collapsed;
                grNoPictures2.Visibility = System.Windows.Visibility.Collapsed;

                // Sperrbildschirm erstellen
                createLockScreen();
            }
        }





        // Auswahl // Hintergrundfarbe // Hintergundbilder
        bool lpBackgroundChange = false;
        private async void lpBackground_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lpBackgroundChange)
            {
                // Hintergrundfarbe
                if (lpBackground.SelectedIndex == 0)
                {
                    // Einstellung anwenden
                    MainPage.backgroundType = "color";
                    await ClassFileMamagment.saveSettings();
                    spBackgroundColor.Visibility = System.Windows.Visibility.Visible;
                    spBackgroundPictures.Visibility = System.Windows.Visibility.Collapsed;
                    grNoPictures2.Visibility = System.Windows.Visibility.Collapsed;
                }

                // Hintergrundbilder
                else
                {
                    // Einstellungen anwenden
                    MainPage.backgroundType = "pictures";
                    await ClassFileMamagment.saveSettings();
                    spBackgroundColor.Visibility = System.Windows.Visibility.Collapsed;
                    spBackgroundPictures.Visibility = System.Windows.Visibility.Visible;

                    // Untere Info nur sichtbar machen wenn Hintergrund auf Bilder gestellt ist
                    if (MainPage.cImagesSquare > 0 | MainPage.cImagesLandscape > 0 | MainPage.cImagesPortrait > 0 | (MainPage.cImagesBackground > 0 & MainPage.backgroundType == "pictures"))
                    {
                    }
                    else
                    {
                        grNoPictures2.Visibility = System.Windows.Visibility.Visible;
                    }
                }
            }
        }

        // Button // Hintergundfarbe // Click
        private void ButtonBackgroundColorClick(object sender, RoutedEventArgs e)
        {
            // Benutzerinformationen Texte speichern
            saveUserInformationText();

            // Zur Farbauswahl
            NavigationService.Navigate(new Uri("/Pages/ChangeColor.xaml?color=backgroundColor", UriKind.Relative));
        }

        // Quadrat // Hintergrundfarbe // MouseLeftButtonUp
        private void RectangleBackgroundColorMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Benutzerinformationen Texte speichern
            saveUserInformationText();

            // Zur Farbauswahl
            NavigationService.Navigate(new Uri("/Pages/ChangeColor.xaml?color=backgroundColor", UriKind.Relative));
        }

        // Button // Hintergrundbilder
        private void btnBackground_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Benutzerinformationen Texte speichern
            saveUserInformationText();

            // Bild Editor öffnen
            MainPage.format = "Background";
            NavigationService.Navigate(new Uri("/Pages/Thumbnails.xaml", UriKind.Relative));
        }





        // Button // Rahmenfarbe // Click
        private void ButtonFrameColorClick(object sender, RoutedEventArgs e)
        {
            // Benutzerinformationen Texte speichern
            saveUserInformationText();

            // Zur Farbauswahl
            NavigationService.Navigate(new Uri("/Pages/ChangeColor.xaml?color=frameColor", UriKind.Relative));
        }

        // Quadrat // Rahmenfarbe // MouseLeftButtonUp
        private void RectangleFrameColorMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Benutzerinformationen Texte speichern
            saveUserInformationText();

            // Zur Farbauswahl
            NavigationService.Navigate(new Uri("/Pages/ChangeColor.xaml?color=frameColor", UriKind.Relative));
        }

        // Slider // Rahmengröße // ValueChanged
        private void slFrameSize_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Rahmengröße neu erstellen
            MainPage.frameSize = Convert.ToInt32(slFrameSize.Value);

            // Größe in Textbox schreiben
            tbFrameSizeValue.Text = MainPage.frameSize.ToString();
        }

        // Slider // Rahmengröße // MouseLeftButtonUp
        private async void slFrameSize_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Rahmengröße speichern
            await ClassFileMamagment.saveSettings();

            // Wenn Bilder vorhanden // Sperrbildschirm Demo erstellen
            if (MainPage.cImagesSquare > 0 | MainPage.cImagesLandscape > 0 | MainPage.cImagesPortrait > 0)
            {
                // Info Unsichtbar machen
                grNoPictures.Visibility = System.Windows.Visibility.Collapsed;
                grNoPictures2.Visibility = System.Windows.Visibility.Collapsed;

                // Sperrbildschirm erstellen
                createLockScreen();
            }
        }





        // Auswahl // Benutzerinformation // Anwenden // Nicht anwenden
        bool lpUserInformationChange = false;
        private async void lpUserInformations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lpUserInformationChange)
            {
                // Anwenden
                if (lpUserInformations.SelectedIndex == 1)
                {
                    MainPage.useUserInformation = true;
                }
                else
                {
                    MainPage.useUserInformation = false;
                }

                // Einstellungen speichern
                await ClassFileMamagment.saveSettings();

                // Einstellungen anzeigen ausblenden 
                if (MainPage.useUserInformation)
                {
                    spUserInformations.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    spUserInformations.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
        }

        // Button // Benutzerinformation // Benutzerbilder
        private void btnUser_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Benutzerinformationen Texte speichern
            saveUserInformationText();

            // Bild Editor öffnen
            MainPage.format = "User";
            NavigationService.Navigate(new Uri("/Pages/Thumbnails.xaml", UriKind.Relative));
        }

        // Button // Benutzerinformation // Hintergrundfarbe
        private void ButtonUserBackbroundColor_Click(object sender, RoutedEventArgs e)
        {
            // Benutzerinformationen Texte speichern
            saveUserInformationText();

            // Zur Farbauswahl
            NavigationService.Navigate(new Uri("/Pages/ChangeColor.xaml?color=userInformationBackgroundColor", UriKind.Relative));
        }

        // Quadrat // Benutzerinformation // Hintergrundfarbe
        private void RectangleUserBackbroundColor_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Benutzerinformationen Texte speichern
            saveUserInformationText();

            // Zur Farbauswahl
            NavigationService.Navigate(new Uri("/Pages/ChangeColor.xaml?color=userInformationBackgroundColor", UriKind.Relative));
        }

        // Slider // Benuterinformation // Vertikale Ausrichtung // MouseLeftButtonUp
        private async void slUserInformationVerticalAlignment_MoseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Rahmengröße speichern
            await ClassFileMamagment.saveSettings();

            // Sperrbildschirm erstellen
            createLockScreen();
        }

        // Slider // Benuterinformation // Vertikale Ausrichtung // Value Changed
        private void slUserInformationVerticalAlignment_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Werte umstellen
            MainPage.userInformationVerticalAlignment = Convert.ToInt32(slUserInformationVerticalAlignment.Value);
            tbUserInformationVerticalAlignment.Text = MainPage.userInformationVerticalAlignment + "%";
        }

        // TextBlöcke // Benutzerinformation // Button Down
        private void userInformationsText_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            // Prüfen ob Return gedrückt wurde
            string tempkey = Convert.ToString(e.Key);
            if (tempkey == "Enter")
            {
                // Sperrbildschirm erstellen
                createLockScreen();
            }
        }

        // Button // Benutzerinformationen // Schriftfarbe
        private void ButtonUserFontColor_Click(object sender, RoutedEventArgs e)
        {
            // Benutzerinformationen Texte speichern
            saveUserInformationText();

            // Zur Farbauswahl
            NavigationService.Navigate(new Uri("/Pages/ChangeColor.xaml?color=userInformationFontColor", UriKind.Relative));
        }

        // Quadrat // Benutzerinformation // Schriftfarbe
        private void RectangleUserFontColor_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Benutzerinformationen Texte speichern
            saveUserInformationText();

            // Zur Farbauswahl
            NavigationService.Navigate(new Uri("/Pages/ChangeColor.xaml?color=userInformationFontColor", UriKind.Relative));
        }





        // Auswahl // Information Hintergrund // Anwenden // Nicht anwenden
        bool lpInformationBackgroundChange = false;
        private async void lpInformationBackground_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lpInformationBackgroundChange)
            {
                // Anwenden
                if (lpInformationBackground.SelectedIndex == 1)
                {
                    MainPage.useInformationBackground = true;
                }
                else
                {
                    MainPage.useInformationBackground = false;
                }

                // Einstellungen speichern
                await ClassFileMamagment.saveSettings();

                // Einstellungen anzeigen ausblenden 
                if (MainPage.useInformationBackground)
                {
                    spInformationBackground.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    spInformationBackground.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
        }

        // Button // Information Hintergrund // Hintergrundfarbe
        private void btnInformationBackground_Click(object sender, RoutedEventArgs e)
        {
            // Benutzerinformationen Texte speichern
            saveUserInformationText();

            // Zur Farbauswahl
            NavigationService.Navigate(new Uri("/Pages/ChangeColor.xaml?color=informationBackgroundColor", UriKind.Relative));
        }

        // Quadrat // Information Hintergrund // Hintergrundfarbe
        private void recInformationBackground_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Benutzerinformationen Texte speichern
            saveUserInformationText();

            // Zur Farbauswahl
            NavigationService.Navigate(new Uri("/Pages/ChangeColor.xaml?color=informationBackgroundColor", UriKind.Relative));
        }

        // Slider // Information Hintergrund // Größe // MouseLeftButtonUp
        private async void slInformationBackgroundSize_MoseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Größe speichern
            await ClassFileMamagment.saveSettings();

            // Sperrbildschirm erstellen
            createLockScreen();
        }

        // Slider // Information Hintergrund // Größe // Value Changed
        private void slInformationBackgroundSize_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Werte umstellen
            MainPage.informationBackgroundSize = Convert.ToInt32(slInformationBackgroundSize.Value);
            tbInformationBackgroundSize.Text = MainPage.informationBackgroundSize + "%";
        }
        // ---------------------------------------------------------------------------------------------------





        // Sperrbildschirm löschen
        // ---------------------------------------------------------------------------------------------------
        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            // Benachrichtigung ausgeben
            if (MessageBox.Show("⚠ " + MyApp.Resources.AppResources.X002_deleteMessage, MyApp.Resources.AppResources.X002_warning, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                // Alle Dateien und Ordner löschen
                await ClassFileMamagment.deleteFolder("/LockScreens/" + MainPage.folderName);

                // Zurück gehen
                NavigationService.GoBack();
            }
        }
        // ---------------------------------------------------------------------------------------------------





        // Button Demo
        // ---------------------------------------------------------------------------------------------------
        private void imgLockScreenDemo_MouseLeftBottomUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Demo ausblenden
            spMain.Visibility = System.Windows.Visibility.Visible;
            grLockScreenDemo.Visibility = System.Windows.Visibility.Collapsed;


            // Angeben das Startseite
            pagePart = "start";
        }
        // ---------------------------------------------------------------------------------------------------










        // Versuchen Sperrbildschirm zu erstellen
        // ---------------------------------------------------------------------------------------------------
        private async void createLockScreen()
        {
            // Benutzerinformationen Texte speichern
            saveUserInformationText();


            // Versuchen Sperrbildschirm erstellen
            try
            {
                // Sperrbildschirm erstellen
                WriteableBitmap wbLockScreenDemo = new WriteableBitmap(0, 0);
                wbLockScreenDemo = await ClassCreateLockScreen.createLockScreen(MainPage.folderName);
                imgLockScreenDemo.Source = wbLockScreenDemo;

                // Demo anzeigen
                spMain.Visibility = System.Windows.Visibility.Collapsed;
                grLockScreenDemo.Visibility = System.Windows.Visibility.Visible;


                // Angeben das Demo angezeigt wird
                pagePart = "demo";
            }

            // Wenn Sperrbildschirm nicht erstellt werden kann
            catch
            {
                MessageBox.Show("⚠ " + MyApp.Resources.AppResources.X002_noPicturesAvailable);
            }
        }
        // ---------------------------------------------------------------------------------------------------





        // Back Button
        //----------------------------------------------------------------------------------------------------
        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            // Wenn Demo angezeigt wird
            if (pagePart == "demo")
            {
                // Zurück abbrechen
                e.Cancel = true;


                // Demo ausblenden
                spMain.Visibility = System.Windows.Visibility.Visible;
                grLockScreenDemo.Visibility = System.Windows.Visibility.Collapsed;


                // Angeben das Startseite
                pagePart = "start";
            }


            // Wenn Startseite
            else
            {
                // Zurück abbrechen
                e.Cancel = true;


                // Benutzerinformationen Texte speichern
                saveUserInformationText();


                // Zurück
                NavigationService.GoBack();
            }
        }
        //----------------------------------------------------------------------------------------------------










        // Farbe erstellen
        // ---------------------------------------------------------------------------------------------------
        private SolidColorBrush getSolidColorBrush(string code)
        {
            // Farbeinstellungen verarbeiten // Hintergrundfarbe
            string colorNew = "";
            if (code == "bc")
            {
                Color backgroundColor = (Color)Application.Current.Resources["PhoneBackgroundColor"];
                colorNew = Convert.ToString(backgroundColor);
            }
            else if (code == "ac")
            {
                Color accentColor = (Color)Application.Current.Resources["PhoneAccentColor"];
                colorNew = Convert.ToString(accentColor);
            }
            else if (code == "fc")
            {
                Color accentColor = (Color)Application.Current.Resources["PhoneForegroundColor"];
                colorNew = Convert.ToString(accentColor);
            }
            else
            {
                colorNew = code;
            }
            byte A = Convert.ToByte(colorNew.Substring(1, 2), 16);
            byte R = Convert.ToByte(colorNew.Substring(3, 2), 16);
            byte G = Convert.ToByte(colorNew.Substring(5, 2), 16);
            byte B = Convert.ToByte(colorNew.Substring(7, 2), 16);
            SolidColorBrush solidColorBrush = new SolidColorBrush(Color.FromArgb(A, R, G, B));

            // Farbe zurück geben
            return solidColorBrush;
        }
        // ---------------------------------------------------------------------------------------------------





        // Benutzerinformationen Texte speichern
        // ---------------------------------------------------------------------------------------------------
        private async void saveUserInformationText()
        {
            // Name speichern, wenn geändert wurde
            if (tbName.Text != teName.Text)
            {
                // Umbenennen abschließen
                teName_Finished();
            }


            // Textblöcke speichern
            if (MainPage.userInformation1 != tbUserInformation1.Text | MainPage.userInformation2 != tbUserInformation2.Text | MainPage.userInformation3 != tbUserInformation3.Text | MainPage.userInformation4 != tbUserInformation4.Text)
            {
                // Textblöcke erstellen
                string stTemp = Regex.Replace(tbUserInformation1.Text, "&", "");
                stTemp = Regex.Replace(stTemp, "=", "");
                MainPage.userInformation1 = stTemp;
                tbUserInformation1.Text = stTemp;
                stTemp = Regex.Replace(tbUserInformation2.Text, "&", "");
                stTemp = Regex.Replace(stTemp, "=", "");
                MainPage.userInformation2 = stTemp;
                tbUserInformation2.Text = stTemp;
                stTemp = Regex.Replace(tbUserInformation3.Text, "&", "");
                stTemp = Regex.Replace(stTemp, "=", "");
                MainPage.userInformation3 = stTemp;
                tbUserInformation3.Text = stTemp;
                stTemp = Regex.Replace(tbUserInformation4.Text, "&", "");
                stTemp = Regex.Replace(stTemp, "=", "");
                MainPage.userInformation4 = stTemp;
                tbUserInformation4.Text = stTemp;

                // Einstellungen speichern
                await ClassFileMamagment.saveSettings();
            }
        }
        // ---------------------------------------------------------------------------------------------------





    }
}