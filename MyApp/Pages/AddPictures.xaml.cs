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
using System.IO.IsolatedStorage;
using System.IO;
using ImageTools.IO.Png;
using Microsoft.Xna.Framework.Media;
using System.Windows.Media.Imaging;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Windows.Storage.Pickers;
using Windows.Storage;
using System.Windows.Threading;





// Namespace
namespace MyApp.Pages
{





    // Seite zum hinzufügen der Bilder
    public partial class AddPictures : PhoneApplicationPage
    {





        // Variablen
        // ---------------------------------------------------------------------------------------------------
        // Liste der Bilder
        ObservableCollection<ClassMediaLibraryImages> listImages = new ObservableCollection<ClassMediaLibraryImages>();


        // Iso Store Variablen
        IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication();
        IsolatedStorageFileStream filestream;
        StreamReader streamReader;
        StreamWriter streamWriter;


        // Variablen zum steuern der Seite
        string pagePart = "start";


        // Variablen zum laden der Bilder
        int imgGes = 0;
        int imgStart = 0;
        int imgEnd = 0;
        int imgArea = 0;
        int imgAreaGes = 0;
        string imgPictures = "all";


        // Variablen zur Auswahl der Bilder
        int[] addImages;
        int addImagesC = -1;
        int addNow = -1;


        // Variablen zum verarbeiten der Bilder
        WriteableBitmap cutBitmap = new WriteableBitmap(0, 0);
        int tileLarge = 475;
        int tileMedium = 229;
        int tileSmall = 106;
        double width100 = -1;
        double height100 = -1;
        double backgroundHeight = 1280;
        double backgroundWidth = 768;
        double userHeight = 240;
        double userWidth = 240;
        int jpgQuality = 80;


        // PhotoCooserTask erstellen
        PhotoChooserTask photoChooserTask;


        // Timer der zurück geht
        DispatcherTimer timer = new DispatcherTimer();
        // ---------------------------------------------------------------------------------------------------





        // Klasse erstellen
        // ---------------------------------------------------------------------------------------------------
        public AddPictures()
        {
            // XAML initialisieren
            InitializeComponent();

            //PhotoCooser Task
            photoChooserTask = new PhotoChooserTask();

            //Angeben was PhotoCooserTask ausführt wenn Bild ausgewählt
            photoChooserTask.Completed += new EventHandler<PhotoResult>(photoChooserTask_Completed);

            // Timer einstellen
            timer.Stop();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 200);
            timer.Tick += timer_Tick;
        }
        // ---------------------------------------------------------------------------------------------------





        // Wird bei jedem Aufruf der Seite ausgeführt
        // ---------------------------------------------------------------------------------------------------
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // png Encoder undpng Decoder erstellen
            ImageTools.IO.Decoders.AddDecoder<PngDecoder>();
            ImageTools.IO.Encoders.AddEncoder<PngEncoder>();


            // Wenn Format 480X800 // 15:9
            if (MainPage.resolution == "480X800")
            {
                tileLarge = 297;
                tileMedium = 143;
                tileSmall = 66;
                backgroundHeight = 800;
                backgroundWidth = 480;
                userHeight = 150;
                userWidth = 150;
            }
            // Wenn Format 768X1280 // 15:9
            else if (MainPage.resolution == "768X1280")
            {
                tileLarge = 475;
                tileMedium = 229;
                tileSmall = 106;
                backgroundHeight = 1280;
                backgroundWidth = 768;
                userHeight = 240;
                userWidth = 240;
            }
            // Wenn Format 720X1280 // 16:9
            else if (MainPage.resolution == "720X1280")
            {
                tileLarge = 444;
                tileMedium = 214;
                tileSmall = 99;
                backgroundHeight = 1280;
                backgroundWidth = 720;
                userHeight = 225;
                userWidth = 225;
            }
            // Wenn Format 1080X1920 // 16:9
            else if (MainPage.resolution == "1080X1920")
            {
                tileLarge = 667;
                tileMedium = 321;
                tileSmall = 148;
                backgroundHeight = 1920;
                backgroundWidth = 1080;
                userHeight = 338;
                userWidth = 338;
            }


            // Wenn quadratisch // Texte anpassen
            if (MainPage.format == "Square")
            {
                // Überschrift anpassen
                tbHeaderFormat.Text = MyApp.Resources.AppResources.X002_squareFormat;
            }


            // Wenn querformat // Texte anpassen
            else if (MainPage.format == "Landscape")
            {
                // Überschrift anpasse
                tbHeaderFormat.Text = MyApp.Resources.AppResources.X002_landscapeFormat;
            }


            // Wenn Hochformat // Texte anpassen
            else if (MainPage.format == "Portrait")
            {
                // Überschrift anpassen
                tbHeaderFormat.Text = MyApp.Resources.AppResources.X002_portraitFormat;
            }


            // Wenn Hintergrund // Texte anpassen
            else if (MainPage.format == "Background")
            {
                // Überschrift anpassen
                tbHeaderFormat.Text = MyApp.Resources.AppResources.X002_lpBackgroundPictures;
            }


            // Wenn Benutzer // Texte anpassen
            else if (MainPage.format == "User")
            {
                // Überschrift anpassen
                tbHeaderFormat.Text = MyApp.Resources.AppResources.X002_userPictures;
            }


            // Bei mehrfacher Auswahl
            if (MainPage.addMultipleImage)
            {

                // AppBar erstellen
                appBar1();


                // Bilder laden
                loadImages("first");


                // Seitenteil festlegen
                pagePart = "select";
            }


            // Bei einfacher Auswahl
            else
            {
                // Wenn Task noch nicht aktiv
                if (!MainPage.photoChooserTaskIsRunning)
                {
                    // Angeben das Task aktiv
                    MainPage.photoChooserTaskIsRunning = true;

                    // Versuchen Bild einzeln zu laden
                    try
                    {
                        photoChooserTask.Show();
                    }
                    catch (System.InvalidOperationException ex)
                    {
                        // Catch the exception, but no handling is necessary.
                    }
                }
            }
        }
        // ---------------------------------------------------------------------------------------------------





        // Listbox // Bilder laden // Bilder durchlaufen
        // ---------------------------------------------------------------------------------------------------
        async void loadImages(string action)
        {
            // Listbox leeren
            listImages.Clear();



            // Wenn alle Bilder ausgewählt werden
            if (action == "allPictures")
            {
                imgPictures = "all";
                action = "first";
            }



            // Wenn gespeicherte Bilder ausgewählt werden
            if (action == "savedPictures")
            {
                imgPictures = "saved";
                action = "first";
            }



            // Bei allen Bildern
            if (imgPictures == "all")
            {
                // Bei allen Bildern
                MediaLibrary mediaLibrary = new MediaLibrary();
                var pictures = mediaLibrary.Pictures;

                // Beim ersten laden
                if (action == "first")
                {
                    // Variabeln erstellen
                    imgGes = pictures.Count;
                    imgStart = imgGes;
                    if ((imgStart - 99) >= 0)
                    {
                        imgEnd = imgStart - 99;
                    }
                    else
                    {
                        imgEnd = 0;
                    }
                    imgArea = 1;
                    imgAreaGes = (imgGes / 100) + 1;
                }

                // Wenn nächste
                if (action == "next")
                {
                    // Prüfen ob möglich
                    if (imgArea < imgAreaGes)
                    {
                        imgArea++;
                        imgStart = imgStart - 100;
                        if ((imgStart - 99) >= 0)
                        {
                            imgEnd = imgStart - 99;
                        }
                        else
                        {
                            imgEnd = 0;
                        }
                    }
                }

                // Wenn vorherige
                if (action == "previous")
                {
                    // Prüfen ob möglich
                    if (imgArea > 1)
                    {
                        imgArea--;
                        imgStart = imgStart + 100;
                        if ((imgStart - 99) >= 0)
                        {
                            imgEnd = imgStart - 99;
                        }
                        else
                        {
                            imgEnd = 0;
                        }
                    }
                }

                // Bilder auslesen und in ListBox schreiben
                for (int i = imgStart; i >= imgEnd; i--)
                {
                    try
                    {
                        BitmapImage image = new BitmapImage();
                        image.SetSource(pictures[i].GetThumbnail());
                        listImages.Add(new ClassMediaLibraryImages((i), image));
                    }
                    catch
                    {
                    }
                }

                // Liste zurück nach oben setzen
                try
                {
                    lbPictures.SelectedIndex = 1;
                    lbPictures.SelectedIndex = -1;
                }
                catch
                {
                }
            }



            // Bei gespeicherten Bildern
            else
            {
                // Bei saved Pictures
                MediaLibrary mediaLibrary = new MediaLibrary();
                var pictures = mediaLibrary.SavedPictures;

                // Beim ersten laden
                if (action == "first")
                {
                    // Variabeln erstellen
                    imgGes = pictures.Count;
                    imgStart = imgGes;
                    if ((imgStart - 99) >= 0)
                    {
                        imgEnd = imgStart - 99;
                    }
                    else
                    {
                        imgEnd = 0;
                    }
                    imgArea = 1;
                    imgAreaGes = (imgGes / 100) + 1;
                }

                // Wenn nächste
                if (action == "next")
                {
                    // Prüfen ob möglich
                    if (imgArea < imgAreaGes)
                    {
                        imgArea++;
                        imgStart = imgStart - 100;
                        if ((imgStart - 99) >= 0)
                        {
                            imgEnd = imgStart - 99;
                        }
                        else
                        {
                            imgEnd = 0;
                        }
                    }
                }

                // Wenn vorherige
                if (action == "previous")
                {
                    // Prüfen ob möglich
                    if (imgArea > 1)
                    {
                        imgArea--;
                        imgStart = imgStart + 100;
                        if ((imgStart - 99) >= 0)
                        {
                            imgEnd = imgStart - 99;
                        }
                        else
                        {
                            imgEnd = 0;
                        }
                    }
                }

                // Bilder auslesen und in ListBox schreiben
                for (int i = imgStart; i >= imgEnd; i--)
                {
                    try
                    {
                        BitmapImage image = new BitmapImage();
                        image.SetSource(pictures[i].GetThumbnail());
                        listImages.Add(new ClassMediaLibraryImages((i), image));
                    }
                    catch
                    {
                    }
                }

                // Liste zurück nach oben setzen
                try
                {
                    lbPictures.SelectedIndex = 1;
                    lbPictures.SelectedIndex = -1;
                }
                catch
                {
                }
            }



            // Daten in Listbox schreiben
            lbPictures.ItemsSource = listImages;



            // AppBar erstellen
            appBar1();
        }
        //---------------------------------------------------------------------------------------------------------





        // App Bar // Vorherige Bilder // Hinzufügen // Nächste Bilder // Alle Bilder // Gespeichete Bilder 
        //---------------------------------------------------------------------------------------------------------
        void appBar1()
        {
            // Neue AppBar anlegen
            ApplicationBar = new ApplicationBar();
            ApplicationBar.IsMenuEnabled = true;
            ApplicationBar.IsVisible = true;



            // IDs zum festlegen der Aktionen erstellen
            int buttonID = 0;
            int itemID = 0;



            //AppBar //Items // Hinzufügen
            ApplicationBarMenuItem item0 = new ApplicationBarMenuItem(MyApp.Resources.AppResources.X002_add);
            ApplicationBar.MenuItems.Add(item0);
            (ApplicationBar.MenuItems[itemID] as ApplicationBarMenuItem).Click += btnAdd;
            itemID++;



            // AppBar // Button + Items // Vorherige
            if (imgArea > 1)
            {
                ApplicationBarIconButton button1 = new ApplicationBarIconButton(new Uri("/Images/appbar.arrow.left.png", UriKind.Relative));
                button1.Text = MyApp.Resources.AppResources.X002_previous;
                ApplicationBar.Buttons.Add(button1);
                (ApplicationBar.Buttons[buttonID] as ApplicationBarIconButton).Click += btnPrevious;
                buttonID++;

                ApplicationBarMenuItem item1 = new ApplicationBarMenuItem(MyApp.Resources.AppResources.X002_previous);
                ApplicationBar.MenuItems.Add(item1);
                (ApplicationBar.MenuItems[itemID] as ApplicationBarMenuItem).Click += btnPrevious;
                itemID++;
            }



            // AppBar // Button // Hinzufügen
            ApplicationBarIconButton button2 = new ApplicationBarIconButton(new Uri("/Images/appbar.add.png", UriKind.Relative));
            button2.Text = MyApp.Resources.AppResources.X002_add;
            ApplicationBar.Buttons.Add(button2);
            (ApplicationBar.Buttons[buttonID] as ApplicationBarIconButton).Click += btnAdd;
            buttonID++;



            // AppBar // Button + Item // Nächste
            if (imgArea < imgAreaGes)
            {
                ApplicationBarIconButton button3 = new ApplicationBarIconButton(new Uri("/Images/appbar.arrow.right.png", UriKind.Relative));
                button3.Text = MyApp.Resources.AppResources.X002_next;
                ApplicationBar.Buttons.Add(button3);
                (ApplicationBar.Buttons[buttonID] as ApplicationBarIconButton).Click += btnNext;
                buttonID++;

                ApplicationBarMenuItem item3 = new ApplicationBarMenuItem(MyApp.Resources.AppResources.X002_next);
                ApplicationBar.MenuItems.Add(item3);
                (ApplicationBar.MenuItems[itemID] as ApplicationBarMenuItem).Click += btnNext;
                itemID++;
            }



            // AppBar // Item // Alle Bilder
            ApplicationBarMenuItem item4 = new ApplicationBarMenuItem(MyApp.Resources.AppResources.X002_allPictures);
            ApplicationBar.MenuItems.Add(item4);
            (ApplicationBar.MenuItems[itemID] as ApplicationBarMenuItem).Click += btnAllPictures;
            itemID++;



            // AppBar // Item // Gespeicherte Bilder
            ApplicationBarMenuItem item5 = new ApplicationBarMenuItem(MyApp.Resources.AppResources.X002_savedPictures);
            ApplicationBar.MenuItems.Add(item5);
            (ApplicationBar.MenuItems[itemID] as ApplicationBarMenuItem).Click += btnSavedPictures;
            itemID++;
        }
        //---------------------------------------------------------------------------------------------------------





        // App Bar // Schneiden // Drehen // Löschen
        //---------------------------------------------------------------------------------------------------------
        void appBar2()
        {
            // Neue AppBar anlegen
            ApplicationBar = new ApplicationBar();
            ApplicationBar.IsMenuEnabled = true;
            ApplicationBar.IsVisible = true;

            // AppBar Button // Schneiden
            ApplicationBarIconButton button0 = new ApplicationBarIconButton(new Uri("/Images/appbar.cut.png", UriKind.Relative));
            button0.Text = MyApp.Resources.AppResources.X002_cut;
            ApplicationBar.Buttons.Add(button0);

            // AppBar Button // Drehen links
            ApplicationBarIconButton button1 = new ApplicationBarIconButton(new Uri("/Images/appbar.rotate.counterclockwise.png", UriKind.Relative));
            button1.Text = MyApp.Resources.AppResources.X002_left;
            ApplicationBar.Buttons.Add(button1);

            // AppBar Button // Drehen rechts
            ApplicationBarIconButton button2 = new ApplicationBarIconButton(new Uri("/Images/appbar.rotate.clockwise.png", UriKind.Relative));
            button2.Text = MyApp.Resources.AppResources.X002_right;
            ApplicationBar.Buttons.Add(button2);

            // AppBar Button // Löschen
            ApplicationBarIconButton button3 = new ApplicationBarIconButton(new Uri("/Images/appbar.delete.png", UriKind.Relative));
            button3.Text = MyApp.Resources.AppResources.X002_delete;
            ApplicationBar.Buttons.Add(button3);

            // AppBar Menü Item // Schneiden
            ApplicationBarMenuItem item0 = new ApplicationBarMenuItem(MyApp.Resources.AppResources.X002_cut);
            ApplicationBar.MenuItems.Add(item0);

            // AppBar Menü Item // Drehen links
            ApplicationBarMenuItem item1 = new ApplicationBarMenuItem(MyApp.Resources.AppResources.X002_rotateLeft);
            ApplicationBar.MenuItems.Add(item1);

            // AppBar Menü Item // Drehen rechts
            ApplicationBarMenuItem item2 = new ApplicationBarMenuItem(MyApp.Resources.AppResources.X002_rotateRight);
            ApplicationBar.MenuItems.Add(item2);

            // AppBar Menü Item // Löschen
            ApplicationBarMenuItem item3 = new ApplicationBarMenuItem(MyApp.Resources.AppResources.X002_delete);
            ApplicationBar.MenuItems.Add(item3);

            // AppBar Funktionen festlegen
            (ApplicationBar.Buttons[0] as ApplicationBarIconButton).Click += btnCut;
            (ApplicationBar.Buttons[1] as ApplicationBarIconButton).Click += btnRotateLeft;
            (ApplicationBar.Buttons[2] as ApplicationBarIconButton).Click += btnRotateRight;
            (ApplicationBar.Buttons[3] as ApplicationBarIconButton).Click += btnDelete;

            // AppMenü Funktionen festlegen
            (ApplicationBar.MenuItems[0] as ApplicationBarMenuItem).Click += btnCut;
            (ApplicationBar.MenuItems[1] as ApplicationBarMenuItem).Click += btnRotateLeft;
            (ApplicationBar.MenuItems[2] as ApplicationBarMenuItem).Click += btnRotateRight;
            (ApplicationBar.MenuItems[3] as ApplicationBarMenuItem).Click += btnDelete;
        }
        //---------------------------------------------------------------------------------------------------------





        // Buttons AppBar
        //---------------------------------------------------------------------------------------------------------
        // Button // Hinzufügen
        private void btnAdd(object sender, EventArgs e)
        {
            addPictures();
        }
        // Bilder hinzufügen
        private async void addPictures()
        {
            // Auslesen, wieviele Bilder ausgewählt sind
            addImagesC = lbPictures.SelectedItems.Count;


            // Wenn Bilder ausgewält sind
            if (addImagesC >= 1)
            {
                // Aktuellen Verlauf zurücksetzen
                addNow = 0;

                // Bilderliste erstellen
                addImages = new int[addImagesC];
                for (int i = (addImagesC - 1); i >= 0; i--)
                {
                    addImages[i] = (lbPictures.SelectedItems[i] as ClassMediaLibraryImages).id;
                }

                // Schnitt Panel anzeigen
                grCut.Visibility = System.Windows.Visibility.Visible;
                grSelect.Visibility = System.Windows.Visibility.Collapsed;

                // Bild laden
                await loadPicture();

                // Bild in Panel setzen
                pictureToPanel();

                // Seitenteil festlegen
                pagePart = "cut";
            }


            // Wenn keine Bilder ausgewählt sind
            else
            {
                // Benachrichtigung ausgeben
                MessageBox.Show(MyApp.Resources.AppResources.X002_addNoImages);
            }
        }



        // Button // Vorherige
        private void btnPrevious(object sender, EventArgs e)
        {
            // Wenn Bilder ausgewählt sind
            if (lbPictures.SelectedItems.Count > 0)
            {
                // Benachrichtigung ausgeben
                if (MessageBox.Show("", MyApp.Resources.AppResources.X002_cutSelected, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    addPictures();
                }
                else
                {
                    // Vorherige Bilder laden
                    loadImages("previous");
                }
            }
            else
            {
                // Vorherige Bilder laden
                loadImages("previous");
            }
        }



        // Button // Nächste
        private void btnNext(object sender, EventArgs e)
        {
            // Wenn Bilder ausgewählt sind
            if (lbPictures.SelectedItems.Count > 0)
            {
                // Benachrichtigung ausgeben
                if (MessageBox.Show("", MyApp.Resources.AppResources.X002_cutSelected, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    addPictures();
                }
                else
                {
                    // Nächste Bilder laden
                    loadImages("next");
                }
            }
            else
            {
                // Nächste Bilder laden
                loadImages("next");
            }
        }



        // Button alle Bilder
        private void btnAllPictures(object sender, EventArgs e)
        {
            loadImages("allPictures");
        }



        // Button gespeicherte Bilder
        private void btnSavedPictures(object sender, EventArgs e)
        {
            loadImages("savedPictures");
        }



        // Button // Schneiden
        private async void btnCut(object sender, EventArgs e)
        {
            // Bild zum speichern erstellen
            WriteableBitmap saveBitmap = new WriteableBitmap(0, 0);


            // Bild zuschneiden
            int cutX = Convert.ToInt32(((imgCut.ActualWidth - recCut.Width) / 2) + transform.TranslateX);
            int cutY = Convert.ToInt32(((imgCut.ActualHeight - recCut.Height) / 2) + transform.TranslateY);
            cutBitmap = cutBitmap.Crop(cutX, cutY, Convert.ToInt32(recCut.Width), Convert.ToInt32(recCut.Height));


            // Name erstellen
            MainPage.imagesCount++;
            string imageName = Convert.ToString(MainPage.imagesCount);
            while (imageName.Length < 8)
            {
                imageName = "0" + imageName;
            }
            imageName += ".jpg";
            await ClassFileMamagment.loadCreateOverwrite("Settings/ImageCount.txt", MainPage.imagesCount.ToString(), true);


            // Bild größe anpassen und speichern // Square
            if (MainPage.format == "Square")
            {
                // Größes Quadrat
                saveBitmap = new WriteableBitmap(0, 0);
                saveBitmap = cutBitmap.Resize(tileLarge, tileLarge, WriteableBitmapExtensions.Interpolation.Bilinear);
                filestream = file.CreateFile("/LockScreens/" + MainPage.folderName + "/Square/Large/" + imageName);
                saveBitmap.SaveJpeg(filestream, saveBitmap.PixelWidth, saveBitmap.PixelHeight, 0, jpgQuality);
                filestream.Close();

                // Mittleres Quadrat
                saveBitmap = new WriteableBitmap(0, 0);
                saveBitmap = cutBitmap.Resize(tileMedium, tileMedium, WriteableBitmapExtensions.Interpolation.Bilinear);
                filestream = file.CreateFile("/LockScreens/" + MainPage.folderName + "/Square/Medium/" + imageName);
                saveBitmap.SaveJpeg(filestream, saveBitmap.PixelWidth, saveBitmap.PixelHeight, 0, jpgQuality);
                filestream.Close();

                // Kleines Quadrat
                saveBitmap = new WriteableBitmap(0, 0);
                saveBitmap = cutBitmap.Resize(tileSmall, tileSmall, WriteableBitmapExtensions.Interpolation.Bilinear);
                filestream = file.CreateFile("/LockScreens/" + MainPage.folderName + "/Square/Small/" + imageName);
                saveBitmap.SaveJpeg(filestream, saveBitmap.PixelWidth, saveBitmap.PixelHeight, 0, jpgQuality);
                filestream.Close();

                // Thumbnail Quadrat
                saveBitmap = new WriteableBitmap(0, 0);
                saveBitmap = cutBitmap.Resize(150, 150, WriteableBitmapExtensions.Interpolation.Bilinear);
                filestream = file.CreateFile("/LockScreens/" + MainPage.folderName + "/Square/Thumbnails/" + imageName);
                saveBitmap.SaveJpeg(filestream, saveBitmap.PixelWidth, saveBitmap.PixelHeight, 0, jpgQuality);
                filestream.Close();

                // Bilderliste bearbeiten Quadrat
                MainPage.imagesListSquare = "~" + imageName + MainPage.imagesListSquare;
                await ClassFileMamagment.loadCreateOverwrite("/LockScreens/" + MainPage.folderName + "/Square/ImagesList.txt", MainPage.imagesListSquare, true);

                // Bilder Count neu erstellen
                MainPage.arImagesSquare = Regex.Split(MainPage.imagesListSquare, "~");
                MainPage.cImagesSquare = MainPage.arImagesSquare.Count() - 2;
            }


            // Bild größe anpassen und speichern // Querformat
            if (MainPage.format == "Landscape")
            {
                // Größes Querformat
                saveBitmap = new WriteableBitmap(0, 0);
                saveBitmap = cutBitmap.Resize(tileLarge, tileMedium, WriteableBitmapExtensions.Interpolation.Bilinear);
                filestream = file.CreateFile("/LockScreens/" + MainPage.folderName + "/Landscape/Large/" + imageName);
                saveBitmap.SaveJpeg(filestream, saveBitmap.PixelWidth, saveBitmap.PixelHeight, 0, jpgQuality);
                filestream.Close();

                // Kleines Querformat
                saveBitmap = new WriteableBitmap(0, 0);
                saveBitmap = cutBitmap.Resize(tileMedium, tileSmall, WriteableBitmapExtensions.Interpolation.Bilinear);
                filestream = file.CreateFile("/LockScreens/" + MainPage.folderName + "/Landscape/Small/" + imageName);
                saveBitmap.SaveJpeg(filestream, saveBitmap.PixelWidth, saveBitmap.PixelHeight, 0, jpgQuality);
                filestream.Close();

                // Thumbnail Querformat
                saveBitmap = new WriteableBitmap(0, 0);
                saveBitmap = cutBitmap.Resize(150, 72, WriteableBitmapExtensions.Interpolation.Bilinear);
                filestream = file.CreateFile("/LockScreens/" + MainPage.folderName + "/Landscape/Thumbnails/" + imageName);
                saveBitmap.SaveJpeg(filestream, saveBitmap.PixelWidth, saveBitmap.PixelHeight, 0, jpgQuality);
                filestream.Close();

                // Bilderliste bearbeiten Querformat
                MainPage.imagesListLandscape = "~" + imageName + MainPage.imagesListLandscape;
                await ClassFileMamagment.loadCreateOverwrite("/LockScreens/" + MainPage.folderName + "/Landscape/ImagesList.txt", MainPage.imagesListLandscape, true);

                // Bilder Count neu erstellen
                MainPage.arImagesLandscape = Regex.Split(MainPage.imagesListLandscape, "~");
                MainPage.cImagesLandscape = MainPage.arImagesLandscape.Count() - 2;
            }


            // Bild größe anpassen und speichern // Hochformat
            if (MainPage.format == "Portrait")
            {
                // Größes Hochformat
                saveBitmap = new WriteableBitmap(0, 0);
                saveBitmap = cutBitmap.Resize(tileMedium, tileLarge, WriteableBitmapExtensions.Interpolation.Bilinear);
                filestream = file.CreateFile("/LockScreens/" + MainPage.folderName + "/Portrait/Large/" + imageName);
                saveBitmap.SaveJpeg(filestream, saveBitmap.PixelWidth, saveBitmap.PixelHeight, 0, jpgQuality);
                filestream.Close();

                // Kleines Hochformat
                saveBitmap = new WriteableBitmap(0, 0);
                saveBitmap = cutBitmap.Resize(tileSmall, tileMedium, WriteableBitmapExtensions.Interpolation.Bilinear);
                filestream = file.CreateFile("/LockScreens/" + MainPage.folderName + "/Portrait/Small/" + imageName);
                saveBitmap.SaveJpeg(filestream, saveBitmap.PixelWidth, saveBitmap.PixelHeight, 0, jpgQuality);
                filestream.Close();

                // Thumbnail Hochformat
                saveBitmap = new WriteableBitmap(0, 0);
                saveBitmap = cutBitmap.Resize(72, 150, WriteableBitmapExtensions.Interpolation.Bilinear);
                filestream = file.CreateFile("/LockScreens/" + MainPage.folderName + "/Portrait/Thumbnails/" + imageName);
                saveBitmap.SaveJpeg(filestream, saveBitmap.PixelWidth, saveBitmap.PixelHeight, 0, jpgQuality);
                filestream.Close();

                // Bilderliste bearbeiten Querformat
                MainPage.imagesListPortrait = "~" + imageName + MainPage.imagesListPortrait;
                await ClassFileMamagment.loadCreateOverwrite("/LockScreens/" + MainPage.folderName + "/Portrait/ImagesList.txt", MainPage.imagesListPortrait, true);

                // Bilder Count neu erstellen
                MainPage.arImagesPortrait = Regex.Split(MainPage.imagesListPortrait, "~");
                MainPage.cImagesPortrait = MainPage.arImagesPortrait.Count() - 2;
            }


            // Bild größe anpassen und speichern // Hintergrundbild
            if (MainPage.format == "Background")
            {
                // Größes Hintergrundbild
                saveBitmap = new WriteableBitmap(0, 0);
                saveBitmap = cutBitmap.Resize(Convert.ToInt32(backgroundWidth), Convert.ToInt32(backgroundHeight), WriteableBitmapExtensions.Interpolation.Bilinear);
                filestream = file.CreateFile("/LockScreens/" + MainPage.folderName + "/Background/Pictures/" + imageName);
                saveBitmap.SaveJpeg(filestream, saveBitmap.PixelWidth, saveBitmap.PixelHeight, 0, jpgQuality);
                filestream.Close();

                // Thumbnail Hintergrundbild
                double percent = Convert.ToDouble(100) / backgroundHeight * Convert.ToDouble(150);
                int newWidth = Convert.ToInt32(backgroundWidth / Convert.ToDouble(100) * percent);
                saveBitmap = new WriteableBitmap(0, 0);
                saveBitmap = cutBitmap.Resize(newWidth, 150, WriteableBitmapExtensions.Interpolation.Bilinear);
                filestream = file.CreateFile("/LockScreens/" + MainPage.folderName + "/Background/Thumbnails/" + imageName);
                saveBitmap.SaveJpeg(filestream, saveBitmap.PixelWidth, saveBitmap.PixelHeight, 0, jpgQuality);
                filestream.Close();

                // Bilderliste bearbeiten Hintergrund
                MainPage.imagesListBackground = "~" + imageName + MainPage.imagesListBackground;
                await ClassFileMamagment.loadCreateOverwrite("/LockScreens/" + MainPage.folderName + "/Background/ImagesList.txt", MainPage.imagesListBackground, true);

                // Bilder Count neu erstellen
                MainPage.arImagesBackground = Regex.Split(MainPage.imagesListBackground, "~");
                MainPage.cImagesBackground = MainPage.arImagesBackground.Count() - 2;
            }


            // Bild größe anpassen und speichern // Benutzerbild
            if (MainPage.format == "User")
            {
                // Größes Benutzerbild
                saveBitmap = new WriteableBitmap(0, 0);
                saveBitmap = cutBitmap.Resize(Convert.ToInt32(userWidth), Convert.ToInt32(userHeight), WriteableBitmapExtensions.Interpolation.Bilinear);
                filestream = file.CreateFile("/LockScreens/" + MainPage.folderName + "/User/Pictures/" + imageName);
                saveBitmap.SaveJpeg(filestream, saveBitmap.PixelWidth, saveBitmap.PixelHeight, 0, jpgQuality);
                filestream.Close();

                // Thumbnail Benutzerbild
                saveBitmap = cutBitmap.Resize(150, 150, WriteableBitmapExtensions.Interpolation.Bilinear);
                filestream = file.CreateFile("/LockScreens/" + MainPage.folderName + "/User/Thumbnails/" + imageName);
                saveBitmap.SaveJpeg(filestream, saveBitmap.PixelWidth, saveBitmap.PixelHeight, 0, jpgQuality);
                filestream.Close();

                // Bilderliste bearbeiten Hintergrund
                MainPage.imagesListUser = "~" + imageName + MainPage.imagesListUser;
                await ClassFileMamagment.loadCreateOverwrite("/LockScreens/" + MainPage.folderName + "/User/ImagesList.txt", MainPage.imagesListUser, true);

                // Bilder Count neu erstellen
                MainPage.arImagesUser = Regex.Split(MainPage.imagesListUser, "~");
                MainPage.cImagesUser = MainPage.arImagesUser.Count() - 2;
            }


            // Aktuellen Count erhöhen
            addNow++;


            // Wenn Einzele Bildauswahl
            if (MainPage.photoChooserTaskIsRunning)
            {
                // Seitenteil festlegen
                pagePart = "select";

                // Zurück
                NavigationService.GoBack();
            }


            // Wenn mehrfache Bildauswahl
            else
            {

                // Wenn noch Bilder vorhanden
                if (addNow < addImagesC)
                {
                    // Bild laden
                    await loadPicture();

                    // Bild in Panel setzen
                    pictureToPanel();
                }


                // Wenn keine Bilder mehr vorhanden
                else
                {
                    // Auswahl Panel alles Abwählen
                    try
                    {
                        lbPictures.SelectedIndex = -1;
                    }
                    catch
                    {
                    }

                    // Auswahl Panel anzeigen
                    grCut.Visibility = System.Windows.Visibility.Collapsed;
                    grSelect.Visibility = System.Windows.Visibility.Visible;
                    appBar1();

                    // Seitenteil festlegen
                    pagePart = "select";
                }
            }
        }



        // Button // Links drehen
        private void btnRotateLeft(object sender, EventArgs e)
        {
            // Bild nach links drehen
            cutBitmap = cutBitmap.Rotate(270);

            // Bild neu in Panel setzen
            pictureToPanel();
        }



        // Button // Rechts drehen
        private void btnRotateRight(object sender, EventArgs e)
        {
            // Bild nach rechts drehen
            cutBitmap = cutBitmap.Rotate(90);

            // Bild neu in Panel setzen
            pictureToPanel();
        }



        // Button löschen
        private async void btnDelete(object sender, EventArgs e)
        {
            // Aktuellen Count erhöhen
            addNow++;

            // Wenn Einzele Bildauswahl
            if (MainPage.photoChooserTaskIsRunning)
            {
                // Seitenteil festlegen
                pagePart = "select";

                // Zurück
                NavigationService.GoBack();
            }


            // Wenn mehrfache Bildauswahl
            else
            {

                // Wenn noch Bilder vorhanden
                if (addNow < addImagesC)
                {
                    // Bild laden
                    await loadPicture();

                    // Bild in Panel setzen
                    pictureToPanel();
                }


                // Wenn keine Bilder mehr vorhanden
                else
                {
                    // Auswahl Panel alles Abwählen
                    try
                    {
                        lbPictures.SelectedIndex = -1;
                    }
                    catch
                    {
                    }

                    // Auswahl Panel anzeigen
                    grCut.Visibility = System.Windows.Visibility.Collapsed;
                    grSelect.Visibility = System.Windows.Visibility.Visible;
                    appBar1();

                    // Seitenteil festlegen
                    pagePart = "select";
                }
            }
        }
        //---------------------------------------------------------------------------------------------------------





        // Bild laden
        //---------------------------------------------------------------------------------------------------------
        async Task loadPicture()
        {
            // Bei allen Bildern
            if (imgPictures == "all")
            {
                // Alle Bilder laden
                MediaLibrary mediaLibrary = new MediaLibrary();
                var mlPictures = mediaLibrary.Pictures;

                // Versuchen Bilder auszulesen
                try
                {
                    cutBitmap.SetSource(mlPictures[addImages[addNow]].GetImage());
                }
                catch
                {
                }
            }


            // Bei gespeicherten Bildern
            else
            {
                MediaLibrary mediaLibrary = new MediaLibrary();
                var mlPictures = mediaLibrary.SavedPictures;

                // Versuchen Bilder auszulesen
                try
                {
                    cutBitmap.SetSource(mlPictures[addImages[addNow]].GetImage());
                }
                catch
                {
                }
            }
        }
        //---------------------------------------------------------------------------------------------------------





        // Bild in Schnitt Panel setzen
        //---------------------------------------------------------------------------------------------------------
        void pictureToPanel()
        {
            // Bild in Cut Panel laden
            imgCut.Source = cutBitmap;


            // Slider zurücksetzen
            slCut.Value = 100;


            // Verschiebung zurücksetzen
            transform.TranslateX = 0;
            transform.TranslateY = 0;


            // Slider Größe zurücksetzen
            slCut.Value = 100;


            // Schnitt Rechteck anpassen // Quadratisches Format
            if (MainPage.format == "Square")
            {
                // Wenn Höhe größer wie Breite
                if (cutBitmap.PixelHeight >= cutBitmap.PixelWidth)
                {
                    recCut.Height = cutBitmap.PixelWidth;
                    recCut.Width = cutBitmap.PixelWidth;
                }
                // Wenn Breite größer als Höhe
                else
                {
                    recCut.Height = cutBitmap.PixelHeight;
                    recCut.Width = cutBitmap.PixelHeight;
                }
            }


            // Schnitt Rechteck anpassen // Querformat
            else if (MainPage.format == "Landscape")
            {
                // Versuchen das Rechteck quer ins Bild zu legen
                recCut.Width = cutBitmap.PixelWidth;
                // Prozentual die Höhe errechnen
                double percent = Convert.ToDouble(100) / tileLarge * cutBitmap.PixelWidth;
                recCut.Height = tileMedium / Convert.ToDouble(100) * percent;

                // Wenn das Recteck zu groß
                if (recCut.Height > cutBitmap.PixelHeight)
                {
                    // Rechteck hoch ins Bild legen
                    recCut.Height = cutBitmap.PixelHeight;
                    // Prozentual die Breite anpassen
                    percent = Convert.ToDouble(100) / tileMedium * cutBitmap.PixelHeight;
                    recCut.Width = tileLarge / Convert.ToDouble(100) * percent;
                }
            }


            // Schnitt Rechteck anpassen // Hochformat
            else if (MainPage.format == "Portrait")
            {
                // Versuchen das Rechteck hoch ins Bild zu legen
                recCut.Height = cutBitmap.PixelHeight;
                // Prozentual die Breite errechnen
                double percent = Convert.ToDouble(100) / tileLarge * cutBitmap.PixelHeight;
                recCut.Width = tileMedium / Convert.ToDouble(100) * percent;

                // Wenn das Rechteck zu groß
                if (recCut.Width > cutBitmap.PixelWidth)
                {
                    // Rechteck quer ins Bild legen
                    recCut.Width = cutBitmap.PixelWidth;
                    // Prozentual die Breite anpassen
                    percent = Convert.ToDouble(100) / tileMedium * cutBitmap.PixelWidth;
                    recCut.Height = tileLarge / Convert.ToDouble(100) * percent;
                }
            }


            // Schnitt Rechteck anpassen // Hintergrundbild
            else if (MainPage.format == "Background")
            {
                // Versuchen das Rechteck hoch ins Bild zu legen
                recCut.Height = cutBitmap.PixelHeight;
                // Prozentual die Breite errechnen
                double percent = Convert.ToDouble(100) / backgroundHeight * cutBitmap.PixelHeight;
                recCut.Width = backgroundWidth / Convert.ToDouble(100) * percent;

                // Wenn das Rechteck zu groß
                if (recCut.Width > cutBitmap.PixelWidth)
                {
                    // Rechteck quer ins Bild legen
                    recCut.Width = cutBitmap.PixelWidth;
                    // Prozentual die Breite anpassen
                    percent = Convert.ToDouble(100) / backgroundWidth * cutBitmap.PixelWidth;
                    recCut.Height = backgroundHeight / Convert.ToDouble(100) * percent;
                }
            }


            // Schnitt Rechteck anpassen // Benutzerbild
            else if (MainPage.format == "User")
            {
                // Wenn Höhe größer wie Breite
                if (cutBitmap.PixelHeight >= cutBitmap.PixelWidth)
                {
                    recCut.Height = cutBitmap.PixelWidth;
                    recCut.Width = cutBitmap.PixelWidth;
                }
                // Wenn Breite größer als Höhe
                else
                {
                    recCut.Height = cutBitmap.PixelHeight;
                    recCut.Width = cutBitmap.PixelHeight;
                }
            }


            // Schnitt Rechteck Variablen erstellen
            width100 = recCut.Width;
            height100 = recCut.Height;


            // Qualität des Bildes errechnen
            getQuality();


            // AppBar // Schneiden erzeugen
            appBar2();
        }
        //---------------------------------------------------------------------------------------------------------





        // Schnitt Rechteck // vergrößern // verkleinern
        //--------------------------------------------------------------------------------------------------------
        private void slCut_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                // Größe in Textbox schreiben
                tbSizePercent.Text = Convert.ToInt32(slCut.Value).ToString() + " %";


                // Rechteck Prozentual vergrößern oder verkleinern
                recCut.Width = Convert.ToInt32(width100 / Convert.ToDouble(100) * slCut.Value);
                recCut.Height = Convert.ToInt32(height100 / Convert.ToDouble(100) * slCut.Value);


                // Qualität des Bildes errechnen
                getQuality();


                // Bild neu ausrichten
                setAligin();
            }
            catch
            {
            }
        }
        //--------------------------------------------------------------------------------------------------------





        // Schnitt Rechteck // ausrichten
        //--------------------------------------------------------------------------------------------------------
        // Variablen
        double maxX;
        double minX;
        double maxY;
        double minY;


        // Beim Starten der Bewegung
        private void OnDragStarted(object sender, DragStartedGestureEventArgs e)
        {
            recCut.Opacity = 0.5;
        }


        // Beim Bewegen
        private void OnDragDelta(object sender, DragDeltaGestureEventArgs e)
        {
            // Rechteck verschieben
            transform.TranslateX += e.HorizontalChange;
            transform.TranslateY += e.VerticalChange;

            // Bild neu ausrichten
            setAligin();
        }


        // Beim Beenden der Bewegung
        private void OnDragCompleted(object sender, DragCompletedGestureEventArgs e)
        {
            recCut.Opacity = 1.0;
        }
        //--------------------------------------------------------------------------------------------------------





        // Schnitt Quadrat neu ausrichten
        //--------------------------------------------------------------------------------------------------------
        void setAligin()
        {
            // Linke Seite ausrichten
            minX = (imgCut.ActualWidth - recCut.Width) / 2;
            minX = minX - (minX * 2);
            if (transform.TranslateX < minX)
            {
                transform.TranslateX = minX;
            }

            // Rechte Seite ausrichten
            maxX = (imgCut.ActualWidth - recCut.Width) / 2;
            if (transform.TranslateX > maxX)
            {
                transform.TranslateX = maxX;
            }

            // Obere Seite ausrichten
            minY = (imgCut.ActualHeight - recCut.Height) / 2;
            minY = minY - (minY * 2);
            if (transform.TranslateY < minY)
            {
                transform.TranslateY = minY;
            }

            // Untere Seite ausrichten
            maxY = (imgCut.ActualHeight - recCut.Height) / 2;
            if (transform.TranslateY > maxY)
            {
                transform.TranslateY = maxY;
            }
        }
        //--------------------------------------------------------------------------------------------------------





        // Wenn einzelnes Bild ausgewählt wurde
        //--------------------------------------------------------------------------------------------------------
        void photoChooserTask_Completed(object sender, PhotoResult e)
        {
            // Wenn Bilder ausgewählt
            if (e.TaskResult == TaskResult.OK)
            {
                // Schnitt Panel anzeigen
                grCut.Visibility = System.Windows.Visibility.Visible;
                grSelect.Visibility = System.Windows.Visibility.Collapsed;

                // SchnittBild laden
                cutBitmap.SetSource(e.ChosenPhoto);

                // Bild in Panal legen
                pictureToPanel();

                // Seitenteil festlegen
                pagePart = "cut";
            }
            else
            {
                // Zurück zur vorherigen Seite
                timer.Start();
            }
        }



        // Timer der bi Abbruch zurück geht
        private void timer_Tick(object sender, EventArgs e)
        {
            // Timer anhalten
            timer.Stop();

            // Zurück zur vorherigen Seite
            NavigationService.GoBack();
        }
        //--------------------------------------------------------------------------------------------------------





        // Qualität des Bildes errechnen
        //--------------------------------------------------------------------------------------------------------
        void getQuality()
        {
            if (MainPage.format == "Square" | MainPage.format == "Landscape" | MainPage.format == "Background" | MainPage.format == "User")
            {
                // Wenn kleiner als 100 %
                if (recCut.Width < tileLarge)
                {
                    tbPictureQuality.Text = Convert.ToInt32(Convert.ToDouble(100) / Convert.ToDouble(tileLarge) * recCut.Width).ToString() + " %";
                }
                // Wenn 100 %
                else
                {
                    tbPictureQuality.Text = "100 %";
                }
            }
            else
            {
                // Wenn kleiner als 100 %
                if (recCut.Height < tileLarge)
                {
                    tbPictureQuality.Text = Convert.ToInt32(Convert.ToDouble(100) / Convert.ToDouble(tileLarge) * recCut.Height).ToString() + " %";
                }
                // Wenn 100 %
                else
                {
                    tbPictureQuality.Text = "100 %";
                }
            }
        }
        //--------------------------------------------------------------------------------------------------------





        // Back Button
        //--------------------------------------------------------------------------------------------------------
        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            // Wenn Schnitt Panel offen
            if (pagePart == "cut")
            {
                // Navigation zurück abbrechen
                e.Cancel = true;


                // Benachrichtigung ausgeben
                if (MessageBox.Show("", MyApp.Resources.AppResources.X002_cancel, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    // Wenn Abbrechen bestätigt
                    grCut.Visibility = System.Windows.Visibility.Collapsed;
                    grSelect.Visibility = System.Windows.Visibility.Visible;
                    appBar1();

                    // Seitenteil festlegen
                    pagePart = "select";
                }
            }
        }
        //--------------------------------------------------------------------------------------------------------





    }
}