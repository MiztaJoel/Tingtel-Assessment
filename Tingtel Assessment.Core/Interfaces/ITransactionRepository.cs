namespace Tingtel_Assessment.Core.Interfaces
{
    public interface ITransactionRepository
    {
        Task ResetAndGenerateTransactions(string userId,int count);

        Task CheckIfRecentTransactionIsHigher(string userid, string categoryname,string username,string email);
    }
}
