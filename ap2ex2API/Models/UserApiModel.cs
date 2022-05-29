using Domain;

namespace ap2ex2API.Models
{
    public class UserApiModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Server { get; set; }

        public UserApiModel() { }

        public UserApiModel(User user)
        {
            Id = user.Id;
            Name = user.Name;
            Server = user.Server;
        }

        public User convertToUser()
        {
            return new User() { Id = Id, Name = Name, Server = Server };
        }
    }
}
