using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

/*8.) Megvalósítjuk az INotifyPropertyChanged interfészt, az ItemViewModel tartalmazza mind azokat a mezőket, melyek elérésére
      szükség lesz az alkalmazás felületén. Látható, hogy minden nevet pont úgy választottam meg, hogy az adatmodelljeink
     (VenueData.cs, TipData.cs) neveinek megfeleltethetőek legyenek. Ezzel egységessé, és következetessé válik a kód.
      Mivel felületi elemen jelenítjük meg az adatokat, elegendő (és szükséges) a string típus használata. 
      Megjelöltem azokat a mezőket, ahhol majd a MainViewModel LoadData metódusában explicit típuskonverziót kell végrehajtanom.
*/

namespace City_Trends.ViewModels
{
    public class VenueView : INotifyPropertyChanged
    {
        // A kiválasztott csempe azonosítója:
        private string _id;
        public string ID        
        {
            get
            {
                return _id;
            }
            set
            {
                if (value != _id)
                {
                    _id = value;
                    NotifyPropertyChanged("ID");
                }
            }
        }

        // VenueData:

        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value != _name) // Name != _name
                {
                    _name = value; // Adatot frissítünk.
                    NotifyPropertyChanged("Name");
                }
            }
        }

        private string _location;   // MainViewModel: explicit típuskonverzió szükséges.
        public string Location
        {
            get
            {
                return _location;
            }
            set
            {
                if (value != _location)
                {
                    _location = value;
                    NotifyPropertyChanged("Location");
                }
            }
        }

        private string _categories; // MainViewModel: explicit típuskonverzió szükséges.
        public string Categories
        {
            get
            {
                return _categories;
            }
            set
            {
                if (value != _categories)
                {
                    _categories = value;
                    NotifyPropertyChanged("Categories");
                }
            }
        }

        private string _stats;      // MainViewModel: explicit típuskonverzió szükséges.
        public string Stats
        {
            get
            {
                return _stats;
            }
            set
            {
                if (value != _stats)
                {
                    _stats = value;
                    NotifyPropertyChanged("Stats");
                }
            }
        }

        private string _hereNow;    // MainViewModel: explicit típuskonverzió szükséges.
        public string HereNow
        {
            get
            {
                return _hereNow;
            }
            set
            {
                if (value != _hereNow)
                {
                    _hereNow = value;
                    NotifyPropertyChanged("HereNow");
                }
            }
        }

        private string _venue_ID;   // Ezzel fogjuk beazonosítani a helyszínt.
        public string Venue_ID
        {
            get
            {
                return _venue_ID;
            }
            set
            {
                if (value != _venue_ID)
                {
                    _venue_ID = value;
                    NotifyPropertyChanged("Venue_ID");
                }
            }
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