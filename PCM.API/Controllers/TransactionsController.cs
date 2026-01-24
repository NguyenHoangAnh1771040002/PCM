using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCM.API.Data;
using PCM.API.DTOs;
using PCM.API.Helpers;
using PCM.API.Models;
using System.Security.Claims;

namespace PCM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TransactionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/transactions
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<TransactionDto>>> GetAll(
            [FromQuery] DateTime? fromDate, 
            [FromQuery] DateTime? toDate,
            [FromQuery] TransactionType? type)
        {
            var query = _context.Transactions
                .Include(t => t.Category)
                .Include(t => t.CreatedBy)
                .AsQueryable();

            if (fromDate.HasValue)
            {
                query = query.Where(t => t.Date >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(t => t.Date <= toDate.Value);
            }

            if (type.HasValue)
            {
                query = query.Where(t => t.Category.Type == type.Value);
            }

            var transactions = await query
                .OrderByDescending(t => t.Date)
                .Select(t => new TransactionDto
                {
                    Id = t.Id,
                    Date = t.Date,
                    Amount = t.Amount,
                    Description = t.Description,
                    CategoryId = t.CategoryId,
                    CategoryName = t.Category.Name,
                    CategoryType = t.Category.Type,
                    CreatedByName = t.CreatedBy != null ? t.CreatedBy.FullName : "Hệ thống",
                    CreatedDate = t.CreatedDate
                })
                .ToListAsync();

            return Ok(transactions);
        }

        // GET: api/transactions/{id}
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<TransactionDto>> GetById(int id)
        {
            var transaction = await _context.Transactions
                .Include(t => t.Category)
                .Include(t => t.CreatedBy)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (transaction == null)
            {
                return NotFound(new { message = "Không tìm thấy giao dịch" });
            }

            return Ok(new TransactionDto
            {
                Id = transaction.Id,
                Date = transaction.Date,
                Amount = transaction.Amount,
                Description = transaction.Description,
                CategoryId = transaction.CategoryId,
                CategoryName = transaction.Category.Name,
                CategoryType = transaction.Category.Type,
                CreatedByName = transaction.CreatedBy?.FullName ?? "Hệ thống",
                CreatedDate = transaction.CreatedDate
            });
        }

        // POST: api/transactions
        [HttpPost]
        [Authorize(Roles = RoleConstants.AdminOrTreasurer)]
        public async Task<ActionResult<TransactionDto>> Create([FromBody] TransactionCreateDto dto)
        {
            var category = await _context.TransactionCategories.FindAsync(dto.CategoryId);
            if (category == null)
            {
                return BadRequest(new { message = "Danh mục không tồn tại" });
            }

            // Lấy member ID từ user hiện tại
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var member = await _context.Members.FirstOrDefaultAsync(m => m.UserId == userId);

            var transaction = new Transaction
            {
                Date = dto.Date,
                Amount = dto.Amount,
                Description = dto.Description,
                CategoryId = dto.CategoryId,
                CreatedById = member?.Id,
                CreatedDate = DateTime.Now
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = transaction.Id }, new TransactionDto
            {
                Id = transaction.Id,
                Date = transaction.Date,
                Amount = transaction.Amount,
                Description = transaction.Description,
                CategoryId = transaction.CategoryId,
                CategoryName = category.Name,
                CategoryType = category.Type,
                CreatedByName = member?.FullName ?? "Hệ thống",
                CreatedDate = transaction.CreatedDate
            });
        }

        // DELETE: api/transactions/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = RoleConstants.AdminOrTreasurer)]
        public async Task<IActionResult> Delete(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound(new { message = "Không tìm thấy giao dịch" });
            }

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Xóa thành công" });
        }

        // GET: api/transactions/summary
        [HttpGet("summary")]
        [Authorize(Roles = RoleConstants.AdminOrTreasurer)]
        public async Task<ActionResult<TransactionSummaryDto>> GetSummary()
        {
            var transactions = await _context.Transactions
                .Include(t => t.Category)
                .ToListAsync();

            var totalIncome = transactions
                .Where(t => t.Category.Type == TransactionType.Income)
                .Sum(t => t.Amount);

            var totalExpense = transactions
                .Where(t => t.Category.Type == TransactionType.Expense)
                .Sum(t => t.Amount);

            return Ok(new TransactionSummaryDto
            {
                TotalIncome = totalIncome,
                TotalExpense = totalExpense,
                Balance = totalIncome - totalExpense
            });
        }
    }
}
