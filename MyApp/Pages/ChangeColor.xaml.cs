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
using System.ComponentModel;





// Namespace
namespace MyApp.Pages
{





    // Seite um Farben einzustellen
    public partial class ChangeColor : PhoneApplicationPage
    {





        // Variablen
        // ---------------------------------------------------------------------------------------------------
        // Art der Farbe
        string colorType = "BackgroundColor";

        // Alte Farbe
        string colorOld = "";

        // Neue Farbe
        string colorNew = "";

        // Transparenz
        string transparency = "FF";

        // Bereits verwendete Farben
        SolidColorBrush usedColor1 = new SolidColorBrush();
        SolidColorBrush usedColor2 = new SolidColorBrush();
        SolidColorBrush usedColor3 = new SolidColorBrush();
        SolidColorBrush usedColor4 = new SolidColorBrush();
        SolidColorBrush usedColor5 = new SolidColorBrush();

        // Farbe
        SolidColorBrush solidColorBrush = new SolidColorBrush();
        // ---------------------------------------------------------------------------------------------------





        // Klasse erzeugen
        // ---------------------------------------------------------------------------------------------------
        public ChangeColor()
        {
            // UI Komponenten initialisieren
            InitializeComponent();
        }
        // ---------------------------------------------------------------------------------------------------





        // Wird bei jedem Aufruf der Seite ausgeführt
        // ---------------------------------------------------------------------------------------------------
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Variablen ermitteln // Art der Farbe
            colorType = NavigationContext.QueryString["color"];



            // Bei Hintergrundfarbe
            if (colorType == "backgroundColor")
            {
                // Farb Code ermitteln
                colorOld = MainPage.backgroundColor;
            }

            // Bei Rahmenfarbe
            else if (colorType == "frameColor")
            {
                // Farb Code ermitteln
                colorOld = MainPage.frameColor;
            }

            // Bei Benutzerinformation Hintergrundfarbe
            else if (colorType == "userInformationBackgroundColor")
            {
                // Farb Code ermitteln
                colorOld = MainPage.userInformationBackgroundColor;
            }

            // Bei Benutzerinformation Schriftfarbe
            else if (colorType == "userInformationFontColor")
            {
                // Farb Code ermitteln
                colorOld = MainPage.userInformationFontColor;
            }

            // Bei Information Hintergundfarbe
            else if (colorType == "informationBackgroundColor")
            {
                // Farb Code ermitteln
                colorOld = MainPage.informationBackgroundColor;
            }



            // Wenn Hintergrundfarbe
            if (colorOld == "bc")
            {
                Color backgroundColor = (Color)Application.Current.Resources["PhoneBackgroundColor"];
                colorNew = Convert.ToString(backgroundColor);
            }

            // Wenn Akzentfarbe
            else if (colorOld == "ac")
            {
                Color accentColor = (Color)Application.Current.Resources["PhoneAccentColor"];
                colorNew = Convert.ToString(accentColor);
            }

            // Wenn Vordergrundfarbe
            else if (colorOld == "fc")
            {
                Color foregroundColor = (Color)Application.Current.Resources["PhoneForegroundColor"];
                colorNew = Convert.ToString(foregroundColor);
            }

            // Wenn sonstigen Farben
            else
            {
                colorNew = colorOld;
            }



            // Farbe in Colorpicker erstellen
            byte A = Convert.ToByte(colorNew.Substring(1, 2), 16);
            byte R = Convert.ToByte(colorNew.Substring(3, 2), 16);
            byte G = Convert.ToByte(colorNew.Substring(5, 2), 16);
            byte B = Convert.ToByte(colorNew.Substring(7, 2), 16);
            solidColorBrush = new SolidColorBrush(Color.FromArgb(A, R, G, B));
            colorPicker.Color = solidColorBrush.Color;



            // Bereits verwendete Farben erstellen
            usedColor1 = getSolidColorBrush(MainPage.backgroundColor, false);
            recUsedColor1.Fill = usedColor1;
            usedColor2 = getSolidColorBrush(MainPage.frameColor, false);
            recUsedColor2.Fill = usedColor2;
            usedColor3 = getSolidColorBrush(MainPage.userInformationBackgroundColor, false);
            recUsedColor3.Fill = usedColor3;
            usedColor4 = getSolidColorBrush(MainPage.userInformationFontColor, false);
            recUsedColor4.Fill = usedColor4;
            usedColor5 = getSolidColorBrush(MainPage.informationBackgroundColor, false);
            recUsedColor5.Fill = usedColor5;



            // Transparenz einstellen stellen
            transparency = colorNew.Substring(1, 2);
            int intTransparency = int.Parse(transparency, System.Globalization.NumberStyles.HexNumber);
            int percent = Convert.ToInt32(Convert.ToDouble(100) / 255 * Convert.ToDouble(intTransparency));
            slTransparency.Value = 100 - percent;
            tbTransparency.Text = (100 - percent).ToString() + "%";
            slTransparencyChange = true;
        }
        // ---------------------------------------------------------------------------------------------------





        // Color Picker ColorChanged
        // ---------------------------------------------------------------------------------------------------
        private void ColorChanged(object sender, Color color)
        {
            // Farbe ändern
            solidColorBrush = new SolidColorBrush(color);
            colorNew = Convert.ToString(color);


            // Transparenz // Prozent auslesen
            double percent = slTransparency.Value;


            // Byte String erstellen
            transparency = "FF";


            // Bei 100%
            if (percent == 100)
            {
                transparency = "00";
            }
            // Bei anders als 100%
            else
            {
                percent = Convert.ToDouble(100) - percent;
                int intA = Convert.ToInt32(Convert.ToDouble(255) / Convert.ToDouble(100) * percent);
                transparency = intA.ToString("X");
                if (transparency.Length == 1)
                {
                    transparency = "0" + transparency;
                }
            }


            // Transparenz der Farbe hinzufügen
            colorNew = "#" + transparency + colorNew.Substring(3, 2) + colorNew.Substring(5, 2) + colorNew.Substring(7, 2);


            // Farbe in Feld eintragen
            colorPicker.Color = getSolidColorBrush(colorNew, true).Color;
        }
        // ---------------------------------------------------------------------------------------------------





        // Buttons
        // ---------------------------------------------------------------------------------------------------
        // Transparenz ändern
        bool slTransparencyChange = false;
        private void slTransparency_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Wenn angewendet wird
            if (slTransparencyChange)
            {


                // Anzeige updaten
                tbTransparency.Text = Convert.ToInt32(slTransparency.Value) + "%";


                // Prozent auslesen
                double percent = slTransparency.Value;


                // Byte String erstellen
                transparency = "FF";


                // Bei 100%
                if (percent == 100)
                {
                    transparency = "00";
                }
                // Bei anders als 100%
                else
                {
                    percent = Convert.ToDouble(100) - percent;
                    int intA = Convert.ToInt32(Convert.ToDouble(255) / Convert.ToDouble(100) * percent);
                    transparency = intA.ToString("X");
                    if (transparency.Length == 1)
                    {
                        transparency = "0" + transparency;
                    }
                }


                // In Code umwandeln // Wenn Hintergrundfarbe
                if (colorNew == "bc")
                {
                    Color backgroundColor = (Color)Application.Current.Resources["PhoneBackgroundColor"];
                    colorNew = Convert.ToString(backgroundColor);
                }

                // In Code umwandeln // // Wenn Akzentfarbe
                else if (colorNew == "ac")
                {
                    Color accentColor = (Color)Application.Current.Resources["PhoneAccentColor"];
                    colorNew = Convert.ToString(accentColor);
                }

                // In Code umwandeln // // Wenn Vordergrundfarbe
                else if (colorNew == "fc")
                {
                    Color foregroundColor = (Color)Application.Current.Resources["PhoneForegroundColor"];
                    colorNew = Convert.ToString(foregroundColor);
                }


                // Transparenz der Farbe hinzufügen
                colorNew = "#" + transparency + colorNew.Substring(3, 2) + colorNew.Substring(5, 2) + colorNew.Substring(7, 2);


                // Farbe in Feld eintragen
                colorPicker.Color = getSolidColorBrush(colorNew, true).Color;
            }
        }



        // Button // Hintergrundfarbe einstellen
        private void btnBackgroundColor_Click(object sender, RoutedEventArgs e)
        {
            // Farbe einstellen
            solidColorBrush = new SolidColorBrush((Color)Application.Current.Resources["PhoneBackgroundColor"]);
            colorPicker.Color = solidColorBrush.Color;
            colorNew = "bc";
            // Transparenz zurücksetzen
            slTransparencyChange = false;
            slTransparency.Value = 0;
            tbTransparency.Text = "0%";
            slTransparencyChange = true;
        }



        // Button // Akzentfarbe einstellen
        private void btnAccentColor_Click(object sender, RoutedEventArgs e)
        {
            // Farbe einstellen
            solidColorBrush = new SolidColorBrush((Color)Application.Current.Resources["PhoneAccentColor"]);
            colorPicker.Color = solidColorBrush.Color;
            colorNew = "ac";
            // Transparenz zurücksetzen
            slTransparencyChange = false;
            slTransparency.Value = 0;
            tbTransparency.Text = "0%";
            slTransparencyChange = true;
        }



        // Button // Vordergrundfarbe einstellen
        private void btnForegroundColor_Click(object sender, RoutedEventArgs e)
        {
            // Farbe einstellen
            solidColorBrush = new SolidColorBrush((Color)Application.Current.Resources["PhoneForegroundColor"]);
            colorPicker.Color = solidColorBrush.Color;
            colorNew = "fc";
            // Transparenz zurücksetzen
            slTransparencyChange = false;
            slTransparency.Value = 0;
            tbTransparency.Text = "0%";
            slTransparencyChange = true;
        }



        // Button // Ja
        private async void btnYes_Click(object sender, RoutedEventArgs e)
        {
            // Wenn Hintergundfarbe
            if (colorType == "backgroundColor")
            {
                MainPage.backgroundColor = colorNew;
                await ClassFileMamagment.saveSettings();
            }

            // Wenn Rahmenfarbe  
            else if (colorType == "frameColor")
            {
                MainPage.frameColor = colorNew;
                await ClassFileMamagment.saveSettings();
            }

            // Wenn Benutzerinformation Hintergrundfarbe
            else if (colorType == "userInformationBackgroundColor")
            {
                MainPage.userInformationBackgroundColor = colorNew;
                await ClassFileMamagment.saveSettings();
            }

            // Wenn Benutzerinformation Schriftfarbe
            else if (colorType == "userInformationFontColor")
            {
                MainPage.userInformationFontColor = colorNew;
                await ClassFileMamagment.saveSettings();
            }

            // Wenn Information Hintergrundfarbe
            else if (colorType == "informationBackgroundColor")
            {
                MainPage.informationBackgroundColor = colorNew;
                await ClassFileMamagment.saveSettings();
            }

            // Zurück Navigieren
            NavigationService.GoBack();
        }



        // Button // Nein
        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            // Zurück Navigieren
            NavigationService.GoBack();
        }
        // ---------------------------------------------------------------------------------------------------





        // Buttons // Bereits verwendete Farben
        // ---------------------------------------------------------------------------------------------------
        // Feld 1 // Hintergrundfarbe
        private void RectangleUsedColor1_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Farbe einstellen
            colorPicker.Color = usedColor1.Color;
            colorNew = MainPage.backgroundColor;
            // Transparenz einstellen
            getTransparency();
        }



        // Feld 2 // Rahmenfarbe
        private void RectangleUsedColor2_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Farbe einstellen
            colorPicker.Color = usedColor2.Color;
            colorNew = MainPage.frameColor;
            // Transparenz einstellen
            getTransparency();
        }



        // Feld 3 // Benutzerinformation Hintergrundfarbe
        private void RectangleUsedColor3_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Farbe einstellen
            colorPicker.Color = usedColor3.Color;
            colorNew = MainPage.userInformationBackgroundColor;
            // Transparenz einstellen
            getTransparency();
        }



        // Feld 4 // Benutzerinformation Schriftfarbe 
        private void RectangleUsedColor4_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Farbe einstellen
            colorPicker.Color = usedColor4.Color;
            colorNew = MainPage.userInformationFontColor;
            // Transparenz einstellen
            getTransparency();
        }



        // Feld 5 // Information Hintergrundfarbe
        private void RectangleUsedColor5_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Farbe einstellen
            colorPicker.Color = usedColor5.Color;
            colorNew = MainPage.informationBackgroundColor;
            // Transparenz einstellen
            getTransparency();
        }



        // Slider Transparenz einstellen
        private async void getTransparency()
        {
            // Wenn Farbcode 
            if (colorNew.Length == 9)
            {


                // Wenn Transparenz vorhanden
                if (colorNew.Substring(1, 2) != "FF")
                {


                    // Slider Transparenz einstellen
                    transparency = colorNew.Substring(1, 2);
                    int intTransparency = int.Parse(transparency, System.Globalization.NumberStyles.HexNumber);
                    int percent = Convert.ToInt32(Convert.ToDouble(100) / 255 * Convert.ToDouble(intTransparency));
                    slTransparencyChange = false;
                    slTransparency.Value = 100 - percent;
                    tbTransparency.Text = (100 - percent).ToString() + "%";
                    slTransparencyChange = true;
                }
            }
        }
        // ---------------------------------------------------------------------------------------------------





        // SolidColorBrush aus Farbcode erstellen
        // ---------------------------------------------------------------------------------------------------
        // Wenn Hintergrundfarbe
        private SolidColorBrush getSolidColorBrush(string code, bool useTransparency)
        {
            // Farbeinstellungen verarbeiten
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
            else
            {
                colorNew = code;
            }
            byte A = new byte();
            if (useTransparency)
            {
                A = Convert.ToByte(transparency, 16);
            }
            else
            {
                A = Convert.ToByte(colorNew.Substring(1, 2), 16);
            }
            byte R = Convert.ToByte(colorNew.Substring(3, 2), 16);
            byte G = Convert.ToByte(colorNew.Substring(5, 2), 16);
            byte B = Convert.ToByte(colorNew.Substring(7, 2), 16);
            SolidColorBrush solidColorBrush = new SolidColorBrush(Color.FromArgb(A, R, G, B));

            // Farbe ausgeben
            return solidColorBrush;
        }
        // ---------------------------------------------------------------------------------------------------





        // Back Button
        //----------------------------------------------------------------------------------------------------
        protected async override void OnBackKeyPress(CancelEventArgs e)
        {
            // Zurück abbrechen
            e.Cancel = true;


            // Abfragen ob gespeichert wird
            if (MessageBox.Show("", MyApp.Resources.AppResources.X002_use, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                // Wenn Hintergundfarbe
                if (colorType == "backgroundColor")
                {
                    MainPage.backgroundColor = colorNew;
                    await ClassFileMamagment.saveSettings();
                }

                // Wenn Rahmenfarbe  
                else if (colorType == "frameColor")
                {
                    MainPage.frameColor = colorNew;
                    await ClassFileMamagment.saveSettings();
                }

                // Wenn Benutzerinformation Hintergrundfarbe
                else if (colorType == "userInformationBackgroundColor")
                {
                    MainPage.userInformationBackgroundColor = colorNew;
                    await ClassFileMamagment.saveSettings();
                }

                // Wenn Benutzerinformation Schriftfarbe
                else if (colorType == "userInformationFontColor")
                {
                    MainPage.userInformationFontColor = colorNew;
                    await ClassFileMamagment.saveSettings();
                }

                // Wenn Information Hintergrundfarbe
                else if (colorType == "informationBackgroundColor")
                {
                    MainPage.informationBackgroundColor = colorNew;
                    await ClassFileMamagment.saveSettings();
                }
            }


            // Zurück Navigieren
            NavigationService.GoBack();
        }
        // ---------------------------------------------------------------------------------------------------
    }
}