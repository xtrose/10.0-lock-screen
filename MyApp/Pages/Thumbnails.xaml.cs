using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Collections.ObjectModel;
using Microsoft.Phone.Tasks;
using ImageTools.IO.Png;
using System.IO.IsolatedStorage;
using System.IO;
using System.Windows.Media.Imaging;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Threading.Tasks;





// Namespace
namespace MyApp.Pages
{





    // Bilder bearbeitet
    public partial class Thumbnails : PhoneApplicationPage
    {





        // Variablen
        // ---------------------------------------------------------------------------------------------------
        // Liste der Bilder
        ObservableCollection<ClassThumbnails> listThumbnails = new ObservableCollection<ClassThumbnails>();


        // Liste der Sperrbildschirme
        ObservableCollection<ClassLockScreens> listLockScreens = new ObservableCollection<ClassLockScreens>();


        // Variablen zum steuern der Seite
        string pagePart = "start";


        // Iso Store Variablen
        IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication();
        IsolatedStorageFileStream filestream;
        StreamReader streamReader;
        StreamWriter streamWriter;
        // ---------------------------------------------------------------------------------------------------





        // Klasse erstellen
        // ---------------------------------------------------------------------------------------------------
        public Thumbnails()
        {
            // XAML initialisieren
            InitializeComponent();
        }
        // ---------------------------------------------------------------------------------------------------





        // Wird bei jedem Aufruf der Seite ausgeführt
        // ---------------------------------------------------------------------------------------------------
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Lade Grid verbergen
            grLoading.Visibility = System.Windows.Visibility.Collapsed;


            // App Bar hinzufügen
            appBar();


            // Thumbnails laden
            loadThumbnails();


            // Liste der Lock Screens erstellen
            listLockScreens.Clear();
            string[] arFolders = file.GetDirectoryNames("/LockScreens/");
            for (int i = 0; i < arFolders.Count(); i++)
            {
                // Wenn Name nicht gleich Name des geöffneten Lock Screens
                if (MainPage.folderName != arFolders[i])
                {
                    listLockScreens.Add(new ClassLockScreens(i, arFolders[i], false));
                }
            }


            // Liste der Lock Screens erstellen
            lbLockScreens.ItemsSource = listLockScreens;


            // Wenn Bilder verfügbar
            if (listThumbnails.Count() > 0)
            {
                // Info unsichtbar machen
                grNoPictures.Visibility = System.Windows.Visibility.Collapsed;
            }
            // Wenn keine Bilder verfügbar
            else
            {
                // Info Sichtbar machen
                grNoPictures.Visibility = System.Windows.Visibility.Visible;
            }


            // Seitenteil festlegen
            pagePart = "thumbnails";
        }
        // ---------------------------------------------------------------------------------------------------





        // Liste der Thumbnails laden
        // ---------------------------------------------------------------------------------------------------
        void loadThumbnails()
        {
            // Liste leeren
            listThumbnails.Clear();


            // Wenn quadratisch
            if (MainPage.format == "Square")
            {
                // Überschrift anpassen
                tbHeaderFormat.Text = MyApp.Resources.AppResources.X002_squareFormat;
                if (MainPage.cImagesSquare == 1)
                {
                    tbHeaderImages.Text = MainPage.cImagesSquare.ToString() + " " + MyApp.Resources.AppResources.X002_picture;
                }
                else
                {
                    tbHeaderImages.Text = MainPage.cImagesSquare.ToString() + " " + MyApp.Resources.AppResources.X002_pictures;
                }

                // Bilder durchlaufen
                for (int i = 1; i < (MainPage.arImagesSquare.Count() - 1); i++)
                {
                    // Versuchen Thumbnail zu laden
                    try
                    {
                        //Bilder laden
                        byte[] data1;
                        using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
                        {
                            using (IsolatedStorageFileStream isfs = isf.OpenFile("/LockScreens/" + MainPage.folderName + "/Square/Thumbnails/" + MainPage.arImagesSquare[i], FileMode.Open, FileAccess.Read))
                            {
                                data1 = new byte[isfs.Length];
                                isfs.Read(data1, 0, data1.Length);
                                isfs.Close();
                            }
                        }
                        MemoryStream ms = new MemoryStream(data1);
                        BitmapImage bi = new BitmapImage();
                        bi.SetSource(ms);

                        // Thumbnail der Liste hinzufügen
                        listThumbnails.Add(new ClassThumbnails((i - 1), MainPage.arImagesSquare[i], bi));
                    }
                    catch
                    {
                    }
                }
            }


            // Wenn querformat
            else if (MainPage.format == "Landscape")
            {
                // Überschrift anpasse
                tbHeaderFormat.Text = MyApp.Resources.AppResources.X002_landscapeFormat;
                if (MainPage.cImagesLandscape == 1)
                {
                    tbHeaderImages.Text = MainPage.cImagesLandscape.ToString() + " " + MyApp.Resources.AppResources.X002_picture;
                }
                else
                {
                    tbHeaderImages.Text = MainPage.cImagesLandscape.ToString() + " " + MyApp.Resources.AppResources.X002_pictures;
                }

                // Bilder durchlaufen
                for (int i = 1; i < (MainPage.arImagesLandscape.Count() - 1); i++)
                {
                    // Versuchen Thumbnail zu laden
                    try
                    {
                        //Bilder laden
                        byte[] data1;
                        using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
                        {
                            using (IsolatedStorageFileStream isfs = isf.OpenFile("/LockScreens/" + MainPage.folderName + "/Landscape/Thumbnails/" + MainPage.arImagesLandscape[i], FileMode.Open, FileAccess.Read))
                            {
                                data1 = new byte[isfs.Length];
                                isfs.Read(data1, 0, data1.Length);
                                isfs.Close();
                            }
                        }
                        MemoryStream ms = new MemoryStream(data1);
                        BitmapImage bi = new BitmapImage();
                        bi.SetSource(ms);

                        // Thumbnail der Liste hinzufügen
                        listThumbnails.Add(new ClassThumbnails((i - 1), MainPage.arImagesLandscape[i], bi));
                    }
                    catch
                    {
                    }
                }
            }


            // Wenn Hochformat
            else if (MainPage.format == "Portrait")
            {
                // Überschrift anpassen
                tbHeaderFormat.Text = MyApp.Resources.AppResources.X002_portraitFormat;
                if (MainPage.cImagesPortrait == 1)
                {
                    tbHeaderImages.Text = MainPage.cImagesPortrait.ToString() + " " + MyApp.Resources.AppResources.X002_picture;
                }
                else
                {
                    tbHeaderImages.Text = MainPage.cImagesPortrait.ToString() + " " + MyApp.Resources.AppResources.X002_pictures;
                }

                // Bilder durchlaufen
                for (int i = 1; i < (MainPage.arImagesPortrait.Count() - 1); i++)
                {
                    // Versuchen Thumbnail zu laden
                    try
                    {
                        //Bilder laden
                        byte[] data1;
                        using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
                        {
                            using (IsolatedStorageFileStream isfs = isf.OpenFile("/LockScreens/" + MainPage.folderName + "/Portrait/Thumbnails/" + MainPage.arImagesPortrait[i], FileMode.Open, FileAccess.Read))
                            {
                                data1 = new byte[isfs.Length];
                                isfs.Read(data1, 0, data1.Length);
                                isfs.Close();
                            }
                        }
                        MemoryStream ms = new MemoryStream(data1);
                        BitmapImage bi = new BitmapImage();
                        bi.SetSource(ms);

                        // Thumbnail der Liste hinzufügen
                        listThumbnails.Add(new ClassThumbnails((i - 1), MainPage.arImagesPortrait[i], bi));
                    }
                    catch
                    {
                    }
                }
            }


            // Wenn Hintergrundbilder
            else if (MainPage.format == "Background")
            {
                // Überschrift anpassen
                tbHeaderFormat.Text = MyApp.Resources.AppResources.X002_lpBackgroundPictures;
                if (MainPage.cImagesBackground == 1)
                {
                    tbHeaderImages.Text = MainPage.cImagesBackground.ToString() + " " + MyApp.Resources.AppResources.X002_picture;
                }
                else
                {
                    tbHeaderImages.Text = MainPage.cImagesBackground.ToString() + " " + MyApp.Resources.AppResources.X002_pictures;
                }

                // Bilder durchlaufen
                for (int i = 1; i < (MainPage.arImagesBackground.Count() - 1); i++)
                {
                    // Versuchen Thumbnail zu laden
                    try
                    {
                        //Bilder laden
                        byte[] data1;
                        using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
                        {
                            using (IsolatedStorageFileStream isfs = isf.OpenFile("/LockScreens/" + MainPage.folderName + "/Background/Thumbnails/" + MainPage.arImagesBackground[i], FileMode.Open, FileAccess.Read))
                            {
                                data1 = new byte[isfs.Length];
                                isfs.Read(data1, 0, data1.Length);
                                isfs.Close();
                            }
                        }
                        MemoryStream ms = new MemoryStream(data1);
                        BitmapImage bi = new BitmapImage();
                        bi.SetSource(ms);

                        // Thumbnail der Liste hinzufügen
                        listThumbnails.Add(new ClassThumbnails((i - 1), MainPage.arImagesBackground[i], bi));
                    }
                    catch
                    {
                    }
                }
            }


            // Wenn Benutzerbilder
            else if (MainPage.format == "User")
            {
                // Überschrift anpassen
                tbHeaderFormat.Text = MyApp.Resources.AppResources.X002_userPictures;
                if (MainPage.cImagesUser == 1)
                {
                    tbHeaderImages.Text = MainPage.cImagesUser.ToString() + " " + MyApp.Resources.AppResources.X002_picture;
                }
                else
                {
                    tbHeaderImages.Text = MainPage.cImagesUser.ToString() + " " + MyApp.Resources.AppResources.X002_pictures;
                }

                // Bilder durchlaufen
                for (int i = 1; i < (MainPage.arImagesUser.Count() - 1); i++)
                {
                    // Versuchen Thumbnail zu laden
                    try
                    {
                        //Bilder laden
                        byte[] data1;
                        using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
                        {
                            using (IsolatedStorageFileStream isfs = isf.OpenFile("/LockScreens/" + MainPage.folderName + "/User/Thumbnails/" + MainPage.arImagesUser[i], FileMode.Open, FileAccess.Read))
                            {
                                data1 = new byte[isfs.Length];
                                isfs.Read(data1, 0, data1.Length);
                                isfs.Close();
                            }
                        }
                        MemoryStream ms = new MemoryStream(data1);
                        BitmapImage bi = new BitmapImage();
                        bi.SetSource(ms);

                        // Thumbnail der Liste hinzufügen
                        listThumbnails.Add(new ClassThumbnails((i - 1), MainPage.arImagesUser[i], bi));
                    }
                    catch
                    {
                    }
                }
            }


            // Listbox mit Liste verknüpfen
            lbThumbnails.ItemsSource = listThumbnails;
        }
        // ---------------------------------------------------------------------------------------------------





        // AppBar // Hinzufügen // Mehrfach hinzufügen // Kopieren // Löschen // Alles anwählen // Alles abwählen
        // ---------------------------------------------------------------------------------------------------
        void appBar()
        {
            //neue AppBar anlegen
            ApplicationBar = new ApplicationBar();
            ApplicationBar.IsMenuEnabled = true;
            ApplicationBar.IsVisible = true;

            //AppBar Buttons anlegen
            ApplicationBarIconButton button0 = new ApplicationBarIconButton(new Uri("/Images/appbar.add.image.single.png", UriKind.Relative));
            button0.Text = MyApp.Resources.AppResources.X002_add;
            ApplicationBar.Buttons.Add(button0);

            ApplicationBarIconButton button1 = new ApplicationBarIconButton(new Uri("/Images/appbar.add.image.multiple.png", UriKind.Relative));
            button1.Text = MyApp.Resources.AppResources.X002_add + " +++";
            ApplicationBar.Buttons.Add(button1);

            ApplicationBarIconButton button2 = new ApplicationBarIconButton(new Uri("/Images/appbar.copy.png", UriKind.Relative));
            button2.Text = MyApp.Resources.AppResources.X002_copy;
            ApplicationBar.Buttons.Add(button2);

            ApplicationBarIconButton button3 = new ApplicationBarIconButton(new Uri("/Images/appbar.delete.png", UriKind.Relative));
            button3.Text = MyApp.Resources.AppResources.X002_delete;
            ApplicationBar.Buttons.Add(button3);

            //AppBar Menü Items anlegen
            ApplicationBarMenuItem item0 = new ApplicationBarMenuItem(MyApp.Resources.AppResources.X002_add);
            ApplicationBar.MenuItems.Add(item0);

            ApplicationBarMenuItem item1 = new ApplicationBarMenuItem(MyApp.Resources.AppResources.X002_add + " +++");
            ApplicationBar.MenuItems.Add(item1);

            ApplicationBarMenuItem item2 = new ApplicationBarMenuItem(MyApp.Resources.AppResources.X002_copy);
            ApplicationBar.MenuItems.Add(item2);

            ApplicationBarMenuItem item3 = new ApplicationBarMenuItem(MyApp.Resources.AppResources.X002_delete);
            ApplicationBar.MenuItems.Add(item3);

            ApplicationBarMenuItem item4 = new ApplicationBarMenuItem(MyApp.Resources.AppResources.X002_selectAll);
            ApplicationBar.MenuItems.Add(item4);

            ApplicationBarMenuItem item5 = new ApplicationBarMenuItem(MyApp.Resources.AppResources.X002_unselectAll);
            ApplicationBar.MenuItems.Add(item5);

            //AppBar Funktionen festlegen
            (ApplicationBar.Buttons[0] as ApplicationBarIconButton).Click += btnAddSingle;
            (ApplicationBar.Buttons[1] as ApplicationBarIconButton).Click += btnAdd;
            (ApplicationBar.Buttons[2] as ApplicationBarIconButton).Click += btnCopy;
            (ApplicationBar.Buttons[3] as ApplicationBarIconButton).Click += btnDelete;

            //AppMenü Funktionen festlegen
            (ApplicationBar.MenuItems[0] as ApplicationBarMenuItem).Click += btnAddSingle;
            (ApplicationBar.MenuItems[1] as ApplicationBarMenuItem).Click += btnAdd;
            (ApplicationBar.MenuItems[2] as ApplicationBarMenuItem).Click += btnCopy;
            (ApplicationBar.MenuItems[3] as ApplicationBarMenuItem).Click += btnDelete;
            (ApplicationBar.MenuItems[4] as ApplicationBarMenuItem).Click += btnSelect;
            (ApplicationBar.MenuItems[5] as ApplicationBarMenuItem).Click += btnUnselect;
        }
        // ---------------------------------------------------------------------------------------------------





        // Buttons AppBar
        //---------------------------------------------------------------------------------------------------------------------------------
        // Button // Bilder hizufügen einzeln
        private async void btnAddSingle(object sender, EventArgs e)
        {
            // Lade Grid anzeigen
            grLoading.Visibility = System.Windows.Visibility.Visible;

            // Angeben wie Bilder geladen werden
            MainPage.addMultipleImage = false;

            // Angeben das Task nicht aktiv
            MainPage.photoChooserTaskIsRunning = false;

            // Bilder hinzufügen öffnen
            NavigationService.Navigate(new Uri("/Pages/AddPictures.xaml", UriKind.Relative));
        }



        // Button // Bilder hizufügen mehrere
        private async void btnAdd(object sender, EventArgs e)
        {
            // Lade Grid anzeigen
            grLoading.Visibility = System.Windows.Visibility.Visible;

            // Angeben wie Bilder geladen werden
            MainPage.addMultipleImage = true;

            // Angeben das Task nicht aktiv
            MainPage.photoChooserTaskIsRunning = false;

            // Bilder hinzufügen öffnen
            NavigationService.Navigate(new Uri("/Pages/AddPictures.xaml", UriKind.Relative));
        }



        // Button // Kopieren
        private void btnCopy(object sender, EventArgs e)
        {
            // Wenn keine Bilder ausgewählt
            if (lbThumbnails.SelectedItems.Count < 1)
            {
                // Benachrichtigung ausgeben
                MessageBox.Show(MyApp.Resources.AppResources.X002_nothingSelected);
            }


            // Wenn Bilder ausgewählt
            else
            {
                // Wenn keine weiteren Sperrbildschirme vorhanden
                if (listLockScreens.Count == 0)
                {
                    // Benachrichtigung ausgeben
                    MessageBox.Show(MyApp.Resources.AppResources.X002_noLockScreens);
                }

                // Wenn weitere Sperrbildschirme vorhanden
                else
                {
                    // Auswahl der Lock Screens anzeigen
                    grThumbnails.Visibility = System.Windows.Visibility.Collapsed;
                    grLockScreens.Visibility = System.Windows.Visibility.Visible;

                    // App Bar unsichtbar machen
                    ApplicationBar.IsVisible = false;

                    // Seitenteil festlegen
                    pagePart = "lockScreens";
                }
            }
        }



        // Button // Löschen
        private async void btnDelete(object sender, EventArgs e)
        {
            // Wenn keine Bilder ausgewählt
            if (lbThumbnails.SelectedItems.Count < 1)
            {
                // Benachrichtigung ausgeben
                MessageBox.Show(MyApp.Resources.AppResources.X002_nothingSelected);
            }


            // Wenn Bilder ausgewählt
            else
            {
                // Benachrichtigung ausgeben
                if (MessageBox.Show("", "⚠ " + MyApp.Resources.AppResources.X002_msgDelete, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    // Anzahl Ausgewählter Bilder ermitteln
                    int deletePicturesC = lbThumbnails.SelectedItems.Count;

                    // Ausgewählte Bilder durchlaufen
                    for (int i = 0; i < deletePicturesC; i++)
                    {
                        // Bilder Name ermitteln
                        string deletePictureName = (lbThumbnails.SelectedItems[i] as ClassThumbnails).name;

                        // Bilder löschen // Square
                        if (MainPage.format == "Square")
                        {
                            // Bild Dateien löschen
                            file.DeleteFile("/LockScreens/" + MainPage.folderName + "/Square/Large/" + deletePictureName);
                            file.DeleteFile("/LockScreens/" + MainPage.folderName + "/Square/Medium/" + deletePictureName);
                            file.DeleteFile("/LockScreens/" + MainPage.folderName + "/Square/Small/" + deletePictureName);
                            file.DeleteFile("/LockScreens/" + MainPage.folderName + "/Square/Thumbnails/" + deletePictureName);

                            // Bild aus Bilderliste löschen
                            MainPage.imagesListSquare = Regex.Replace(MainPage.imagesListSquare, "~" + deletePictureName + "~", "~");
                            await ClassFileMamagment.loadCreateOverwrite("/LockScreens/" + MainPage.folderName + "/Square/ImagesList.txt", MainPage.imagesListSquare, true);

                            // Bilder Count neu erstellen
                            MainPage.arImagesSquare = Regex.Split(MainPage.imagesListSquare, "~");
                            MainPage.cImagesSquare = MainPage.arImagesSquare.Count() - 2;
                        }


                        // Bilder löschen // Landscape
                        if (MainPage.format == "Landscape")
                        {
                            // Bild Dateien löschen
                            file.DeleteFile("/LockScreens/" + MainPage.folderName + "/Landscape/Large/" + deletePictureName);
                            file.DeleteFile("/LockScreens/" + MainPage.folderName + "/Landscape/Small/" + deletePictureName);
                            file.DeleteFile("/LockScreens/" + MainPage.folderName + "/Landscape/Thumbnails/" + deletePictureName);

                            // Bild aus Bilderliste löschen
                            MainPage.imagesListLandscape = Regex.Replace(MainPage.imagesListLandscape, "~" + deletePictureName + "~", "~");

                            await ClassFileMamagment.loadCreateOverwrite("/LockScreens/" + MainPage.folderName + "/Landscape/ImagesList.txt", MainPage.imagesListLandscape, true);

                            // Bilder Count neu erstellen
                            MainPage.arImagesLandscape = Regex.Split(MainPage.imagesListLandscape, "~");
                            MainPage.cImagesLandscape = MainPage.arImagesLandscape.Count() - 2;
                        }


                        // Bilder löschen // Portrait
                        if (MainPage.format == "Portrait")
                        {
                            // Bild Dateien löschen
                            file.DeleteFile("/LockScreens/" + MainPage.folderName + "/Portrait/Large/" + deletePictureName);
                            file.DeleteFile("/LockScreens/" + MainPage.folderName + "/Portrait/Small/" + deletePictureName);
                            file.DeleteFile("/LockScreens/" + MainPage.folderName + "/Portrait/Thumbnails/" + deletePictureName);

                            // Bild aus Bilderliste löschen
                            MainPage.imagesListPortrait = Regex.Replace(MainPage.imagesListPortrait, "~" + deletePictureName + "~", "~");
                            await ClassFileMamagment.loadCreateOverwrite("/LockScreens/" + MainPage.folderName + "/Portrait/ImagesList.txt", MainPage.imagesListPortrait, true);

                            // Bilder Count neu erstellen
                            MainPage.arImagesPortrait = Regex.Split(MainPage.imagesListPortrait, "~");
                            MainPage.cImagesPortrait = MainPage.arImagesPortrait.Count() - 2;
                        }


                        // Bilder löschen // Hintergrund
                        if (MainPage.format == "Background")
                        {
                            // Bild Dateien löschen
                            file.DeleteFile("/LockScreens/" + MainPage.folderName + "/Background/Pictures/" + deletePictureName);
                            file.DeleteFile("/LockScreens/" + MainPage.folderName + "/Background/Thumbnails/" + deletePictureName);

                            // Bild aus Bilderliste löschen
                            MainPage.imagesListBackground = Regex.Replace(MainPage.imagesListBackground, "~" + deletePictureName + "~", "~");
                            await ClassFileMamagment.loadCreateOverwrite("/LockScreens/" + MainPage.folderName + "/Background/ImagesList.txt", MainPage.imagesListBackground, true);

                            // Bilder Count neu erstellen
                            MainPage.arImagesBackground = Regex.Split(MainPage.imagesListBackground, "~");
                            MainPage.cImagesBackground = MainPage.arImagesBackground.Count() - 2;
                        }


                        // Bilder löschen // Benutzer
                        if (MainPage.format == "User")
                        {
                            // Bild Dateien löschen
                            file.DeleteFile("/LockScreens/" + MainPage.folderName + "/User/Pictures/" + deletePictureName);
                            file.DeleteFile("/LockScreens/" + MainPage.folderName + "/User/Thumbnails/" + deletePictureName);

                            // Bild aus Bilderliste löschen
                            MainPage.imagesListUser = Regex.Replace(MainPage.imagesListUser, "~" + deletePictureName + "~", "~");
                            await ClassFileMamagment.loadCreateOverwrite("/LockScreens/" + MainPage.folderName + "/User/ImagesList.txt", MainPage.imagesListUser, true);

                            // Bilder Count neu erstellen
                            MainPage.arImagesUser = Regex.Split(MainPage.imagesListUser, "~");
                            MainPage.cImagesUser = MainPage.arImagesUser.Count() - 2;
                        }
                    }

                    // Thumbnais neu laden
                    loadThumbnails();

                    // Wenn Bilder verfügbar
                    if (listThumbnails.Count() > 0)
                    {
                        // Info unsichtbar machen
                        grNoPictures.Visibility = System.Windows.Visibility.Collapsed;
                    }
                    // Wenn keine Bilder verfügbar
                    else
                    {
                        // Info Sichtbar machen
                        grNoPictures.Visibility = System.Windows.Visibility.Visible;
                    }
                }
            }
        }



        // Button // Alles auswählen
        private void btnSelect(object sender, EventArgs e)
        {
            // Alles auswählen
            lbThumbnails.SelectAll();
        }



        // Button // Alles abwählen
        private void btnUnselect(object sender, EventArgs e)
        {
            // Alles abwählen
            try
            {
                lbThumbnails.SelectedIndex = -1;
            }
            catch
            {
            }
        }
        //---------------------------------------------------------------------------------------------------------------------------------





        // Auswahl Sperrbildschirme // Bilder kopieren
        //---------------------------------------------------------------------------------------------------------------------------------
        private async void lbLockScreens_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Wenn Auswahl erfolgt
            if (lbLockScreens.SelectedIndex != -1)
            {
                // Benachrichtigung ausgeben
                if (MessageBox.Show("", "⚠ " + MyApp.Resources.AppResources.X002_msgCopy, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    // Name des Ziel Sperrbildschirms emitteln
                    string copyToName = (lbLockScreens.SelectedItem as ClassLockScreens).name;

                    // Anzahl Ausgewählter Bilder ermitteln
                    int copyPicturesC = lbThumbnails.SelectedItems.Count;

                    // Ausgewählte Bilder durchlaufen
                    for (int i = 0; i < copyPicturesC; i++)
                    {
                        // Bilder Name ermitteln
                        string copyPictureName = (lbThumbnails.SelectedItems[i] as ClassThumbnails).name;

                        // Bilder kopieren // Square
                        if (MainPage.format == "Square")
                        {
                            // Bild Dateien kopieren
                            file.CopyFile("/LockScreens/" + MainPage.folderName + "/Square/Large/" + copyPictureName, "/LockScreens/" + copyToName + "/Square/Large/" + copyPictureName, true);
                            file.CopyFile("/LockScreens/" + MainPage.folderName + "/Square/Medium/" + copyPictureName, "/LockScreens/" + copyToName + "/Square/Medium/" + copyPictureName, true);
                            file.CopyFile("/LockScreens/" + MainPage.folderName + "/Square/Small/" + copyPictureName, "/LockScreens/" + copyToName + "/Square/Small/" + copyPictureName, true);
                            file.CopyFile("/LockScreens/" + MainPage.folderName + "/Square/Thumbnails/" + copyPictureName, "/LockScreens/" + copyToName + "/Square/Thumbnails/" + copyPictureName, true);

                            // Bild der Bilderliste hinzufügen
                            string imagesListSquare = await ClassFileMamagment.loadCreateOverwrite("/LockScreens/" + copyToName + "/Square/ImagesList.txt", "", false);
                            imagesListSquare = "~" + copyPictureName + imagesListSquare;
                            await ClassFileMamagment.loadCreateOverwrite("/LockScreens/" + copyToName + "/Square/ImagesList.txt", imagesListSquare, true);
                        }


                        // Bilder kopieren // Landscape
                        if (MainPage.format == "Landscape")
                        {
                            // Bild Dateien kopieren
                            file.CopyFile("/LockScreens/" + MainPage.folderName + "/Landscape/Large/" + copyPictureName, "/LockScreens/" + copyToName + "/Landscape/Large/" + copyPictureName, true);
                            file.CopyFile("/LockScreens/" + MainPage.folderName + "/Landscape/Small/" + copyPictureName, "/LockScreens/" + copyToName + "/Landscape/Small/" + copyPictureName, true);
                            file.CopyFile("/LockScreens/" + MainPage.folderName + "/Landscape/Thumbnails/" + copyPictureName, "/LockScreens/" + copyToName + "/Landscape/Thumbnails/" + copyPictureName, true);

                            // Bild der Bilderliste hinzufügen
                            string imagesListLandscape = await ClassFileMamagment.loadCreateOverwrite("/LockScreens/" + copyToName + "/Landscape/ImagesList.txt", "", false);
                            imagesListLandscape = "~" + copyPictureName + imagesListLandscape;
                            await ClassFileMamagment.loadCreateOverwrite("/LockScreens/" + copyToName + "/Landscape/ImagesList.txt", imagesListLandscape, true);
                        }


                        // Bilder kopieren // Portrait
                        if (MainPage.format == "Portrait")
                        {
                            // Bild Dateien kopieren
                            file.CopyFile("/LockScreens/" + MainPage.folderName + "/Portrait/Large/" + copyPictureName, "/LockScreens/" + copyToName + "/Portrait/Large/" + copyPictureName, true);
                            file.CopyFile("/LockScreens/" + MainPage.folderName + "/Portrait/Small/" + copyPictureName, "/LockScreens/" + copyToName + "/Portrait/Small/" + copyPictureName, true);
                            file.CopyFile("/LockScreens/" + MainPage.folderName + "/Portrait/Thumbnails/" + copyPictureName, "/LockScreens/" + copyToName + "/Portrait/Thumbnails/" + copyPictureName, true);

                            // Bild der Bilderliste hinzufügen
                            string imagesListPortrait = await ClassFileMamagment.loadCreateOverwrite("/LockScreens/" + copyToName + "/Portrait/ImagesList.txt", "", false);
                            imagesListPortrait = "~" + copyPictureName + imagesListPortrait;
                            await ClassFileMamagment.loadCreateOverwrite("/LockScreens/" + copyToName + "/Portrait/ImagesList.txt", imagesListPortrait, true);
                        }


                        // Bilder kopieren // Hintergrund
                        if (MainPage.format == "Background")
                        {
                            // Bild Dateien kopieren
                            file.CopyFile("/LockScreens/" + MainPage.folderName + "/Background/Pictures/" + copyPictureName, "/LockScreens/" + copyToName + "/Background/Pictures/" + copyPictureName, true);
                            file.CopyFile("/LockScreens/" + MainPage.folderName + "/Background/Thumbnails/" + copyPictureName, "/LockScreens/" + copyToName + "/Background/Thumbnails/" + copyPictureName, true);

                            // Bild der Bilderliste hinzufügen
                            string imagesListBackground = await ClassFileMamagment.loadCreateOverwrite("/LockScreens/" + copyToName + "/Background/ImagesList.txt", "", false);
                            imagesListBackground = "~" + copyPictureName + imagesListBackground;
                            await ClassFileMamagment.loadCreateOverwrite("/LockScreens/" + copyToName + "/Background/ImagesList.txt", imagesListBackground, true);
                        }

                        // Bilder kopieren // Benutzer
                        if (MainPage.format == "User")
                        {
                            // Bild Dateien kopieren
                            file.CopyFile("/LockScreens/" + MainPage.folderName + "/User/Pictures/" + copyPictureName, "/LockScreens/" + copyToName + "/User/Pictures/" + copyPictureName, true);
                            file.CopyFile("/LockScreens/" + MainPage.folderName + "/User/Thumbnails/" + copyPictureName, "/LockScreens/" + copyToName + "/User/Thumbnails/" + copyPictureName, true);

                            // Bild der Bilderliste hinzufügen
                            string imagesListUser = await ClassFileMamagment.loadCreateOverwrite("/LockScreens/" + copyToName + "/User/ImagesList.txt", "", false);
                            imagesListUser = "~" + copyPictureName + imagesListUser;
                            await ClassFileMamagment.loadCreateOverwrite("/LockScreens/" + copyToName + "/User/ImagesList.txt", imagesListUser, true);
                        }
                    }


                    // Auswahl der Thumbnails anzeigen
                    grThumbnails.Visibility = System.Windows.Visibility.Visible;
                    grLockScreens.Visibility = System.Windows.Visibility.Collapsed;

                    // App Bar sichtbar machen
                    ApplicationBar.IsVisible = true;

                    // Seitenteil festlegen
                    pagePart = "thumbnails";
                }
            }


            // Auswahl der Sperrbildschirme aufheben
            try
            {
                lbLockScreens.SelectedIndex = -1;
            }
            catch
            {
            }
        }
        //---------------------------------------------------------------------------------------------------------------------------------





        // Back Button
        //--------------------------------------------------------------------------------------------------------
        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            // Wenn Schnitt Panel offen
            if (pagePart == "lockScreens")
            {
                // Navigation zurück abbrechen
                e.Cancel = true;


                // Thumbnails anzeigen
                grThumbnails.Visibility = System.Windows.Visibility.Visible;
                grLockScreens.Visibility = System.Windows.Visibility.Collapsed;


                // App Bar sichtbar machen
                ApplicationBar.IsVisible = true;


                // Seitenteil anzeigen
                pagePart = "thumbnails";
            }
        }
        //--------------------------------------------------------------------------------------------------------





    }
}