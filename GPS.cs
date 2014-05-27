using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Device.Location;
using Windows.Devices.Geolocation;
using Microsoft.Phone.Shell;
using System.Windows;

namespace City_Trends
{            /*1.)
                Létrehozunk egy Geolocator típusú változót, és a GetGeopositionAsync tulajdonságán keresztül
                meghatározzuk a földrajzi helyzetet, majd az értékét átadjuk egy Geoposition típusú változónak.
                Az adatokat (földrajzi szélesség, hosszúság) elmentjük a GeoCoordinate típusú változóba,
                majd ennek értékével állítjuk be a Map vezérlő SetView tulajdonságát: 
             */

    public class GPS
    {   
        public async Task<double[]> UpdateMyMap()
        {
            Geolocator location = new Geolocator();
            DataQuery radius = new DataQuery();
            location.DesiredAccuracyInMeters = (uint)radius._radius; // Szükséges típuskényszerítés.
            try
            {
                SystemTray.ProgressIndicator = new ProgressIndicator();
                SystemTray.ProgressIndicator.Text = "GPS pozíció bemérése...";
                SetProgressIndicator(true); // Folyamatjelző: a földrajzi pozíció meghatározásának végéig látszik.
                Geoposition position = await location.GetGeopositionAsync(TimeSpan.FromMinutes(1), TimeSpan.FromSeconds(20));
                SystemTray.ProgressIndicator.Text = "GPS pozíció rendben...";
                SetProgressIndicator(false);
                double[] latitude = new double[2]; // Optimalizálás: helypazarlás lenne egy egész GeoCoordinate objektumot visszaadni.
                latitude[0] = position.Coordinate.Latitude;
                latitude[1] = position.Coordinate.Longitude;
                return latitude;
            }
            catch (UnauthorizedAccessException){MessageBox.Show("Engedélyezze a helyadatok megosztását a beállításokban.");return null;}
            catch (Exception e){MessageBox.Show(e.Message); // Ha a kivétel ismeretlen, szeretnénk tudni, hogy mi okozhatta a problémát.
                return null;
            }
        }

        public void SetProgressIndicator(bool isVisible)
        {
            SystemTray.ProgressIndicator.IsVisible = isVisible;
            SystemTray.ProgressIndicator.IsIndeterminate = isVisible;
        }
    }
}
