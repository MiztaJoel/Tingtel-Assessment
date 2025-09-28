using System.Linq.Expressions;
using Tingtel_Assessment.Core.Constant;

namespace Tingtel_Assessment.Interfaces
{
	public interface IGenericRepository<T> where T : class
	{
		Task<T> GetByIdAsync(string id);
		Task<IEnumerable<T>> GetAllAsync();
		Task<(IEnumerable<T> Items,int TotalCount)> GetPagedAsync(PaginationParameters paginationParams,
			Expression<Func<T,bool>> filter = null!, Expression<Func<T, bool>> customSearch = null!);
		Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
		Task AddAsync(T entity);
		Task UpdateAsync(T entity);
		Task DeleteAsync(T entity);
		Task RemoveRangeAsync(IEnumerable<T> entities);
		Task AddRangeAsync (IEnumerable<T> entities);
	}
}
