namespace ReniecRamos.Models
{
    public class Procedure
    {
        public int ProcedureId { get; set; }
        public int CitizenId { get; set; }
        public virtual Citizen Citizen { get; set; }

        public string ProcedureType { get; set; } // Renewal, Duplicate
        public DateTime RequestDate { get; set; } = DateTime.Now;
        public string Status { get; set; } // Pending, Completed, Rejected
    }
}
