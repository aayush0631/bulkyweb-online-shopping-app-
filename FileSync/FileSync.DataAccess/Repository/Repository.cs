using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using FileSync.DataAccess.Repository.IRepository;
using FileSync.DataAccess.Data;
namespace FileSync.DataAccess.Repository;


public class Repository<T> : IRepository<T> where T : class
{
	private readonly ApplicationDbContext _db;
	internal DbSet<T> dbSet;

	public Repository(ApplicationDbContext db)
	{
		_db = db;
		dbSet = db.Set<T>();
	}

	public void Add(T entity)
	{
		dbSet.Add(entity);
	}

	public T? Get(Expression<Func<T, bool>> filter,
				  string? includeProperties = null,
				  bool tracked = false)
	{
		IQueryable<T> query = tracked
			? dbSet
			: dbSet.AsNoTracking();

		query = query.Where(filter);

		if (!string.IsNullOrEmpty(includeProperties))
		{
			foreach (var includeProp in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
			{
				query = query.Include(includeProp);
			}
		}

		return query.FirstOrDefault();
	}

	public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null,
								 string? includeProperties = null)
	{
		IQueryable<T> query = dbSet;

		if (filter != null)
			query = query.Where(filter);

		if (!string.IsNullOrEmpty(includeProperties))
		{
			foreach (var includeProp in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
			{
				query = query.Include(includeProp);
			}
		}

		return query.ToList();
	}

	public void Remove(T entity)
	{
		dbSet.Remove(entity);
	}

	public void RemoveRange(IEnumerable<T> entities)
	{
		dbSet.RemoveRange(entities);
	}
}