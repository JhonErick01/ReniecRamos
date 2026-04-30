using System.ComponentModel.DataAnnotations;

namespace ReniecRamos.Models.ViewModels
{
    public class CitizenCreateViewModel
    {
        [Required]
        public string DocumentNumber { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string FirstLastName { get; set; }
        [Required]
        public string SecondLastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public int DocumentTypeId { get; set; }
        public int CivilStatusId { get; set; }
        public string BirthUbigeoId { get; set; }
        public string CurrentUbigeoId { get; set; }
        public string CurrentAddress { get; set; }

        // Propiedad para capturar el archivo del formulario
        public IFormFile? PhotoFile { get; set; }
    }
}