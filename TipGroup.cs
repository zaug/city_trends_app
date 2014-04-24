using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace City_Trends.Tips
{   // Dokumentálva!
    public class TipGroup           // Lekérdezett tippek adatai.
    {
        public Tips tips { get; set; }
    }
    public class Tips
    {
        public int count { get; set; }
        public List<TipData> items { get; set; }
    }
    public class ReturnCode
    {
        public int code { get; set; }       // API hívás visszatérési értéke (200 ha sikeres hívás).
    }
}
