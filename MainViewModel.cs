using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using City_Trends.Resources;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Controls;

/*9.)Ebben a fájlban megvalósítom az MVVM patternt. (Model-View-View-Model) Ahhoz, hogy az adatkötéseket a felületi elemek,
     és a mögöttes kód között megvalósíthassuk, létre kell hozni (minden speciális típushoz pl. VenueData) View-eket
     (pl. VenueView) az INotifyPropertyChanged interfész implementációjával. Emellett szükségünk lesz az ObservableCollection<T>
     listára is amiben majd ezeket az elemeket el tudjuk tárolni. Minderre azért lesz szükség, hogy a mögöttes kód
     riportálhasson a felületnek (LongListSelector), ha az objektum valamelyik példánya (vagy annak valamely tagja) frissül.
*/

namespace City_Trends.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()  //Dokumentálva!
        {
            this.Venues = new ObservableCollection<VenueView>();
            this.Tips = new ObservableCollection<TipView>();
            this.Favourites = new ObservableCollection<FavouriteView>();
        }
        
        // Dokumentálva!
        public ObservableCollection<VenueView> Venues { get; private set; }
        public ObservableCollection<TipView> Tips { get; private set; }
        public ObservableCollection<FavouriteView> Favourites { get; private set; }
        

        private string _sampleProperty = "Sample Runtime Property Value";
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding
        /// </summary>
        /// <returns></returns>
        public string SampleProperty
        {
            get
            {
                return _sampleProperty;
            }
            set
            {
                if (value != _sampleProperty)
                {
                    _sampleProperty = value;
                    NotifyPropertyChanged("SampleProperty");
                }
            }
        }

        public string LocalizedSampleProperty
        {
            get
            {
                return AppResources.SampleProperty;
            }
        }
        // Dokumentálva!
        public bool IsDataLoaded
        {
            get;
            private set;
        }

        // Dokumentálva!
        public async void LoadVenues(string searchKey)
        {
            this.Venues.Clear();         // Frissítjük a felületet minden egyes újratöltéskor.

            string ID = string.Empty;
            string Name = string.Empty;
            string Location = string.Empty;
            string Categories = string.Empty;
            string Stats = string.Empty;
            string HereNow = string.Empty;
            string Venue_ID = string.Empty;

            VenueModel venueModel = new VenueModel();
            DataQuery dataQuery = new DataQuery();
            GPS gps = new GPS();
            List<VenueData> venueData = new List<VenueData>();  // Generikus lista VenueData típusokból: az ő elemeit kötjük a következő for ciklussal.
            
            try
            {
                var gpsData = await gps.UpdateMyMap(); //  Frissítjük a koordinátákat minden betöltéskor.
                venueModel = await dataQuery.DataQueryForVenueViewModel(gpsData[0], gpsData[1], searchKey); // Lekérdezzük az összes adatot.
                venueData = venueModel.response.venues;              // Az összes adat VenueData részhalmazára van csak szükség.
            }
            catch(System.NullReferenceException){MessageBox.Show("Nincs internetkapcsolat. :(");}

            for (int item = 0; item < venueData.Count; ++item)  // Hozzáadjuk az VenueModel példányokat a Venue listához.
            {   
                // VenueData kimenetek:
                Venue_ID = venueData[item].id;
                // Minta: "Margitsziget \/ Margit híd (4, 6)" 
                Name = venueData[item].name;
                // Minta: "Margit híd * Tőled 6 m távolságra * Itt: Budapest II. kerülete, Magyarország, (HU)." 
                Location = string.Format("{0} ▲ Tőled {1}m távolságra ►     Itt: {2}, {3}, ({4}).",
                                         venueData[item].location.address,
                                         venueData[item].location.distance.ToString(),
                                         venueData[item].location.city,
                                         venueData[item].location.country,
                                         venueData[item].location.cc);
                // Minta: "#Trams, #Bridge, #Island"
                for (int categoryName = 0; categoryName < venueData[item].categories.Count; ++categoryName)
                {
                    Categories += string.Join(", ", "#"+venueData[item].categories[categoryName].pluralName); // Hashtag képzés.
                }
                // Minta: "3949 check-in ▼ 1883 látogató ◄ 5 tipp ☺ .
                Stats = string.Format("{0} check-in ▼ {1} látogató ◄ {2} tipp.",
                                      venueData[item].stats.checkinsCount.ToString(),
                                      venueData[item].stats.usersCount.ToString(),
                                      venueData[item].stats.tipCount.ToString());
                // Minta: "3 ember van ott."
                HereNow = string.Format("{0} ember van ott.",venueData[item].hereNow.count.ToString());
                // Hozzáadjuk a rekordot:
                this.Venues.Add(new VenueView()
                {
                    ID = item.ToString(),
                    Name = Name,
                    Location = Location,
                    Categories = Categories,
                    Stats = Stats,
                    HereNow = HereNow,
                    Venue_ID = Venue_ID
                });
            }
            this.IsDataLoaded = true;    
        }
        // Dokumentálva!
        public async void LoadTips(string selectedVenue)
        {
            this.Tips.Clear();

            string CreatedAt = string.Empty;
            string Text = string.Empty;
            string Likes = string.Empty;
            string User = string.Empty;
            // Lekérdezzük a választott helyszín összes tippjét:
            DataQuery dataQuery = new DataQuery();
            var tipModel = await dataQuery.TipsFromVenueData(selectedVenue);

            // Felkészítjük a Tips kollekciót:
            for (int item = 0; item < tipModel.response.tips.items.Count; ++item)
            {
                /*Minta:  2010.06.25 ▲ Zsolt R.:
                      A villamos megáll, mert már átadták a megállót.
                      4 ember kedveli ezt.
                */
                //CreatedAt = tipModel.response.tips.items[item].createdAt.ToString();
                Text = tipModel.response.tips.items[item].text;
                Likes = tipModel.response.tips.items[item].likes.count.ToString();
                User = (tipModel.response.tips.items[item].user.firstName) + " " +
                       (tipModel.response.tips.items[item].user.lastName);

                this.Tips.Add(new TipView()
                {
                    CreatedAt = CreatedAt,
                    Text = Text,
                    Likes = string.Format("{0} ember kedveli ezt.",Likes),
                    User = User
                });
            }
            this.IsDataLoaded = true;
        }

        public void LoadFavourites(Dictionary<string, string> favourites, bool refresh) // Dokumentálva!
        {
            this.Favourites.Clear();

            foreach (KeyValuePair<string, string> item in favourites) // Betöltöm a nézetbe az összes kedvencet:
            {
                this.Favourites.Add(new FavouriteView() { UserFavID = item.Key, UserFavVenue = item.Value });
            }

            this.IsDataLoaded = true;
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