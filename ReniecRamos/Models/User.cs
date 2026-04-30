using System.Data;

namespace ReniecRamos.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public string FullName { get; set; }
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
