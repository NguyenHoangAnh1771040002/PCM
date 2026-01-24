using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCM.API.Models
{
    [Table("002_TransactionCategories")]
    public class TransactionCategory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        // Loại: Thu hoặc Chi
        public TransactionType Type { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation Properties
        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
