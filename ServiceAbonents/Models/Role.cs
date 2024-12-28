namespace ServiceAbonents.Models
{
    public class Role : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public List<User> Users { get; set; } = new();

        public Role(string name)
        {
            Name = name;
        }
    }
}
