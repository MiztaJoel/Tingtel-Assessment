
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tingtel_Assessment.Models;


namespace Tingtel_Assessment.DataContext;

public partial class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
   {

   }

    public virtual DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
  
        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.Property(e => e.TransactionId).ValueGeneratedNever();
        });

        base.OnModelCreating(modelBuilder);
	}
}