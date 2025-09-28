using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Tingtel_Assessment.Models;

[Table("Transaction")]
public partial class Transaction
{
    [Key]
    public Guid TransactionId { get; set; }

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
