using Core.Interfaces.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Tingtel_Assessment.Core.Interfaces;
using Tingtel_Assessment.Core.Utilities;
using Tingtel_Assessment.DataContext;
using Tingtel_Assessment.Models;
using Tingtel_Assessment.Repositories;

namespace Tingtel_Assessment.Core.Repositories
{
    public class TransactionRespository : GenericRepository<Transaction>, ITransactionRepository
    {
        
        private static readonly string[] Categories = { "Food", "Transport", "Utilities", "Shopping", "Health", "Education" };
        private static readonly Random random = new Random();
        private readonly IEmailService _emailService;
        public TransactionRespository(ApplicationDbContext context, IEmailService emailService) : base(context)
        {  
            _emailService=emailService;
        }

        public async Task ResetAndGenerateTransactions(string userId, int count)
        {
            Log.Information("Generate {count} transaction by {userId}", count,userId);
            var existingTransaction = _context.Transactions.Where(t => t.UserId == userId);
            await RemoveRangeAsync(existingTransaction);

            var newTransactions = new List<Transaction>();

            for (int i = 0;i< count; i++)
            {
                string categoryName = Categories[random.Next(Categories.Length)];

                newTransactions.Add(new Transaction
                {
                    TransactionId=Guid.NewGuid(),
                    UserId = userId,
                    CategoryName = categoryName,
                    Amount = Math.Round((decimal)(random.NextDouble() * 10000), 2),
                    DateTime = RandomDate(DateTime.UtcNow.AddMonths(-2),DateTime.UtcNow)

                });
            }
            await AddRangeAsync(newTransactions);
            Log.Information("{Count} transaction successfully created", count);
        }
        private static DateTime RandomDate (DateTime start, DateTime end)
        {
            var range = (end - start).Days;
            return start.AddDays(random.Next(range))
                        .AddHours(random.Next(0, 24))
                        .AddMinutes(random.Next(0, 60));
        }

        public async Task CheckIfRecentTransactionIsHigher(string userid, string categoryname,string username, string email)
        {
            var now = DateTime.UtcNow;
            var thirtyDaysAgo = now.AddDays(-30);

            var transactions = await _context.Transactions.
                Where(t => t.UserId == userid && t.CategoryName == categoryname)
                .AsNoTracking().ToListAsync();
             

            if (transactions is null)  throw new NotFoundException($"No record found");

            var recent = transactions.Where(t => t.DateTime >= thirtyDaysAgo).ToList();
            var previous = transactions.Where(t => t.DateTime < thirtyDaysAgo).ToList();

            var recentavg = recent.Any() ? recent.Average(t => t.Amount) : 0;
            var previousavg = previous.Any() ? previous.Average(t => t.Amount) : 0;

            if (recentavg > previousavg) 
            {
                var increase = recentavg - previousavg;
                var percentage =(increase / previousavg) * 100;

                Log.Information("Calculating percentage increment for {UserId}", userid);

                var subject = "Reset Your Password";
                var emailBody = await UtilityService.LoadTemplateAsync("Analysis.html");

                emailBody = emailBody.Replace("{UserName}", username
                                    .Replace("{Category}", categoryname
                                    .Replace("{Amount}", recentavg.ToString()!
                                    .Replace("{percentage}", percentage.ToString())
                                    )));

                var toEmail = new List<string>() { email! };
                var mailRequest = new MailRequest(toEmail, emailBody, emailBody, true);

                await _emailService.SendEmailAsync(mailRequest);

                Log.Information("email sent successfully to {Email}", email);
            }
        }
    }

}
