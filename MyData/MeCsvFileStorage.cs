using MyAPI.Entities;
using System.Reflection;
using static System.Int32;
using Microsoft.Extensions.Logging;

namespace MyAPI.MyData
{
    public class MyCsvFileStorage : IMyStorage
    {
        private static readonly IEqualityComparer<string> collation = StringComparer.OrdinalIgnoreCase;

        private readonly Dictionary<string, Home> homes = new Dictionary<string, Home>(collation);
        private readonly Dictionary<string, Spec> specs = new Dictionary<string, Spec>(collation);
        private readonly Dictionary<string, User> users = new Dictionary<string, User>(collation);
        private readonly ILogger<MyCsvFileStorage> logger;

        public MyCsvFileStorage(ILogger<MyCsvFileStorage> logger)
        {
            this.logger = logger;
            ReadHomesFromCsvFile("Home.csv");
            ReadSpecsFromCsvFile("Spec.csv");
            ReadUsersFromCsvFile("User.csv");
            ResolveReferences();
        }

        private void ResolveReferences()
        {
            foreach (var sp in specs.Values)
            {
                sp.User = users.Values.Where(u => u.SpecClass == sp.Specc).ToList();
                foreach (var user in sp.User) user.UserSpec = sp;
            }

            foreach (var hm in homes.Values)
            {
                hm.User = users.Values.Where(u => u.HomeRace == hm.HomeName).ToList();
                foreach (var user in hm.User) user.UserHome = hm;
            }
        }

        private string ResolveCsvFilePath(string filename)
        {
            var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var csvFilePath = Path.Combine(directory, "csv-data");
            return Path.Combine(csvFilePath, filename);
        }

        private void ReadUsersFromCsvFile(string filename)
        {
            var filePath = ResolveCsvFilePath(filename);
            foreach (var line in File.ReadAllLines(filePath))
            {
                var tokens = line.Split(",");
                var user = new User
                {
                    Name = tokens[0],
                    SpecClass = tokens[1],
                    HomeRace = tokens[3],
                };
                if (TryParse(tokens[2], out var range)) user.Range = range;
                users[user.Name] = user;
            }
            logger.LogInformation($"Loaded {users.Count} models from {filePath}");
        }

        private void ReadHomesFromCsvFile(string filename)
        {
            var filePath = ResolveCsvFilePath(filename);
            foreach (var line in File.ReadAllLines(filePath))
            {
                var tokens = line.Split(",");
                var spec = new Spec
                {
                    Class = tokens[0],
                    Specc = tokens[1],
                };
                specs.Add(spec.Class, spec);
            }
            logger.LogInformation($"Loaded {specs.Count} models from {filePath}");
        }

        private void ReadSpecsFromCsvFile(string filename)
        {
            var filePath = ResolveCsvFilePath(filename);
            foreach (var line in File.ReadAllLines(filePath))
            {
                var tokens = line.Split(",");
                var home = new Home
                {
                    Race = tokens[0],
                    HomeName = tokens[1]
                };
                homes.Add(home.Race, home);
            }
            logger.LogInformation($"Loaded {homes.Count} manufacturers from {filePath}");
        }

        public int CountUsers() => users.Count;

        public IEnumerable<User> ListUsers() => users.Values;

        public IEnumerable<Home> ListHomes() => homes.Values;

        public IEnumerable<Spec> ListSpecs() => specs.Values;

        public User FindUser(string name) => users.GetValueOrDefault(name);

        public Spec FindSpecl(string specc) => specs.GetValueOrDefault(specc);

        public Home FindHome(string race) => homes.GetValueOrDefault(race);

        public void CreateUser(User user)
        {
            user.UserSpec.User.Add(user);
            user.UserHome.User.Add(user);
            UpdateUser(user);
        }

        public void UpdateUser(User user)
        {
            users[user.Name] = user;
        }

        public void DeleteUser(User user)
        {
            users.Remove(user.Name);
        }
    }
}
