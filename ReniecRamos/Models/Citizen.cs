using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReniecRamos.Models
{
    public class Citizen
    {
        public int CitizenId { get; set; }

        [Required, StringLength(15)]
        public string DocumentNumber { get; set; }

        public int DocumentTypeId { get; set; }
        public virtual DocumentType DocumentType { get; set; }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string FirstLastName { get; set; }
        [Required]
        public string SecondLastName { get; set; }

        public DateTime BirthDate { get; set; }
        public string Gender { get; set; } // M/F

        public int CivilStatusId { get; set; }
        public virtual CivilStatus CivilStatus { get; set; }

        public string BirthUbigeoId { get; set; }
        [ForeignKey("BirthUbigeoId")]
        public virtual Ubigeo BirthPlace { get; set; }

        public string CurrentAddress { get; set; }
        public string CurrentUbigeoId { get; set; }
        [ForeignKey("CurrentUbigeoId")]
        public virtual Ubigeo ResidencePlace { get; set; }

        [StringLength(250)]
        [Display(Name = "Imagen del producto")]
        public string ImagePath { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
