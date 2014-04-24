using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace City_Trends
{
    // Módosítsuk az App.xaml.cs-t, hogy az új adatmodellt használja.
    // Dokumentálva!
    public class VenueModel // Az adatszerkezet modellje.
    {
        public ReturnCode meta { get; set; }             // Viszatérési érték.
        public VenueGroup response { get; set; }        // Csoportosított adatok.
    }

}
