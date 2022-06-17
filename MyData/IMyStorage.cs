using MyAPI.Entities;

namespace MyAPI.MyData
{
    public interface IMyStorage
    {
        public int CountUsers();
        public IEnumerable<User> ListUsers();
        public IEnumerable<Spec> ListSpecs();
        public IEnumerable<Home> ListHomes();

        public User FindUser(string name);
        public Spec FindSpecl(string specc);
        public Home FindHome(string race);

        public void CreateUser(User user);
        public void UpdateUser(User user);
        public void DeleteUser(User user);
    }
}
