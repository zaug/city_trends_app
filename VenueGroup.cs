using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*5.)A JSON adatstruktúra C#-os ábrázolása "kívülről-befelé-felülről-lefelé" módszerrel történik. 
     Tehát minden fogalmat tisztán megfeleltetek a hozzá tartozó adattagnak (név), az osztályhierarchia kialakítását pedig
     osztályok közötti kapcsolatok meghatározásával, teszem. ( Itt most a gyermek-szülő kapcsolatokra kell gondolni).
     Megjegyzem, hogy a nagyobb, átláthatatlanabb struktúrákat nem szokás manuálisan lekódolni.      
     Erre léteznek külső eszközök példáért lásd a http://www.json2csharp.com webhelyet.
*/

namespace City_Trends
{   // Dokumentálva!
    /*{
             "meta":{},
	         "venues":[{}],
			 "neighborhoods":[],		
			 "confident":
       }
    */
    public class VenueGroup
    {
        public List<VenueData> venues { get; set; }     // Tartalmazza az összes adatot, az összes helyszínről.
        public List<object> neighborhoods { get; set; }
        public bool confident { get; set; }
    }

    public class ReturnCode
    {
        public int code { get; set; }       // API hívás visszatérési értéke (200 ha sikeres hívás).
    }
}
