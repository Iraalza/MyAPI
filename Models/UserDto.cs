using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MyAPI.Models
{
    public class UserDto
    {
        [HiddenInput(DisplayValue = false)]
        public string Name { get; set; }
        public int Range { get; set; }

        [Required]
        [DisplayName("Rase")]
        public string HomeRace { get; set; }

        private static readonly string[] races = new[] {
        "people", "dwarves", "nightElves", "gnome", "draenei", "worgens",
        "pandaren", "orc", "undead", "tauren", "troll", "bloodElf", "goblin"
        };

        private static readonly SelectListItem blankSelectListItem = new SelectListItem("select...", String.Empty);
        public static IEnumerable<SelectListItem> ListRaces(string selectedRace)
        {
            var items = new List<SelectListItem> { blankSelectListItem };
            items.AddRange(races.Select(c => new SelectListItem(c, c, c == selectedRace)));
            return items;
        }

        [Required]
        [DisplayName("Class")]
        public string SpecClass { get; set; }

        private static readonly string[] classes = new[] {
        "warrior", "paladin", "hunter", "rogue", "priest", "shaman",
        "wizard", "warlock", "monk", "druid", "demonHunter", "deathKnight"
        };

        /*private static readonly SelectListItem bblankSelectListItem = new SelectListItem("select...", String.Empty);*/
        public static IEnumerable<SelectListItem> ListSpecs(string selectedClass)
        {
            var items = new List<SelectListItem> { blankSelectListItem };
            items.AddRange(classes.Select(c => new SelectListItem(c, c, c == selectedClass)));
            return items;
        }
    }
}
