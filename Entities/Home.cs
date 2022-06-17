using System.Text.Json.Serialization;

namespace MyAPI.Entities
{
    public class Home
    {
        public Home()
        {
            User = new HashSet<User>();
        }

        public string Race { get; set; }
        public string HomeName { get; set; }

        [JsonIgnore]
        public virtual ICollection<User> User { get; set; }
    }
}

