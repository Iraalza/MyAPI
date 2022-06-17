using System.Text.Json.Serialization;

namespace MyAPI.Entities
{
    public class User
    {
        public string Name { get; set; }
        public string SpecClass { get; set; }
        public int Range { get; set; }
        public string HomeRace { get; set; }

        [JsonIgnore]
        public virtual Spec UserSpec { get; set; }
        public virtual Home UserHome { get; set; }
    }
}

