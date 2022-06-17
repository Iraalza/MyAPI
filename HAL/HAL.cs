using MyAPI.Entities;
using System.ComponentModel;
using System.Dynamic;

namespace MyAPI.HAL
{
    public static class HAL
    {
        public static dynamic PaginateAsDynamic(string baseUrl, int index, int count, int total)
        {
            dynamic links = new ExpandoObject();
            links.self = new { href = "/api/users" };
            if (index < total)
            {
                links.next = new { href = $"/api/users?index={index + count}" };
                links.final = new { href = $"{baseUrl}?index={total - (total % count)}&count={count}" };
            }
            if (index > 0)
            {
                links.prev = new { href = $"/api/users?index={index - count}" };
                links.first = new { href = $"/api/users?index=0" };
            }
            return links;
        }

        public static Dictionary<string, object> PaginateAsDictionary(string baseUrl, int index, int count, int total)
        {
            var links = new Dictionary<string, object>();
            links.Add("self", new { href = "/api/users" });
            if (index < total)
            {
                links["next"] = new { href = $"/api/vusers?index={index + count}" };
                links["final"] = new { href = $"{baseUrl}?index={total - (total % count)}&count={count}" };
            }
            if (index > 0)
            {
                links["prev"] = new { href = $"/api/users?index={index - count}" };
                links["first"] = new { href = $"/api/users?index=0" };
            }
            return links;
        }

        public static dynamic ToResource(this User user)
        {
            var resource = user.ToDynamic();
            resource._links = new
            {
                self = new
                {
                    href = $"/api/users/{user.Name}"
                },
                spec = new
                {
                    href = $"/api/specs/{user.SpecClass}"
                },
                home = new
                {
                    href = $"/api/homes/{user.HomeRace}"
                }
            };
            return resource;
        }

        public static dynamic ToDynamic(this object value)
        {
            IDictionary<string, object> result = new ExpandoObject();
            var properties = TypeDescriptor.GetProperties(value.GetType());
            foreach (PropertyDescriptor prop in properties)
            {
                if (Ignore(prop)) continue;
                result.Add(prop.Name, prop.GetValue(value));
            }
            return result;
        }

        private static bool Ignore(PropertyDescriptor prop)
        {
            return prop.Attributes.OfType<System.Text.Json.Serialization.JsonIgnoreAttribute>().Any();
        }
    }
}
