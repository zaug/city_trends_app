using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using Newtonsoft.Json;
using System.IO.IsolatedStorage;
using System.IO;

// balint.dezso@gmail.com - konzultáció.

namespace City_Trends
{
    public partial class UserFavourites : PhoneApplicationPage
    {
        /* Szükséges redundancia (beletöltöm az OnNavigatedTo-ban, és magának az osztály konstruktorának belsejében használom
           fel, mint függvényparamétert. Az egységesség érdekében itt is ugyanazokat a neveket adtam a változóimnak (lsd. DetailsPage):
        */
        private string _userFavVenue;   // Dokumentálva!
        private string _userFavID;      // Dokumentálva!

        /* A következő szótárat 2 helyen is felhasználjuk: a törlő és a hozzáadó metódusoknál. A program indulásakor 
         * a szótárat abból a fájlból inicializáljuk, amibe az előző munkamenet idején mentettük adatainkat:
        */
        private const string FavFile = "Favourite.json"; // Dokumentálva!

        private static Dictionary<string, string> _favourites = new Dictionary<string, string>();  // Dokumentálva!

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Fontos, hogy erre a kezelőre a MainPage-ről is navigálhatunk:
            try 
            {
                _userFavID = NavigationContext.QueryString["userFavID"]; // Ezzel az egyéni azonosítóval fogunk hivatkozni a lista elemére, ha törölni akarunk.
                _userFavVenue = NavigationContext.QueryString["userFavVenue"];
            }
            catch(System.Collections.Generic.KeyNotFoundException) // A MainPage-ről navigáltak ide:
            {
                // Újrainicializáljuk a szótárat:
                _favourites = ReLoadFavourites(FavFile);
                if (_favourites == null) { return; }
                if (_favourites.Count == 0)
                {
                    MessageBox.Show("Még nincsennek kedvenceid. :(\n\nTipp:\nKedvenc hely hozzáadásához tapints egy csempére, és a mentés gombra!");
                    NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                    return;
                }
                else
                {
                    App.ViewModel.LoadFavourites(_favourites, false);  // Frissítjük a nézetet.
                }
            }

            UserFavourites favItems = new UserFavourites();

            try
            {
                if (!_favourites.ContainsKey(_userFavID)) // Defenzív kód: ugyanazt a kulcsot nem visszük fel kétszer a táblába.
                {
                    _favourites.Add(_userFavID, _userFavVenue);
                    App.ViewModel.LoadFavourites(_favourites, false);  // Frissítjük a nézetet.
                    // Elmentjük a jelenlegi kedvenceket
                    ReSaveFavourites(_favourites, FavFile);
                }
            }
            catch (System.ArgumentNullException)
            {
                // Nincs kiválasztott elem.
                ApplicationBar.IsVisible = false;
            }

            FavList.ItemsSource = App.ViewModel.Favourites;
        }   // Dokumentálva!

        public UserFavourites()
        {
            InitializeComponent();
            BuildLocalizedApplicationBar();
        }

        private void BuildLocalizedApplicationBar()
        {
            ApplicationBar = new ApplicationBar();
            ApplicationBarIconButton deleteItems = new ApplicationBarIconButton(new Uri("/Toolkit.Content/ApplicationBar.Delete.png", UriKind.Relative));
            deleteItems.Text = "törlés";

            deleteItems.Click+=deleteItems_Click;
            ApplicationBar.Buttons.Add(deleteItems);
            ApplicationBar.IsVisible = false;
        }               // Dokumentálva!

        private void deleteItems_Click(object sender, EventArgs e)
        {
            // Lekérdezzük a kijelölt elemek azonosítóit, és elvégezzük a törlést.
            foreach (City_Trends.ViewModels.FavouriteView selected in FavList.SelectedItems)
            {
                _favourites.Remove(selected.UserFavID); // A törléshez elegendő a kulcs is.
                // Elmentjük a jelenlegi kedvenceket:
                ReSaveFavourites(_favourites, FavFile);
            }            
            // Ha nincs megjelenítendő elem, eltüntetjük az AppBart:
            if (_favourites.Count == 0)
            {
                ApplicationBar.IsVisible = false;
            }
            // Frissítjük a felületet:
            App.ViewModel.LoadFavourites(_favourites,true);
        }           // Dokumentálva!

        private void FavListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Ha van kijelölt elem, megjelenik (törölhetjük a kijelölt elemeket):

            if (FavList.SelectedItems.Count == 0)
            {
                ApplicationBar.IsVisible = false;
            }
            if (FavList.SelectedItems.Count != 0)
            {
                ApplicationBar.IsVisible = true;
            }
        }       // Dokumentálva!
        /* Lekérdezzük az alkalmazás által használt tárterületet (IsolatedStorage), megmondjuk az alkalmazásnak,
        hogy hová (mappa) mentse az adatokat (file). Mivel a függvény szignatúrája Dictionary, és nekünk elég, ha
        csak egy json file-t írunk, Szerializálnunk kell a szótár adatait. A folyamatjelzőt itt is megjelenítem:
        */

        private void ReSaveFavourites(Dictionary<string, string> _favourites,string FavFile)
        {
            string favourites = JsonConvert.SerializeObject(_favourites);

            using (IsolatedStorageFile cityTrendsStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream favStream = cityTrendsStore.CreateFile(FavFile))
                {
                    using (StreamWriter favWrite = new StreamWriter(favStream)) // Írunk a fájlba:
                    {
                        favWrite.Write(favourites);
                    }
                }
            }
        }   // Dokumentálva!
        private Dictionary<string, string> ReLoadFavourites(string FavFile) // Dokumentálva!
        {
            Dictionary<string, string> reloadedFavourites = new Dictionary<string, string>();

            using (IsolatedStorageFile cityTrendsStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                try
                {
                    using (IsolatedStorageFileStream favStream = cityTrendsStore.OpenFile(FavFile, FileMode.Open)) // Megnyitjuk olvasásra.
                    {
                        using (StreamReader favRead = new StreamReader(favStream)) // Bemásoljuk, és deszerializáljuk a szótárba:
                        {
                            string jsonFav = favRead.ReadToEnd();
                            return reloadedFavourites = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonFav); // Szükséges explicit típuskonverzió.
                        }
                    }
                }
                catch (System.IO.IsolatedStorage.IsolatedStorageException) { MessageBox.Show("Nincsennek kedvenceid. :("); return null; }
            }
        }
    }
}