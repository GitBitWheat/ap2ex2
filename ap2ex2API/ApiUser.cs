using Domain;

namespace ap2ex2API
{
    public class ApiUser
    {
        public ApiUser(User user)
        {
            Id = user.Id;
            Name = user.Name;
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Server { get; } = "I'll figure it out";
    }
}
