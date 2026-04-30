using System.ComponentModel.DataAnnotations;

namespace ReniecRamos.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "El usuario es obligatorio")]
        public string Username { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "El nombre completo es obligatorio")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Seleccione un rol")]
        public int RoleId { get; set; } // 1 para Admin, 2 para Asistent
    }
}
