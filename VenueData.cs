using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*6.) Az alábbi 5 osztály fogja tartalmazni azokat az adatokat, amelyekkel a végfelhasználó találkozni fog alkalmazásunk
      használata során. További példányosítások során csak ezekre az osztályokra lesz szükségünk. 
 */

namespace City_Trends
{   // Dokumentálva!
    /*Venues:[{
            "id":,
            "name":,
            "contact":{},
            "location":{},
            "categories":[{}],
            "verified":,
            "stats":{},
            "specials":{},
            "hereNow":{},
            "referralId":
         }],
    */
    public class VenueData
    {
        public string id { get; set; }
        public string name { get; set; }
        public Contact contact { get; set; }
        public Location location { get; set; }
        public List<Categories> categories { get; set; }
        public bool verified { get; set; }
        public Stats stats { get; set; }
        public Specials specials { get; set; }
        public HereNow hereNow { get; set; }
        public string referralId { get; set; }
    }

        /*"location":{			 
            "address":,		 
            "lat":,			 
            "lng":,			 
            "distance":,		 
            "cc":,			 
            "city":,		 
            "country":		 
             },
    */
    public class Location
    {
        public string address { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
        public int distance { get; set; }
        public string cc { get; set; }
        public string city { get; set; }
        public string country { get; set; }
    }

    /*"categories":[{			 
            "id":,			 
            "name":,		 
            "pluralName":,		 
            "shortName":,							
            "icon":{},
            "primary":		 
                }],
    */
    public class Categories
    {
        public string id { get; set; }
        public string name { get; set; }
        public string pluralName { get; set; }
        public string shortName { get; set; }
        public Icon icon { get; set; }
        public bool primary { get; set; }
    }

    /*"hereNow":{			 
				"count":,	
                "summary":, 
				"groups":		 
				  },
    */
    public class HereNow
    {
        public int count { get; set; }
        public string summary { get; set; }
        public List<object> groups { get; set; }
    }

    /*"stats":{			 
            "checkinsCount":,	 
            "usersCount":,		 
            "tipCount":  		 
            },
    */
    public class Stats
    {
        public int checkinsCount { get; set; }
        public int usersCount { get; set; }
        public int tipCount { get; set; }
    }

    /*"icon":{		 
                "prefix":, 	 
                "suffix": 	 
                },
    */
    public class Icon
    {
        public string prefix { get; set; }
        public string suffix { get; set; }
    }

    /*"specials":{			 
				"count":,		 
				"items":[]		 
				   },
    */
    public class Specials
    {
        public int count { get; set; }
        public List<object> items { get; set; }
    }

    public class Contact
    {
        public string contact { get; set; }
    }

}
