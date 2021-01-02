using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;





// Namespace
namespace MyApp
{





    // Klasse der Thumbnails
    class ClassThumbnails
    {





        // Variablen
        // ---------------------------------------------------------------------------------------------------
        // ID des Bilders
        public int id { get; set; }


        // Name des Bildes
        public string name { get; set; }


        // Klassen Variablen
        public BitmapImage thumbnail { get; set; }
        // ---------------------------------------------------------------------------------------------------





        // Klasse erstellen
        // ---------------------------------------------------------------------------------------------------
        public ClassThumbnails(int id, string name, BitmapImage thumbnail)
        {
            // Variablen übernehmen
            this.id = id;
            this.name = name;
            this.thumbnail = thumbnail;
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
