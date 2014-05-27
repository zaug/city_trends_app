using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace City_Trends.Tips
{   
    public class TipData
    {
        public string id { get; set; }
        public int createdAt { get; set; }
        public string text { get; set; }
        public List<Entity> entities { get; set; }
        public string canonicalUrl { get; set; }
        public string lang { get; set; }
        public Likes likes { get; set; }
        public bool logView { get; set; }
        public Todo todo { get; set; }
        public User user { get; set; }
    }
    public class Entity
    {
        public List<int> indices { get; set; }
        public string type { get; set; }
        public Object @object { get; set; }
    }
    public class Object
    {
        public string url { get; set; }
    }
    public class Likes
    {
        public int count { get; set; }
        public List<object> groups { get; set; }
        public string summary { get; set; }
    }
    public class Todo
    {
        public int count { get; set; }
    }
    public class User
    {
        public string id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string gender { get; set; }
        public Photo photo { get; set; }
    }
    public class Photo
    {
        public string prefix { get; set; }
        public string suffix { get; set; }
    }

}
