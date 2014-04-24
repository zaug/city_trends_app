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
using System.Net.Http;
using System.Threading.Tasks;
using City_Trends.Tips;

namespace City_Trends
{
    public class DataQuery
    {

        /*3.) Definiáljuk a lekérdezéshez szükséges változókat, készítsük fel az osztály QueryDataForItemViewModel metódusát
              az adatok letöltésére. Ehhez "el kell kapnunk" a küldő oldal NavigationService.Navigate URI-ját, és ki
              kell nyernünk belőlük a kulcsszavakat. Szintén be kell vezetni az API hívásokhoz szükséges kulcsokat.
              A metódusok implementálásához szükségünk lesz harmadik féltől származó csomagok installációjára.
              Ehhez a NuGet Package Manager konzolba a következőket kell beírnunk:
              
              install-package Newtonsoft.Json       // A továbbiakban sűrűn fogunk JSON adatokkal dolgozni.
              install-package Microsoft.Net.Http    // Alkalmazásunkból lehetőségünk lesz online adatot használni.
        */
        /*4.)Fel kell használnunk a requestURL-t, és le kell töltenünk az adatokat. A kapott JSON adatot deszerializálnunk kell,
             hogy meg tudjunk feleltetni minden adattagot azok osztálypéldányának. Ehhez az adatokat osztályokba kell szerveznem.
             Hogy az adatok osztályokon belüli ábrázolása illetve azok terjedelme miatt ne legyen káosz,
            számos új modult készítek az adatmodellhez.
            (VenueModel.cs, VenueGroup.cs, VenueData.cs).
        */
        /*7.)Miután minden adatot a helyére szerveztünk, a következő lépés az MVVM (Model-View-View-Model) minta implementációja.
             Kiindulási pontként az IDE által nyújtott Windows Phone DataBound App projektsablonját választottam, mint remek
             kiindulópontot és kódmintát. A következőkben xaml vezérlőkhöz készítek erőforrásokat, hogy azt a LongListSelector
             szerte az alkalmazás területén újrafelhasználhatóvá tegye. Miután ezzel készen vagyok, felépítem az ItemViewModelt,
             megszervezem a felület és az adatok közötti kötést (Binding). Ehhez a már lekérdezett és deszerializált adatokat 
             fogom felhasználni.
                  
        */


        // foursquare API kulcsok: // Dokumentálva!
        private const string _client_ID = "MEEMBBKDBZOVGYOZXTRG2PTQ0Q5GKMV3A2XJIDCDVMFF4EQ5";
        private const string _client_SECRET = "KII3G1GW3ONBWG2W5FH1NJQZTT055A1C4H0DJWJCA3N1B5WU";
        private const int _api_VERSION = 20131016;

        // API specifikus tárolók: // Dokumentálva!
        private int _limit = 50;
        private string _intent = "checkin";
        public int _radius = 30000;

        // További információkért lásd a https://developer.foursquare.com/docs/venues/search webhelyet: // Dokumentálva!
        public async Task<VenueModel>DataQueryForVenueViewModel(double latitude, double longitude, string searchKey = null)// Szükséges alapértelmezett paraméter.
        {
            // Meghívom a segédfüggvényemet, így átláthatóbbá a kód.
            string requestURL = createRequestURL(_api_VERSION, latitude, longitude, searchKey, _limit, _intent, _radius, _client_ID, _client_SECRET);
            HttpClient trendsClient = new HttpClient();
            // Állapotváltozók eredmény és állapot jelzésére:
            string apiResult = null;
            bool dataStatus = false;
            try
            {
                apiResult = await trendsClient.GetStringAsync(requestURL); // Aszinkron lekérdezés.
                dataStatus = true;
            }
            catch (HttpRequestException){ MessageBox.Show("Nincs internetkapcsolat. :(");}
            VenueModel venueModel = new VenueModel();
            if (dataStatus == false) { return null; }
            if (dataStatus == true) { venueModel = JsonConvert.DeserializeObject<VenueModel>(apiResult); } // Deszerializáció
            return venueModel;

        }

        // További információkért lásd a https://developer.foursquare.com/docs/venues/tips webhelyet: // Dokumentálva!
        public async Task<TipModel> TipsFromVenueData(string venue_ID)
        {
            string requestURL = createRequestURL(_api_VERSION, venue_ID, _limit, _client_ID, _client_SECRET);
            HttpClient trendsClient = new HttpClient();
            string apiResult = string.Empty;
            bool dataStatus = false;
            try
            {
                apiResult = await trendsClient.GetStringAsync(requestURL);
                dataStatus = true;
            }
            catch (HttpRequestException) { MessageBox.Show("Nincs internetkapcsolat. :("); }
            catch (Exception e){MessageBox.Show(e.Message);}
            TipModel tipModel = new TipModel();
            if (dataStatus == false) { return null; }
            if (dataStatus == true) { tipModel = JsonConvert.DeserializeObject<TipModel>(apiResult); }
            return tipModel;
        }

        // Search Venues verzió: // Dokumentálva!
        private string createRequestURL(int _api_VERSION,double latitude,double longitude,
                                        string searchKey,int _limit,string _intent,int _radius,
                                        string _client_ID,string _client_SECRET)
        {
            string requestURL = "https://api.foursquare.com/v2/venues/search?" +
                                "v={0}" +
                                "&ll={1}" +  // Figyeljük meg az elválasztó karaktert Debug módban
                                "&query={2}"+
                                "&limit={3}" +
                                "&intent={4}" +
                                "&radius={5}" +
                                "&client_id={6}" +
                                "&client_secret={7}";

            string ll = string.Join("%2C", latitude, longitude);
            ll = ll.Replace(",", ".");
            requestURL = string.Format(requestURL, _api_VERSION, ll, searchKey, _limit, _intent, _radius, _client_ID, _client_SECRET);
            return requestURL;
        }
        // Tips from a Venue verzió: // Dokumentálva!
        private string createRequestURL(int _api_VERSION,string venue_ID,int _limit,    
                                        string _client_ID,string _client_SECRET)
        {
            string requestURL = "https://api.foursquare.com/v2/venues/" +
                                venue_ID +
                                "/tips?" +
                                "v={0}" +
                                "&sort=popular" +
                                "&limit={1}" +
                                "&client_id={2}" +
                                "&client_secret={3}";
            requestURL = string.Format(requestURL, _api_VERSION, _limit, _client_ID, _client_SECRET);
            return requestURL;
        }
    }
}