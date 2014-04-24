using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace City_Trends.ViewModels
{
    public class FavouriteView : INotifyPropertyChanged
    {
        private string _userFavID;
        public string UserFavID         // A kedvenc Venue azonosítója.
        {
            get
            {
                return _userFavID;
            }
            set
            {
                if (value != _userFavID)
                {
                    _userFavID = value;
                    NotifyPropertyChanged("UserFavID");
                }
            }
        }

        private string _userFavVenue;
        public string UserFavVenue
        {
            get
            {
                return _userFavVenue;
            }
            set
            {
                if (value != _userFavVenue)
                {
                    _userFavVenue = value;
                    NotifyPropertyChanged("UserFavVenue");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
