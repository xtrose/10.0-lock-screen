using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;
using System.IO;
using System.Collections.ObjectModel;
using Windows.UI;
using System.Globalization;
using System.Threading;





// Namespace
namespace MyApp.Pages
{





    // Seite zum ändern der Sprache
    public partial class Language : PhoneApplicationPage
    {





        // Allgemeine Variablen
        // ---------------------------------------------------------------------------------------------------
        // IsoStore Variabeln
        IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication();
        IsolatedStorageFileStream filestream;
        StreamReader sr;
        StreamWriter sw;

        // Seiten Variabeln
        string[] LangCodes = { "ms-MY", "cs-CZ", "da-DK", "de-DE", "en-US", "es-ES", "es-MX", "fr-FR", "hr-HR", "it-IT", "lt-LT", "lv-LV", "hu-HU", "nl-NL", "nb-NO", "pl-PL", "pt-BR", "pt-PT", "ro-RO", "sk-SK", "sv-SE", "vi-VN", "tr-TR", "el-GR", "bg-BG", "ru-RU", "ar-SA", "fa-IR", "hi-IN", "th-TH", "ko-KR", "zh-CN", "zh-TW", "ja-JP", "uk-UA" };
        string[] LangNames = { "Behasa Melayu", "Čeština", "dansk", "deutsch", "English", "español (España)", "Español (México)", "Français", "hrvatski", "italiano", "Lietuvių", "Latviešu", "magyar", "Nederlands", "norsk", "polski", "português (Brasil)", "português (Portugal)", "română", "Slovenský", "Svenska", "Tiếng Việt", "Türkçe", "Ελληνικά", "Български", "русский", "العربية", "فارسی", "हिंदी", "ไทย", "한국어", "简体中文", "繁體中文", "日本語", "Український" };

        //Neue Datenliste erstellen //ClassStyles
        ObservableCollection<ClassLanguages> datalist = new ObservableCollection<ClassLanguages>();

        // Index momentaner Sprache
        int SelectedLanguage = -1;
        // ---------------------------------------------------------------------------------------------------





        // Klasse erzeugen
        // ---------------------------------------------------------------------------------------------------
        public Language()
        {
            // UI Komponenten laden
            InitializeComponent();


        }
        // ---------------------------------------------------------------------------------------------------





        // Wird bei jedem Aufruf der Seite ausgeführt
        // ---------------------------------------------------------------------------------------------------
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Liste der Sprachen erstellen
            CreateLanguages();
        }
        // ---------------------------------------------------------------------------------------------------





        // Wird am Anfang der Seite geladen
        //---------------------------------------------------------------------------------------------------------
        void CreateLanguages()
        {
            // Prüfen wieviel Sprachen
            int cLang = LangNames.Count();

            // Sprachen durchlaufen
            datalist.Clear();
            for (int i = 0; i < cLang; i++)
            {
                if (LangCodes[i] == MyApp.Resources.AppResources.ResourceLanguage)
                {
                    datalist.Add(new ClassLanguages(LangNames[i], LangCodes[i], "#cf2929"));
                    SelectedLanguage = i;
                }
                else
                {
                    datalist.Add(new ClassLanguages(LangNames[i], LangCodes[i], "#ffcf2828"));
                }

            }

            // Sprachen in Listbox Setzen
            lbLanguage.ItemsSource = datalist;
        }
        //---------------------------------------------------------------------------------------------------------





        // Sprache auswählen
        // ---------------------------------------------------------------------------------------------------
        // Variabeln
        bool SelectLang = true;
        // Aktion
        private void lbLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Prüfen ob Aktion ausgefüfrt wird
            if (SelectLang == true & SelectedLanguage != lbLanguage.SelectedIndex)
            {
                // Index ermitteln
                int SI = lbLanguage.SelectedIndex;

                // Code aus Array laden
                string cul = (datalist[SI] as ClassLanguages).code;
                string lang = (datalist[SI] as ClassLanguages).name;

                // Abfrage ob Sprache geändert werden soll
                if (MessageBox.Show("", "⚠ " + MyApp.Resources.AppResources.X002_changeLanguage, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    // Sprache ändern
                    CultureInfo newCulture = new CultureInfo(cul);
                    Thread.CurrentThread.CurrentUICulture = newCulture;

                    // IsoStore file erstellen
                    IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication();
                    // Prüfen ob alte Datei vorhanden
                    if (file.FileExists("Cul.dat"))
                    {
                        file.DeleteFile("Cul.dat");
                    }
                    // Neue Datei erstellen
                    IsolatedStorageFileStream filestream = file.CreateFile("Cul.dat");
                    StreamWriter sw = new StreamWriter(filestream);
                    sw.WriteLine(Convert.ToString(cul));
                    sw.Flush();
                    filestream.Close();

                    // Benachrichtigung ausgeben
                    // MessageBox.Show(MyApp.Resources.AppResources.XFR_Restart);

                    //Zurück
                    Terminate();
                }
            }

            //Auswahl zurücksetzen
            SelectLang = false;
            try
            {
                lbLanguage.SelectedIndex = -1;
            }
            catch { }
            SelectLang = true;
        }
        // ---------------------------------------------------------------------------------------------------





        // App Schließen
        // ---------------------------------------------------------------------------------------------------
        private static void Terminate()
        {
            Application.Current.Terminate();
        }
        // ---------------------------------------------------------------------------------------------------





    }
}