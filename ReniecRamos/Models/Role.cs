namespace ReniecRamos.Models
{
    public class Role
    {
        public int RoleId { get; set; }
        public string Name { get; set; } // Administrator, Assistant
        public virtual ICollection<User> Users { get; set; }
    }
}
