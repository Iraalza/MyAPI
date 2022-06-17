using System.Text.Json.Serialization;

namespace MyAPI.Entities
{
    public class Spec
    {
        public Spec()
        {
            User = new HashSet<User>();
        }
        public string Class { get; set; }
        public string Specc { get; set; }

        [JsonIgnore]
        public virtual ICollection<User> User { get; set; }
    }
}