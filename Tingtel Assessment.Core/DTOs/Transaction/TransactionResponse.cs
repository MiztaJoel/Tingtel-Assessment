using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tingtel_Assessment.Core.DTOs.Transaction
{
    public class TransactionResponse
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
    }
}
