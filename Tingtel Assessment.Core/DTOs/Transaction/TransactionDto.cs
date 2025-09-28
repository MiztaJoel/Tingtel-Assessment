using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tingtel_Assessment.Core.DTOs.Transaction
{
    public class TransactionDto
    {
        [StringLength(50)]
        [Unicode(false)]
        public string? UserId { get; set; }

        [Column(TypeName = "decimal(12, 2)")]
        public decimal? Amount { get; set; }

        [StringLength(50)]
        [Unicode(false)]
        public string? CategoryName { get; set; }

        [StringLength(50)]
        [Unicode(false)]
        public string? Description { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? DateTime { get; set; }
    }
}
