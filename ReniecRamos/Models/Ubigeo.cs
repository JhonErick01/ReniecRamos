using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReniecRamos.Models
{
    public class Ubigeo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string UbigeoId { get; set; }
        public string Department { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
    }
}
