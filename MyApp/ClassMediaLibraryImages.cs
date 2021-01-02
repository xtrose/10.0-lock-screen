using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;





// Namespace
namespace MyApp
{





    // Klasse die Bilder aus dem Media Library verwaltet
    class ClassMediaLibraryImages
    {






        // Variablen
        // ---------------------------------------------------------------------------------------------------
        // ID des Bilders
        public int id { get; set; }

        // Bild
        public BitmapImage image { get; set; }
        // ---------------------------------------------------------------------------------------------------





        // Klasse erzeugen
        // ---------------------------------------------------------------------------------------------------
        public ClassMediaLibraryImages (int id, BitmapImage image)
        {
            this.id = id;
            this.image = image;
        }
        // ---------------------------------------------------------------------------------------------------





    }
}
