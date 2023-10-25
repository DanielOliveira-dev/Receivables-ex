using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class Debtor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Reference { get; set; }
        public string Name { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? Town { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }
        public CountryCode CountryCode { get; set; }
        public string? RegistrationNumber { get; set; }
        public ICollection<Receivable> Receivables { get; set; } = new List<Receivable>();

        public void AddReceivable(Receivable receivable)
        {
            Receivables.Add(receivable);
        }
    }
}
