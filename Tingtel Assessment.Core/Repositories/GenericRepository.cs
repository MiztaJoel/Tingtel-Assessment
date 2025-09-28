
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Tingtel_Assessment.Core.Constant;
using Tingtel_Assessment.DataContext;
using Tingtel_Assessment.Interfaces;

namespace Tingtel_Assessment.Repositories
{
	public class GenericRepository<T>:IGenericRepository<T> where T:class
	{
		protected readonly ApplicationDbContext _context;
		protected readonly DbSet<T> _dbSet;

		public GenericRepository( ApplicationDbContext context)
		{
			_context = context;
			_dbSet = _context.Set<T>();
		}

		public async Task<IEnumerable<T>> GetAllAsync() =>
			await _dbSet.ToListAsync();

		public async Task<T> GetByIdAsync(string id) =>
			await _dbSet.FindAsync(id);

		public async Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(
			PaginationParameters paginationParams, Expression<Func<T,bool>> filter = null!, Expression<Func<T, bool>> customSearch = null!)
		{
			var query = _dbSet.AsQueryable();

			if (filter != null)
				query = query.Where(filter);

			if (customSearch != null)
				query = query.Where(customSearch);

			var totalCount = await query.CountAsync();

			// Get the primary key property name dynamically
			var keyName = _context.Model.FindEntityType(typeof(T))
				.FindPrimaryKey()
				.Properties
				.Select(p => p.Name)
				.FirstOrDefault();

			if (string.IsNullOrEmpty(keyName))
				throw new InvalidOperationException($"Entity {typeof(T).Name} does not have a primary key.");

			// Order by primary key (descending)
			query = query.OrderByDescending(e => EF.Property<object>(e, keyName));

			var items = await query
				.Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
				.Take(paginationParams.PageSize)
				.ToListAsync();

			return (items, totalCount);
		}

		public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
		{
			return await _dbSet.Where(predicate).ToListAsync();
		}

		public async Task AddAsync(T entity)
		{
			await _dbSet.AddAsync(entity);
			await _context.SaveChangesAsync();

		}

		public async Task UpdateAsync(T entity)
		{
			_dbSet.Update(entity);
			await _context.SaveChangesAsync();
		}


		public async Task DeleteAsync(T entity)
		{
			_dbSet.Remove(entity);
			await _context.SaveChangesAsync();
		}

        public async Task RemoveRangeAsync(IEnumerable<T> entities)
        {
			 _dbSet.RemoveRange(entities);
			await _context.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }
    }
}
