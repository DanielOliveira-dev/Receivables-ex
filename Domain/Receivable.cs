using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class Receivable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Reference { get; set; }
        public CurrencyCode CurrencyCode { get; set; }
        public DateTime IssueDate { get; set; }
        public double OpeningValue { get; set; }
        public double PaidValue { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public bool? Cancelled { get; set; }
    }
}
