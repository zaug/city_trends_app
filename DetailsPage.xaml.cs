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


namespace City_Trends
{
    public partial class DetailsPage : PhoneApplicationPage
    {
        // A kedvenceket direkt privát elérésűeknek választottam:
        private string _userFavVenue; // Ezt jelenítjük majd meg a kedvencek listáján.  
        private string _userFavID;    // Ez pedig a venue azonosítója lesz.            
        
        protected override void OnNavigatedTo(NavigationEventArgs e) 
        {
            if (DataContext == null)
            {
                string selectedIndex = "";
                if (NavigationContext.QueryString.TryGetValue("selectedItem", out selectedIndex))  
                {
                    int index = int.Parse(selectedIndex);
                    DataContext = App.ViewModel.Venues[index];
                }
            }
            string  selectedVenue = NavigationContext.QueryString["selectedVenue"]; // Kinyerjük a Venue_ID-t a QueryString-ből.
            App.ViewModel.LoadTips(selectedVenue);
            TipLonglistSelector.ItemsSource = App.ViewModel.Tips; // Beállítjük a lista forrásadatait.
            // Elmentjük a venue ID-ját, még szükségünk lesz rá:
            _userFavID = selectedVenue;
            _userFavVenue = NavigationContext.QueryString["selectedVenueName"];
        }              

        public DetailsPage()
        {
            InitializeComponent();
            BuildLocalizedApplicationBar();
        }
        
        private void BuildLocalizedApplicationBar()
        {
            ApplicationBar = new ApplicationBar();

            ApplicationBarIconButton saveBarButton = new ApplicationBarIconButton(new Uri("/Assets/save.png", UriKind.Relative));

            saveBarButton.Text = "mentés";
            ApplicationBar.Buttons.Add(saveBarButton);

            saveBarButton.Click += saveBarButton_Click;
        }

        /*10.) Az oldalon megjelenő venue címét, nevét el kell menteni egy típusos listába, később
                    ezt a listát fogjuk megjeleníteni a Favourites.xaml felületen. Minden elem mellé egy checkbox kerül,
                    tehát törölni is lehet majd a listából. Ahhoz, hogy a beállítások ne vesszenek el, 
                    minden mentéskor/törléskor újraírunk egy JSON fájlt az IsolatedStorage-ben, így ha a felhasználó
                    meg akarja nézni a kedvenceit, ki fogjuk tudni íratni azt a listát.
        */
       
        void saveBarButton_Click(object sender, EventArgs e)
        {          
             
            // Átnavigálunk az UserFavourites oldalra, és elküldjük a szükséges adatokat is:
            string queryString = string.Format("/UserFavourites.xaml?userFavID={0}&userFavVenue={1}", _userFavID, _userFavVenue);
            NavigationService.Navigate(new Uri(queryString, UriKind.Relative));
        }
    }
}
