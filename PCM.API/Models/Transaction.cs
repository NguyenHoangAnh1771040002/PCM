using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCM.API.Models
{
    [Table("002_Transactions")]
    public class Transaction
    {
        [Key]
        public int Id { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        // FK to TransactionCategory
        public int CategoryId { get; set; }
        public virtual TransactionCategory Category { get; set; } = null!;

        // FK to Member (người tạo)
        public int? CreatedById { get; set; }
        public virtual Member? CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
