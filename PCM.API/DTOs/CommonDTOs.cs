using System.ComponentModel.DataAnnotations;
using PCM.API.Models;

namespace PCM.API.DTOs
{
    // ========== MEMBER DTOs ==========
    public class MemberDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime JoinDate { get; set; }
        public double RankLevel { get; set; }
        public bool IsActive { get; set; }
        public int TotalMatches { get; set; }
        public int WinMatches { get; set; }
        public double WinRate => TotalMatches > 0 ? Math.Round((double)WinMatches / TotalMatches * 100, 1) : 0;
    }

    public class MemberCreateDto
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [EmailAddress]
        public string? Email { get; set; }

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public double RankLevel { get; set; } = 3.0;
    }

    public class MemberUpdateDto
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [EmailAddress]
        public string? Email { get; set; }

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        public DateTime? DateOfBirth { get; set; }
    }

    // ========== NEWS DTOs ==========
    public class NewsDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public bool IsPinned { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class NewsCreateDto
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        public bool IsPinned { get; set; } = false;
    }

    // ========== COURT DTOs ==========
    public class CourtDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class CourtCreateDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        [MaxLength(500)]
        public string? Description { get; set; }
    }

    // ========== TRANSACTION CATEGORY DTOs ==========
    public class TransactionCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public TransactionType Type { get; set; }
        public string TypeName => Type == TransactionType.Income ? "Thu" : "Chi";
    }

    public class TransactionCategoryCreateDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public TransactionType Type { get; set; }
    }

    // ========== TRANSACTION DTOs ==========
    public class TransactionDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public TransactionType CategoryType { get; set; }
        public string CreatedByName { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
    }

    public class TransactionCreateDto
    {
        public DateTime Date { get; set; } = DateTime.Now;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Số tiền phải lớn hơn 0")]
        public decimal Amount { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }

    public class TransactionSummaryDto
    {
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal Balance { get; set; }
        public bool IsNegative => Balance < 0;
    }
}
