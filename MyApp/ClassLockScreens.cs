using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Windows.UI;





// Namespace
namespace MyApp
{





    // Klasse der Lock Screens
    public class ClassLockScreens
    {





        // Variablen
        // ---------------------------------------------------------------------------------------------------
        public int index { get; set; }
        public string name { get; set; }
        public int cSquare { get; set; }
        public int cLandscape { get; set; }
        public int cPortrait { get; set; }
        public int cBackground { get; set; }
        public int cUser { get; set; }
        public int id { get; set; }
        public ShellTile shellTile { get; set; }
        public Uri imagePin { get; set; }
        // ---------------------------------------------------------------------------------------------------





        // Klasse erstellen
        // ---------------------------------------------------------------------------------------------------
        public ClassLockScreens(int index, string name, bool loadInformations)
        {
            // Name übernehmen
            this.index = index;
            this.name = name;


            // Wenn erweiterte Informationen geladen werden
            if (loadInformations)
            {
                // Bild Infos laden
                loadPicturesFiles();

                // Tile Infos laden
                loadTileInformations();
            }
        }
        // ---------------------------------------------------------------------------------------------------





        // Bilder listen laden
        // ---------------------------------------------------------------------------------------------------
        async void loadPicturesFiles()
        {
            // Liste laden // Square
            string imagesListSquare = await ClassFileMamagment.loadCreateOverwrite("/LockScreens/" + name + "/Square/ImagesList.txt", "", false);
            // Bilder Count erstellen
            string[] arImagesSquare = Regex.Split(imagesListSquare, "~");
            cSquare = arImagesSquare.Count() - 2;


            // Liste laden // Landscape
            string imagesListLandscape = await ClassFileMamagment.loadCreateOverwrite("/LockScreens/" + name + "/Landscape/ImagesList.txt", "", false);
            // Bilder Count erstellen
            string[] arImagesLandscape = Regex.Split(imagesListLandscape, "~");
            cLandscape = arImagesLandscape.Count() - 2;


            // Liste laden // Portrait
            string imagesListPortrait = await ClassFileMamagment.loadCreateOverwrite("/LockScreens/" + name + "/Portrait/ImagesList.txt", "", false);
            // Bilder Count erstellen
            string[] arImagesPortrait = Regex.Split(imagesListPortrait, "~");
            cPortrait = arImagesPortrait.Count() - 2;


            // Liste laden // Background
            string imagesListBackground = await ClassFileMamagment.loadCreateOverwrite("/LockScreens/" + name + "/Background/ImagesList.txt", "", false);
            // Bilder Count erstellen
            string[] arImagesBackground = Regex.Split(imagesListBackground, "~");
            cBackground = arImagesBackground.Count() - 2;


            // Liste laden // User
            string imagesListUser = await ClassFileMamagment.loadCreateOverwrite("/LockScreens/" + name + "/User/ImagesList.txt", "", false);
            // Bilder Count erstellen
            string[] arImagesUser = Regex.Split(imagesListUser, "~");
            cUser = arImagesUser.Count() - 2;
        }
        // ---------------------------------------------------------------------------------------------------





        // Tile Informationen laden
        // ---------------------------------------------------------------------------------------------------
        async void loadTileInformations()
        {
            // Id laden
            this.id = Convert.ToInt32(await ClassFileMamagment.loadCreateOverwrite("/LockScreens/" + name + "/Id.dat", "", false));

            // Prüfen ob Tile gesezt
            this.shellTile = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("Id=" + id));

            // Wenn Tile nicht existiert
            if (shellTile == null)
            {
                // Bilder laden
                imagePin = new Uri("/Images/icon_pin.png", UriKind.RelativeOrAbsolute);
            }
            // Wenn Tile existiert
            else
            {
                // Liste der Sperrbildschirme mit Tiles aktuallisieren
                MainPage.createdTiles.Add(name);
                // Bilder laden
                imagePin = new Uri("/Images/icon_unpin_Light.png", UriKind.RelativeOrAbsolute);
            }
        }
        // ---------------------------------------------------------------------------------------------------





        // Änderungen aktuallisieren
        // ---------------------------------------------------------------------------------------------------
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        // ---------------------------------------------------------------------------------------------------





    }
}
