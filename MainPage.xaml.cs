using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using City_Trends.Resources;
using City_Trends.ViewModels;
using Windows.Devices.Geolocation;
using System.Device.Location;

using Coding4Fun.Toolkit.Controls;

namespace City_Trends
{
    public partial class MainPage : PhoneApplicationPage // Dokumentálva!
    {
        public string searchValue = string.Empty; // Dokumentálva!

        public MainPage() // Dokumentálva!
        {
            InitializeComponent();
            BuildLocalizedApplicationBar();
            Loaded += MainPage_Loaded;

            DataContext = App.ViewModel;
        }
        // Dokumentálva!
        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            Geolocator location = new Geolocator();
            location.DesiredAccuracyInMeters = 250;
            Geoposition position = await location.GetGeopositionAsync(TimeSpan.FromMinutes(1), TimeSpan.FromSeconds(30));
            var myMapCenter = new GeoCoordinate(position.Coordinate.Latitude, position.Coordinate.Longitude);

            MyMap.Center = myMapCenter;
            MyMap.ZoomLevel = 15;            
        }
        // Dokumentálva!
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            searchValue = HttpUtility.UrlEncode(SearchBox.Text); // Explicit kasztolás (defenzív kód).
            App.ViewModel.LoadVenues(searchValue); // Kereső kulcsszó esetén új lekérdezést indítunk, és frissítjük a felületet.
        }

        
        // Alapértelmezetten betölti a VenueView-et: // Dokumentálva!
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadVenues(searchValue);    
            }
        }
        // Dokumentálva!
        private void MainLongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MainLongListSelector.SelectedItem == null) { return; }
            // Elküldöm a DetailsPage-nek a Venue_ID-t:
            string queryString = string.Format("/DetailsPage.xaml?selectedItem={0}&selectedVenue={1}&selectedVenueName={2}",
                                                            (MainLongListSelector.SelectedItem as VenueView).ID,
                                                            (MainLongListSelector.SelectedItem as VenueView).Venue_ID,
                                                            (MainLongListSelector.SelectedItem as VenueView).Name);
            NavigationService.Navigate(new Uri(queryString,UriKind.Relative));
            MainLongListSelector.SelectedItem = null;
        }
        private void BuildLocalizedApplicationBar()
        {
            ApplicationBar = new ApplicationBar();

            ApplicationBarIconButton appBarAbout = new ApplicationBarIconButton(new Uri("/Toolkit.Content/ApplicationBar.Check.png", UriKind.Relative));
            appBarAbout.Text = "App infó";
            ApplicationBarIconButton appBarFavourites = new ApplicationBarIconButton(new Uri("/Toolkit.Content/ApplicationBar.Select.png", UriKind.Relative));
            appBarFavourites.Text = "Kedvencek";

            ApplicationBar.Buttons.Add(appBarAbout);
            ApplicationBar.Buttons.Add(appBarFavourites);

            appBarAbout.Click += appBarAbout_Click;
            appBarFavourites.Click += appBarFavourites_Click;
        }

        void appBarAbout_Click(object sender, EventArgs e)
        {
            AboutPrompt aboutMe = new AboutPrompt();
            aboutMe.Show("Teket Dávid  \n\n"+
                "Szakdolgozat szoftver.\nComputer School Számítástechnikai \n"+
                "Szakképző Iskola\nBudapest\n2014-04-19\n\nfunkciók:\n\n\t\t>kulcsszavas kereső\n"+
                "\t\t>helyszínadatok megjelenítése \n+ térkép\n\t\t>kedvencek hozzáadása/törlése\n"+
                "\t\t>az alkalmazás 30 km-es\nkörzetben keres helyszíneket\n\n\n"+
            "felhasznált könyvtárak:\n\n"+
            "\t\t>JSON.NET\n\t\t>Http Client Library\n\t\t>Coding4Fun Toolkit\n\t\t>tWindows Phone ToolKit\n\n\n"+
            "az alkalmazás foursquare api-t, és a \ntelefon gps-ét használja", "@zaugheek", "zaug@outlook.com");
        }

        void appBarFavourites_Click(object sender, EventArgs e)
        {
            string queryString = string.Format("/UserFavourites.xaml"); // Átnavigál a kedvencekhez.
            NavigationService.Navigate(new Uri(queryString, UriKind.Relative));
        }
    }
}