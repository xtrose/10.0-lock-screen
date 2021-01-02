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
using Microsoft.Phone.Tasks;
using System.ComponentModel;





// Namespace
namespace MyApp.Pages
{





    // Bewertungsseite
    public partial class Rate : PhoneApplicationPage
    {





        // Variablen
        // ---------------------------------------------------------------------------------------------------
        // Variablen zum steuern der Seite
        string pagePart = "start";
        // ---------------------------------------------------------------------------------------------------





        // Klasse erstellen
        // ---------------------------------------------------------------------------------------------------
        public Rate()
        {
            // UI Komponenten laden
            InitializeComponent();


            // Buttons und Texte einstellen
            tbRateText.Text = MyApp.Resources.AppResources.X002_rateText1;
            btnRate.Visibility = System.Windows.Visibility.Visible;
            btnMsg.Visibility = System.Windows.Visibility.Collapsed;
            btnNotNow.Visibility = System.Windows.Visibility.Visible;
            btnBack.Visibility = System.Windows.Visibility.Collapsed;
        }
        // ---------------------------------------------------------------------------------------------------





        // Buttons
        // ---------------------------------------------------------------------------------------------------
        // Button bewerten
        private void btnRate_Click(object sender, RoutedEventArgs e)
        {
            // Bewertung öffnen
            MarketplaceReviewTask review = new MarketplaceReviewTask();
            review.Show();


            // Zurück
            NavigationService.GoBack();
        }



        // Button zurück
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            // Zurück
            NavigationService.GoBack();
        }



        // Button nicht jetzt
        private void btnNotNow_Click(object sender, RoutedEventArgs e)
        {
            // Buttons und Text umstellen
            tbRateText.Text = MyApp.Resources.AppResources.X002_rateText2;
            btnRate.Visibility = System.Windows.Visibility.Visible;
            btnMsg.Visibility = System.Windows.Visibility.Visible;
            btnNotNow.Visibility = System.Windows.Visibility.Collapsed;
            btnBack.Visibility = System.Windows.Visibility.Visible;

            // Seitenteil umstellen
            pagePart = "page2";
        }



        // Button Nachricht
        private void btnMsg_Click(object sender, RoutedEventArgs e)
        {
            // Kontakt E-Mail erstellen
            EmailComposeTask emailcomposer = new EmailComposeTask();
            emailcomposer.To = "xtrose@hotmail.com";
            emailcomposer.Subject = "10.0 Lock Screen Support";
            emailcomposer.Body = "";
            emailcomposer.Show();


            // Zurück
            NavigationService.GoBack();
        }
        // ---------------------------------------------------------------------------------------------------





        // Back Button
        //--------------------------------------------------------------------------------------------------------
        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            // Wenn Schnitt Panel offen
            if (pagePart == "start")
            {
                // Navigation zurück abbrechen
                e.Cancel = true;

                // Buttons und Text umstellen
                tbRateText.Text = MyApp.Resources.AppResources.X002_rateText2;
                btnRate.Visibility = System.Windows.Visibility.Visible;
                btnMsg.Visibility = System.Windows.Visibility.Visible;
                btnNotNow.Visibility = System.Windows.Visibility.Collapsed;
                btnBack.Visibility = System.Windows.Visibility.Visible;

                // Seitenteil umstellen
                pagePart = "page2";
            }
        }
        //--------------------------------------------------------------------------------------------------------





    }
}