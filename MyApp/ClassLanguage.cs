using System;
using System.ComponentModel;





// Namespace
namespace MyApp
{





    // Klasse der Sprachen
    class ClassLanguages
    {
        public string name { get; set; }
        public string code { get; set; }
        public string background { get; set; }

        public ClassLanguages(string name, string code, string background)
        {
            this.name = name;
            this.code = code;
            this.background = background;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
